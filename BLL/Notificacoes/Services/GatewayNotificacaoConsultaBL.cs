using System;
using System.Linq;
using BLL.Services;
using DAL.Notificacoes;

namespace BLL.Notificacoes {

    public class GatewayNotificacaoConsultaBL : DefaultBL, IGatewayNotificacaoConsultaBL {

        public const string keyCache = "gateways_notificacao";
        
        //
        public GatewayNotificacao carregar(int id) {

            var query = from NS in db.GatewayNotificacao
                        where
                            NS.id == id &&
                            !NS.dtExclusao.HasValue
                        select NS;

            return query.FirstOrDefault();
        }

        //
        public IQueryable<GatewayNotificacao> listar(string valorBusca, bool? ativo = true) {

            var query = from NS in db.GatewayNotificacao
                        where !NS.dtExclusao.HasValue
                        select NS;

            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca));
            }

            if (ativo != null) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

    }
    
}
