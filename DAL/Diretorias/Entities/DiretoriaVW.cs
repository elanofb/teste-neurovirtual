using System.Data.Entity.ModelConfiguration;
using System;
using DAL.Organizacoes;

namespace DAL.Diretorias {
	
    //
	public partial class DiretoriaVW {

        public int id { get; set; }

        public Int16 anoInicioGestao { get; set; }

        public Int16 anoFimGestao { get; set; }

        public int? idOrganizacao { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime? dtAlteracao { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public bool? ativo { get; set; }

        public bool? flagExcluido { get; set; }

        public string nomePresidente { get; set; }
    }

	//
	internal sealed class DiretoriaVWMapper : EntityTypeConfiguration<DiretoriaVW> {

		public DiretoriaVWMapper() {
			this.ToTable("vw_diretoria");
			this.HasKey(o => o.id);
        }
	}
}