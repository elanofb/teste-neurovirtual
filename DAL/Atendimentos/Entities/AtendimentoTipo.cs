using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Atendimentos {

    [Serializable]
    public class AtendimentoTipo {

        public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public string descricao { get; set; }

        public bool? flagOnline { get; set; }

        public int idUsuarioCadastro { get; set; }

        public DateTime dtCadastro { get; set; }

        public int idUsuarioAlteracao { get; set; }

        public DateTime dtAlteracao { get; set; }

        public bool? ativo { get; set; }

        public bool? flagExcluido { get; set; }

        public bool? flagSistema { get; set; }

    }

    internal sealed class AtendimentoTipoMapper : EntityTypeConfiguration<AtendimentoTipo> {

        public AtendimentoTipoMapper() {

            this.ToTable("tb_atendimento_tipo");

            this.HasKey(o => o.id);

            this.Property(o => o.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

        }
    }
}