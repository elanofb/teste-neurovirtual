using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Notificacoes;
using DAL.Notificacoes;
using PagedList;
using WEB.Areas.AvisosNotificacoes.ViewModels;
using DAL.Permissao;
using System.Collections.Generic;
using BLL.AvisosNotificacoes.Services;
using DAL.Associados;
using MvcFlashMessages;

namespace WEB.Areas.AvisosNotificacoes.Controllers {

    [OrganizacaoFilter] 
    public class NotificacaoCadastroController : Controller {

        // Atributos
        private INotificacaoSistemaEnvioBL _INotificacaoSistemaEnvioBL;
        private INotificacaoAssociadoAvulsaBL _INotificacaoAssociadoAvulsaBL;
        private ITemplateMensagemConsultaBL _ITemplateMensagemConsultaBL;

        // Propriedades
        private INotificacaoSistemaConsultaBL ONotificacaoSistemaConsultaBL { get; }
        private INotificacaoSistemaEnvioBL ONotificacaoSistemaEnvioBL => _INotificacaoSistemaEnvioBL = _INotificacaoSistemaEnvioBL ?? new NotificacaoSistemaEnvioBL();
        private INotificacaoAssociadoAvulsaBL ONotificacaoAssociadoAvulsaBL => _INotificacaoAssociadoAvulsaBL = _INotificacaoAssociadoAvulsaBL ?? new NotificacaoAssociadoAvulsaBL();
        private ITemplateMensagemConsultaBL OTemplateMensagemConsultaBL => _ITemplateMensagemConsultaBL = _ITemplateMensagemConsultaBL ?? new TemplateMensagemConsultaBL();

        /// <summary>
        /// Construtor
        /// </summary>
        public NotificacaoCadastroController(INotificacaoSistemaConsultaBL _NotificacaoConsultaBL) {

            this.ONotificacaoSistemaConsultaBL = _NotificacaoConsultaBL;
            
        }

        [HttpGet]
        public ActionResult editar(int? id) {

            var ViewModel = new AvisoNotificacaoForm();

            ViewModel.ONotificacaoSistema = this.ONotificacaoSistemaConsultaBL.carregar(UtilNumber.toInt32(id)) ??
                                            new NotificacaoSistema() { dtProgramacaoEnvio = DateTime.Today };
            
            ViewModel.carregarFlags();

            return View(ViewModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult editar(AvisoNotificacaoForm ViewModel) {

            ViewModel.carregarFlags();
            
            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            ViewModel.ONotificacaoSistema.flagAssociadosAdimplentes = ViewModel.flagAssociados == SituacaoContribuicaoConst.ADIMPLENTE;

            ViewModel.ONotificacaoSistema.flagAssociadosEspecificos = ViewModel.flagAssociados == "espec";

            ViewModel.ONotificacaoSistema.flagAssociadosInadimplentes = ViewModel.flagAssociados == SituacaoContribuicaoConst.INADIMPLENTE;

            ViewModel.ONotificacaoSistema.dtProgramacaoEnvio = ViewModel.ONotificacaoSistema.dtProgramacaoEnvio ?? DateTime.Today;

            ViewModel.preencherListaDestinatarios();

            if (!ViewModel.listaDestinatarios.Any()) {

                if (ViewModel.ONotificacaoSistema.flagAssociadosEspecificos == true) {
                    this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Informe ao menos um destinatário para a criação da notificação.");   
                }
                
                if (ViewModel.ONotificacaoSistema.flagAssociadosEspecificos != true) {
                    this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Nenhum destinatário foi encontrado com as condições informadas.");   
                }

                return View(ViewModel);
            }

            if (ViewModel.ONotificacaoSistema.idTemplate > 0) {
                var OTemplate = OTemplateMensagemConsultaBL.carregar(ViewModel.ONotificacaoSistema.idTemplate.toInt());

                ViewModel.ONotificacaoSistema.notificacao = OTemplate.corpoHTML;
                ViewModel.ONotificacaoSistema.notificacaoTexto = OTemplate.corpoTexto;
            }

            var flagSucesso = this.ONotificacaoAssociadoAvulsaBL.salvar(ViewModel.ONotificacaoSistema, ViewModel.listaDestinatarios);

            if (flagSucesso) {

                SessionNotificacoes.setListAssociadosEspecificos(new List<NotificacaoSistemaEnvio>());

                SessionNotificacoes.setListUsuariosEspecificos(new List<UsuarioSistema>());

                SessionNotificacoes.setListPerfisEspecificos(new List<PerfilAcesso>());

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "Os dados foram salvos com sucesso.");

                return RedirectToAction("editar", new { ViewModel.ONotificacaoSistema.id });

            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Houve um problema ao salvar o registro. Tente novamente.");

            return View(ViewModel);
        }
        
        /// <summary>
        /// Apresentacao dos associados vinculados na notificacao
        /// </summary>
        public ActionResult partialAssociadosNotificacao(int idNotificacao) {

            var ViewModel = new PessoasNotificadasVW();
            
            ViewModel.idNotificacao = idNotificacao;

            var query = this.ONotificacaoSistemaEnvioBL.listar(0, idNotificacao)
                            .Where(x => x.NotificacaoSistema.flagAssociadosEspecificos == true || x.NotificacaoSistema.flagTodosAssociados == true ||
                                        x.NotificacaoSistema.flagAssociadosAdimplentes == true || x.NotificacaoSistema.flagAssociadosInadimplentes == true);
            
            var valorBusca = UtilRequest.getString("valorBusca");

            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.nome.Contains(valorBusca) || x.email.Contains(valorBusca));
            }

            ViewModel.listaPessoasNotificadas = query.OrderBy(x => x.id).ToPagedList(UtilRequest.getNroPagina(), 30);

            ViewModel.qtdeEnviados = query.Count(x => x.dtEnvioEmail.HasValue);

            ViewModel.qtdeProblemaEnvio = query.Count(x => x.flagExcluido == true);

            ViewModel.qtdeFila = query.Count(x => !x.dtEnvioEmail.HasValue && x.flagExcluido == false);

            return PartialView(ViewModel);
        }

        /// <summary>
        /// Apresentacao dos associados vinculados na notificacao
        /// </summary>
        public ActionResult partialUsuariosNotificacao(int idNotificacao) {

            var ViewModel = new PessoasNotificadasVW();

            var query = this.ONotificacaoSistemaEnvioBL.listar(0, idNotificacao)
                                                    .Where(x => x.NotificacaoSistema.flagUsuariosEspecificos == true || x.NotificacaoSistema.flagTodosUsuarios == true);

            var valorBusca = UtilRequest.getString("valorBusca");

            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.nome.Contains(valorBusca) || x.email.Contains(valorBusca) );
            }

            ViewModel.idNotificacao = idNotificacao;
            
            ViewModel.listaPessoasNotificadas = query.OrderBy(x => x.id).ToPagedList(UtilRequest.getNroPagina(), 30);

            ViewModel.qtdeEnviados = query.Count(x => x.dtEnvioEmail.HasValue);

            ViewModel.qtdeProblemaEnvio = query.Count(x => x.flagExcluido == true);

            ViewModel.qtdeFila = query.Count(x => !x.dtEnvioEmail.HasValue && x.flagExcluido == null);

            return PartialView(ViewModel);
        }

    }
}