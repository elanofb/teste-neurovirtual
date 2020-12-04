using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Arquivos;
using DAL.Organizacoes;
using DAL.Portais;

namespace DAL.Publicacoes {

	public class Jornal : Geral {

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public int? idPortal { get; set; }

        public virtual Portal Portal { get; set; }

        public int idTipoNoticia { get; set; }

		public TipoNoticia TipoNoticia { get; set; }

		public string titulo { get; set; }

		public string autor { get; set; }

		public DateTime? dtJornal { get; set; }

		public string chamada { get; set; }

		public string flagCompartilharFB { get; set; }

        public bool? flagSomenteAssociado { get; set; }

		public virtual ArquivoUpload Foto { get; set;}

		public virtual List<ArquivoUpload> listaFoto { get; set;}

		public Jornal() { 
			this.listaFoto = new List<ArquivoUpload>();
		}
	}

	//
	internal sealed class JornalMapper : EntityTypeConfiguration<Jornal> {

		public JornalMapper() {

			this.ToTable("tb_jornal");

			this.HasKey(x => x.id);

			this.Ignore(x => x.Foto);

			this.Ignore(x => x.listaFoto);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

			this.HasRequired(o => o.TipoNoticia).WithMany().HasForeignKey(o => o.idTipoNoticia);

            this.HasOptional(x => x.Portal).WithMany().HasForeignKey(x => x.idPortal);
        }
	}
}