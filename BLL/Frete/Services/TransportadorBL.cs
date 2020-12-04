using System;
using System.Linq;
using DAL.Frete;
using BLL.Services;

namespace BLL.Frete {


    public class TransportadorBL : DefaultBL, ITransportadorBL{
        
        //
        public TransportadorBL() {

        }


        //Carregar registro a partir do ID
        public Transportador carregar(int id)
        {

            var query = from T in db.Transportador
                where
                T.id == id &&
                T.flagExcluido == "N"
                select
                T;

            var Retorno = query.FirstOrDefault();

            return Retorno;
        }

        //Listagem 
        public IQueryable<Transportador> listar(string valorBusca, string ativo)
        {

            var query = from Tip in db.Transportador
                        where
                Tip.flagExcluido == "N"
                select
                Tip;

            if (!String.IsNullOrEmpty(valorBusca))
            {
                query = query.Where(x => x.descricao.Contains(valorBusca));
            }

            if (!String.IsNullOrEmpty(ativo))
            {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

    }
}
