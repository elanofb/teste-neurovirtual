using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Caches;
using BLL.Telefones;
using DAL.Telefones;

namespace WEB.Areas.Telefones.Helpers {

    public class OperadoraTelefoniaHelper {

        //Atributos
        private static OperadoraTelefoniaHelper _instance;
        private IOperadoraTelefoniaBL _OperadoraTelefoniaBL;

        //Propriedades
        public static OperadoraTelefoniaHelper getInstance => _instance = _instance ?? new OperadoraTelefoniaHelper();
        private IOperadoraTelefoniaBL OOperadoraTelefoniaBL => this._OperadoraTelefoniaBL = this._OperadoraTelefoniaBL ?? new OperadoraTelefoniaBL();

        //Private Constructor
        private OperadoraTelefoniaHelper() {

        }

        /// <summary>
        /// Dropdown com opcao de unidades cadastradas no sistema
        /// </summary>
        public SelectList selectList(int? selected, bool flagCache = true) {

            string keyCache = "dd_tipo_operadora_telefone";


            if (!flagCache) {

                var query = this.OOperadoraTelefoniaBL.listar("");

                var lista = query.ToList();

                var subList = lista.Select(x => new { value = x.id, text = x.nome }).ToList();

                return new SelectList(subList, "value", "text", selected);

            }

            var OCacheService = CacheService.getInstance;

            var listaCache = OCacheService.carregar<List<OperadoraTelefonia>>(keyCache);

            if (listaCache == null) {

                var query = this.OOperadoraTelefoniaBL.listar("");

                listaCache = query.ToList();

                OCacheService.adicionar(keyCache, listaCache);
            }

            var list = listaCache.Select(x => new { value = x.id, text = x.nome }).ToList();
            ;

            return new SelectList(list, "value", "text", selected);

        }


    }
}