using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using DAL.Arquivos;
using DAL.Pessoas;

namespace DAL.Associados {

	//
	[Serializable]
	public class AssociadoEmailVW {

		public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public int? idUnidade { get; set; }

        public byte? idTipoEmail { get; set; }

        public string descricaoTipoEmail { get; set; }

        public string email { get; set; }

        public string nome { get; set; }

		public int? idAssociado { get; set; }
		
        public byte? idTipoCadastro { get; set; }

        public int? idAssociadoEstipulante { get; set; }

        public int? idTipoAssociado { get; set; }                

        public string ativo { get; set; }

        public DateTime dtCadastro { get; set; }


        //Construtor
		public AssociadoEmailVW() {

		}

	}

	//
	internal sealed class AssociadoEmailVWMapper : EntityTypeConfiguration<AssociadoEmailVW> {

		public AssociadoEmailVWMapper() {

			this.ToTable("vw_associado_email");

			this.HasKey(o => o.id);

		}
	}
}