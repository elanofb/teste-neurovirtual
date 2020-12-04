using System;
using System.Linq;
using System.Web.Mvc;
using BLL.CuponsDesconto;
using BLL.Financeiro;
using WEB.Areas.CuponsDesconto.ViewModels;
using DAL.CuponsDesconto;
using PagedList;
using MvcFlashMessages;

namespace WEB.Areas.CuponsDesconto.Controllers {

    [OrganizacaoFilter]
    public class CupomDescontoController : Controller {

        //Atributos
        private ICupomDescontoBL _CupomDescontoBL;
        private ITituloReceitaPagamentoBL _TituloReceitaPagamentoBL;
        private IReceitaConsultaBL _ReceitaConsultaBL;

        //Propriedades
        private ICupomDescontoBL OCupomDescontoBL => this._CupomDescontoBL = this._CupomDescontoBL ?? new CupomDescontoBL();
        private ITituloReceitaPagamentoBL OTituloReceitaPagamentoBL => this._TituloReceitaPagamentoBL = this._TituloReceitaPagamentoBL ?? new TituloReceitaPagamentoBL();
        private IReceitaConsultaBL OReceitaConsultaBL => this._ReceitaConsultaBL = this._ReceitaConsultaBL ?? new ReceitaConsultaBL();

        //
        public ActionResult listar() {
            string descricao = UtilRequest.getString("valorBusca");
            string ativo = UtilRequest.getString("flagAtivo");
            bool? flagUsado = UtilRequest.getBool("flagUsado");

            var query = this.OCupomDescontoBL.listar(descricao, ativo);

            var lista = query.ToList();

            var idsCupons = lista.Select(x => x.id).ToList();

            var listaTitulosPagamentos = OTituloReceitaPagamentoBL.listar(0).Where(x => idsCupons.Contains(x.idCupomDesconto.Value)).Select(x => new {x.idCupomDesconto, x.id}).ToList();
            lista.ForEach(Item => Item.qtdeUsados = listaTitulosPagamentos.Count(x => x.idCupomDesconto == Item.id));

            if (!flagUsado.isEmpty()) {
                if (flagUsado == true) {
                    lista = lista.Where(x => x.flagUtilizado || x.qtdeUsos <= x.qtdeUsados).ToList();
                }
                if (flagUsado == false) {
                    lista = lista.Where(x => !x.flagUtilizado && x.qtdeUsos > x.qtdeUsados).ToList();
                }
            }
            
            return View(lista.OrderByDescending(x => x.dtCadastro).ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {
            var ViewModel = new CupomDescontoForm();

            ViewModel.CupomDesconto = this.OCupomDescontoBL.carregar(UtilNumber.toInt32(id)) ?? new CupomDesconto();

            return View(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(CupomDescontoForm ViewModel) {
            
            if (!ModelState.IsValid) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Houve um problema ao salvar o registro. Tente novamente.");

                return View(ViewModel);
            }

            if (ViewModel.CupomDesconto.id > 0) {

                var OCupom = this.OCupomDescontoBL.carregar(ViewModel.CupomDesconto.id);

                var flagUsadoQtde = OTituloReceitaPagamentoBL.listar(0).Count(x => x.idCupomDesconto == ViewModel.CupomDesconto.id);

                if (OCupom?.flagUtilizado == true || (OCupom?.qtdeUsos > 0 && flagUsadoQtde > 0)) {

                    this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não é possível editar um cupom de desconto que já foi usado.");

                    return View(ViewModel);
                }

            }

            bool flagSalvo = this.OCupomDescontoBL.salvar(ViewModel.CupomDesconto);

            if (flagSalvo) {

                this.OCupomDescontoBL.enviarCupom(ViewModel.CupomDesconto.id);

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "<strong>Sucesso!</strong><br />As informações foram salvas com sucesso!");

                return View(ViewModel);
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "Não foi possível salvar as informações, tente novamente.");

            return View(ViewModel);
        }

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {

            var Retorno = new UtilRetorno();

            Retorno.flagError = false;

            foreach (var idUsuario in id) {

                var RetornoItem = this.OCupomDescontoBL.excluir(idUsuario);

                if (RetornoItem.flagError) {
                    Retorno.flagError = true;
                    Retorno.listaErros.Add(RetornoItem.listaErros.FirstOrDefault());
                }
            }

            if (!Retorno.flagError) {
                Retorno.listaErros.Add("Os registros informados foram removidos com sucesso.");
            }

            return Json(Retorno);
        }

        //Envio de cupom por email
        public ActionResult enviarCupom() {

			int idCupomDesconto = UtilRequest.getInt32("idCupomDesconto");

            var Retorno = this.OCupomDescontoBL.enviarCupom(idCupomDesconto);

            return Json(Retorno);
        }

        [ActionName("partial-listar-utlizacoes")]
        public PartialViewResult listarUtilizacoes(int? idCupomDesconto) {

            idCupomDesconto = idCupomDesconto.toInt();

            var listaUtilizacoes = OReceitaConsultaBL.listarPagamentos(0).Where(x => x.idCupomDesconto == idCupomDesconto).ToList();

            return PartialView(listaUtilizacoes);
        }
    }
}