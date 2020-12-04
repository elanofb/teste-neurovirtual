using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using BLL.Caches;
using BLL.Financeiro;
using DAL.Financeiro;
using Newtonsoft.Json;

namespace WEB.Areas.Financeiro.Helpers{
    public class CentroCustoHelper{

        //Atributos
        private static CentroCustoHelper _instance;
        private ICentroCustoBL _CentroCustoBL;

        //Propriedades
        public static CentroCustoHelper getInstance => _instance = _instance ?? new CentroCustoHelper();
        private ICentroCustoBL OCentroCustoBL => (_CentroCustoBL = _CentroCustoBL ?? new CentroCustoBL());
        
        public SelectList selectList(int? selected, bool flagCache = true){

            var lista = JsonConvert.DeserializeObject<List<CentroCusto>>(this.getList(flagCache));

            return new SelectList(lista, "id", "descricao", selected);
        }

        //
        public MultiSelectList selectMultList(List<int?> selected, bool flagCache = true) {

            var lista = JsonConvert.DeserializeObject<List<CentroCusto>>(this.getList(flagCache));

            return new MultiSelectList(lista, "id", "descricao", selected);
        }


        private string getList(bool flagCache = true) {

            var listaCampos = OCentroCustoBL.listar("", true).ToList()
                .Select(x => new {x.id, descricao = x.descricaoCentroCusto(), x.codigoFiscal})
                .OrderBy(x => x.codigoFiscal).ThenBy(x => x.descricao).ToList();

            if (!flagCache){
                return JsonConvert.SerializeObject(listaCampos);
            }

            var OCacheService = CacheService.getInstance;

            object jsonData = OCacheService.carregar(CentroCustoBL.keyCache);

            if (jsonData != null) {

                return jsonData.ToString();
            }


            OCacheService.remover(CentroCustoBL.keyCache);

            var json = JsonConvert.SerializeObject(listaCampos);

            OCacheService.adicionar(CentroCustoBL.keyCache, json);

            return json;
        }
    }
}