using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Configuracoes;
using BLL.Contribuicoes;
using BLL.Services;
using DAL.Configuracoes;
using DAL.Contribuicoes;
using DAL.Permissao.Security.Extensions;
using WEB.Areas.Contribuicoes.ViewModels;
using MvcFlashMessages;

namespace WEB.Areas.Contribuicoes.Controllers {

    [OrganizacaoFilter]
    public class ContribuicaoController : Controller {

        //Constantes

        //Atributos
        private IContribuicaoValidacaoBL _IContribuicaoValidacaoBL;
        private IContribuicaoBL _ContribuicaoPadraoBL;

        //Propriedades
        private IContribuicaoValidacaoBL OContribuicaoValidacaoBL => _IContribuicaoValidacaoBL = _IContribuicaoValidacaoBL ?? new ContribuicaoValidacaoBL();
        private IContribuicaoBL OContribuicaoPadraoBL => this._ContribuicaoPadraoBL = this._ContribuicaoPadraoBL ?? new ContribuicaoPadraoBL();

        // GET: Contribuicoes/ContribuicaoPadrao/listar
        [HttpGet, OrganizacaoFilter]
        public ActionResult listar() {

            string descricao = UtilRequest.getString("valorBusca");
            string ativo = UtilRequest.getString("flagAtivo");


            var query = this.OContribuicaoPadraoBL.listar(descricao, ativo)
                                                              .Where(x => x.idPeriodoContribuicao > 0)
                                                              .OrderByDescending(x => x.dtCadastro)
                                                              .Select(x => new
                                                              {
                                                                  x.id, 
                                                                  x.dtValidade,
                                                                  x.descricao,
                                                                  x.idTipoVencimento,
                                                                  TipoVencimento = new { id = x.idTipoVencimento, x.TipoVencimento.descricao},
                                                                  x.idPeriodoContribuicao,
                                                                  PeriodoContribuicao = new { id = x.idPeriodoContribuicao, x.PeriodoContribuicao.descricao },
                                                                  x.ativo
                                                                  
                                                              });

            var listaContribuicao = query.ToListJsonObject<Contribuicao>();

            return View(listaContribuicao.ToList());
        }


        // GET: Contribuicoes/ContribuicaoPadrao/editar
        [HttpGet, OrganizacaoFilter]
        public ActionResult editar(int? id) {

            ContribuicaoPadraoForm ViewModel = new ContribuicaoPadraoForm();

            ViewModel.Contribuicao = this.OContribuicaoPadraoBL.carregar(UtilNumber.toInt32(id)) ?? new Contribuicao();

            if (ViewModel.Contribuicao.id == 0) {

                ConfiguracaoNotificacao ConfiguracaoNotificacao = ConfiguracaoNotificacaoBL.getInstance.carregar();
                
                ViewModel.Contribuicao.emailCobrancaTitulo = ConfiguracaoNotificacao.tituloEmailCobrancaContribuicao;

                ViewModel.Contribuicao.emailCobrancaHtml = ConfiguracaoNotificacao.corpoEmailCobrancaContribuicao;

                ViewModel.Contribuicao.emailPagamentoTitulo = ConfiguracaoNotificacao.tituloEmailPagamentoContribuicao;

                ViewModel.Contribuicao.emailPagamentoHtml = ConfiguracaoNotificacao.corpoEmailPagamentoContribuicao;

            }

            ViewModel.carregarDadosContribuicao();

            return View(ViewModel);
        }

        // POST: Contribuicoes/ContribuicaoPadrao/editar
        [HttpPost, ValidateInput(false), OrganizacaoFilter]
        public ActionResult salvar(ContribuicaoPadraoForm ViewModel) {

            if (!ModelState.IsValid) {

                if (ViewModel.Contribuicao.id > 0) {

                    var OContribuicao = this.OContribuicaoPadraoBL.carregar(ViewModel.Contribuicao.id);

                    ViewModel.Contribuicao.idPeriodoContribuicao = OContribuicao.idPeriodoContribuicao;

                    ViewModel.Contribuicao.idTipoVencimento = OContribuicao.idTipoVencimento;

                }

                return View("editar", ViewModel);
            }

            ViewModel.Contribuicao.idUsuarioCadastro = User.id();

            ViewModel.Contribuicao.idUsuarioAlteracao = User.id();

            var ORetornoValidacao = this.OContribuicaoValidacaoBL.validar(ViewModel.Contribuicao);

            if (ORetornoValidacao.flagError) {
                
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", String.Join("<br />", ORetornoValidacao.listaErros)));
            
                return View("editar", ViewModel);
                
            }

            bool flagSucesso = this.OContribuicaoPadraoBL.salvar(ViewModel.Contribuicao);

            if (flagSucesso) {
                
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "A contribuição foi salva com sucesso!");
                
                return RedirectToAction("editar", new { ViewModel.Contribuicao.id });
                
            } 
            
            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não foi possível salvar a contribuição. Tente novamente!");
            
            return View("editar", ViewModel);
            
        }


    }
}
