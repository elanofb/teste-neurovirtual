using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Financeiro.Entities;

namespace DAL.Financeiro {

    public static class DescontoAntecipacaoExtensions {

        //
        public static string descricaoDescontoBoletos(this List<TituloReceitaDescontoAntecipacao> listaDescontos) {

            if (!listaDescontos.Any()) {

                return "";

            }

            string descricao = "";

            foreach (var ODesconto in listaDescontos) {

                descricao = string.Concat(descricao, $"Conceder desconto de {ODesconto.valor.ToString("C")} até {ODesconto.dtLimiteDesconto.exibirData()}<br />");
            }


            return descricao;
        }

        /// <summary>
        /// Retornar a lista de descontos removendo os registros inválidos
        /// </summary>
        /// <returns></returns>
        public static List<TituloReceitaDescontoAntecipacao> retornarDescontosAntecipacao(this List<TituloReceitaDescontoAntecipacao> listaDescontosAntecipacao,DateTime dtBase) {

            var lista = listaDescontosAntecipacao.Where(x => x.dtExclusao == null && x.dtLimiteDesconto >= dtBase)
                                                 .OrderBy(x => x.dtLimiteDesconto)
                                                 .ToList();

            return lista;
        }    
    }
}