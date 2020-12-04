using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Pessoas;
using DAL.Organizacoes;

namespace DAL.Unidades {

	//
	public class Unidade : DefaultEntity {

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public int idPessoa { get; set; }

        public virtual Pessoa Pessoa { get; set; }

		public bool? flagOptanteSimplesNacional { get; set; }
		
		public string sigla { get; set; }

		public string tipoUnidade { get; set; }

        public int? idUnidadeMatriz { get; set; }

        public string flagSistema { get; set; }

        public Unidade() {
	        
		}
		
	}
	
	//
	internal sealed class UnidadeMapper : EntityTypeConfiguration<Unidade> {
		
		public UnidadeMapper() {
			
			this.ToTable("tb_unidade");
			
            //FKs
			this.HasRequired(u => u.Pessoa).WithMany().HasForeignKey(u => u.idPessoa);

		    this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);
		}
	}
}