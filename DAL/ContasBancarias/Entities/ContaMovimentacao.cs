using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.ContasBancarias {

    public class ContaBancariaMovimentacao : Geral {

        public int? idOrganizacao { get; set; }

        public int? idUnidade { get; set; }

        public int idTipoOperacao { get; set; }

        public virtual ContaTipoOperacao ContaTipoOperacao { get; set; }

        public int idContaBancariaOrigem { get; set; }

        public virtual ContaBancaria ContaBancariaOrigem { get; set; }

        public int idContaBancariaDestino { get; set; }

        public virtual ContaBancaria ContaBancariaDestino { get; set; }

        public DateTime dtOperacao { get; set; }

        public decimal valor { get; set; }

        public bool flagConciliado { get; set; }

        public string observacoes { get; set; }

    }

    internal sealed class ContaBancariaMovimentacaoMapper : EntityTypeConfiguration<ContaBancariaMovimentacao> {

        public ContaBancariaMovimentacaoMapper() {

            this.ToTable("tb_conta_bancaria_movimentacao");

            this.HasKey(x => x.id);

            this.HasRequired(x => x.ContaBancariaOrigem).WithMany(o => o.listaMovimentacoesOrigem).HasForeignKey(x => x.idContaBancariaOrigem);

            this.HasRequired(x => x.ContaBancariaDestino).WithMany(o => o.listaMovimentacoesDestino).HasForeignKey(x => x.idContaBancariaDestino);

            this.HasRequired(x => x.ContaTipoOperacao).WithMany().HasForeignKey(x => x.idTipoOperacao);

        }
    }
}