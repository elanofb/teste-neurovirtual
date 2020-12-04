using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Arquivos {

	//
	public class ArquivoAssociadoVW {

		public int idArquivoUpload { get; set; }
        public string titulo { get; set; }
        public string nomeArquivo { get; set; }
        public string legenda { get; set; }
        public DateTime dtCadastro { get; set; }
        public string path { get; set; }
		public string extensao { get; set; }
		public int idEntidadeArquivo { get; set; }
		public string descOcorrencia { get; set; }
		public string nomeUsuarioCadastro { get; set; }
			
	    public int idAssociado { get; set; }
        public int? nroAssociado { get; set; }
        public string nome { get; set; }
        public string razaoSocial { get; set; }
        public int idOrganizacao { get; set; }
        public int? idUnidade { get; set; }
        public int idTipoAssociado { get; set; }
        public string flagTipoPessoa { get; set; }
        public string descricaoTipoAssociado { get; set; }
        public string nroDocumento { get; set; }
        public string rg { get; set; }        
        public string ativo { get; set; }
		
    }

	//
	internal sealed class ArquivoAssociadoVWMapper : EntityTypeConfiguration<ArquivoAssociadoVW> {

		public ArquivoAssociadoVWMapper() {
			this.ToTable("vw_arquivo_associado");
			this.HasKey(o => o.idArquivoUpload);
		}
	}
}