using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace BLL.Services{

    public static class BLLExtensions {

        /// <summary>
        /// Permite pre selecionar campos de uma entidade e retonar a lista com o tipo de objeto especificado
        /// </summary>
        public static List<T> ToListJsonObject<T>(this IQueryable entity, bool? flagIgnoreObjects = false){
            
            var settings = new JsonSerializerSettings();

            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.NullValueHandling = NullValueHandling.Ignore;

            if (flagIgnoreObjects == true) {
                settings.ContractResolver = new IgnoreObjectsResolver();
            }
            
            settings.Converters.Add(new StringDecimalConverter());

            var json = JsonConvert.SerializeObject(entity, settings);

            return JsonConvert.DeserializeObject<List<T>>(json, settings);
        }

        /// <summary>
        /// Permite pre selecionar campos de uma entidade e retonar a lista com o tipo de objeto especificado
        /// </summary>
        public static List<T> ToListJsonObject<T>(this object list, bool? flagIgnoreObjects = false){
            
            var settings = new JsonSerializerSettings();

            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            settings.NullValueHandling = NullValueHandling.Ignore;
            
            if (flagIgnoreObjects == true) {
                settings.ContractResolver = new IgnoreObjectsResolver();
            }

            settings.Converters.Add(new StringDecimalConverter());
            
            var json = JsonConvert.SerializeObject(list, settings);

            return JsonConvert.DeserializeObject<List<T>>(json, settings);
        }

        /// <summary>
        /// Permite pre selecionar campos de uma entidade e retonar um objeto do tipo de objeto especificado
        /// </summary>
        public static T ToJsonObject<T>(this object entity, bool? flagIgnoreObjects = false) {

            var settings = new JsonSerializerSettings();

            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            settings.NullValueHandling = NullValueHandling.Ignore;
            
            if (flagIgnoreObjects == true) {

                settings.ContractResolver = new IgnoreObjectsResolver();

            }
            
            settings.Converters.Add(new StringDecimalConverter());
            
            var json = JsonConvert.SerializeObject(entity, settings);

            return JsonConvert.DeserializeObject<T>(json, settings);
        }
    }
}