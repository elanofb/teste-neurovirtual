using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DAL.Associados;
using BLL.Email;
using DAL.CuponsDesconto;

namespace BLL.CuponsDesconto {

    public class EnvioCupom : EnvioEmailAdapter, IEnvioCupom {

        //Private Construtor
        private EnvioCupom(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) :
            base(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta) {
        }

        //Factory
        public static EnvioCupom factory(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia) {
            return new EnvioCupom(idOrganizacaoParam, listaDestino, listaCopia, null);
        }

        //Sobreposicao para designar corpo do e-mail
        //Como vamos capturar o conteudo do banco de dados, nao precisamos informar o arquivo html do email no parametro
        protected override string capturarConteudoHTML(string arquivoHTML) {

            StringBuilder conteudoHTML = new StringBuilder();

            conteudoHTML.Append("Parabéns, #NOME#! <br /><br />");

            conteudoHTML.Append("Você recebeu um  cupom de desconto:<br /><br />");

            conteudoHTML.Append("Código do Cupom: <strong>#CODIGO#</strong><br />");

            conteudoHTML.Append("Valor do desconto: <strong>#DESCONTO#</strong><br />");

            conteudoHTML.Append("#LIMITE_UTILIZACAO#");

            conteudoHTML.Append("#DATA_VENCIMENTO#");

            conteudoHTML.Append("No caso de dúvidas entre em contato conosco. <br /><br />");

            conteudoHTML.Append("Atenciosamente,<br />");

            string htmlMaster = this.capturarMasterpage("");

            string htmlFinal = htmlMaster.Replace("#CONTEUDO_MENSAGEM#", conteudoHTML.ToString());

            htmlFinal = htmlFinal.Replace("#DATA_EMAIL#", DateTime.Now.ToShortDateString());

            htmlFinal = htmlFinal.Replace("#NOME_APLICACAO#", OConfiguracaoSistema.tituloSistema);

            return htmlFinal;
        }


        //
        public UtilRetorno enviar(CupomDesconto OCupomDesconto) {

            Dictionary<string, object> infos = new Dictionary<string, object>();

            infos["nome"] = OCupomDesconto.nome;

            infos["codigo"] = OCupomDesconto.codigo;

            infos["valor"] = string.Format("{0:C}", OCupomDesconto.valorDesconto);

            infos["vencimento"] = OCupomDesconto.dtVencimento != null ? OCupomDesconto.dtVencimento.Value.ToShortDateString() : "";

            infos["qtdeUsos"] = OCupomDesconto.qtdeUsos.toInt();

            string tituloEmail = string.Format("{0} - Você recebeu um cupom de desconto", OConfiguracaoSistema.tituloSistema);

            return this.enviar(infos, tituloEmail);

        }

        //Sobreposicao obrigatorio do metodo abstrato
        public override UtilRetorno enviar(IDictionary<string, object> info, string assunto) {

            this.Subject = assunto;

            this.prepararMensagem();

            this.Body = this.Body.Replace("#NOME#", info["nome"].ToString());

            this.Body = this.Body.Replace("#CODIGO#", info["codigo"].ToString());

            this.Body = this.Body.Replace("#DESCONTO#", info["valor"].ToString());

            if (info["qtdeUsos"].toInt() > 0) {
                this.Body = this.Body.Replace("#LIMITE_UTILIZACAO#", $"Esse cupom pode ser utilizado até {info["qtdeUsos"].toInt()} vezes.<br />");
            } else {
                this.Body = this.Body.Replace("#LIMITE_UTILIZACAO#", "");
            }

            if (!info["vencimento"].stringOrEmpty().isEmpty()) {

                this.Body = this.Body.Replace("#DATA_VENCIMENTO#", $"O desconto pode ser utilizado até: <strong>{info["vencimento"]}</strong><br />");

            } else {
                this.Body = this.Body.Replace("#DATA_VENCIMENTO#", "");
            }

            return this.disparar();
        }

    }
    
}