using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Associados;
using DAL.Permissao;

namespace DAL.ProcessoAdmissoes {

	//
	public class AssociadoProcessoAdmissao {

		public int id { get; set; }
        
        public int idAssociado { get; set; }

        public virtual Associado Associado { get; set; }

	    public int idFaseAdmissao { get; set; }

        public virtual TipoAssociadoFaseAdmissao FaseAdmissao { get; set; }

	    public DateTime? dtConclusao { get; set; }        
        
        public DateTime dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public virtual UsuarioSistema UsuarioCadastro { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

	}

	//
	internal sealed class AssociadoProcessoAdmissaoMapper : EntityTypeConfiguration<AssociadoProcessoAdmissao> {

	    public AssociadoProcessoAdmissaoMapper() {
	        
	        this.ToTable("tb_associado_processo_admissao");

            this.HasKey(o => o.id);

	        this.HasRequired(x => x.Associado).WithMany().HasForeignKey(x => x.idAssociado);

	        this.HasRequired(x => x.FaseAdmissao).WithMany().HasForeignKey(x => x.idFaseAdmissao);

            this.HasOptional(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);
            
		}
	}
}