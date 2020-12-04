using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DAL.Permissao;

namespace DAL.Atendimentos {

    [Serializable]
    public class AtendimentoHistorico {

        public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public int idAtendimento { get; set; }

        public virtual Atendimento Atendimento { get; set; }

        public int? idStatusAtendimento { get; set; }

        public virtual AtendimentoStatus AtendimentoStatus { get; set; }

        public string nome { get; set; }

        public string mensagem { get; set; }
        
        public int idUsuarioCadastro { get; set; }

        public DateTime dtCadastro { get; set; }

        public int idUsuarioAlteracao { get; set; }

        public DateTime dtAlteracao { get; set; }

        public bool? ativo { get; set; }

        public bool? flagExcluido { get; set; }

        public bool? flagAtendido { get; set; }

    }

    internal sealed class AtendimentoHistoricoMapper : EntityTypeConfiguration<AtendimentoHistorico> {

        public AtendimentoHistoricoMapper() {

            this.ToTable("tb_atendimento_historico");

            this.HasKey(o => o.id);

            this.Property(o => o.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Ignore(x => x.flagAtendido);

            this.HasRequired(x => x.Atendimento).WithMany().HasForeignKey(x => x.idAtendimento);

            this.HasOptional(x => x.AtendimentoStatus).WithMany().HasForeignKey(x => x.idStatusAtendimento);
            
        }
    }
}