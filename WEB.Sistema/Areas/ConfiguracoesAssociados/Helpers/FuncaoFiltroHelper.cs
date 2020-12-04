using System;
using System.Linq;
using System.Web.Mvc;
using BLL.ConfiguracoesAssociados;
using EntityFramework.Caching;
using EntityFramework.Extensions;

namespace WEB.Areas.ConfiguracoesAssociados.Helpers {

    public class FuncaoFiltroHelper {

        //Atributos
        private static FuncaoFiltroHelper _instance;
        private IFuncaoFiltroBL _FuncaoFiltroBL;

        //Propriedades
        public static FuncaoFiltroHelper getInstance => _instance = _instance ?? new FuncaoFiltroHelper();
        private IFuncaoFiltroBL OFuncaoFiltroBL => this._FuncaoFiltroBL = this._FuncaoFiltroBL ?? new FuncaoFiltroBL();

        //Private Constructor
        private FuncaoFiltroHelper() {

        }

        //
        public SelectList selectList(int? selected) {
            var query = this.OFuncaoFiltroBL.listar("", true)
                                    .FromCache(CachePolicy.WithDurationExpiration(TimeSpan.FromMinutes(20)))
                                    .ToList();

            var list = query.Select(x => new { text = x.nomeFuncao, id = x.id }).OrderBy(x => x.text);

            return new SelectList(list, "id", "text", selected);
        }
    }
}