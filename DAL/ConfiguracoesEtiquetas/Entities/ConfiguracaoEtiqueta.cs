using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.ConfiguracoesEtiquetas {

	//
	public class ConfiguracaoEtiqueta{

		public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao {get; set;}
        
        public string descricao { get; set; }

        public string cssCustomizado { get; set; }

	    public string html { get; set; }
        
        public decimal? width { get; set; }
        public decimal? height { get; set; }

        public int? qtdeEtiquetasLinha { get; set; }
        public int? qtdeLinhasPagina { get; set; }

	    public decimal? margPagTop { get; set; }
	    public decimal? margPagLef { get; set; }

        public decimal? margEtiquetaBot { get; set; }
        public decimal? margEtiquetaTop { get; set; }
        public decimal? margEtiquetaLef { get; set; }
        public decimal? margEtiquetaRig { get; set; }

        public bool? flagImpressoraTermica { get; set; }

        public DateTime dtCadastro { get; set; }
	    public DateTime? dtExclusao { get; set; }

        public int? idUsuarioCadastro { get; set; }
        public virtual UsuarioSistema UsuarioCadastro { get; set; }
        
        public int? idUsuarioExclusao { get; set; }
	    public virtual UsuarioSistema UsuarioExclusao { get; set; }

    }

	//
	internal sealed class ConfiguracaoEtiquetaMapper : EntityTypeConfiguration<ConfiguracaoEtiqueta> {

		public ConfiguracaoEtiquetaMapper() {

			this.ToTable("systb_configuracao_etiqueta");

			this.HasKey(o => o.id);

            this.HasOptional(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);

            this.HasOptional(x => x.UsuarioExclusao).WithMany().HasForeignKey(x => x.idUsuarioExclusao);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);
		}
	}
}