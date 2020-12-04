using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Permissao {

	[Serializable]
	public class UsuarioSistemaVW {

		public int id { get; set; }

		public string nome { get; set; }

		public string login { get; set; }

		public string senha { get; set; }

		public string flagAlterarSenha { get; set; }

		public string ativo { get; set; }

		public string email { get; set; }

		public int idPerfilAcesso { get; set; }

		public string descricaoPerfilAcesso { get; set; }
	}

	//
	internal sealed class UsuarioSistemaVWMapper : EntityTypeConfiguration<UsuarioSistemaVW> {

		public UsuarioSistemaVWMapper() {
			this.ToTable("vw_usuario_sistema");
			this.HasKey(o => o.id);
		}
	}
}