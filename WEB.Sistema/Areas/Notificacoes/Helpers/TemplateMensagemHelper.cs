using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using BLL.Caches;
using BLL.Notificacoes;
using DAL.Notificacoes;
using Newtonsoft.Json;

namespace WEB.Helpers {
    public class TemplateMensagemHelper {

        // Atributos
        private static TemplateMensagemHelper _instance;
        private ITemplateMensagemConsultaBL _ITemplateMensagemConsultaBL;
        
        // Propriedades
        public static TemplateMensagemHelper getInstance => _instance = _instance ?? new TemplateMensagemHelper();
        private ITemplateMensagemConsultaBL OTemplateMensagemConsultaBL => _ITemplateMensagemConsultaBL = _ITemplateMensagemConsultaBL ?? new TemplateMensagemConsultaBL();
        
        //
        public SelectList selectList(int? selected, bool flagCache = true, List<int> idsExclude = null) {

            var lista = JsonConvert.DeserializeObject<List<TemplateMensagem>>(this.getList(flagCache));

            if (idsExclude != null) {
                lista = lista.Where(x => !idsExclude.Contains(x.id)).ToList();
            }

            return new SelectList(lista, "id", "titulo", selected);
        }
        
        //
        private string getList(bool flagCache = true) {
            
            if (!flagCache){

                var query = OTemplateMensagemConsultaBL.listar("");

                var list = query.Select(x => new { value = x.id, text = x.titulo }).ToList();

                return JsonConvert.SerializeObject(list);
            }

            var OCacheService = CacheService.getInstance;

            object jsonData = OCacheService.carregar(TemplateMensagemConsultaBL.keyCache);

            if (jsonData != null) {

                return jsonData.ToString();
            }

            var listaCampos = OTemplateMensagemConsultaBL.listar("").Select(x => new { x.id, x.titulo }).ToList();

            OCacheService.remover(TemplateMensagemConsultaBL.keyCache);

            var json = JsonConvert.SerializeObject(listaCampos);

            OCacheService.adicionar(TemplateMensagemConsultaBL.keyCache, json);

            return json;
        }

    }
}