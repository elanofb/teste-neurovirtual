using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Pessoas;
using DAL.CuponsDesconto;

namespace DAL.Planos {

    //
    public class PlanoContratacao : DefaultEntity {

        public int? idOrganizacao { get; set; }
        public int idPlano { get; set; }
        public Plano Plano { get; set; }
        public int idPessoa { get; set; }
        public Pessoa Pessoa { get; set; }
        public int idStatus { get; set; }
        public StatusPlanoContratacao Status { get; set; }
        public int? idCupomDesconto { get; set; }
		public virtual CupomDesconto CupomDesconto { get; set; }
        public decimal valor { get; set; }
        public DateTime? dtAprovacao { get; set; }
        public DateTime? dtTermino { get; set; }
        public DateTime? dtPagamento { get; set; }

        public PlanoContratacao() {
        }

    }

    //
    internal sealed class PlanoContratacaoMapper : EntityTypeConfiguration<PlanoContratacao> {

        public PlanoContratacaoMapper() {
            this.ToTable("tb_plano_contratacao");
            this.HasKey(o => o.id);

            this.HasRequired(x => x.Plano).WithMany().HasForeignKey(x => x.idPlano);
            this.HasRequired(x => x.Pessoa).WithMany().HasForeignKey(x => x.idPessoa);
            this.HasRequired(x => x.Status).WithMany().HasForeignKey(x => x.idStatus);
            this.HasOptional(x => x.CupomDesconto).WithMany().HasForeignKey(x => x.idCupomDesconto);
        }
    }
}
