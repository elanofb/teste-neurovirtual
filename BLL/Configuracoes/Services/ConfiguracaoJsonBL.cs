using System;
using System.IO;
using Newtonsoft.Json;

namespace BLL.Configuracoes.Services {

    public class ConfiguracaoJsonBL : IConfigJsonBL {

        //Statics 
        public static string CADASTRO_ASSOCIADO_CAMPOS = "cadastro_associado_campos.json";

        //Atributos
        private static ConfiguracaoJsonBL _instance;

        //Services

        //Propriedades
        public static ConfiguracaoJsonBL getInstance => _instance = _instance ?? new ConfiguracaoJsonBL();

        /// <summary>
        /// Carregar um objeto a partir de arquivo json de configuracao
        /// </summary>
        public T carregar<T>(string fileConfig) where T : class {

            var pathFile = HttpContextFactory.Current.Server.MapPath(string.Concat("~/files/config/", fileConfig));

            FileInfo OArquivo = new FileInfo(pathFile);

            if (!OArquivo.Exists) {

                return null;
            }

            string json;

            using (StreamReader r = new StreamReader(OArquivo.FullName)) {

                json = r.ReadToEnd();

            }

            if (string.IsNullOrEmpty(json)) {
                return null;
            }

            T Retorno = JsonConvert.DeserializeObject<T>(json);

            return Retorno;
        }
    }
}
