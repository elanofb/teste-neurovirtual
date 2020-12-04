using System;
using BLL.AssociadosContribuicoes.Emails;
using BLL.Configuracoes;
using BLL.Services;
using DAL.AssociadosContribuicoes;
using DAL.Pessoas;

namespace BLL.AssociadosContribuicoes {

    public class AssociadoContribuicaoEmailBL : DefaultBL, IAssociadoContribuicaoEmailBL {

        //Atributos
        private IAssociadoContribuicaoBL _AssociadoContribuicaoBL;

        //Propriedades
        private IAssociadoContribuicaoBL OAssociadoContribuicaoBL => _AssociadoContribuicaoBL = _AssociadoContribuicaoBL ?? new AssociadoContribuicaoBL();

        //Events

        // 1) Enviar email de cobranca
        public UtilRetorno enviarEmailCobranca(int idAssociadoContribuicao) {

            AssociadoContribuicao OAssociadoContribuicao = this.OAssociadoContribuicaoBL.carregar(idAssociadoContribuicao);

            if (OAssociadoContribuicao == null) {

                return UtilRetorno.newInstance(true, "O registro de anuidade não foi localizado.");

            }

            if (string.IsNullOrEmpty(OAssociadoContribuicao.Contribuicao.emailCobrancaHtml)) {

                return UtilRetorno.newInstance(true, "Não há cadastro do conteúdo que deve ser enviado por e-mail.");

            }

            var listaEmails = OAssociadoContribuicao.Associado.Pessoa.ToEmailList();

            if (listaEmails.Count == 0) {

                return UtilRetorno.newInstance(true, "Não existem contas de e-mails válidas para envio para esse associado.");
            }

            var listaCopias = ConfiguracaoNotificacaoBL.getInstance.carregar().emailCobrancaContribuicao.ToEmailList(";");

            IEnvioCobrancaContribuicao EnvioEmail = EnvioCobrancaContribuicao.factory(OAssociadoContribuicao.idOrganizacao, listaEmails, listaCopias);

            //bool flagEnviado = EnvioEmail.enviar(OAssociadoContribuicao);

            bool flagEnviado = true;

            if (flagEnviado) {

                return UtilRetorno.newInstance(false, "A cobrança foi enviada com sucesso.");

            }

            return UtilRetorno.newInstance(true, "Ocorreu um problema ao enviar a cobrança, por favor tente novamente.");
        }
    }
}