using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;
using DAL.Entities;
using DAL.Pessoas;

namespace DAL.Permissao {

	//
	public class UsuarioSistema {

		public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public int idPessoa { get; set; }

        public virtual Pessoa Pessoa { get; set; }

		public DateTime? dtNascimento { get; set; }

		public string nome { get; set; }

		public string email { get; set; }

		public string login { get; set; }

		public string senha { get; set; }

		public string flagAlterarSenha { get; set; }

		public int idPerfilAcesso { get; set; }

		public virtual PerfilAcesso PerfilAcesso { get; set; }

        public DateTime? dtInicioDegustacao { get; set; }

        public DateTime? dtFimDegustacao { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public int idUsuarioAlteracao { get; set; }

		public int idUsuarioCadastro { get; set; }

		public string flagSistema { get; set; }

		public string ativo { get; set; }

		public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

        public string motivoExclusao { get; set;}

        public ICollection<UsuarioUnidade> listaUsuarioUnidade { get; set; }

        public virtual IList<UsuarioOrganizacao> listaUsuarioOrganizacao { get; set; }

	    public UsuarioSistema() {
	        this.listaUsuarioOrganizacao = new List<UsuarioOrganizacao>();
	        this.listaUsuarioUnidade = new List<UsuarioUnidade>();
	    }
	}

	//
	internal sealed class UsuarioSistemaMapper : EntityTypeConfiguration<UsuarioSistema> {

		public UsuarioSistemaMapper() {
			this.ToTable("systb_usuario_sistema");
			this.HasKey(o => o.id);

			//FKs
			this.HasRequired(u => u.PerfilAcesso).WithMany().HasForeignKey(u => u.idPerfilAcesso);

            this.HasRequired(x => x.Pessoa).WithMany().HasForeignKey(x => x.idPessoa);

		    this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);
		}
	}
}