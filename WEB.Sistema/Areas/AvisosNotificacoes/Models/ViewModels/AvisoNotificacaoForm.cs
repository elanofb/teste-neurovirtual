using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using DAL.Associados;
using FluentValidation.Attributes;
using DAL.Permissao;
using DAL.Notificacoes;
using BLL.Associados;
using BLL.Notificacoes;
using BLL.Organizacoes;
using BLL.Permissao;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.AvisosNotificacoes.ViewModels {

    [Validator(typeof(AvisoNotificacaoFormValidator))]
    public class AvisoNotificacaoForm {

        // Atributos Serviços
        private IAssociadoBL _IAssociadoBL;
        private IUsuarioSistemaBL _IUsuarioSistemaBL;
        private IAcessoRecursoGrupoOrganizacaoBL _IAcessoRecursoGrupoOrganizacaoBL;
        private ITemplateMensagemConsultaBL _ITemplateMensagemConsultaBL;

        // Propriedades Serviços
        private IAssociadoBL OAssociadoBL => _IAssociadoBL = _IAssociadoBL ?? new AssociadoBL();
        private IUsuarioSistemaBL OUsuarioSistemaBL => _IUsuarioSistemaBL = _IUsuarioSistemaBL ?? new UsuarioSistemaBL();
        private IAcessoRecursoGrupoOrganizacaoBL OAcessoRecursoGrupoOrganizacaoBL => _IAcessoRecursoGrupoOrganizacaoBL = _IAcessoRecursoGrupoOrganizacaoBL ?? new AcessoRecursoGrupoOrganizacaoBL();
        private ITemplateMensagemConsultaBL OTemplateMensagemConsultaBL => _ITemplateMensagemConsultaBL = _ITemplateMensagemConsultaBL ?? new TemplateMensagemConsultaBL();

        // Propriedades
        public NotificacaoSistema ONotificacaoSistema { get; set; }

        public List<NotificacaoSistemaEnvio> listaDestinatarios { get; set; }

        public string flagAssociados { get; set; }

        public bool flagPermiteEnvioMobile { get; set; }
        
        public bool flagTemTemplates { get; set; }
        
        // Constantes
        private IPrincipal User => HttpContextFactory.Current.User;

        //
        public AvisoNotificacaoForm() {
            
            this.listaDestinatarios = new List<NotificacaoSistemaEnvio>();
        }
        
        //
        public void carregarFlags() {
            
            this.flagPermiteEnvioMobile = this.OAcessoRecursoGrupoOrganizacaoBL.listar(this.User.idOrganizacao())
                                              .Any(x => x.idRecursoGrupo == AcessoRecursoGrupoConst.APLICATIVO_ASSOCIACAO);
            
            this.flagTemTemplates = this.OTemplateMensagemConsultaBL.query(User.idOrganizacao()).Any();
            
        }

        //
        private void agruparAssociados(List<NotificacaoSistemaEnvio> listaEnvios) {

            foreach (var Item in listaEnvios) { 

                Item.idNotificacao = this.ONotificacaoSistema.id;

                if (UtilValidation.isEmail(Item.email)) {
                    this.listaDestinatarios.Add(Item);
                }

            }

        }

        private void agruparUsuarios(List<UsuarioSistema> listaUsuarios) {

            foreach (var Item in listaUsuarios) { 

                var ONotificacaoPessoa = new NotificacaoSistemaEnvio(){
                    idReferencia = Item.id,
                    idPessoa = Item.idPessoa,
                    idNotificacao = this.ONotificacaoSistema.id
                };

                this.listaDestinatarios.Add(ONotificacaoPessoa);
            }

        }
        
        public void preencherListaDestinatarios() {

            // Agrupar Associados
            if (this.ONotificacaoSistema.flagAssociadosEspecificos == true) { 

                this.agruparAssociados(SessionNotificacoes.getListAssociadosEspecificos());

                return;
            }

            var queryAssociados = this.OAssociadoBL.listar(0, "", "", this.ONotificacaoSistema.flagStatusAssociados);
            
            // Todos
            if(this.ONotificacaoSistema.flagTodosAssociados == true) {
                
                this.listaDestinatarios = this.monstaListaEnvioAssociados(queryAssociados);
            }


            // Agrupar Usuarios
            if (this.ONotificacaoSistema.flagUsuariosEspecificos == true) { 

                this.agruparUsuarios(SessionNotificacoes.getListUsuariosEspecificos());
            }

            // Agrupar Usuarios - Por Perfil
            if (this.ONotificacaoSistema.flagUsuariosEspecificos == true) {
                 
                var idsPerfis = SessionNotificacoes.getListPerfisEspecificos().Select(x => x.id).ToArray();

                var listaUsuarios = this.OUsuarioSistemaBL.listar(0, "", "").Where(x => idsPerfis.Contains(x.idPerfilAcesso)).ToList();

                this.agruparUsuarios(listaUsuarios);

            }
        }

        /// <summary>
        /// Carregar os e-mails dos associados pre-selecionados na query
        /// </summary>
        private List<NotificacaoSistemaEnvio> monstaListaEnvioAssociados(IQueryable<Associado> queryAssociados) {

            var listaEmailEnvios = new List<NotificacaoSistemaEnvio>();

            var listaAssociados = queryAssociados.Select(x => new  {
                                      x.id,
                                      x.idPessoa,
                                      x.Pessoa.nome,
                                      listaEmails = x.Pessoa.listaEmails
                                                     .Where(e => e.dtExclusao == null && !string.IsNullOrEmpty(e.email))
                                                     .Select(e => new { e.id, e.email })
                                  }).ToList().Where(x => x.listaEmails.Any(y => UtilValidation.isEmail(y.email))).ToList();

            if (!listaAssociados.Any()) {

                return listaEmailEnvios;
            }

            foreach(var OAssociado in listaAssociados){

                foreach (var OEmail in OAssociado.listaEmails) {
                    
                    var OEnvio = new NotificacaoSistemaEnvio();

                    OEnvio.idReferencia = OAssociado.id;

                    OEnvio.idPessoa = OAssociado.idPessoa;

                    OEnvio.nome = OAssociado.nome;

                    OEnvio.email = OEmail.email;

                    if (!UtilValidation.isEmail(OEnvio.email)) {

                        OEnvio.flagExcluido = true;

                        OEnvio.motivoExclusao = "O e-mail configurado não é válido.";
                    }

                    listaEmailEnvios.Add(OEnvio);
                }

            }
                    
            this.agruparAssociados(listaEmailEnvios);

            return listaEmailEnvios;

        } 

    }
}