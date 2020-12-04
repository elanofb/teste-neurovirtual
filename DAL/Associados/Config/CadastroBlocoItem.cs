using System.Collections.Generic;

namespace DAL.Associados.Config {

    public class CadastroBlocoItem {

        public string label { get; set; }

        public string idDOM { get; set; }

        public string cssClasses { get; set; }

        public bool flagExibir { get; set; }

        public bool? flagEdicao { get; set; }

        public List<CadastroCampo> listaCampos { get; set; }

        //
        public CadastroBlocoItem() {
            this.listaCampos = new List<CadastroCampo>();
        }
    }
}
