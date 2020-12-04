using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Associados;
using DAL.Contribuicoes;
using DAL.Financeiro;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.AssociadosContribuicoes {

	//
	public class AssociadoContribuicao {

		public int id { get; set; }

		public int idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; } 

        public int? idUnidade { get; set; }

        public int? idAssociadoContribuicaoPrincipal { get; set; }

        public int? idAssociadoEstipulante { get; set; }

		public int idAssociado { get; set; }

		public virtual Associado Associado { get; set; }

		public int idContribuicao { get; set; }

		public virtual Contribuicao Contribuicao { get; set; }

		public int idTipoAssociado { get; set; }

		public virtual TipoAssociado TipoAssociado { get; set; }

		public decimal valorOriginal { get; set; }

		public decimal valorAtual { get; set; }

		public DateTime dtVencimentoOriginal { get; set; }

		public DateTime dtVencimentoAtual { get; set; }

        public int? idContribuicaoVencimento { get; set; }

        public virtual ContribuicaoVencimento ContribuicaoVencimento { get; set; }

        public DateTime? dtInicioVigencia { get; set; }

        public DateTime? dtFimVigencia { get; set; }

		public DateTime? dtPagamento { get; set; }

		public bool? flagIsento { get; set; }

		public int? idUsuarioIsencao { get; set; }

		public DateTime? dtIsencao { get; set; }

        public string motivoIsencao { get; set; }

		public string observacoes { get; set; }

		public DateTime dtCadastro { get; set; }

		public int? idUsuarioExclusao { get; set; }

		public int? idUsuarioCadastro { get; set; }

        public UsuarioSistema UsuarioCadastro { get; set; }

		public DateTime? dtExclusao { get; set; }

        public string motivoExclusao { get; set; }

        public bool flagImportado { get; set; }

        public TituloReceita TituloReceita { get; set; }
	}

	//
	internal sealed class AssociadoContribuicaoMapper : EntityTypeConfiguration<AssociadoContribuicao> {

		public AssociadoContribuicaoMapper() {

            this.ToTable("tb_associado_contribuicao");

            this.HasKey(o => o.id);

			this.HasRequired(o => o.Associado).WithMany().HasForeignKey(o => o.idAssociado);

            this.HasRequired(o => o.Contribuicao).WithMany().HasForeignKey(o => o.idContribuicao);

            this.HasRequired(o => o.TipoAssociado).WithMany().HasForeignKey(o => o.idTipoAssociado);

            this.HasRequired(o => o.Organizacao).WithMany().HasForeignKey(o => o.idOrganizacao);

            this.HasOptional(o => o.ContribuicaoVencimento).WithMany().HasForeignKey(o => o.idContribuicaoVencimento);

            this.HasOptional(o => o.UsuarioCadastro).WithMany().HasForeignKey(o => o.idUsuarioCadastro);

		    this.Ignore(o => o.TituloReceita);
		}
	}
}