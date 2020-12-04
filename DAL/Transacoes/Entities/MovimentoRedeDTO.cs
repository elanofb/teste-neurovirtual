using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Transacoes {

    public class MovimentoRedeDTO {

        public List<Movimento> listaMovimentoRede { get; set; }
        
        public decimal percentualDistribuido { get; set; }

        public decimal valorDistribuido { get; set; }
        
        /// <summary>
        /// Valores que estao configurados para comissão, mas nao existe nenhum membro no nível em questao
        /// </summary>
        public decimal valorSemDestino { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public decimal carregarValorDistribuido() {

            valorDistribuido = listaMovimentoRede.Where(x => x.idMembroDestino.toInt() > 0 && x.valor > 0)
                                                 .Select(x => x.valor.toDecimal())
                                                 .DefaultIfEmpty(0)
                                                 .Sum();

            return valorDistribuido;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal carregarPercentualDistribuido() {

            percentualDistribuido = listaMovimentoRede.Where(x => x.idMembroDestino.toInt() > 0 && x.percentualMovimentoPrincipal > 0)
                                                 .Select(x => x.percentualMovimentoPrincipal.toDecimal())
                                                 .DefaultIfEmpty(0)
                                                 .Sum();

            return percentualDistribuido;
        }       
        
        /// <summary>
        /// 
        /// </summary>
        public decimal carregarValorSemDestino() {

            valorSemDestino = listaMovimentoRede.Where(x => x.idMembroDestino.toInt() == 0 && x.valor > 0)
                                                      .Select(x => x.valor.toDecimal())
                                                      .DefaultIfEmpty(0)
                                                      .Sum();

            return valorSemDestino;
        }           
    }

}
