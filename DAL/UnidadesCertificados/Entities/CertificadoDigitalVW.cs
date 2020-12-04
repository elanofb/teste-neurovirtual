using System;
using System.Data.Entity.ModelConfiguration;
using System.IO;

namespace DAL.UnidadesCertificados {

    public class CertificadoDigitalVW {

        public int id { get; set; }

        public int idUnidade { get; set; }

        public string siglaUnidade { get; set; }

        public string descricao { get; set; }

        public string senha { get; set; }

        public int? idOrganizacao { get; set; }

        public int? idArquivoUpload { get; set; }

        public string path { get; set; }

        public string extensao { get; set; }

        public DateTime? dtCadastro { get; set; }

        /// <summary>
        /// Montar o caminho físico do certificado
        /// </summary>
        public string absolutePath(){

            string finalPath = Path.Combine(UtilConfig.pathAbsUploadFiles, this.path);

            return finalPath;
        }

    }

    internal sealed class CertificadoDigitalVWMapper : EntityTypeConfiguration<CertificadoDigitalVW> {

		public CertificadoDigitalVWMapper() {

			this.ToTable("vw_certificado_digital");

			this.HasKey(o => o.id);

		}
	}

}
