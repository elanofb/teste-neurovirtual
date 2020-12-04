using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Arquivos;
using DAL.Permissao;
using DAL.Unidades;

namespace DAL.UnidadesCertificados {

    public class UnidadeCertificado {

        public int id { get; set; }

	    public int idOrganizacao { get; set; }

	    public int idUnidade { get; set; }

        public virtual Unidade Unidade { get; set; }

        public DateTime dtVencimento { get; set; }

        public string descricao { get; set; }

        public string senha { get; set; }

        public DateTime dtCadastro { get; set; }

	    public DateTime? dtAlteracao { get; set; }

	    public DateTime? dtExclusao { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public virtual UsuarioSistema UsuarioCadastro { get; set;}

	    public int? idUsuarioAlteracao { get; set; }

	    public int? idUsuarioExclusao { get; set; }

        public bool? ativo { get; set; }

        public virtual ArquivoUpload Certificado { get; set; }

    }

    internal sealed class UnidadeCertificadoMapper : EntityTypeConfiguration<UnidadeCertificado> {

		public UnidadeCertificadoMapper() {

			this.ToTable("tb_unidade_certificado");

			this.HasKey(o => o.id);

		    this.HasRequired(x => x.Unidade).WithMany().HasForeignKey(x => x.idUnidade);

		    this.HasRequired(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);

		    this.Ignore(x => x.Certificado);
		}
	}

}
