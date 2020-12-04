using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Associados;
using DAL.DocumentosDigitais;
using DAL.Permissao;

namespace DAL.ProcessoAdmissoes {

	//
	public class TipoAssociadoDocumentoAdmissao {

		public int id { get; set; }
        
        public int idTipoAssociado { get; set; }

        public virtual TipoAssociado TipoAssociado { get; set; }

	    public string descricaoDocumento { get; set; }

	    public bool? flagObrigatorio { get; set; }

	    public bool? flagAprovacao { get; set; }

        public DateTime dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public virtual UsuarioSistema UsuarioCadastro { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

	}

	//
	internal sealed class TipoAssociadoDocumentoAdmissaoMapper : EntityTypeConfiguration<TipoAssociadoDocumentoAdmissao> {

		public TipoAssociadoDocumentoAdmissaoMapper() {

			this.ToTable("tb_tipo_associado_documento_admissao");

            this.HasKey(o => o.id);

		    this.HasRequired(x => x.TipoAssociado).WithMany().HasForeignKey(x => x.idTipoAssociado);

            this.HasOptional(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);
            
		}
	}
}