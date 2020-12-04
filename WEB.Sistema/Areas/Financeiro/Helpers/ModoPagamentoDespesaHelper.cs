using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Caches;
using BLL.Financeiro;
using DAL.Financeiro;
using Newtonsoft.Json;

namespace WEB.Areas.Financeiro.Helpers{
    public class ModoPagamentoDespesaHelper{

        //Atributos
        private static ModoPagamentoDespesaHelper _instance;
        private IModoPagamentoDespesaConsultaBL _ModoPagamentoDespesaConsultaBL;

        //Propriedades
        public static ModoPagamentoDespesaHelper getInstance => _instance = _instance ?? new ModoPagamentoDespesaHelper();
        private IModoPagamentoDespesaConsultaBL OModoPagamentoDespesaConsultaBL => (_ModoPagamentoDespesaConsultaBL = _ModoPagamentoDespesaConsultaBL ?? new ModoPagamentoDespesaConsultaBL());
        //Construtor

        public SelectList selectList(int? selected, List<int> idsExclude = null) {

            var query = OModoPagamentoDespesaConsultaBL.listar(true);

            var lista = query.Select(x => new { value = x.id, text = x.descricao }).ToList();

            if (idsExclude != null) {
                lista = lista.Where(x => !idsExclude.Contains(x.value)).ToList();
            }
            
            return new SelectList(lista, "value", "text", selected);
        }

        //
        public MultiSelectList selectMultList(List<int?> selected, bool flagCache = true) {

            var lista = JsonConvert.DeserializeObject<List<ModoPagamentoDespesa>>(this.getList(flagCache));

            return new MultiSelectList(lista, "id", "descricao", selected);
        }

        private string getList(bool flagCache = true) {
            
            if (!flagCache){

                var query = OModoPagamentoDespesaConsultaBL.listar(true);

                var list = query.Select(x => new { value = x.id, text = x.descricao }).ToList();

                return JsonConvert.SerializeObject(list);
            }

            var OCacheService = CacheService.getInstance;

            object jsonData = OCacheService.carregar(ModoPagamentoDespesaConsultaBL.keyCache);

            if (jsonData != null) {

                return jsonData.ToString();
            }

            var listaCampos = OModoPagamentoDespesaConsultaBL.listar(true).Select(x => new { x.id, x.descricao }).ToList();

            OCacheService.remover(ModoPagamentoDespesaConsultaBL.keyCache);

            var json = JsonConvert.SerializeObject(listaCampos);

            OCacheService.adicionar(ModoPagamentoDespesaConsultaBL.keyCache, json);

            return json;
        }
    }
}