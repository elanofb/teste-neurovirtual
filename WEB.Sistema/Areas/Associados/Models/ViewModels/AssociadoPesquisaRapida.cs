namespace WEB.Areas.Associados.ViewModels {

    public class AssociadoPesquisaRapida {
        
        public int id { get; set; }

        public string nomeAssociado { get; set; }

        public string nroDocumento { get; set; }

        public int idTipoAssociado { get; set; }

        public string descricaoTipoAssociado { get; set; }

        public int idOrganizacao { get; set; }

        public string nomeOrganizacao { get; set; }

        public int? idAssociadoEstipulante { get; set; }

        public string nomeAssociadoEstipulante { get; set; }

        public string ativo { get; set; }

        
    }
}