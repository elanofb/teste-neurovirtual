using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using DAL.Arquivos;
using DAL.Pessoas;

namespace DAL.Associados {

	//
	[Serializable]
	public class AssociadoAreaAtuacaoVW {
		
		public int id { get; set; }
		
		public int idAssociado { get; set; }

        public int? idOrganizacao { get; set; }
		
		public int idAreaAtuacao { get; set; }

        public int? idUnidade { get; set; }       

        public string descricaoAreaAtuacao { get; set; }
		
		public string descricaoTipoEmail { get; set; }   
		
        public string nome { get; set; }		

        public byte? idTipoCadastro { get; set; }
		
		public string nroDocumento { get; set; }
		
		public string descricaoTipoAssociado { get; set; }

        public int? idAssociadoEstipulante { get; set; }

        public int? idTipoAssociado { get; set; }
        
        public string flagSituacaoContribuicao { get; set; }

        public string ativo { get; set; }

        public DateTime dtCadastro { get; set; }
		
		public string email { get; set; }
		
		public int? ddi { get; set; }
		
		public string nroTelefone { get; set; }
		

        //Construtor
		public AssociadoAreaAtuacaoVW() {

		}

	}

	//
	internal sealed class AssociadoAreaAtuacaoVWMapper : EntityTypeConfiguration<AssociadoAreaAtuacaoVW> {

		public AssociadoAreaAtuacaoVWMapper() {

			this.ToTable("vw_associado_area_atuacao");

			this.HasKey(o => o.id);

		}
	}
}