using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using BLL.AssociadosOperacoes.Events;
using BLL.Configuracoes;
using DAL.Configuracoes;
using BLL.Core.Events;
using BLL.Emails;
using DAL.Emails;

namespace BLL.Email {

    public abstract class EnvioEmailAdapter : MailMessage {

        //Atributos Serviços
        
        
        //Constantes
        private readonly string basePathEmailFiles = "Areas/Emails/files/emails/";

        //Atributos
        protected IMensagemEmailConsultaBL _MensagemEmailConsultaBL;

        //Propriedades Serviços
        protected IMensagemEmailConsultaBL OMensagemEmailConsultaBL => this._MensagemEmailConsultaBL = this._MensagemEmailConsultaBL ?? new MensagemEmailConsultaBL();

        //Propriedades
        protected  int idOrganizacao { get; set; }
        protected ConfiguracaoSistema OConfiguracaoSistema { get; set; }
        protected ConfiguracaoEmail OConfiguracaoEmail { get; set; }
        protected ConfiguracaoNotificacao OConfiguracaoNotificacao { get; set; }
        protected MensagemEmail MensagemEmail { get; set; }        

        //Events
        private readonly EventAggregator onEmailEnviado = OnEmailEnviado.getInstance;

        //Construtor
        protected EnvioEmailAdapter() {
        }

        //Construtor
        protected EnvioEmailAdapter(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) {

            if (listaDestino != null && listaDestino.Any(x => !x.isEmpty())) {
                listaDestino.Where(x => !x.isEmpty()).ToList().ForEach(x => { this.To.Add(x); });
            }
            
            if (listaCopia != null && listaCopia.Any(x => !x.isEmpty())) {
                listaCopia.Where(x => !x.isEmpty()).ToList().ForEach(x => { this.CC.Add(x); });
            }

            if (listaCopiaOculta != null && listaCopiaOculta.Any(x => !x.isEmpty())) {
                listaCopiaOculta.Where(x => !x.isEmpty()).ToList().ForEach(x => { this.Bcc.Add(x); });
            }

            this.idOrganizacao = idOrganizacaoParam;

            this.OConfiguracaoSistema = ConfiguracaoSistemaBL.getInstance.carregar(idOrganizacao);

            this.OConfiguracaoEmail = ConfiguracaoEmailBL.getInstance.carregar(this.idOrganizacao);

            this.OConfiguracaoNotificacao = ConfiguracaoNotificacaoBL.getInstance.carregar(this.idOrganizacao);
        }

        //Implementado pelas classes filhas
        public abstract UtilRetorno enviar(IDictionary<string, object> infosEmail, string assunto);

        //Capturar a estrutura HTML da masterpage dos e-mails
        protected virtual string capturarMasterpage(string assunto) {

            string body = OConfiguracaoEmail.masterpageEmail;

            body = body.Replace("#NOME_APLICACAO#", OConfiguracaoSistema.tituloSistema);

            body = body.Replace("#ASSINATURA_ENVIO#", OConfiguracaoEmail.assinaturaEmail);

            body = body.Replace("#URL_LOGO#", ConfiguracaoImagemBL.linkImagemOrganizacao(idOrganizacao, ConfiguracaoImagemBL.IMAGEM_EMAIL_SISTEMA));

            body = body.Replace("#DOMINIO#", HttpContextFactory.Current.Request.Url?.Host);

            body = body.Replace("#ASSINATURA_ENVIO#", OConfiguracaoEmail.assinaturaEmail);

            body = body.Replace("#ASSUNTO#", String.IsNullOrEmpty(assunto) ? this.Subject : assunto);

            return body;
        }

        //Capturar conteúdo HTML
        protected virtual string capturarConteudoHTML(string arquivoHTML) {

            string pathArquivo = Path.Combine(UtilConfig.pathAbsRaiz, String.Concat(basePathEmailFiles, arquivoHTML));

            string conteudoHTML;

            using (StreamReader reader = new StreamReader(pathArquivo, Encoding.GetEncoding("iso8859-1"))) {
                conteudoHTML = reader.ReadToEnd();
            }

            string htmlMaster = this.capturarMasterpage(this.Subject);

            string htmlFinal = htmlMaster.Replace("#CONTEUDO_MENSAGEM#", conteudoHTML);

            htmlFinal = htmlFinal.Replace("#URL_LOGO#",ConfiguracaoImagemBL.linkImagemOrganizacao(idOrganizacao, ConfiguracaoImagemBL.IMAGEM_EMAIL_SISTEMA));

            htmlFinal = htmlFinal.Replace("#DOMINIO#", HttpContextFactory.Current.Request.Url?.Host);

            htmlFinal = htmlFinal.Replace("#NOME_APLICACAO#", OConfiguracaoSistema.tituloSistema);

            return htmlFinal;
        }

