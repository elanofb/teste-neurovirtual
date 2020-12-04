using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Associados {

    public class MembroSaldo{

        public int id { get; set; }
        
        public int idOrganizacao { get; set; }

        public int idMembro { get; set; }
        
        public int? idPessoa { get; set; }
        
        public Associado Membro { get; set; }
        
        public decimal saldoAtual { get; set; }
        
        public DateTime? dtAtualizacaoSaldo { get; set; }
    }

    internal sealed class MembroSaldoMapper : EntityTypeConfiguration<MembroSaldo> {

        public MembroSaldoMapper() {

            this.ToTable("tb_membro_saldo");

            this.HasKey(o => o.id);

            this.Property(x => x.saldoAtual).HasPrecision(18, 4);
            
            this.HasRequired(x => x.Membro).WithMany().HasForeignKey(x => x.idMembro);
            
        }
    }

}


