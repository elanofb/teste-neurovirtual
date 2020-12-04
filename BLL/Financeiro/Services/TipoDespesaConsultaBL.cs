using System;
using System.Linq;
using DAL.Financeiro;
using BLL.Services;

namespace BLL.Financeiro {

    public class TipoDespesaConsultaBL: DefaultBL, ITipoDespesaConsultaBL {

        public const string keyCache = "tipo_despesa";

        //
        public TipoDespesaConsultaBL() {
        }

        // 
        public IQueryable<TipoDespesa> query() {

            var query = from Obj in db.TipoDespesa
                where Obj.flagExcluido == false
                select Obj;

            return query;

        }

        //Carregamento de registro pelo ID
        public TipoDespesa carregar(int id) {

            var query = this.query().condicoesSeguranca();

            return query.FirstOrDefault(x => x.id == id);

        }

        //Listagem de registros de acordo com filtros
        public IQueryable<TipoDespesa> listar() {

            var query = this.query().condicoesSeguranca();

            return query;
        }

    }
}