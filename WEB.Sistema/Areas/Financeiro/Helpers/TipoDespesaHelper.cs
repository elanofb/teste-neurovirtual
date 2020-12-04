using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Caches;
using BLL.Financeiro;
using DAL.Financeiro;
using Newtonsoft.Json;

namespace WEB.Areas.Financeiro.Helpers{
    public class TipoDespesaHelper{

        //Atributos
        private static TipoDespesaHelper _instance;
        private ITipoDespesaConsultaBL _TipoDespesaConsultaBL;

        //Propriedades
        public static TipoDespesaHelper getInstance => _instance = _instance ?? new TipoDespesaHelper();
        private ITipoDespesaConsultaBL OTipoDespesaConsultaBL => (_TipoDespesaConsultaBL = _TipoDespesaConsultaBL ?? new TipoDespesaConsultaBL());
        //Construtor

        public SelectList selectList(int? selected, bool flagCache = true, List<int> idsExclude = null) {

            var lista = JsonConvert.DeserializeObject<List<TipoDespesa>>(this.getList(flagCache));

            if (idsExclude != null) {
                lista = lista.Where(x => !idsExclude.Contains(x.id)).ToList();
            }

            lista = lista.OrderBy(x => x.descricao).ToList();
            
            return new SelectList(lista, "id", "descricao", selected);
        }

        //
        public MultiSelectList selectMultList(List<int?> selected, bool flagCache = true) {

            var lista = JsonConvert.DeserializeObject<List<TipoDespesa>>(this.getList(flagCache));

            return new MultiSelectList(lista, "id", "descricao", selected);
        }

        private string getList(bool flagCache = true) {
            
            if (!flagCache){

                var query = OTipoDespesaConsultaBL.listar();

                var list = query.Select(x => new { value = x.id, text = x.descricao }).ToList();

                return JsonConvert.SerializeObject(list);
            }

            var OCacheService = CacheService.getInstance;

            object jsonData = OCacheService.carregar(TipoDespesaConsultaBL.keyCache);

            if (jsonData != null) {

                return jsonData.ToString();
            }

            var listaCampos = OTipoDespesaConsultaBL.listar().Select(x => new { x.id, x.descricao }).ToList();

            OCacheService.remover(TipoDespesaConsultaBL.keyCache);

            var json = JsonConvert.SerializeObject(listaCampos);

            OCacheService.adicionar(TipoDespesaConsultaBL.keyCache, json);

            return json;
        }
    }
}