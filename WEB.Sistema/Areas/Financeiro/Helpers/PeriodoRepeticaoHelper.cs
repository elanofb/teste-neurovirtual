using System;
using System.Web.Mvc;
using BLL.Financeiro;
using System.Linq;
using DAL.Financeiro;

namespace WEB.Areas.Financeiro.Helpers{
    public class PeriodoRepeticaoHelper{

        //Atributos
        private static IPeriodoRepeticaoBL _PeriodoRepeticaoBL;
        //Propriedades
        private static IPeriodoRepeticaoBL OPeriodoRepeticaoBL { get { return (_PeriodoRepeticaoBL = _PeriodoRepeticaoBL ?? new PeriodoRepeticaoBL());} }

        //Construtor
        public PeriodoRepeticaoHelper() {
        }

        //
        public static SelectList selectList(int? selected){

            var list = OPeriodoRepeticaoBL.listar("","S");
            list = list.Where(x => x.id != 1).OrderBy(x => x.descricao);

            if(selected == null) selected = 4;

            return new SelectList(list, "id", "descricao", selected);
        }

        //Retorna uma data acrescentando dias, meses ou anos de acordo com o periodo de repeticao.
        public static DateTime getProximaDatePorPeriodo(DateTime data, int idPeriodoRepeticao) { 
                
            DateTime newData = data;

            switch (idPeriodoRepeticao) { 

                case (int)TipoPeriodoRepeticaoEnum.SEMANALMENTE:
                    newData = newData.AddDays(7);
                    break;

                case (int)TipoPeriodoRepeticaoEnum.MENSALMENTE:
                    newData = newData.AddMonths(1);
                    break;

                case (int)TipoPeriodoRepeticaoEnum.BIMESTRALMENTE:
                    newData = newData.AddMonths(2);
                    break;

                case (int)TipoPeriodoRepeticaoEnum.TRIMESTRALMENTE:
                    newData = newData.AddMonths(3);
                    break;

                case (int)TipoPeriodoRepeticaoEnum.SEMESTRALMENTE:
                    newData = newData.AddMonths(6);
                    break;

                case (int)TipoPeriodoRepeticaoEnum.ANUALMENTE:
                    newData = newData.AddYears(1);
                    break;
            }

            return newData;
        }

        //Retorna uma data acrescentando dias, meses ou anos de acordo com o periodo de repeticao.
        public static DateTime getUltimaDatePorPeriodo(DateTime data, int idPeriodoRepeticao, int qtdeRepeticao) { 
                
            var newData = data;

            switch (idPeriodoRepeticao) { 

                case (int)TipoPeriodoRepeticaoEnum.SEMANALMENTE:
                    newData = newData.AddDays(7 * qtdeRepeticao);
                    break;

                case (int)TipoPeriodoRepeticaoEnum.MENSALMENTE:
                    newData = newData.AddMonths(1 * qtdeRepeticao);
                    break;

                case (int)TipoPeriodoRepeticaoEnum.BIMESTRALMENTE:
                    newData = newData.AddMonths(2 * qtdeRepeticao);
                    break;

                case (int)TipoPeriodoRepeticaoEnum.TRIMESTRALMENTE:
                    newData = newData.AddMonths(3 * qtdeRepeticao);
                    break;

                case (int)TipoPeriodoRepeticaoEnum.SEMESTRALMENTE:
                    newData = newData.AddMonths(6 * qtdeRepeticao);
                    break;

                case (int)TipoPeriodoRepeticaoEnum.ANUALMENTE:
                    newData = newData.AddYears(1 * qtdeRepeticao);
                    break;
            }

            return newData;
        }

    }
}