using System.Collections.Generic;

namespace DAL.Configuracoes.Config {

    public class ApplicationJson {

        public string nomeOrganizacao { get; set; }

        public HeaderConfig Header { get; set; }

        public StyleConfig Style { get; set; }

        public string siglaOrganizacao { get; set; }

        public string chaveFroala { get; set; }

        public string chaveApi { get; set; }

        public CadastroAssociadoConfig CadastroAssociado { get; set; }

        public NaoAssociadoConfig NaoAssociado { get; set; }

        public AreaAssociadoConfig AreaAssociado { get; set; }

        public CheckoutConfig Checkout { get; set; }


    }
}
