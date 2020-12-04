using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DAL.Associados;
using DAL.Permissao;
using DAL.Pessoas;

namespace DAL.Atendimentos {

    [Serializable]
    public class Atendimento {

        public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public int? idTipoAtendimento { get; set; }

        public virtual AtendimentoTipo AtendimentoTipo { get; set; }

        public int? idAreaAtendimento { get; set; }

        public virtual AtendimentoArea AtendimentoArea { get; set; }

        public int idStatusAtendimento { get; set; }

        public virtual AtendimentoStatus AtendimentoStatus { get; set; }

        public int? idStatusAtendimentoAnterior { get; set; }

        public virtual AtendimentoStatus AtendimentoStatusAnterior { get; set; }

        public int? idNaoAssociado { get; set; }

        public virtual Associado NaoAssociado { get; set; }
        
        public int? idAssociado { get; set; }

        public virtual Associado Associado { get; set; }

        public int? idPessoa { get; set; }

        public virtual Pessoa Pessoa { get; set; }

        public string titulo { get; set; }
        
        public string nome { get; set; }
        
        public string nroDocumento { get; set; }

        public string email { get; set; }

        public string nroTelefone { get; set; }

        public string mensagem { get; set; }

        public DateTime? dtInicioAtendimento { get; set; }

        public DateTime? dtFinalizacaoAtendimento { get; set; }

        public bool? flagAtendido { get; set; }

        public int? idUsuarioInicioAtendimento { get; set; }

        public virtual UsuarioSistema UsuarioInicioAtendimento { get; set; }

        public int? idUltimoUsuarioAtendimento { get; set; }

        public virtual UsuarioSistema UltimoUsuarioAtendimento { get; set; }

        public DateTime? dtUltimoAtendimento { get; set; }

        public DateTime? dtUltimaInteracao { get; set; }

        public int idUsuarioCadastro { get; set; }

        public DateTime dtCadastro { get; set; }

        public int idUsuarioAlteracao { get; set; }

        public DateTime dtAlteracao { get; set; }

        public bool? ativo { get; set; }

        public bool? flagExcluido { get; set; }

        public bool? flagSistema { get; set; }

        public decimal? valor { get; set; }

    }

    internal sealed class AtendimentoMapper : EntityTypeConfiguration<Atendimento> {

        public AtendimentoMapper() {

            this.ToTable("tb_atendimento");

            this.HasKey(o => o.id);

            this.Property(o => o.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.HasRequired(x => x.AtendimentoStatus).WithMany().HasForeignKey(x => x.idStatusAtendimento);

            this.HasOptional(x => x.AtendimentoStatusAnterior).WithMany().HasForeignKey(x => x.idStatusAtendimentoAnterior);

            this.HasOptional(x => x.AtendimentoTipo).WithMany().HasForeignKey(x => x.idTipoAtendimento);

            this.HasOptional(x => x.AtendimentoArea).WithMany().HasForeignKey(x => x.idAreaAtendimento);

            this.HasOptional(x => x.Associado).WithMany().HasForeignKey(x => x.idAssociado);
            
            this.HasOptional(x => x.NaoAssociado).WithMany().HasForeignKey(x => x.idNaoAssociado);

            this.HasOptional(x => x.Pessoa).WithMany().HasForeignKey(x => x.idPessoa);

            this.HasOptional(x => x.UsuarioInicioAtendimento).WithMany().HasForeignKey(x => x.idUsuarioInicioAtendimento);

            this.HasOptional(x => x.UltimoUsuarioAtendimento).WithMany().HasForeignKey(x => x.idUltimoUsuarioAtendimento);

        }
    }
}