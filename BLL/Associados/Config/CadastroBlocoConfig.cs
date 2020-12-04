using BLL.Configuracoes;
using DAL.Associados.Config;
using Newtonsoft.Json;

namespace BLL.Associados.Config {
    public class CadastroBlocoConfig {

        //Atributos
        private static CadastroBlocoConfig _instance;
        private IConfigJsonBL _ConfigJsonBL;

        //Propriedades
        public static CadastroBlocoConfig getInstance => _instance = _instance ?? new CadastroBlocoConfig();
        private IConfigJsonBL OConfigJsonBL => this._ConfigJsonBL = this._ConfigJsonBL ?? new ConfigJsonBL();

        //Construtor 
        private CadastroBlocoConfig() {
            
        }

        //Carrregar configuracoes de blocos do cadastro de associado PF
        public CadastroBlocoPFConfig carregarConfigPF() {
            var jsonConfig = OConfigJsonBL.load("associado_blocos_opcionais_pf.json");

            if (string.IsNullOrEmpty(jsonConfig)) {
                return new CadastroBlocoPFConfig();
            }

            CadastroBlocoPFConfig OCadastroBlocoPFConfig = JsonConvert.DeserializeObject<CadastroBlocoPFConfig>(jsonConfig);

            return OCadastroBlocoPFConfig;
        }

        //Carrregar configuracoes de blocos do cadastro de associado PJ
        public CadastroBlocoPJConfig carregarConfigPJ() {

            var jsonConfig = OConfigJsonBL.load("associado_blocos_opcionais_pj.json");

            if (string.IsNullOrEmpty(jsonConfig)) {
                return new CadastroBlocoPJConfig();
            }

            CadastroBlocoPJConfig OCadastroBlocoConfig = JsonConvert.DeserializeObject<CadastroBlocoPJConfig>(jsonConfig);

            return OCadastroBlocoConfig;

        }

        //Carrregar configuracoes de blocos do cadastro de associado PJ
        public CadastroForm carregarForm(string jsonName) {

            var jsonConfig = OConfigJsonBL.load(jsonName);

            if (string.IsNullOrEmpty(jsonConfig)) {
                return new CadastroForm();
            }

            CadastroForm OCadastroBlocoConfig = JsonConvert.DeserializeObject<CadastroForm>(jsonConfig);

            return OCadastroBlocoConfig;

        }
    }
}
