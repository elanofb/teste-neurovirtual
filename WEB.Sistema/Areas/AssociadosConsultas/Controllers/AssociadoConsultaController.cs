using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using BLL.ConfiguracoesCarteirinha;
using DAL.Associados.DTO;
using WEB.Helpers;
using WEB.Areas.AssociadosConsultas.ViewModels;

namespace WEB.Areas.AssociadosConsultas.Controllers {

    [OrganizacaoFilter]
    public class AssociadoConsultaController : Controller {

        // Atributos
        private IAssociadoBL _AssociadoBL;

        // Propriedades
        private IAssociadoBL OAssociadoBL => _AssociadoBL = _AssociadoBL ?? new AssociadoBL();

        //
        public ActionResult index() {

            var ViewModel = new AssociadoConsultaForm();

            return View("index", ViewModel);
        }

        //
        [HttpGet]
        public ActionResult consultar() {

            var ViewModel = new AssociadoConsultaForm();

            return View("index", ViewModel);
        }

        //
        [HttpPost]
        public ActionResult consultar(AssociadoConsultaForm ViewModel) {

            var OAssociadoConsultaVM = new AssociadoConsultaVM();

            var queryAssociados = OAssociadoConsultaVM.montarQuery(ViewModel);

            if(ViewModel.flagTipoSaida == TipoSaidaHelper.EXCEL) {
                
                var OAssociadoConsultaExportacao = new AssociadoConsultaExportacao();
                
                OAssociadoConsultaExportacao.baixarExcel(queryAssociados.ToList());
            }
            
            var query = queryAssociados.Select(x => new ItemListaAssociado {
                id = x.id,
                idPessoa = x.idPessoa,
                nroAssociado = x.nroAssociado,
                descricaoTipoAssociado = x.descricaoTipoAssociado,
                flagTipoPessoa = x.flagTipoPessoa,
                nome = x.nome,
                razaoSocial = x.razaoSocial,
                nroDocumento = x.nroDocumento,
                dtCadastro = x.dtCadastro,
                ativo = x.ativo,
                //flagSituacaoContribuicao = x.flagSituacaoContribuicao
            });

            query = query.OrderBy(x => x.nome);

            ViewModel.listaAssociados = query.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());
            
            ViewModel.carregarEmails();
            
            ViewModel.carregarTelefones();
                            
            string htmlCarteirinha = ConfiguracaoCarteirinhaBL.getInstance.carregar().htmlCarteirinha;

            ViewModel.flagTemCarteirinha = !String.IsNullOrEmpty(htmlCarteirinha);

            return View("index", ViewModel);

        }

        [HttpGet, ActionName("busca-auto-complete")]
        public ActionResult autoCompleteSearch(string term) {

            var query = this.OAssociadoBL.listar(0, "", term, "S")
                        .Select(x => new {
                                    x.id,
                                    value = x.Pessoa.flagTipoPessoa == "F" ? x.Pessoa.nome : x.Pessoa.razaoSocial,
                                    label = x.Pessoa.flagTipoPessoa == "F" ? x.Pessoa.nome : x.Pessoa.razaoSocial
                        }).Take(10).ToList();

            return Json(query, JsonRequestBehavior.AllowGet);
        }
        
    }
}
