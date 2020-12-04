using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Financeiro {

    public static class TituloDespesaExtensions {

        //
        public static decimal valorLiquido(this TituloDespesa OTitulo) {

            decimal valorLiquido = Decimal.Subtract(UtilNumber.toDecimal(OTitulo.valorTotal), UtilNumber.toDecimal(OTitulo.valorTarifas()));

            return valorLiquido;
        }

        public static decimal valorTarifas(this TituloDespesa OTitulo) {
            
            var valorOutrasTarifas = OTitulo.listaTituloDespesaPagamento.Sum(x => x.valorOutrasTarifas);

            decimal valorTarifas = valorOutrasTarifas;//

            return valorTarifas;
        }
        
        //
        public static decimal valorTotalComDesconto(this TituloDespesa OTitulo) {

            decimal valorTotal = new decimal(0);

            if (OTitulo == null) {
                return valorTotal;
            }

            var listaPagamentos = OTitulo.retornarListaPagamentos();

            decimal valorTotalJuros = listaPagamentos.Sum(x => x.valorJuros ?? 0);

            decimal valorTotalDesconto = listaPagamentos.Sum(x => x.valorDesconto ?? 0);

            valorTotal = decimal.Add(OTitulo.valorTotal.toDecimal(), valorTotalJuros);

            valorTotal = decimal.Subtract(valorTotal, valorTotalDesconto);

            return valorTotal;
        }

        public static string descricaoCategoriaPessoa(this TituloDespesa OTitulo) {

            switch (OTitulo.flagCategoriaPessoa) {
                case "AS":
                    return "Associado";
                case "FO":
                    return "Fornecedor";
                case "FU":
                    return "Funcionário";
                case "PA":
                    return "Patrocinador";
                default:
                    return "";
            }
        }
    }
}