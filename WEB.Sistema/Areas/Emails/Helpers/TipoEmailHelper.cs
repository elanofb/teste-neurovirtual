using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Caches;
using BLL.Emails;
using DAL.Emails;

namespace WEB.Areas.Emails.Helpers {

    public class TipoEmailHelper {

        //Atributos
        private static TipoEmailHelper _instance;
        private ITipoEmailBL _TipoEmailBL;

        //Propriedades
        public static TipoEmailHelper getInstance => _instance = _instance ?? new TipoEmailHelper();
        private ITipoEmailBL OTipoEmailBL => this._TipoEmailBL = this._TipoEmailBL ?? new TipoEmailBL();

        //Private Constructor
        private TipoEmailHelper() {

        }

        /// <summary>
        /// Dropdown com opcao de unidades cadastradas no sistema
        /// </summary>
        public SelectList selectList(int? selected, bool flagCache = true) {

            string keyCache = "dd_tipo_email";


            if (!flagCache) {

                var query = this.OTipoEmailBL.listar("");

                var lista = query.ToList();

                var subList = lista.Select(x => new { value = x.id, text = x.descricao }).ToList();

                return new SelectList(subList, "value", "text", selected);

            }

            var OCacheService = CacheService.getInstance;

            var listaCache = OCacheService.carregar<List<TipoEmail>>(keyCache);

            if (listaCache == null) {

                var query = this.OTipoEmailBL.listar("");

                listaCache = query.ToList();

                OCacheService.adicionar(keyCache, listaCache);
            }

            var list = listaCache.Select(x => new { value = x.id, text = x.descricao }).ToList();
            ;

            return new SelectList(list, "value", "text", selected);

        }


    }
}