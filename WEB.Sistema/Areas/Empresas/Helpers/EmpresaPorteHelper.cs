using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using BLL.Caches;
using BLL.Empresas;
using DAL.Empresas;

namespace WEB.Areas.Empresas.Helpers{

    public class EmpresaPorteHelper{

        //Atributos
        private static EmpresaPorteHelper _instance;
        private IEmpresaPorteBL _EmpresaPorteBL;

        //Propriedades
        public static EmpresaPorteHelper getInstance => _instance = _instance ?? new EmpresaPorteHelper();
        private IEmpresaPorteBL OEmpresaPorteBL => this._EmpresaPorteBL = this._EmpresaPorteBL ?? new EmpresaPorteBL();

        //Private Constructor
        private EmpresaPorteHelper() {

        }

        /// <summary>
        /// Dropdown com opcao de unidades cadastradas no sistema
        /// </summary>
        public SelectList selectList(int? selected, bool flagCache = true) {

            string keyCache = "dd_empresa_porte";


            if (!flagCache) {

                var query = this.OEmpresaPorteBL.listar(0, "", true);

                var lista = query.ToList();

                var subList = lista.Select(x => new { value = x.id, text = x.descricao }).ToList();

                return new SelectList(subList, "value", "text", selected);

            }

            var OCacheService = CacheService.getInstance;

            var listaCache = OCacheService.carregar<List<EmpresaPorte>>(keyCache);

            if (listaCache == null) {

                var query = this.OEmpresaPorteBL.listar(0, "", true);

                listaCache = query.ToList();

                OCacheService.adicionar(keyCache, listaCache);
            }

            var list = listaCache.Select(x => new { value = x.id, text = x.descricao }).ToList();
            

            return new SelectList(list, "value", "text", selected);

        }


    }
}