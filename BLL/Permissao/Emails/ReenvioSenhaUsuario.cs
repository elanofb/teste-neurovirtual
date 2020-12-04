using System;
using System.Collections.Generic;
using BLL.Configuracoes;
using BLL.Email;
using DAL.Permissao;
using DAL.Configuracoes.Default;

namespace BLL.Permissao.Emails {

    public class ReenvioSenhaUsuario : EnvioEmailAdapter, IEnvioNovoUsuario {

        //Constantes

        //Private Construtor
        private ReenvioSenhaUsuario(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) :
            base(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta) {
        }

        //Factory
        public static ReenvioSenhaUsuario factory(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia) {
            return new ReenvioSenhaUsuario(idOrganizacaoParam, listaDestino, listaCopia, null);
        }


        //
        public UtilRetorno enviar(UsuarioSistema OUsuarioSistema, string senhaProvisoria) {

            var OConfig = ConfiguracaoNotificacaoBL.getInstance.carregar(this.idOrganizacao);
            
            Dictionary<string, object> infos = new Dictionary<string, object>();

            infos["nome"] = OUsuarioSistema.nome;

            infos["login"] = OUsuarioSistema.login;

            infos["senha"] = senhaProvisoria;

            string tituloEmail = OConfig.reenvioSenhaUsuarioTitulo;

            return this.enviar(infos, tituloEmail);
        }


        //Sobreposicao obrigatorio do metodo abstrato
        public override UtilRetorno enviar(IDictionary<string, object> info, string assunto) {

            this.Subject = assunto;

            this.prepararMensagem();

            this.Body = this.Body.Replace("#NOME#", info["nome"].ToString());

            this.Body = this.Body.Replace("#LOGIN#", info["login"].ToString());

            this.Body = this.Body.Replace("#SENHA#", info["senha"].ToString());

            return this.disparar();
        }

        protected override string capturarConteudoHTML(string arquivoHTML) {

            string conteudoHTML = ConfiguracaoNotificacaoBL.getInstance.carregar(this.idOrganizacao).reenvioSenhaUsuarioCorpo;

            string htmlMaster = this.capturarMasterpage("");

            string htmlFinal = htmlMaster.Replace("#CONTEUDO_MENSAGEM#", conteudoHTML);

            return htmlFinal;
        }

    }
}