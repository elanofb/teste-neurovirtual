using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Configuracoes;
using BLL.Email;
using DAL.Configuracoes;
using DAL.Pedidos;

namespace BLL.Pedidos.Emails {

    public class EnvioNovoPedido : EnvioEmailAdapter, IEnvioNovoPedido {


        //Private Construtor
        private EnvioNovoPedido(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) :
            base(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta) {
        }

        //Factory
        public static EnvioNovoPedido factory(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia) {
            return new EnvioNovoPedido(idOrganizacaoParam, listaDestino, listaCopia, null);
        }

        //Sobreposicao para designar corpo do e-mail
        //Como vamos capturar o conteudo do banco de dados, nao precisamos informar o arquivo html do email no parametro
        protected override string capturarConteudoHTML(string arquivoHTML) {

            ConfiguracaoNotificacao OConfiguracao = ConfiguracaoNotificacaoBL.getInstance.carregar(this.idOrganizacao);

            if (!OConfiguracao.emailPedidos.isEmpty()) {

                var listaEmailsCopia = OConfiguracao.emailPedidos.Split(';').Where(UtilValidation.isEmail).ToList();

                if (listaEmailsCopia.Any(x => !x.isEmpty())) {

                    listaEmailsCopia.Where(x => !x.isEmpty()).ToList().ForEach(x => { this.Bcc.Add(x); });
                }

            }

            string conteudoHTML = OConfiguracao.corpoEmailNovoPedido;

            if (conteudoHTML.isEmpty()) {
                conteudoHTML = PedidoEmailsConst.corpoEmailNovoPedido;
            }

            string htmlMaster = this.capturarMasterpage("");

            string htmlFinal = htmlMaster.Replace("#CONTEUDO_MENSAGEM#", conteudoHTML);

            htmlFinal = htmlFinal.Replace("#NOME_APLICACAO#", OConfiguracaoSistema.tituloSistema);

            return htmlFinal;

        }

        //
        public UtilRetorno enviar(Pedido OPedido) {

            ConfiguracaoNotificacao OConfiguracao = ConfiguracaoNotificacaoBL.getInstance.carregar(this.idOrganizacao);

            Dictionary<string, object> infos = new Dictionary<string, object>();

            infos["idPedido"] = OPedido.id.ToString();
            infos["nomePessoa"] = OPedido.nomePessoa;
            infos["valorPedido"] = OPedido.getValorTotal().ToString("C");

            infos["infoPgto"] = "";
            infos["infoEntrega"] = "";

            if (OPedido.TituloReceita == null) {

                infos["infoPgto"] = $"O valor total dos itens adquiridos é { infos["valorPedido"] }.";
                
                if (OPedido.listaPedidoEntrega.Any()) {
                    infos["infoEntrega"] = "O prazo para entrega iniciará a partir próximo dia útil.";
                }
                
            }

            if (OPedido.TituloReceita != null) {

                var linkPgto = UtilConfig.linkPgtoTitulo(OPedido.TituloReceita.id);
                
                infos["infoPgto"] = $"O valor total dos itens adquiridos é { infos["valorPedido"] }, caso ainda não tenha realizado o pagamento, <a href='{ linkPgto }'>clique aqui para escolher como desejar pagar</a><br><br> Caso o link não abra, copie o endereço abaixo e cole em seu navegador:<br><small>{ linkPgto }</small>";;
                
                if (OPedido.listaPedidoEntrega.Any()) {
                    infos["infoEntrega"] = "O prazo para entrega iniciará após recebermos a confirmação do pagamento.";
                }
                
            }

            string tituloEmail = OConfiguracao.tituloEmailNovoPedido;

            if (tituloEmail.isEmpty()) {
                tituloEmail = PedidoEmailsConst.tituloEmailNovoPedido;
            }

            tituloEmail = tituloEmail.Replace("#ID_PEDIDO#", infos["idPedido"].ToString());

            return this.enviar(infos, tituloEmail);

        }

        //Sobreposicao obrigatorio do metodo abstrato
        public override UtilRetorno enviar(IDictionary<string, object> info, string assunto) {

            this.Subject = assunto;

            this.prepararMensagem();

            this.Body = this.Body.Replace("#CLIENTE#", info["nomePessoa"].ToString());

            this.Body = this.Body.Replace("#ID_PEDIDO#", info["idPedido"].ToString());

            this.Body = this.Body.Replace("#INFO_PGTO#", info["infoPgto"].ToString());

            this.Body = this.Body.Replace("#INFO_ENTREGA#", info["infoEntrega"].ToString());

            return this.disparar();
        }

    }
    
}