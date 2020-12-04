using System;
using System.Linq;
using DAL.Financeiro;
using BLL.Services;

namespace BLL.Financeiro {

    public class TituloReceitaResumoBL : DefaultBL, ITituloReceitaResumoBL {

        //
        public TituloReceitaResumoBL() {
        }

        //Carregamento de registro pelo ID
        public TituloReceitaResumoVW carregar(int id) {

            var query = from P in db.TituloReceitaResumoVW
                        where P.id == id
                        select P;

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();
        }

        //Listagem de registros de acordo com filtros
        public IQueryable<TituloReceitaResumoVW> listar(string valorBusca, bool incluirExcluidos = false) {

            var query = from P in db.TituloReceitaResumoVW
                        select P;

            query = query.condicoesSeguranca();

            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricaoTitulo.Contains(valorBusca));
            }

            if (incluirExcluidos == false) {
                query = query.Where(x => x.dtExclusao == null);
            }

            return query;
        }



    }
}