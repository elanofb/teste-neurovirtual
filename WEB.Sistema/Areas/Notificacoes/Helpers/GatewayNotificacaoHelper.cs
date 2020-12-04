using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using BLL.Caches;
using BLL.Notificacoes;
using DAL.Notificacoes;
using Newtonsoft.Json;

namespace WEB.Helpers {
    
    public class GatewayNotificacaoHelper {
        
        // Atributos
        private static GatewayNotificacaoHelper _instance;
        private IGatewayNotificacaoConsultaBL _IGatewayNotificacaoConsultaBL;
        
        // Propriedades
        public static GatewayNotificacaoHelper getInstance => _instance = _instance ?? new GatewayNotificacaoHelper();
        private IGatewayNotificacaoConsultaBL OGatewayNotificacaoConsultaBL => _IGatewayNotificacaoConsultaBL = _IGatewayNotificacaoConsultaBL ?? new GatewayNotificacaoConsultaBL();
        
        //
        public SelectList selectList(int? selected, bool flagCache = true, List<int> idsExclude = null) {

            var lista = JsonConvert.DeserializeObject<List<GatewayNotificacao>>(this.getList(flagCache));

            if (idsExclude != null) {
                lista = lista.Where(x => !idsExclude.Contains(x.id)).ToList();
            }

            return new SelectList(lista, "id", "descricao", selected);
        }    
        
        //
        private string getList(bool flagCache = true) {
            
            if (!flagCache){

                var query = OGatewayNotificacaoConsultaBL.listar("");

                var list = query.Select(x => new { value = x.id, text = x.descricao }).ToList();

                return JsonConvert.SerializeObject(list);
            }

            var OCacheService = CacheService.getInstance;

            object jsonData = OCacheService.carregar(GatewayNotificacaoConsultaBL.keyCache);

            if (jsonData != null) {

                return jsonData.ToString();
            }

            var listaCampos = OGatewayNotificacaoConsultaBL.listar("").Select(x => new { x.id, x.descricao }).ToList();

            OCacheService.remover(GatewayNotificacaoConsultaBL.keyCache);

            var json = JsonConvert.SerializeObject(listaCampos);

            OCacheService.adicionar(GatewayNotificacaoConsultaBL.keyCache, json);

            return json;
        }

    }
    
}