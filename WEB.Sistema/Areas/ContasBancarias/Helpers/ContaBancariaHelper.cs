using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using BLL.Caches;
using BLL.ContasBancarias;
using BLL.Services;
using Newtonsoft.Json;
using ContaBancaria = DAL.ContasBancarias.ContaBancaria;

namespace WEB.Areas.ContasBancarias.Helpers {
    public class ContaBancariaHelper {

        //Atributos
        private static ContaBancariaHelper _instance;
        private IContaBancariaBL _ContaBancariaBL;

        //Propriedades
        public static ContaBancariaHelper getInstance => _instance = _instance ?? new ContaBancariaHelper();
        private IContaBancariaBL OContaBancariaBL => (_ContaBancariaBL = _ContaBancariaBL ?? new ContaBancariaBL());

        //
        public SelectList selectList(int? selected, bool flagCache = true) {

            var lista = JsonConvert.DeserializeObject<List<ContaBancaria>>(this.getList(flagCache));

            return new SelectList(lista, "id", "descricao", selected);
        }
        
        //
        public MultiSelectList multiSelectList(List<int> selected, bool flagCache = true) {

            var lista = JsonConvert.DeserializeObject<List<ContaBancaria>>(this.getList(flagCache));

            return new MultiSelectList(lista, "id", "descricao", selected);
        }

        //
        public SelectList selectTransferencia(int? selected, int idContaBancariaPrincipal) {

            var list = OContaBancariaBL.listar("", true).Where(x => x.id != idContaBancariaPrincipal).OrderBy(x => x.descricao);

            return new SelectList(list, "id", "descricao", selected);
        }

        private string getList(bool flagCache = true) {

            if (!flagCache){

                var query = OContaBancariaBL.listar("", true).Where(x => x.idBanco > 0).OrderBy(x => x.descricao);
                
                var list = query.ToListJsonObject<ContaBancaria>()
                                .Select(x => new { x.id, x.descricao })
                                .ToList();

                return JsonConvert.SerializeObject(list);
            }

            var OCacheService = CacheService.getInstance;
            var keyCache = ContaBancariaBL.keyCache;

            object jsonData = OCacheService.carregar(keyCache);

            if (jsonData != null) {
                return jsonData.ToString();
            }

            var queryConta = OContaBancariaBL.listar("", true).Where(x => x.idBanco > 0).ToList();
                
            var listaCampos = queryConta.ToListJsonObject<ContaBancaria>()
                                .Select(x => new { x.id, x.descricao })
                                .OrderBy(x => x.descricao)
                                .ToList();


            OCacheService.remover(keyCache);

            var json = JsonConvert.SerializeObject(listaCampos);

            OCacheService.adicionar(keyCache, json);

            return json;
        }
    }
}