        //Perarar os dados e validar as mensagens
        protected virtual void prepararMensagem(string nomeArquivoConteudo = "") {

            foreach (MailAddress email in this.To) {
                if (!UtilValidation.isEmail(email.Address))
                    this.To.Remove(email);
            }

            foreach (MailAddress email in this.CC) {
                if (!UtilValidation.isEmail(email.Address))
                    this.CC.Remove(email);
            }

            foreach (MailAddress email in this.Bcc) {
                if (!UtilValidation.isEmail(email.Address))
                    this.Bcc.Remove(email);
            }

            if (!UtilConfig.emProducao()) {

                this.To.Clear();

                this.CC.Clear();

                this.Bcc.Clear();

                if (UtilValidation.isEmail(OConfiguracaoEmail.emailResposta)) {

                    this.To.Add(OConfiguracaoEmail.emailResposta);
                    
                }

            }

            this.Body = this.capturarConteudoHTML(nomeArquivoConteudo);

            this.BodyEncoding = Encoding.GetEncoding("iso8859-1");

            this.IsBodyHtml = true;
            
        }

        // 1 - Disparo de e-mails
        // 2 - Publicar o evento
        public UtilRetorno disparar() {

            var ORetorno = UtilRetorno.newInstance(false);
            
            if (this.To.Count == 0) {

                ORetorno.flagError = true;
                
                ORetorno.listaErros.Add("Não é possível enviar e-mail sem nenhum destino informado.");

                return ORetorno;

            }
            
            this.Subject = this.Subject.Replace("#NOME_ORGANIZACAO#", OConfiguracaoSistema.nomeEmpresaResumo);

            this.Body = this.Body.Replace("#NOME_APLICACAO#", OConfiguracaoSistema.tituloSistema);

            this.Body = this.Body.Replace("#NOME_ORGANIZACAO#", OConfiguracaoSistema.nomeEmpresaResumo);

            this.Body = this.Body.Replace("#DATA_EMAIL#", DateTime.Now.ToLongDateString());

		    this.Body = this.Body.Replace("#ASSUNTO#", this.Subject);
            
            if (!UtilValidation.isEmail(OConfiguracaoEmail.contaEmailSistema)) {

                ORetorno.flagError = true;
                
                ORetorno.listaErros.Add("O e-mail de envio ainda não foi configurado para o sistema ou é inválido.");

                return ORetorno;

            }
            
            this.From = new MailAddress(OConfiguracaoEmail.contaEmailSistema, OConfiguracaoSistema.tituloSistema);

            this.Priority = MailPriority.High;

            SmtpClient Smtp = new SmtpClient();

            try {

                Smtp.Timeout = 20000;
                
                Smtp.Host = OConfiguracaoEmail.servidorSMTPEmailSistema;

                Smtp.Port = Convert.ToInt32(OConfiguracaoEmail.portaSMTPEmailSistema);

                Smtp.UseDefaultCredentials = false;

                Smtp.EnableSsl = OConfiguracaoEmail.flagSSLSMTPEmailSistema == "S";

                NetworkCredential credenciais = new NetworkCredential(OConfiguracaoEmail.contaEmailSistema, OConfiguracaoEmail.senhaEmailSistema);

                Smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                Smtp.Credentials = credenciais;
                
                Smtp.Send(this);

                Smtp.Dispose();

                this.publicarEventoEmailEnviado();

            } catch (SmtpException smtpEx) {

                ORetorno.flagError = true;
                
                ORetorno.listaErros.Add($"Erro SMTP ao disparar e-mail de: { OConfiguracaoEmail.contaEmailSistema } em: { OConfiguracaoSistema.tituloSistema }. Code: {smtpEx.StatusCode.toInt()}. Message: {smtpEx.Message}");

                return ORetorno;

            } catch (Exception ex) {

                UtilLog.saveError(ex, $"Erro Generico ao disparar e-mail de: {OConfiguracaoEmail.contaEmailSistema} em: {OConfiguracaoSistema.tituloSistema}");

                ORetorno.flagError = true;
                
                ORetorno.listaErros.Add($"Erro Generico ao disparar e-mail de: {OConfiguracaoEmail.contaEmailSistema} em: {OConfiguracaoSistema.tituloSistema}");

                return ORetorno;

            } finally {

                Smtp.Dispose();

            }

            Dispose();
            
            return ORetorno;
            
        }

        //Publicar o evento de email enviado
        private void publicarEventoEmailEnviado() {

            //onEmailEnviado.subscribe(new EmailEnviadoHandler());

            //var listaEmailsTo = this.To.Select(m => m.Address).ToList();
            //var listaEmailsCC = this.CC.Select(m => m.Address).ToList();
            //var listaEmailsBCC = this.Bcc.Select(m => m.Address).ToList();
            //var listaEmails = listaEmailsTo;

            //if (listaEmailsCC.Count > 0)
            //    listaEmails.AddRange(listaEmailsCC);

            //if (listaEmailsBCC.Count > 0)
            //    listaEmails.AddRange(listaEmailsBCC);

            //List<object> InfoEvento = new List<object>(2);

            //InfoEvento.Add(listaEmails as object);
            //InfoEvento.Add(this.Subject as object);
            //InfoEvento.Add(this.Body as object);

            //this.onEmailEnviado.publish(InfoEvento as object);
        }

    }
    
}