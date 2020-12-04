using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.RelatoriosFinanceiros {

    //
    public class ResultadoDREVW {

        public Guid id { get; set; }
        
        public int idPagamento { get; set; }
        
        public int idTitulo { get; set; }
        
        public int? idOrganizacao { get; set; }
        
        public string flagTipoTitulo { get; set; }
        
        public byte idTipoTitulo { get; set; }
        
        public string descricaoTipoTitulo { get; set; }
        
        public int idCentroCusto { get; set; }
        
        public string descricaoCentroCusto { get; set; }
        
        public int idMacroConta { get; set; }
        
        public string descricaoMacroConta { get; set; }
        
        public int? idCentroCustoDRE { get; set; }
        
        public int idSubConta { get; set; }
        
        public string descricaoSubConta { get; set; }
        
        public int? idSubContaPai { get; set; }
        
        public string descricaoSubContaPai { get; set; }
        
        public DateTime? dtCompetencia { get; set; }
        
        public byte? mesCompetencia { get; set; }

        public short? anoCompetencia { get; set; }
        
        public DateTime? dtCadastro { get; set; }
        
        public decimal valor { get; set; }
        
        public decimal valorRealizado { get; set; }
        
        public decimal valorDesconto { get; set; }
        
        public decimal valorDescontoCupom { get; set; }
        
        public decimal valorDescontoAntecipacao { get; set; }
        
        public decimal valorTarifasBancarias { get; set; }
        
        public decimal valorTarifasTransacao { get; set; }
        
        public decimal percentualICMS { get; set; }
        
        public decimal valorICMS { get; set; }
        
        public decimal percentualPIS { get; set; }
        
        public decimal valorPIS { get; set; }
        
        public decimal percentualCOFINS { get; set; }
        
        public decimal valorCOFINS { get; set; }
        
        public decimal percentualISS { get; set; }
        
        public decimal valorISS { get; set; }

        //
        public ResultadoDREVW() {
            
        }
        
    }

    internal sealed class ResultadoDREVWMapper : EntityTypeConfiguration<ResultadoDREVW> {

        public ResultadoDREVWMapper() {

            this.ToTable("vw_resultado_dre");
            
            this.HasKey(o => o.id);

        }
    }
}