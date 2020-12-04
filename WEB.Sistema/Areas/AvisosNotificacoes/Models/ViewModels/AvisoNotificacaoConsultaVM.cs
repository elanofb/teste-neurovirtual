using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using DAL.Notificacoes;
using BLL.Notificacoes;
using BLL.Services;
using PagedList;
using WEB.Extensions;

namespace WEB.Areas.AvisosNotificacoes.ViewModels {
    
    public class AvisoNotificacaoConsultaVM {

        // Atributos Serviços
        private INotificacaoSistemaEnvioBL _INotificacaoSistemaEnvioBL;

        // Propriedades Serviços
        private INotificacaoSistemaConsultaBL ONotificacaoSistemaConsultaBL { get; }
        private INotificacaoSistemaEnvioBL ONotificacaoSistemaEnvioBL => _INotificacaoSistemaEnvioBL = _INotificacaoSistemaEnvioBL ?? new NotificacaoSistemaEnvioBL();

        // Propriedades
        public IPagedList<NotificacaoSistema> listaNotificacoes { get; set; }
        public List<NotificacaoSistemaEnvio> listaDestinatarios { get; set; }

        // Constantes
        private IPrincipal User => HttpContextFactory.Current.User;

        /// <summary>
        /// Construtor com inject
        /// </summary>
        public AvisoNotificacaoConsultaVM(INotificacaoSistemaConsultaBL _NotificacaoConsultaBL) {

            ONotificacaoSistemaConsultaBL = _NotificacaoConsultaBL;
            
            this.listaNotificacoes = new List<NotificacaoSistema>().ToPagedList(1, 20);
            
            this.listaDestinatarios = new List<NotificacaoSistemaEnvio>();
            
        }
        
        //
        public void carregarInformacoes() {
            
            this.montarLista();
            
            this.carregarDestinatarios();
            
        }
        
        //
        private void montarLista() {
            
            string valorBusca = UtilRequest.getString("valorBusca");
            
            string alvoEnvio = UtilRequest.getString("alvoEnvio");

            var dtProgramacaoInicio = UtilRequest.getDateTime("dtProgramacaoInicio");
            var dtProgramacaoFim = UtilRequest.getDateTime("dtProgramacaoFim");

            var query = this.ONotificacaoSistemaConsultaBL.listar(valorBusca, null).Where(x => x.idTipoNotificacao > 0);

            if (!alvoEnvio.isEmpty()) {

                if (alvoEnvio.Equals("app")) {
                    query = query.Where(x => x.flagMobile == true);
                }
                
                if (alvoEnvio.Equals("email")) {
                    query = query.Where(x => x.flagEmail == true);
                }

            }

            if (dtProgramacaoInicio.HasValue) {
                query = query.Where(x => x.dtProgramacaoEnvio >= dtProgramacaoInicio);
            }
            
            if (dtProgramacaoFim.HasValue) {
                var dtFiltro = dtProgramacaoFim.Value.AddDays(1); 
                query = query.Where(x => x.dtProgramacaoEnvio < dtFiltro);
            }

            this.listaNotificacoes = query.Select(x => new {
                                               x.id, x.titulo, x.dtProgramacaoEnvio, x.flagMobile, x.flagEmail,
                                               x.dtCadastro, x.idUsuarioCadastro,
                                               UsuarioCadastro = new { x.UsuarioCadastro.nome }
                                           }).OrderByDescending(x => x.id).ToPagedListJsonObject<NotificacaoSistema>(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

        }
        
        //
        private void carregarDestinatarios() {

            var idsNotificacoes = this.listaNotificacoes.Select(x => x.id).ToList();
            
            this.listaDestinatarios = this.ONotificacaoSistemaEnvioBL.listar(0, 0).Where(x => idsNotificacoes.Contains(x.idNotificacao))
                                          .Select(x => new { x.id, x.idNotificacao, x.flagExcluido, x.dtEnvioEmail }).ToListJsonObject<NotificacaoSistemaEnvio>();

        }

    }
}