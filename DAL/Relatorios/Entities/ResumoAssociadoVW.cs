using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Relatorios {

	public class ResumoAssociadoVW {

		public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public int idPessoa { get; set; }

        public int idEstadoAssociado { get; set; }

        public string descEstadoAssociado { get; set; }

        public string flagPessoaAssociada { get; set; }

        public int idTipoAssociado { get; set; }

        public string descTipoAssociado { get; set; }

        public string ativo { get; set; }

        public string descStatus { get; set; }

        public string nroDocumento { get; set; }

        public string nome { get; set; }

        public string razaoSocial { get; set; }

        public DateTime dtCadastro { get; set; }

        public string emailPrincipal { get; set; }

        public string dddTelPrincipal { get; set; }

        public string nroTelPrincipal { get; set; }

        public decimal valorAnuidade { get; set; }
		
	}

	//
	internal sealed class ResumoAssociadoVWMapper : EntityTypeConfiguration<ResumoAssociadoVW> {

		public ResumoAssociadoVWMapper() {

			this.ToTable("vw_resumo_associado");
			this.HasKey(o => o.id);
		}
	}
}