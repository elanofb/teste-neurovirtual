using System;

namespace WEB.Areas.AssociadosDependentes.ViewModels {

    public class ItemListaDependente {

        public int id { get; set; }

        public string flagTipoPessoa { get; set; }

        public string descricaoTipoAssociado { get; set; }

        public string descricaoTipoDependente { get; set; }

        public int? idAssociadoResponsavel { get; set; }

        public string nomeAssociado { get; set; }

        public string razaoSocialAssociado { get; set; }

        public string nome { get; set; }

        public string razaoSocial { get; set; }

        public string nroDocumento { get; set; }

        public string rg { get; set; }

        public string dddTelefonePrincipal { get; set; }

        public string nroTelefonePrincipal { get; set; }

        public DateTime dtCadastro { get; set; }

        public string ativo { get; set; }
    }
}