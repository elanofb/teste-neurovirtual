using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.Financeiro {

	//
	public class MacroConta  {

		public int id { get; set; }

        public int idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

		public string descricao { get; set; }

	    public string codigoFiscal { get; set; }
        
        public string flagReceitaDespesa { get; set; }

        public int? idCentroCustoDRE { get; set; }

		public virtual CentroCusto CentroCustoDRE { get; set; } 

        public int? idUsuarioAprovacao { get; set; }

        public UsuarioSistema UsuarioAprovacao { get; set; }

		public DateTime? dtCadastro { get; set; }

		public int idUsuarioCadastro { get; set; }

		public DateTime? dtAlteracao { get; set; }

		public int idUsuarioAlteracao { get; set; }

		public bool? ativo { get; set; }

		public bool? flagExcluido { get; set; }

        public bool? flagSistema { get; set; }

    }

    //
    internal sealed class MacroContaMapper : EntityTypeConfiguration<MacroConta> {

		public MacroContaMapper() {

			this.ToTable("tb_financeiro_macro_conta");

			this.HasKey(o => o.id);

			this.HasOptional(x => x.CentroCustoDRE).WithMany().HasForeignKey(x => x.idCentroCustoDRE);
			
            this.HasRequired(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

            this.HasOptional(x => x.UsuarioAprovacao).WithMany().HasForeignKey(x => x.idUsuarioAprovacao);

		}
	}
}