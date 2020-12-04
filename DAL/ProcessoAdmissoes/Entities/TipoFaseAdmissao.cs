using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Permissao;

namespace DAL.ProcessoAdmissoes {

	//
	public class TipoFaseAdmissao {

		public byte id { get; set; }
        
        public string descricao { get; set; }

        public bool? ativo { get; set; }

        public DateTime dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public virtual UsuarioSistema UsuarioCadastro { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

	}

	//
	internal sealed class TipoFaseAdmissaoMapper : EntityTypeConfiguration<TipoFaseAdmissao> {

		public TipoFaseAdmissaoMapper() {

			this.ToTable("datatb_tipo_fase_admissao");

            this.HasKey(o => o.id);

		    this.HasOptional(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);
            
		}
	}
}