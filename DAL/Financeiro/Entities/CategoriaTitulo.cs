using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Organizacoes;

namespace DAL.Financeiro {

	//
	public class CategoriaTitulo : Geral {

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

		public int? idMacroConta { get; set; }

		public virtual MacroConta MacroConta { get; set; }

		public int? idCategoriaPai { get; set; }

		public virtual CategoriaTitulo CategoriaPai { get; set; }

	    public bool? flagExibirDRE { get; set; }

	    public string codigoFiscal { get; set; }

	}

	//
	internal sealed class CategoriaTituloMapper : EntityTypeConfiguration<CategoriaTitulo> {

		public CategoriaTituloMapper() {

			this.ToTable("tb_financeiro_categoria");

			this.HasKey(o => o.id);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

			this.HasOptional(o => o.MacroConta).WithMany().HasForeignKey(o => o.idMacroConta);

			this.HasOptional(o => o.CategoriaPai).WithMany().HasForeignKey(o => o.idCategoriaPai);

		}
	}
}