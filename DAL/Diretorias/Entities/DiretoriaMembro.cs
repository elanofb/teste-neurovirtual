using System.Data.Entity.ModelConfiguration;
using DAL.Associados;
using DAL.Cargos;
using DAL.Arquivos;
using System;

namespace DAL.Diretorias {
	
    //
	public partial class DiretoriaMembro {

        public int id { get; set; }

        public int? idDiretoria { get; set; }

        public virtual Diretoria Diretoria { get; set; }

        public int? idAssociado { get; set; }

        public virtual Associado Associado { get; set; }

        public int? idCargo { get; set; }

        public virtual Cargo Cargo { get; set; }

        public string nomeMembro { get; set; }

        public string nroDocumentoMembro { get; set; }

        public string telPrincipal { get; set; }

        public string email { get; set; }

        public string biografia { get; set; }

        public bool? flagPresidente { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime? dtAlteracao { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public bool? ativo { get; set; }

        public bool? flagExcluido { get; set; }

        public virtual ArquivoUpload Arquivo { get; set; }
    }

	//
	internal sealed class DiretoriaMembroMapper : EntityTypeConfiguration<DiretoriaMembro> {

		public DiretoriaMembroMapper() {
			this.ToTable("tb_diretoria_membro");
			this.HasKey(o => o.id);

            this.HasOptional(o => o.Diretoria).WithMany().HasForeignKey(o => o.idDiretoria);

            this.HasOptional(o => o.Associado).WithMany().HasForeignKey(o => o.idAssociado);

            this.HasOptional(o => o.Cargo).WithMany().HasForeignKey(o => o.idCargo);

            this.Ignore(m => m.Arquivo);
        }
	}
}