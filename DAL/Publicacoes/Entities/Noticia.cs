using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Arquivos;
using DAL.Organizacoes;
using DAL.Portais;

namespace DAL.Publicacoes {
    
	public class Noticia {

        public int id { get; set; }

        public string descricao { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime dtAlteracao { get; set; }

        public int idUsuarioCadastro { get; set; }

        public int idUsuarioAlteracao { get; set; }

        public string ativo { get; set; }

        public string flagExcluido { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public int? idPortal { get; set; }

        public virtual Portal Portal { get; set; }

		public int idTipoNoticia { get; set; }

		public TipoNoticia TipoNoticia { get; set; }

        public int? idCategoriaNoticia { get; set; }

        public CategoriaNoticia CategoriaNoticia { get; set; }

        public string titulo { get; set; }

        public string chaveUrl { get; set; }

		public string autor { get; set; }

		public DateTime? dtNoticia { get; set; }

		public string chamada { get; set; }

		public string flagCompartilharFB { get; set; }

        public bool? flagSomenteAssociado { get; set; }

		public virtual ArquivoUpload Foto { get; set;}

		public virtual List<ArquivoUpload> listaFoto { get; set;}

		public Noticia() { 
			this.listaFoto = new List<ArquivoUpload>();
		}
	}

	//
	internal sealed class NoticiaMapper : EntityTypeConfiguration<Noticia> {

		public NoticiaMapper() {

			this.ToTable("tb_noticia");

			this.HasKey(x => x.id);

			this.Ignore(x => x.Foto);

			this.Ignore(x => x.listaFoto);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

			this.HasRequired(o => o.TipoNoticia).WithMany().HasForeignKey(o => o.idTipoNoticia);

			this.HasOptional(o => o.CategoriaNoticia).WithMany().HasForeignKey(o => o.idCategoriaNoticia);

            this.HasOptional(x => x.Portal).WithMany().HasForeignKey(x => x.idPortal);

            this.HasOptional(x => x.CategoriaNoticia).WithMany(x => x.listaNoticias).HasForeignKey(x => x.idCategoriaNoticia);
        }
	}
}