using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;
using System;

namespace DAL.Associados {
    
    //
    public class MotivoDesativacao {

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public int id { get; set; }

        public string descricao { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime? dtAlteracao { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public bool? ativo { get; set; }

        public bool? flagExcluido { get; set; }

    }

    //
    internal sealed class MotivoDesativacaoMapper : EntityTypeConfiguration<MotivoDesativacao> {

        public MotivoDesativacaoMapper() {

            this.ToTable("tb_motivo_desativacao");

            this.HasKey(o => o.id);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

        }
    }
}