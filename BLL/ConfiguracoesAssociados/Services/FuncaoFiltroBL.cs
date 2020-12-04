using System;
using System.Linq;
using BLL.Services;
using DAL.ConfiguracoesAssociados;

namespace BLL.ConfiguracoesAssociados {

    public class FuncaoFiltroBL : DefaultBL, IFuncaoFiltroBL {

        //
        public FuncaoFiltroBL() {

        }

        //Listagem de registro a partir de parametros
        public IQueryable<FuncaoFiltro> listar(string valorBusca, bool? ativo) {

            var query = from FuncF in db.FuncaoFiltro
                        select FuncF;

            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.nomeFuncao.Contains(valorBusca));
            }

            if (ativo.HasValue) {
                query = query.Where(x => x.ativo == ativo);
            }
            return query;
        }
    }
}