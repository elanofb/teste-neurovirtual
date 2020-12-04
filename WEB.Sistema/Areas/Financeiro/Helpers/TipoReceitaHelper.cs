using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using BLL.Caches;
using BLL.Financeiro;
using DAL.Financeiro;
using Newtonsoft.Json;

namespace WEB.Areas.Financeiro.Helpers {
	public class TipoReceitaHelper {

        //Atributos
	    private static TipoReceitaHelper _instance;
        private ITipoReceitaBL _TipoReceitaBL;

        //Propriedades
	    public static TipoReceitaHelper getInstance => _instance = _instance ?? new TipoReceitaHelper();
        private ITipoReceitaBL OTipoReceitaBL => (_TipoReceitaBL = _TipoReceitaBL ?? new TipoReceitaBL() );

	    //
		public SelectList selectList(int? selected, bool flagCache = true) {

		    var lista = JsonConvert.DeserializeObject<List<TipoReceita>>(this.getList(flagCache));

		    return new SelectList(lista, "id", "descricao", selected);
        }

        //
		public MultiSelectList multiSelectList(int[] selected, bool flagCache = true) {


		    var lista = JsonConvert.DeserializeObject<List<TipoReceita>>(this.getList(flagCache));

		    return new SelectList(lista, "id", "descricao", selected);
        }

        private string getList(bool flagCache = true) {
            if (!flagCache){

                var query = OTipoReceitaBL.listar("", true).OrderBy(x => x.descricao);

                var list = query.Select(x => new { value = x.id, text = x.descricao }).ToList();

                return JsonConvert.SerializeObject(list);
            }

            var OCacheService = CacheService.getInstance;

            object jsonData = OCacheService.carregar(TipoReceitaBL.keyCache);

            if (jsonData != null) {

                return jsonData.ToString();
            }

            var listaCampos = OTipoReceitaBL.listar("", true).OrderBy(x => x.descricao).Select(x => new {x.id, x.descricao}).ToList();

            OCacheService.remover(TipoReceitaBL.keyCache);

            var json = JsonConvert.SerializeObject(listaCampos);

            OCacheService.adicionar(TipoReceitaBL.keyCache, json);

            return json;
        }

	}
}