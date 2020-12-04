using System;
using System.Web.Mvc;
using System.Linq;
using BLL.Associados;
using BLL.Atendimentos;
using BLL.NaoAssociados;
using DAL.Associados.DTO;
using DAL.Atendimentos;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using PagedList;
using WEB.Areas.Atendimentos.Extensions;
using WEB.Areas.Atendimentos.ViewModels;

namespace WEB.Areas.Transacoes.Controllers {

    [OrganizacaoFilter]
	public class SaquesRealizadosController : Controller {
        
		//Atributos
		private IAtendimentoConsultaBL _AtendimentoConsultaBL;
        private IAssociadoRelatorioVWBL _AssociadoRelatorioVWBL;
        private INaoAssociadoRelatorioVWBL _NaoAssociadoRelatorioVWBL;
        
		//Propriedades
		private IAtendimentoConsultaBL OAtendimentoConsultaBL => _AtendimentoConsultaBL = _AtendimentoConsultaBL ?? new AtendimentoConsultaBL();
        private IAssociadoRelatorioVWBL OAssociadoRelatorioVWBL => _AssociadoRelatorioVWBL = _AssociadoRelatorioVWBL ?? new AssociadoRelatorioVWBL();
        private INaoAssociadoRelatorioVWBL ONaoAssociadoRelatorioVWBL => _NaoAssociadoRelatorioVWBL = _NaoAssociadoRelatorioVWBL ?? new NaoAssociadoRelatorioVWBL();
        
        //
        [ActionName("index")]
		public ActionResult index() {
            
            if (User.idAtendimentoAberto() > 0) {
                        
                return RedirectToAction("detalhe", "SaqueAtendimentoDetalhe", new { id = User.idAtendimentoAberto() });

            }
            
            var queryAtendimentos = this.OAtendimentoConsultaBL.listar(true)
                                        .Where(x => x.idStatusAtendimento == AtendimentoStatusConst.FINALIZADO && x.idTipoAtendimento == 0);
            
            queryAtendimentos = this.completarQuery(queryAtendimentos);
            
            queryAtendimentos = queryAtendimentos.OrderBy(x => x.id);
            
            return View(queryAtendimentos.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));

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