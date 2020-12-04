
namespace DAL.Financeiro {

   public class ImportacaoFinanceiroDTO {
        public string ano { get;set;}
        public string mes { get;set;}
        public string dia { get;set;}
        public string flagFixa { get;set;}
        public string descricao { get;set;}
        public string centroCusto { get;set;}
        public string macroConta { get;set;}
        public string categoria { get;set;}
        public string tipoCategoria { get;set;}
        public string detalheCategoria { get;set;}
        public string qtdeParcela { get;set;}
        public string formaPagamento { get;set;}
        public string descricaoFormaPagamento { get;set;}
        public string valor { get;set;}
    }

     public class ImportacaoFinanceiroReceitaDTO {
        public string flagFoiPago { get;set;}
        public string dtRecebimento { get;set;}
        public string descricao { get;set;}
        public string dtVencimento { get;set;}
        public string valor { get;set;}
        public string centroCusto { get;set;}
        public string macroConta { get;set;}
        public string categoria { get;set;}
        public string tipoCategoria { get;set;}
        public string detalheCategoria { get;set;}
    }
}
