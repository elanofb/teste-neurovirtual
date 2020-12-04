using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Associados {

    public class AssociadoTipoCadastro {

        public byte id { get; set; }

        public string descricao { get; set; }

        public int idUsuarioCadastro { get; set; }

        public DateTime dtCadastro { get; set; }

        public bool ativo { get; set; }

        public bool flagExcluido { get; set; }
    }

    internal sealed class AssociadoTipoCadastroMapper : EntityTypeConfiguration<AssociadoTipoCadastro> {

        public AssociadoTipoCadastroMapper() {

            this.ToTable("tb_associado_tipo_cadastro");

            this.HasKey(o => o.id);

        }
    }
}