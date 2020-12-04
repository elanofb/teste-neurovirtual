using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Caches;
using BLL.Financeiro;
using DAL.Financeiro;
using Newtonsoft.Json;

namespace WEB.Areas.Financeiro.Helpers{
    public class CategoriaPaiHelper{

        //Atributos
        private static CategoriaPaiHelper _instance;
        private ICategoriaTituloBL _CategoriaTituloBL;

        //Propriedades
        public static CategoriaPaiHelper getInstance => _instance = _instance ?? new CategoriaPaiHelper();
        private ICategoriaTituloBL OCategoriaTituloBL => (_CategoriaTituloBL = _CategoriaTituloBL ?? new CategoriaTituloBL());
        //Construtor

        public SelectList selectList(int? selected, bool flagCache = true, List<int> idsExclude = null) {

            var lista = JsonConvert.DeserializeObject<List<CategoriaTitulo>>(this.getList(flagCache));

            if (idsExclude != null) {
                lista = lista.Where(x => !idsExclude.Contains(x.id)).ToList();
            }
            
            return new SelectList(lista, "id", "descricao", selected);
        }

        //
        public MultiSelectList selectMultList(List<int?> selected, int? idMacroConta, bool flagCache = true) {

            var lista = JsonConvert.DeserializeObject<List<CategoriaTitulo>>(this.getList(flagCache));

            if (idMacroConta > 0) {
                lista = lista.Where(x => x.idMacroConta == idMacroConta).ToList();
            }

            return new MultiSelectList(lista, "id", "descricao", selected);
        }

        private string getList(bool flagCache = true) {
            if (!flagCache){

                var query = OCategoriaTituloBL.listar(0, "", "S").Where(x => x.idCategoriaPai == null);

                var list = query.Select(x => new { value = x.id, text = x.descricao }).ToList();

                return JsonConvert.SerializeObject(list);
            }

            var OCacheService = CacheService.getInstance;

            object jsonData = OCacheService.carregar(CategoriaTituloBL.keyCache);

            if (jsonData != null) {

                return jsonData.ToString();
            }

            var listaCampos = OCategoriaTituloBL.listar(0, "", "S").Where(x => x.idCategoriaPai == null).Select(x => new { x.id, x.descricao, x.idMacroConta, x.idCategoriaPai }).ToList();

            OCacheService.remover(CategoriaTituloBL.keyCache);

            var json = JsonConvert.SerializeObject(listaCampos);

            OCacheService.adicionar(CategoriaTituloBL.keyCache, json);

            return json;
        }
    }
}