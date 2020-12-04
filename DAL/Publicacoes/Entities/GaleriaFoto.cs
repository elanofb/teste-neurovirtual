using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Arquivos;
using DAL.Organizacoes;
using DAL.Portais;

namespace DAL.Publicacoes {

	//
	public class GaleriaFoto : Geral {

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

	    public int? idPortal { get; set; }

	    public virtual Portal Portal { get; set; }

        public int idTipoGaleria { get; set; }

		public virtual TipoGaleriaFoto TipoGaleria { get; set; }

		public string titulo { get; set; }

        public string chamada { get; set; }

		public DateTime dtGaleria { get; set; }

        public bool? flagSomenteAssociado { get; set; }

        public virtual ArquivoUpload Foto { get; set;}

		public virtual List<ArquivoUpload> listaFotos { get; set;}

	}

	//
	internal sealed class GaleriaFotoMapper : EntityTypeConfiguration<GaleriaFoto> {

		public GaleriaFotoMapper() {

			this.ToTable("tb_galeria_foto");

			this.HasKey(o => o.id);

			this.Ignore(x => x.listaFotos);

            this.Ignore(x => x.Foto);

            this.HasRequired(o => o.TipoGaleria).WithMany().HasForeignKey(o => o.idTipoGaleria);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

		    this.HasOptional(x => x.Portal).WithMany().HasForeignKey(x => x.idPortal);

        }
	}
}