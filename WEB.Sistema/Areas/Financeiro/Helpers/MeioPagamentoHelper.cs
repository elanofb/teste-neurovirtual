using System.Linq;
using System.Web.Mvc;
using BLL.Caches;
using BLL.Financeiro;
using Newtonsoft.Json;
using DAL.Financeiro;
using System.Collections.Generic;

namespace WEB.Areas.Financeiro.Helpers{
    public class MeioPagamentoHelper{

        //Atributos
        private static MeioPagamentoHelper _instance;
        private IMeioPagamentoBL _MeioPagamentoBL;

        //Propriedades
        public static MeioPagamentoHelper getInstance => _instance = _instance ?? new MeioPagamentoHelper();
        private IMeioPagamentoBL OMeioPagamentoBL => (_MeioPagamentoBL = _MeioPagamentoBL ?? new MeioPagamentoBL());

        public SelectList selectList(int? selected, bool flagCache = true, int[] idsExclude = null) {

            var lista = JsonConvert.DeserializeObject<List<MeioPagamento>>(this.getList(flagCache, idsExclude));

            return new SelectList(lista, "id", "descricao", selected);
        }

        public MultiSelectList multiSelectList(List<int> selecteds, bool flagCache = true) {

            var lista = JsonConvert.DeserializeObject<List<MeioPagamento>>(this.getList());

            return new MultiSelectList(lista, "id", "descricao", selecteds);
        }

        private string getList(bool flagCache = true, int[] idsExclude = null) {
            if (!flagCache){
                var query = OMeioPagamentoBL.listar("", "S");
                if (idsExclude != null) {
                    query = query.Where(x => !idsExclude.Contains(x.id));
                }
                var list = query.Select(x => new { value = x.id, text = x.descricao }).ToList();
                return JsonConvert.SerializeObject(list);
            }

            var OCacheService = CacheService.getInstance;
            var keyCache = MeioPagamentoBL.keyCache;

            object jsonData = OCacheService.carregar(keyCache);

            if (jsonData != null) {
                return jsonData.ToString();
            }

            var queryCampos = OMeioPagamentoBL.listar("", "S");
            if (idsExclude != null) {
                queryCampos = queryCampos.Where(x => !idsExclude.Contains(x.id));
            }

            var listaCampos = queryCampos.Select(x => new {x.id, x.descricao}).ToList();
            OCacheService.remover(keyCache);

            var json = JsonConvert.SerializeObject(listaCampos);
            OCacheService.adicionar(keyCache, json);

            return json;
        }
    }
}