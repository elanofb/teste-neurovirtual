using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Caches;
using BLL.ConfiguracoesTextos;
using DAL.ConfiguracoesTextos;

namespace WEB.Areas.ConfiguracoesTextos.Helpers {

    public class ConfiguracaoTextoCategoriaHelper {

        //Atributos
        private static ConfiguracaoTextoCategoriaHelper _instance;
        private ConfiguracaoTextoCategoriaBL _ConfiguracaoTextoCategoriaBL;

        //Propriedades
        public static ConfiguracaoTextoCategoriaHelper getInstance => _instance = _instance ?? new ConfiguracaoTextoCategoriaHelper();
        private ConfiguracaoTextoCategoriaBL OConfiguracaoTextoCategoriaBL => this._ConfiguracaoTextoCategoriaBL = this._ConfiguracaoTextoCategoriaBL ?? new ConfiguracaoTextoCategoriaBL();

        public SelectList selectList(int? selected, bool flagCache = true) {

            var query = this.OConfiguracaoTextoCategoriaBL.listar("");

            if (!flagCache) {

                var lista = query.ToList();

                return new SelectList(lista, "id", "descricao", selected);

            }

            var OCacheService = CacheService.getInstance;

            string keyCache = "dd_configuracaotextocategoria";

            var listaCache = OCacheService.carregar<List<ConfiguracaoTextoCategoria>>(keyCache);

            if (listaCache == null) {

                listaCache = query.ToList();

                OCacheService.adicionar(keyCache, listaCache);

            }
            return new SelectList(listaCache, "id", "descricao", selected);
        }
    }
}