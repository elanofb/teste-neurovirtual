using DAL.Infraestrutura.Config;

namespace DAL.Institucionais.Config {

    public class MenuItem {

        public string id { get; set; }

        public string grupo { get; set; }

        public string label { get; set; }

        public bool? flagHabilitado { get; set; }

        public bool? flagAssociado { get; set; }

        public bool? flagAssociadoEmAdmissao { get; set; }

        public bool? flagAssociadoAdimplente { get; set; }

        public bool? flagAssociadoInadimplente { get; set; }

        public bool? flagNaoAssociado { get; set; }

        public bool? flagPF { get; set; }

        public bool? flagPJ { get; set; }

        public LinkUrl url { get; set; }

        //
        public MenuItem() {

            this.url = new LinkUrl();
            
        }
    }
}
