using System.Collections.Generic;

namespace DAL.Institucionais.Config {

    public class MenuConfig {

        public string id { get; set; }

        public List<MenuItem> listaMenus { get; set; }

        //
        public MenuConfig() {

            this.listaMenus = new List<MenuItem>();

        }
    }
}
