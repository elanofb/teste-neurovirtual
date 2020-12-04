using BLL.AssociadosNotificacoes;
using DAL.Notificacoes;

namespace BLL.Notificacoes {

    public class NotificacaoSistemaEnvioCobrancaBL {

        public static INotificacaoSistemaEnvioCobrancaBL factory(NotificacaoSistema ONotificacao) {
            

            if (ONotificacao.idTipoNotificacao == TipoNotificacaoConst.COBRANCA_CONTRIBUICAO) {

                return new NotificacaoSistemaEnvioCobrancaContribuicaoBL();

            }

            if (ONotificacao.idTipoNotificacao == TipoNotificacaoConst.COBRANCA_PARCELA) {


            }

            return null;

        }

    }

}
