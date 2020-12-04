using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using BLL.Caches;
using BLL.Tipos;
using DAL.Tipos;

namespace WEB.Areas.AssociadosDependentes.Helpers {

    public class TipoDependenteHelper{

        private static TipoDependenteHelper _instance;
        private ITipoDependenteBL _TipoDependenteBL;

        public static TipoDependenteHelper getInstance => _instance = _instance ?? new TipoDependenteHelper();
        private ITipoDependenteBL OTipoDependenteBL => _TipoDependenteBL = _TipoDependenteBL ?? new TipoDependenteBL();


        //Carregar combo de selecao dos tipos de associados
        public SelectList selectList(int? selected, bool flagCache = true) {

            string keyCache = "tipo_dependente";


            if (!flagCache) {

                var query = this.OTipoDependenteBL.listar("");

                var lista = query.ToList();

                var subList = lista.Select(x => new { value = x.id, text = x.descricao }).OrderBy(x => x.text).ToList();

                return new SelectList(subList, "value", "text", selected);

            }

            var OCacheService = CacheService.getInstance;

            var listaCache = OCacheService.carregar<List<TipoDependente>>(keyCache);

            if (listaCache == null) {

                var query = this.OTipoDependenteBL.listar("");

                listaCache = query.ToList();

                OCacheService.adicionar(keyCache, listaCache);
            }

            var list = listaCache.Select(x => new { value = x.id, text = x.descricao }).OrderBy(x => x.text).ToList();
            return new SelectList(list, "value", "text", selected);

        }
    }
}