using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Unidades;

namespace DAL.Unidades.Extensions {

    public static class UnidadeRotaExtensions {

        //Tratamento para tamanho de string
        public static bool flagExistemRotas(this UnidadeRota OUnidadeRota) {

            if (OUnidadeRota == null) {
                return false;
            }

            if (OUnidadeRota.flagSegunda == true ||
                OUnidadeRota.flagTerca == true ||
                OUnidadeRota.flagQuarta == true ||
                OUnidadeRota.flagQuinta == true ||
                OUnidadeRota.flagSexta == true ||
                OUnidadeRota.flagSabado == true ||
                OUnidadeRota.flagDomingo == true) {

                return true;
            }

            return false;
        }

        //Tratamento para tamanho de string
        public static bool flagTemRota(this UnidadeRota OUnidadeRota, DayOfWeek day) {

            if (OUnidadeRota == null) {
                return false;
            }

            if (OUnidadeRota.flagSegunda == true && day == DayOfWeek.Monday) {
                return true;
            }

            if (OUnidadeRota.flagTerca == true && day == DayOfWeek.Tuesday) {
                return true;
            }

            if (OUnidadeRota.flagQuarta == true && day == DayOfWeek.Wednesday) {
                return true;
            }

            if (OUnidadeRota.flagQuinta == true && day == DayOfWeek.Thursday) {
                return true;
            }

            if (OUnidadeRota.flagSexta == true && day == DayOfWeek.Friday) {
                return true;
            }

            if (OUnidadeRota.flagSabado == true && day == DayOfWeek.Saturday) {
                return true;
            }

            if (OUnidadeRota.flagDomingo == true && day == DayOfWeek.Sunday) {
                return true;
            }

            return false;
        }

        //Tratamento para tamanho de string
        public static bool flagTemRota(this UnidadeRota OUnidadeRota, List<DayOfWeek> listaDias) {

            if (OUnidadeRota == null) {
                return false;
            }

            if (OUnidadeRota.flagSegunda == true && listaDias.Contains(DayOfWeek.Monday)) {
                return true;
            }

            if (OUnidadeRota.flagTerca == true && listaDias.Contains(DayOfWeek.Tuesday)) {
                return true;
            }

            if (OUnidadeRota.flagQuarta == true && listaDias.Contains(DayOfWeek.Wednesday)) {
                return true;
            }

            if (OUnidadeRota.flagQuinta == true && listaDias.Contains(DayOfWeek.Thursday)) {
                return true;
            }

            if (OUnidadeRota.flagSexta == true && listaDias.Contains(DayOfWeek.Friday)) {
                return true;
            }

            if (OUnidadeRota.flagSabado == true && listaDias.Contains(DayOfWeek.Saturday)) {
                return true;
            }

            if (OUnidadeRota.flagDomingo == true && listaDias.Contains(DayOfWeek.Sunday)) {
                return true;
            }

            return false;
        }

        //Tratamento para tamanho de string
        public static DateTime retornarDataAgendamento(this UnidadeRota OUnidadeRota, DateTime dtAgendamento) {

            if (dtAgendamento.DayOfWeek == DayOfWeek.Monday && OUnidadeRota.flagSegunda == true) {

                return dtAgendamento;

            }

            if (dtAgendamento.DayOfWeek == DayOfWeek.Tuesday && OUnidadeRota.flagTerca == true) {

                return dtAgendamento;

            }

            if (dtAgendamento.DayOfWeek == DayOfWeek.Wednesday && OUnidadeRota.flagQuarta == true) {

                return dtAgendamento;

            }

            if (dtAgendamento.DayOfWeek == DayOfWeek.Thursday && OUnidadeRota.flagQuinta == true) {

                return dtAgendamento;

            }

            if (dtAgendamento.DayOfWeek == DayOfWeek.Friday && OUnidadeRota.flagSexta == true) {

                return dtAgendamento;

            }

            if (dtAgendamento.DayOfWeek == DayOfWeek.Saturday && OUnidadeRota.flagSabado == true) {

                return dtAgendamento;

            }

            if (dtAgendamento.DayOfWeek == DayOfWeek.Sunday && OUnidadeRota.flagDomingo == true) {

                return dtAgendamento;

            }

            dtAgendamento = dtAgendamento.AddDays(1);

            return OUnidadeRota.retornarDataAgendamento(dtAgendamento);

        }
    }
}
