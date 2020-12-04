using System.Data.Entity.ModelConfiguration;
using System;

namespace DAL.Unidades {

	//
	public class UnidadeRota {

        public int id { get; set; }

        public int idUnidade { get; set; }

        public virtual Unidade Unidade { get; set; }
        
        public string nomeRota { get; set; }

        public string cepInicial { get; set; }

        public string cepFinal { get; set; }

        public bool flagDomingo { get; set; }

        public bool flagSegunda { get; set; }

        public bool flagTerca { get; set; }

        public bool flagQuarta { get; set; }

        public bool flagQuinta { get; set; }

        public bool flagSexta { get; set; }

        public bool flagSabado { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime dtAlteracao { get; set; }

        public int idUsuarioCadastro { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public bool flagExcluido { get; set; }

        public UnidadeRota() {
		}
	}

	//
	internal sealed class UnidadeRotaMapper : EntityTypeConfiguration<UnidadeRota> {

		public UnidadeRotaMapper() {
			this.ToTable("tb_unidade_rota");
			//FKs
			this.HasRequired(u => u.Unidade).WithMany().HasForeignKey(u => u.idUnidade);
		}
	}
}