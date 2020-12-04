using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Pessoas;

namespace DAL.Financeiro {

    //
    public class GestaoTituloVW {

        public Guid id { get; set; }

        public int? idPessoa { get; set; }

        public Pessoa Pessoa { get; set; }

        public int tipo { get; set; }

        public string descricao { get; set; }

        public int idTipoAssociado { get; set; }

        public string tipoAssociado { get; set; }

        public int idContaBancaria { get; set; }

        public string contaBancaria { get; set; }

        public string flagPago { get; set; }

        public string flagFixa { get; set; }

        public int idCentroCusto { get; set; }

        public string descCentroCusto { get; set; }

        public int idMacroConta { get; set; }

        public string descMacroConta { get; set; }

        public int idCategoria { get; set; }

        public string categoria { get; set; }

        public DateTime? dtVencimento { get; set; }

        public DateTime? dtPagamento { get; set; }

        public DateTime? dtCredito { get; set; }

        public decimal valor { get; set; }

        public decimal valorPago { get; set; }

        public string tipoMovimentacao { get; set; }

        public bool flagConciliado { get; set; }

        public int idTitulo { get; set; }

        public int idPagamento { get; set; }

        public int? nroNotaFiscal { get; set; }

        public string nomeDestinatario { get; set; }

        public string docDestinatario { get; set; }

        public bool flagArquivo { get; set; }

        public GestaoTituloVW() {
        }
    }

    //
    internal sealed class GestaoTituloVWMapper : EntityTypeConfiguration<GestaoTituloVW> {

        public GestaoTituloVWMapper() {

            this.ToTable("vw_gestao_titulo");
            this.HasKey(o => o.id);

            //Ignorar
            this.Ignore(x => x.flagArquivo);

			this.HasOptional(o => o.Pessoa).WithMany().HasForeignKey(o => o.idPessoa);
        }
    }
}