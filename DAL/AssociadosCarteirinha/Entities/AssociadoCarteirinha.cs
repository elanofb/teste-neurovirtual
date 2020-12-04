using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Associados;
using DAL.Permissao;

namespace DAL.AssociadosCarteirinha {

	//
	[Serializable]
	public class AssociadoCarteirinha : DefaultEntity {
        
		public int idAssociado { get; set; }

		public virtual Associado Associado { get; set; }
        
        public DateTime dtEnvio { get; set; }

        public string flagTipoEnvio { get; set; }

        public string flagTipoEmissao { get; set; }

        public string observacao { get; set; }

		public virtual UsuarioSistema UsuarioCadastro { get; set; }

	}

	//
	internal sealed class AssociadoCarteirinhaMapper : EntityTypeConfiguration<AssociadoCarteirinha> {

		public AssociadoCarteirinhaMapper() {
			this.ToTable("tb_associado_carteirinha");
			this.HasKey(o => o.id);
            
			this.HasRequired(o => o.Associado).WithMany().HasForeignKey(o => o.idAssociado);
			this.HasRequired(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);
		}
	}
}