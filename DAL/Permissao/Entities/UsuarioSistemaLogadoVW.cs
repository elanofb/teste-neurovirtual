using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Permissao {

	[Serializable]
	public class UsuarioSistemaLogadoVW {

		public int id { get; set; }

		public string nomeUsuario { get; set; }

        public int? idOrganizacao { get; set; }

        public string nomeOrganizacao { get; set; }

        public int idPerfilAcesso { get; set; }

		public string descricaoPerfilAcesso { get; set; }

		public bool? flagTodasUnidades { get; set; }

		public string sessionIDUltimoAcesso { get; set; }

		public DateTime? dtUltimoAcesso { get; set; }

		public string ativo { get; set; }


	}

	//
	internal sealed class UsuarioSistemaLogadoVWMapper : EntityTypeConfiguration<UsuarioSistemaLogadoVW> {

		public UsuarioSistemaLogadoVWMapper() {

			this.ToTable("vw_usuario_sistema_logado");

            this.HasKey(o => o.id);
		}
	}
}