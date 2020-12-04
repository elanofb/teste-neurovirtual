using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Configuracoes;
using BLL.Emails;
using BLL.Notificacoes;
using BLL.Notificacoes.Services;
using DAL.Associados;
using DAL.Emails;
using DAL.Notificacoes;
using DAL.Pessoas;
using DAL.Repository.Base;
using Newtonsoft.Json;

namespace BLL.AssociadosOperacoes {
    
    public class NotificacaoAtualizacaoCadastroBL  {
        
        //Dependencias
        private IMensagemEmailConsultaBL MensagemEmailConsultaBL{ get; }
         
        public NotificacaoAtualizacaoCadastroBL(
                                                INotificacaoSistemaCadastroBL _NotificacaoSistemaCadastroBL,
                                                DataContext _db,
                                                IMensagemEmailConsultaBL _MensagemEmailConsultaBL
            ){

            MensagemEmailConsultaBL = _MensagemEmailConsultaBL;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public UtilRetorno gerarNotificacao(object Origem) {
            
            ListaAssociadosDTO OListaAssociadosDTO = (ListaAssociadosDTO)Origem;
            
            var ORetorno = UtilRetorno.newInstance(false);
            
            var OMensagem = this.MensagemEmailConsultaBL.listar(IdentificacaoMensagemEmailConst.ASSOCIADO_LINK_ATUALIZACAO).FirstOrDefault() ?? new MensagemEmail();
            
            if (OMensagem.titulo.isEmpty()) {
                OMensagem.titulo = AssociadoEmailsConst.tituloEmailAtualizacaoCadastral;
            }
            
            if (OMensagem.corpoEmail.isEmpty()){
                OMensagem.corpoEmail = AssociadoEmailsConst.corpoEmailAtualizacaoCadastral;
            }
            
            var ONotificacao = new NotificacaoSistema();
            
            ONotificacao.flagEmail = true;

            ONotificacao.flagTodosAssociados = false;

            ONotificacao.flagAssociadosEspecificos = true;

            ONotificacao.flagSistema = false;
            
            ONotificacao.flagMobile = false;
            
            ONotificacao.idTipoNotificacao = TipoNotificacaoConst.ASSOCIADO_ATUALIZACAO_CADASTRAL;
            
            ONotificacao.titulo = OMensagem.titulo;

            ONotificacao.notificacao = OMensagem.corpoEmail;
            
            ONotificacao.dtProgramacaoEnvio = DateTime.Now;
            
            var flagSucesso = false;

            if (!flagSucesso) {

                ORetorno.flagError = true;
                
                ORetorno.listaErros.Add("Houve algum problema ao gerar o e-mail de atualização cadastral. Tente novamente mais tarde.");
                
                return ORetorno;

            }
            
            
            return ORetorno;

        }


        /// <summary>
        /// 
        /// </summary>
        private void vincularDestinos(ListaAssociadosDTO OListaAssociadosDTO, NotificacaoSistema ONotificacao) {
            
            var listaAssociados = OListaAssociadosDTO.listaAssociados;
            
            var listaNotificacoesVinculadas = new List<NotificacaoSistemaEnvio>();
            
            foreach(var OAssociado in listaAssociados){

                var listaEmailsAssociado = OAssociado.Pessoa.ToEmailList();

                var listaEmail = new List<string>();
                
                listaEmail.AddRange(listaEmailsAssociado);
                                               
                foreach (var email in listaEmail) {
                    
                    var OEnvio = new NotificacaoSistemaEnvio();

                    OEnvio.idNotificacao = ONotificacao.id;

                    OEnvio.idReferencia = OAssociado.id;

                    OEnvio.nome = OAssociado.Pessoa.nome;

                    OEnvio.email = email;

               
                    string parametrosPersonalizados = this.montarParametrosPersonalizados(OAssociado, email).abreviar(8000);
                    
                    OEnvio.personalizacao = parametrosPersonalizados;
                     
                    if (!UtilValidation.isEmail(OEnvio.email)) {

                        OEnvio.flagExcluido = true;

                        OEnvio.motivoExclusao = "O e-mail configurado não é válido.";
                    }

                    listaNotificacoesVinculadas.Add(OEnvio);
                    
                }
                
            }
           
        }                
        
        /// <summary>
        /// 
        /// </summary>
        private string montarParametrosPersonalizados(Associado OAssociado, string email){
            
            string linkPreAtualizacao = ConfiguracaoLinkBaseBL.linkPreAtualizacaoAreaAssociado(OAssociado.idOrganizacao, OAssociado.id, email);
                    
            Dictionary<string, string> personalizacaoParams = new Dictionary<string, string>();
            
            personalizacaoParams.Add("#LINK_ENVIO#", linkPreAtualizacao);
            personalizacaoParams.Add("#TIPO_ASSOCIADO#", OAssociado.TipoAssociado.descricao);
            
            string infosPersonalizadas = JsonConvert.SerializeObject(personalizacaoParams, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            
            return infosPersonalizadas;
           
        }

    }
    
}