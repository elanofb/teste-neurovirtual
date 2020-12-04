using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Runtime.InteropServices.WindowsRuntime;
using BLL.ComissoesAvaliacao;
using BLL.Configuracoes;
using BLL.Core.Events;
using BLL.Emails;
using BLL.Notificacoes;
using BLL.Notificacoes.Interface;
using BLL.Notificacoes.Services;
using BLL.ProcessosAvaliacoes;
using BLL.Services;
using BLL.Tarefas;
using DAL.Associados;
using DAL.Configuracoes;
using DAL.Emails;
using DAL.Eventos;
using DAL.Notificacoes;
using DAL.Pessoas;
using DAL.Repository.Base;
using DAL.Tarefas;
using Newtonsoft.Json;

namespace BLL.AssociadosOperacoes {
    
    public class NotificacaoLinkCertificadoBL : GeradorNotificacaoBaseBL {
        
        //Dependencias
        private IMensagemEmailConsultaBL MensagemEmailConsultaBL{ get; }
         
        public NotificacaoLinkCertificadoBL(
                                                INotificacaoSistemaCadastroBL _NotificacaoSistemaCadastroBL,
                                                ITarefaSistemaConsultaBL _TarefaSistemaBL,
                                                ITarefaSistemaCadastroBL _TarefaCadastroBL,
                                                DataContext _db,
                                                IMensagemEmailConsultaBL _MensagemEmailConsultaBL
            ){

            this.NotificacaoSistemaCadastroBL = _NotificacaoSistemaCadastroBL;
            
            this.TarefaSistemaBL = _TarefaSistemaBL;

            this.TarefaCadastroBL = _TarefaCadastroBL;
            
            this.db = _db;

            MensagemEmailConsultaBL = _MensagemEmailConsultaBL;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override UtilRetorno gerarNotificacao(object Origem) {
            
            List<EventoInscricao> listaInscricoes = (List<EventoInscricao>)Origem;
            
            var ORetorno = UtilRetorno.newInstance(false);

            var idEvento = listaInscricoes.FirstOrDefault()?.idEvento;
            var tituloEvento = listaInscricoes.FirstOrDefault()?.Evento.titulo;

            var OMensagem = this.MensagemEmailConsultaBL.listar(IdentificacaoMensagemEmailConst.EVENTO_ENVIO_CERTIFICADO).FirstOrDefault(x => x.idReferencia == idEvento) ?? new MensagemEmail();
            
            if (OMensagem.corpoEmail.isEmpty()){
                ConfiguracaoNotificacao OConfiguracao = ConfiguracaoNotificacaoBL.getInstance.carregar();
                
                OMensagem.corpoEmail = OConfiguracao.corpoEmailEnvioCerficadoEvento;
            }
            
            if (OMensagem.titulo.isEmpty()) {
                OMensagem.titulo = EventoEmailsConst.tituloEmailEnvioCertificado;
            }
            
            if (OMensagem.corpoEmail.isEmpty()){
                OMensagem.corpoEmail = EventoEmailsConst.corpoEmailEnvioCertificado;
            }
            
            var ONotificacao = new NotificacaoSistema();
            
            ONotificacao.flagEmail = true;

            ONotificacao.flagTodosAssociados = false;

            ONotificacao.flagAssociadosEspecificos = true;

            ONotificacao.flagSistema = false;
            
            ONotificacao.flagMobile = false;
            
            ONotificacao.idTipoNotificacao = TipoNotificacaoConst.CERTIFICADO_EVENTO;
            
            ONotificacao.titulo = OMensagem.titulo;

            ONotificacao.notificacao = OMensagem.corpoEmail;
            
            ONotificacao.dtProgramacaoEnvio = DateTime.Now;

            ONotificacao.titulo = ONotificacao.titulo.Replace("#TITULO_EVENTO#", tituloEvento);
            
            var flagSucesso = this.NotificacaoSistemaCadastroBL.salvar(ONotificacao);

            if (!flagSucesso) {

                ORetorno.flagError = true;
                
                ORetorno.listaErros.Add("Houve algum problema ao gerar o e-mail de envio de certificado. Tente novamente mais tarde.");
                
                return ORetorno;

            }
            
            TarefaSistema OTarefaSistema = this.gerarTarefa(ONotificacao);
            
            this.vincularDestinos(listaInscricoes, ONotificacao, OTarefaSistema);
            
            return ORetorno;

        }


        /// <summary>
        /// 
        /// </summary>
        private void vincularDestinos(List<EventoInscricao> listaInscricoes, NotificacaoSistema ONotificacao, TarefaSistema OTarefaSistema) {
            
            var listaNotificacoesVinculadas = new List<NotificacaoSistemaEnvio>();

            foreach (var OInscricao in listaInscricoes) {
                
                var listaEmail = new List<string>();
            
                listaEmail.Add(OInscricao.emailPrincipal);
                listaEmail.Add(OInscricao.emailSecundario);
                                           
                foreach (var email in listaEmail) {
                
                    var OEnvio = new NotificacaoSistemaEnvio();

                    OEnvio.idNotificacao = ONotificacao.id;

                    OEnvio.idReferencia = OInscricao.id;

                    OEnvio.nome = OInscricao.nomeInscrito;

                    OEnvio.email = email;

                    OEnvio.idTarefa = OTarefaSistema?.id; 
            
                    string parametrosPersonalizados = this.montarParametrosPersonalizados(OInscricao).abreviar(8000);
                
                    OEnvio.personalizacao = parametrosPersonalizados;
                 
                    if (!UtilValidation.isEmail(OEnvio.email)) {

                        OEnvio.flagExcluido = true;

                        OEnvio.motivoExclusao = "O e-mail configurado não é válido.";
                    }

                    listaNotificacoesVinculadas.Add(OEnvio);
                
                }
                
            }
            
            this.NotificacaoSistemaCadastroBL.salvarDetalhesNotificacao(listaNotificacoesVinculadas);
           
        }                
        
        /// <summary>
        /// 
        /// </summary>
        private string montarParametrosPersonalizados(EventoInscricao OInscricao){
            
            var i = UtilCrypt.toBase64Encode(OInscricao.id.toInt());

            var icr = UtilCrypt.SHA512(OInscricao.id.toInt().ToString());
            
            string linkCertificado = ConfiguracaoLinkBaseBL.linkAreaAssociado(OInscricao.idOrganizacao, $"EventosPro/EventoCertificadoAcesso/index?i={i}&icr={icr}");
                
            Dictionary<string, string> personalizacaoParams = new Dictionary<string, string>();
            
            personalizacaoParams.Add("#LINK_CERTIFICADO#", linkCertificado);
            personalizacaoParams.Add("#TITULO_EVENTO#", OInscricao.Evento.titulo);
            personalizacaoParams.Add("#NOME_INSCRITO#", OInscricao.nomeInscrito);
            
            string infosPersonalizadas = JsonConvert.SerializeObject(personalizacaoParams, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            
            return infosPersonalizadas;
           
        }

    }
    
}