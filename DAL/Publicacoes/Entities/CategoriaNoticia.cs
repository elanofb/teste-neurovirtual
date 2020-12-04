using System.Data.Entity.ModelConfiguration;
using System;
using DAL.Portais;
using DAL.Organizacoes;
using System.Collections.Generic;

namespace DAL.Publicacoes {

	public partial class CategoriaNoticia {

        public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public int? idPortal { get; set; }

        public virtual Portal Portal { get; set; }

        public string descricao { get; set; }

        public string chaveUrl { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime? dtAlteracao { get; set; }

        public int idUsuarioCadastro { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public bool? ativo { get; set; }

        public bool? flagExcluido { get; set; }

        public virtual List<Noticia> listaNoticias { get; set; }

        public CategoriaNoticia() {
            this.listaNoticias = new List<Noticia>();
        }

    }


	internal sealed class CategoriaNoticiaMapper : EntityTypeConfiguration<CategoriaNoticia> {

		public CategoriaNoticiaMapper() {
			this.ToTable("tb_categoria_noticia");
			this.HasKey(o => o.id);

            this.HasOptional(x => x.Portal).WithMany().HasForeignKey(x => x.idPortal);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);
        }
	}
}