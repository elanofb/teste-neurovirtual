using System.Data.Entity.ModelConfiguration;
using System;
using DAL.Organizacoes;

namespace DAL.Diretorias {
	
    //
	public partial class Diretoria {

        public int id { get; set; }

        public Int16 anoInicioGestao { get; set; }

        public Int16 anoFimGestao { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime? dtAlteracao { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public bool? ativo { get; set; }

        public bool? flagExcluido { get; set; }
    }

	//
	internal sealed class DiretoriaMapper : EntityTypeConfiguration<Diretoria> {

		public DiretoriaMapper() {
			this.ToTable("tb_diretoria");
			this.HasKey(o => o.id);

            this.HasOptional(o => o.Organizacao).WithMany().HasForeignKey(o => o.idOrganizacao);
        }
	}
}