using System.Collections.Generic;
using System.Linq;
using BLL.Services;
using Newtonsoft.Json;
using PagedList;

namespace WEB.Extensions {

    public static class JsonObjectExtension {

        public static IPagedList<T> ToPagedListJsonObject<T>(this IQueryable<dynamic> entity, int nroPagina, int nroRegistros = 20, bool? flagIgnoreObjects = false){

            var settings = new JsonSerializerSettings();

            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            settings.NullValueHandling = NullValueHandling.Ignore;

            if (flagIgnoreObjects == true){

                settings.ContractResolver = new IgnoreObjectsResolver();

            }

            var PagedList = entity.ToPagedList(nroPagina, nroRegistros);

            var json = JsonConvert.SerializeObject(PagedList, settings);

            var itens = JsonConvert.DeserializeObject<IEnumerable<T>>(json);

            return new StaticPagedList<T>(itens, PagedList.PageNumber, PagedList.PageSize, PagedList.TotalItemCount);
        }
    }
}