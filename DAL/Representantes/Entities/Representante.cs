using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Pessoas;

namespace DAL.Representantes {

	public class Representante {
        
		public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public int? idPessoa { get; set; }
		
		public Pessoa Pessoa { get; set; }

		public int idUsuarioCadastro { get; set; }
		
		public DateTime dtCadastro { get; set; }

		public DateTime? dtAlteracao { get; set; }

		public int? idUsuarioExclusao { get; set; }

		public DateTime? dtExclusao { get; set; }
		
		public bool? ativo { get; set; }
		
	}

	internal sealed class RepresentanteMapper : EntityTypeConfiguration<Representante> {

		public RepresentanteMapper() {
			
			this.ToTable("tb_representante");
			this.HasKey(o => o.id);

			this.HasOptional(x => x.Pessoa).WithMany().HasForeignKey(x => x.idPessoa);

		}
	}
}