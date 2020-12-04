using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Arquivos {

	//
	public class ArquivoUpload {

		public Int32 id { get; set; }

	    public int? idOrganizacao { get; set; }

		public string titulo { get; set; }

		public string legenda { get; set; }

		public string extensao { get; set; }

		public string categoria { get; set; }

		public string entidade { get; set; }

		public int idReferenciaEntidade { get; set; }

		public string path { get; set; }

		public string pathThumb { get; set; }

        public string nomeArquivo { get; set; }

		public string flagPrincipal { get; set; }

		public byte ordem { get; set; }

		public string ativo { get; set; }

		//public string flagExcluido { get; set; }

		public DateTime dtCadastro { get; set; }

		public string mimeType { get; set; }

		public string contentType { get; set; }

	    public DateTime? dtExclusao { get; set; }

		public int? idUsuarioExclusao { get; set; }

		public int? idUsuarioCadastro { get; set; }

	}

	//
	internal sealed class ArquivoUploadMapper : EntityTypeConfiguration<ArquivoUpload> {

		public ArquivoUploadMapper() {
			
			this.ToTable("systb_arquivo_upload");
			
			this.HasKey(o => o.id);
		}
	}
}