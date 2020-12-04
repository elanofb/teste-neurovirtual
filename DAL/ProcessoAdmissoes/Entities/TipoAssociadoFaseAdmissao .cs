using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Associados;
using DAL.DocumentosDigitais;
using DAL.Permissao;

namespace DAL.ProcessoAdmissoes {

	//
	public class TipoAssociadoFaseAdmissao {

		public int id { get; set; }
        
        public int idTipoAssociado { get; set; }

        public virtual TipoAssociado TipoAssociado { get; set; }

        public byte idTipoFaseAdmissao { get; set; }

	    public virtual TipoFaseAdmissao TipoFaseAdmissao { get; set; }

	    public byte ordem { get; set; }

	    public int? idDocumentoDigital { get; set; }

	    public virtual DocumentoDigital DocumentoDigital { get; set; }

        public DateTime dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public virtual UsuarioSistema UsuarioCadastro { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

	}

	//
	internal sealed class TipoAssociadoFaseAdmissaoMapper : EntityTypeConfiguration<TipoAssociadoFaseAdmissao> {

		public TipoAssociadoFaseAdmissaoMapper() {

			this.ToTable("tb_tipo_associado_fase_admissao");

            this.HasKey(o => o.id);

		    this.HasRequired(x => x.TipoAssociado).WithMany().HasForeignKey(x => x.idTipoAssociado);

		    this.HasRequired(x => x.TipoFaseAdmissao).WithMany().HasForeignKey(x => x.idTipoFaseAdmissao);

		    this.HasOptional(x => x.DocumentoDigital).WithMany().HasForeignKey(x => x.idDocumentoDigital);

            this.HasOptional(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);
            
		}
	}
}