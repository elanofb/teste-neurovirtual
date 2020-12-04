using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Configuracoes;
using BLL.Email;
using DAL.Configuracoes;
using DAL.Pedidos;

namespace BLL.Pedidos.Emails {

    public class EnvioCobrancaPedido : EnvioEmailAdapter, IEnvioCobrancaPedido {


        //Private Construtor
        private EnvioCobrancaPedido(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) :
            base(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta) {
        }

        //Factory
        public static EnvioCobrancaPedido factory(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia) {
            return new EnvioCobrancaPedido(idOrganizacaoParam, listaDestino, listaCopia, null);
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

            string conteudoHTML = OConfiguracao.corpoEmailCobrancaPedido;

            string htmlMaster = this.capturarMasterpage("");

            string htmlFinal = htmlMaster.Replace("#CONTEUDO_MENSAGEM#", conteudoHTML);

            htmlFinal = htmlFinal.Replace("#NOME_APLICACAO#", OConfiguracaoSistema.tituloSistema);

            return htmlFinal;
        }

        //
        public UtilRetorno enviar(Pedido OPedido) {

            Dictionary<string, object> infos = new Dictionary<string, object>();

            infos["idPedido"] = OPedido.id.ToString();

            infos["linkPgto"] = UtilConfig.linkPgtoTitulo(OPedido.TituloReceita.id);

            infos["nomePessoa"] = OPedido.nomePessoa;

            string tituloEmail = string.Format("{0} - Pedido {1} recebido", OConfiguracaoSistema.tituloSistema, infos["idPedido"]);

            return this.enviar(infos, tituloEmail);
        }


        //Sobreposicao obrigatorio do metodo abstrato
        public override UtilRetorno enviar(IDictionary<string, object> info, string assunto) {

            this.Subject = assunto;

            this.prepararMensagem();

            this.Body = this.Body.Replace("#NOME_PESSOA#", info["nomePessoa"].ToString());

            this.Body = this.Body.Replace("#ID_PEDIDO#", info["idPedido"].ToString());

            this.Body = this.Body.Replace("#LINK_PGTO#", info["linkPgto"].ToString());

            return this.disparar();
        }


    }
}