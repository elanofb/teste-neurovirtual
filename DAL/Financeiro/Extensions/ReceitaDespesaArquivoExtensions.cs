using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Financeiro.Entities;

namespace DAL.Financeiro {

    public static class ReceitaDespesaArquivoExtensions {

        //Calcular valor total das tarifas
        public static decimal valorTotalComDescontos(this ReceitaDespesaArquivoVW OLancamento) {

            decimal valorTotalDescontos = new decimal(0);

            if (OLancamento == null) {
                return valorTotalDescontos;
            }
            
            if (OLancamento.valorDesconto > 0) {

                valorTotalDescontos = decimal.Add(valorTotalDescontos, OLancamento.valorDesconto);

            }

            if (OLancamento.valorDescontoCupom > 0) {

                valorTotalDescontos = decimal.Add(valorTotalDescontos, OLancamento.valorDescontoCupom);

            }

            if (OLancamento.valorDescontoAntecipacao > 0) {

                valorTotalDescontos = decimal.Add(valorTotalDescontos, OLancamento.valorDescontoAntecipacao);

            }

            var valorTotal = OLancamento.valor - valorTotalDescontos;

            if (valorTotal < 0) {
                return new decimal(0);
            }

            return valorTotal;
        }     
        
        //Link completo da imagem
        public static string linkArquivo(this ReceitaDespesaArquivoVW OLancamento) {

            int idOrganizacao = OLancamento.idOrganizacao.toInt();

            string basePath = idOrganizacao > 0 ? UtilConfig.linkAbsSistemaUpload(idOrganizacao) : $"{UtilConfig.linkAbsSistema}upload/";
            
            return String.Concat(basePath, OLancamento.path);
            
        }
        
    }
}