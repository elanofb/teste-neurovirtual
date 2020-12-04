using System;
using System.Web.Mvc;
using System.Linq;
using BLL.Associados;
using BLL.Atendimentos;
using BLL.NaoAssociados;
using BLL.Services;
using DAL.Associados;
using DAL.Associados.DTO;
using DAL.Atendimentos;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using PagedList;
using WEB.Areas.Atendimentos.Extensions;
using WEB.Areas.Atendimentos.ViewModels;

namespace WEB.Areas.Transacoes.Controllers {

    [OrganizacaoFilter]
	public class SaqueAtendimentoDetalheController : Controller {
        
		//Atributos
		private IAtendimentoConsultaBL _AtendimentoConsultaBL;
        private IAssociadoRelatorioVWBL _AssociadoRelatorioVWBL;
        private INaoAssociadoRelatorioVWBL _NaoAssociadoRelatorioVWBL;
        private IMembroSaldoConsultaBL _SaldoConsultaBL;
        
		//Propriedades
		private IAtendimentoConsultaBL OAtendimentoConsultaBL => _AtendimentoConsultaBL = _AtendimentoConsultaBL ?? new AtendimentoConsultaBL();
        private IAssociadoRelatorioVWBL OAssociadoRelatorioVWBL => _AssociadoRelatorioVWBL = _AssociadoRelatorioVWBL ?? new AssociadoRelatorioVWBL();
        private INaoAssociadoRelatorioVWBL ONaoAssociadoRelatorioVWBL => _NaoAssociadoRelatorioVWBL = _NaoAssociadoRelatorioVWBL ?? new NaoAssociadoRelatorioVWBL();
        private IMembroSaldoConsultaBL OSaldoConsultaBL => _SaldoConsultaBL = _SaldoConsultaBL ?? new MembroSaldoConsultaBL();
        
        //
        public ActionResult detalhe(int id, string returnUrl) {
                
            var ViewModel = new AtendimentoForm();
        
            ViewModel.Atendimento = this.OAtendimentoConsultaBL.carregar(id);
            
            var idUnidade = User.idUnidade() == 0 ? null : (int?)User.idUnidade();
            
            if (ViewModel.Atendimento == null) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "O atendimento informado não foi encontrado."));

                return RedirectToAction(returnUrl);
            }

            if (!idUnidade.isEmpty() && ViewModel.Atendimento.Associado?.idUnidade != idUnidade){
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "O atendimento informado pertence a outra unidade."));
                return RedirectToAction("index", "home", new { area = ""});
            }
            
            if (ViewModel.Atendimento.idAssociado > 0) {
                
                var idAssociado = ViewModel.Atendimento.idAssociado.toInt();

                ViewModel.AssociadoVinculado = this.OAssociadoRelatorioVWBL.listar(0, "", "", "").Where(x => x.id == idAssociado)
                                                   .Select(x => new ItemListaAssociado {
                                                        id = x.id, nroAssociado = x.nroAssociado,
                                                        descricaoTipoAssociado = x.descricaoTipoAssociado,
                                                        flagTipoPessoa = x.flagTipoPessoa, nome = x.nome,
                                                        razaoSocial = x.razaoSocial, nroDocumento = x.nroDocumento,
                                                        dtCadastro = x.dtCadastro, ativo = x.ativo
                                                        //flagSituacaoContribuicao = x.flagSituacaoContribuicao 
                                                        }).FirstOrDefault();

            }

            if (ViewModel.Atendimento.idNaoAssociado > 0) {

                var idNaoAssociado = ViewModel.Atendimento.idNaoAssociado.toInt();
                
                ViewModel.AssociadoVinculado = this.ONaoAssociadoRelatorioVWBL.listar(0, "", "", "").Where(x => x.id == idNaoAssociado)
                                                   .Select(x => new ItemListaAssociado {
                                                        id = x.id, nroAssociado = x.nroAssociado,
                                                        descricaoTipoAssociado = x.descricaoTipoAssociado,
                                                        flagTipoPessoa = x.flagTipoPessoa, nome = x.nome,
                                                        razaoSocial = x.razaoSocial, nroDocumento = x.nroDocumento,
                                                        dtCadastro = x.dtCadastro, ativo = x.ativo}).FirstOrDefault();

            }

            int idMembro = ViewModel.Atendimento.idNaoAssociado > 0 ? ViewModel.Atendimento.idNaoAssociado.toInt() : ViewModel.Atendimento.idAssociado.toInt();
            
            MembroSaldo Saldo = this.OSaldoConsultaBL.query(idMembro)
                                    .Select(x => new { x.id, x.saldoAtual, x.dtAtualizacaoSaldo})
                                    .FirstOrDefault()
                                    .ToJsonObject<MembroSaldo>() ?? new MembroSaldo();

            ViewBag.Saldo = Saldo;
            
            return View(ViewModel);

        }

        //
        [ActionName("partial-botoes-acao")]
        public PartialViewResult partialBotoesAcao(int id) {

            var OAtendimento = this.OAtendimentoConsultaBL.carregar(id);

            return PartialView(OAtendimento);

        }

        #region NONACTIONS

        [NonAction]
        private IQueryable<Atendimento> completarQuery(IQueryable<Atendimento> query) {

            var idUsuario = UtilRequest.getInt32("idUsuario");

            var idStatus = UtilRequest.getInt32("idStatus");

            var dtAberturaInicio = UtilRequest.getDateTime("dtAberturaInicio");
            var dtAberturaFim = UtilRequest.getDateTime("dtAberturaFim");

            var dtFinalizacaoInicio = UtilRequest.getDateTime("dtFinalizacaoInicio");
            var dtFinalizacaoFim = UtilRequest.getDateTime("dtFinalizacaoFim");

            var flagAtendido = UtilRequest.getBool("flagAtendido");

            var valorBusca = UtilRequest.getString("valorBusca");

            var idUnidade = User.idUnidade() == 0 ? null : (int?)User.idUnidade();

            if (idUnidade > 0) {
                query = query.Where(x => x.Associado.idUnidade == idUnidade);
            }

            if (idUsuario > 0) {
                query = query.Where(x => x.idUltimoUsuarioAtendimento == idUsuario);
            }

            if (idStatus > 0) {
                query = query.Where(x => x.idStatusAtendimento == idStatus);
            }

            if (dtAberturaInicio.HasValue) {
                query = query.Where(x => x.dtInicioAtendimento >= dtAberturaInicio);
            }

            if (dtAberturaFim.HasValue) {
                var dtFiltro = dtAberturaFim.Value.AddDays(1);

                query = query.Where(x => x.dtInicioAtendimento < dtFiltro);
            }

            if (dtFinalizacaoInicio.HasValue) {
                query = query.Where(x => x.dtFinalizacaoAtendimento >= dtFinalizacaoInicio);
            }

            if (dtFinalizacaoFim.HasValue) {
                var dtFiltro = dtFinalizacaoFim.Value.AddDays(1);

                query = query.Where(x => x.dtFinalizacaoAtendimento < dtFiltro);
            }

            if (flagAtendido != null) {

                query = query.Where(x => x.flagAtendido == flagAtendido);
            }

            if (!valorBusca.isEmpty()) {

                query = query.Where(x => x.nome.Contains(valorBusca) || x.email.Contains(valorBusca) || x.mensagem.Contains(valorBusca));

            }

            return query;

        }

        #endregion

    }
}