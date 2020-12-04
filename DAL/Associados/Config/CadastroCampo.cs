using System.Collections.Generic;
using UTIL.UtilClasses;

namespace DAL.Associados.Config {

    public class CadastroCampo {

        public string label { get; set; }

        public string type { get; set; }

        public string idDOM { get; set; }

        public string name { get; set; }

        public string value { get; set; }

        public string defaultValue { get; set; }

        public bool flagNaoAlterarDefault { get; set; }

        public bool flagRemoverOpcaoVazia { get; set; }

        public bool? flagEdicao { get; set; }

        public bool? flagCadastro { get; set; }

        public string mask { get; set; }

        public string cssClassBox { get; set; }

        public string cssClassField { get; set; }

        public bool flagExibir { get; set; }

        public bool flagObrigatorio { get; set; }

        public string htmlAfterField { get; set; }

        public string htmlAfterBox { get; set; }

        public string textoInstrucoes { get; set; }

        public string errorMessage { get; set; }

        public List<CampoPropriedade> listaAtributos { get; set; }

        public List<OptionSelect> listaOpcoes { get; set; }

        //
        public CadastroCampo() {
            
            this.listaAtributos = new List<CampoPropriedade>();    

            this.listaOpcoes = new List<OptionSelect>();
        }

    }
}
