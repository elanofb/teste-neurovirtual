using System;

namespace DAL.Associados {

	public class SpAssociadosPesquisaRapida {

		public int id { get; set; }

	    public int? nroAssociado { get; set; }
		
		public byte? idTipoCadastro { get; set; }

	    public string nomeOrganizacao { get; set; }

	    public string nome { get; set; }

		public string nroDocumento { get; set; }
        
		public string descricaoTipoAssociado { get; set; }

		public int? idAssociadoEstipulante { get; set; }

	    public string nomeAssociadoEstipulante { get; set; }

	    public string ativo { get; set; }

	}

}