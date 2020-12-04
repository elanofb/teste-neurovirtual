using System;
using System.Linq;
using System.Web;
using System.Web.Caching;
using BLL.Caches;
using BLL.Configuracoes;
using DAL.Institucional.Config;
using Newtonsoft.Json;

namespace BLL.Institucional.Config {

    public class MenuConfigBL {

        //Atributos
        private static MenuConfigBL _instance;
        private IConfigJsonBL _ConfigJsonBL;

        //Propriedades
        public static MenuConfigBL getInstance => _instance = _instance ?? new MenuConfigBL();
        private IConfigJsonBL OConfigJsonBL => this._ConfigJsonBL = this._ConfigJsonBL ?? new ConfigJsonBL();

        //Construtor 
        private MenuConfigBL() {
            
        }

        //Carrregar configuracoes de blocos do cadastro de associado PF
        public MenuConfig carregar(bool flagCache = true) {

            var cacheData = HttpContext.Current.Cache.Get(CacheService.MENU_AREA_ASSOCIADO);

            MenuConfig OMenu = (MenuConfig) cacheData;

             if (OMenu != null && OMenu.listaMenus.Count > 0 && flagCache) {

                 OMenu.listaMenus = OMenu.listaMenus.Where(x => x.flagHabilitado == true).ToList();

                return OMenu;
		    }

            var jsonConfig = OConfigJsonBL.load("area_associado_menu.json");

            if (string.IsNullOrEmpty(jsonConfig)) {
                return new MenuConfig();
            }

            MenuConfig OConfig = JsonConvert.DeserializeObject<MenuConfig>(jsonConfig);

            OConfig.listaMenus = OConfig.listaMenus.Where(x => x.flagHabilitado == true).ToList();

            HttpContext.Current.Cache.Insert(CacheService.MENU_AREA_ASSOCIADO, OConfig, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);

            OMenu = OConfig;

            return OMenu;
        }

    }
}
