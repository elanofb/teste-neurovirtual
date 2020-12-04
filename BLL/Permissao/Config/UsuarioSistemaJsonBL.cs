using System;
using System.Web;
using System.Web.Caching;
using BLL.Caches;
using Newtonsoft.Json;
using BLL.Configuracoes;
using DAL.Permissao.Config;

namespace BLL.Permissao.Config {

    public class UsuarioSistemaJsonBL {

        //Atributos
        private static UsuarioSistemaJsonBL _instance;
        private IConfigJsonBL _ConfigJsonBL;

        //Propriedades
        public static UsuarioSistemaJsonBL getInstance => _instance = _instance ?? new UsuarioSistemaJsonBL();
        private IConfigJsonBL OConfigJsonBL => this._ConfigJsonBL = this._ConfigJsonBL ?? new ConfigJsonBL();

        //Construtor 
        private UsuarioSistemaJsonBL() {
            
        }

        public UsuarioSistemaJson carregar(bool flagCache = true) {

            var cacheData = HttpContext.Current.Cache.Get(CacheService.USUARIO_SISTEMA);

            UsuarioSistemaJson OConfig = (UsuarioSistemaJson) cacheData;

            if (OConfig != null && flagCache) {

                return OConfig;
		    }

            var jsonConfig = OConfigJsonBL.load("usuariosistema_config.json");

            if (string.IsNullOrEmpty(jsonConfig)) {
                return new UsuarioSistemaJson();
            }

            OConfig = JsonConvert.DeserializeObject<UsuarioSistemaJson>(jsonConfig);


            HttpContext.Current.Cache.Insert(CacheService.USUARIO_SISTEMA, OConfig, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);

            return OConfig;
        }

    }
}
