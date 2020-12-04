using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.BoletosBancarios {

    //
    public class BoletoLayout {

        public int id { get; set; }

        public string descricao { get; set; }

        public bool? ativo { get; set; }

        public DateTime? dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }
    }


    //
    internal sealed class BoletoLayoutMapper : EntityTypeConfiguration<BoletoLayout> {

        public BoletoLayoutMapper() {

            this.ToTable("datatb_boleto_layout");

            this.HasKey(o => o.id);

        }
    }
}