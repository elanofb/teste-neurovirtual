using System;
using System.Data.Entity;
using System.Linq;
using BLL.Caches;
using BLL.Services;
using DAL.Configuracoes;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Configuracoes {

    public class ValidacaoSaqueBL : DefaultBL, IValidacaoSaqueBL {
                                        
        public UtilRetorno validarOperacaoSaque(ConfiguracaoSaque OConfiguracao){
            
            UtilRetorno OUtilRetorno = new UtilRetorno{ flagError =  false};
            
            if (OConfiguracao.id.toInt() == 0){
                
                OUtilRetorno.flagError = true;
                OUtilRetorno.listaErros.Add("Não foi possível consultar a disponibilidade de saque!");

                return OUtilRetorno;
                
            }
            
            DateTime dtSolicitacao = DateTime.Now;
            
            DayOfWeek diaSemama = dtSolicitacao.DayOfWeek;
                        
            //Validação dos dias da semana 
            if (diaSemama == DayOfWeek.Sunday && OConfiguracao.flagDomingo != true){
                
                OUtilRetorno.flagError = true;
                OUtilRetorno.listaErros.Add("Período indisponível para saque!");

                return OUtilRetorno;
                
            }
            
            if (diaSemama == DayOfWeek.Monday && OConfiguracao.flagSegunda != true){
                
                OUtilRetorno.flagError = true;
                OUtilRetorno.listaErros.Add("Período indisponível para saque!");

                return OUtilRetorno;
                
            }
            
            if (diaSemama == DayOfWeek.Tuesday && OConfiguracao.flagTerca != true){
                
                OUtilRetorno.flagError = true;
                OUtilRetorno.listaErros.Add("Período indisponível para saque!");

                return OUtilRetorno;
                
            }
            
            if (diaSemama == DayOfWeek.Wednesday && OConfiguracao.flagQuarta != true){
                
                OUtilRetorno.flagError = true;
                OUtilRetorno.listaErros.Add("Período indisponível para saque!");

                return OUtilRetorno;
                
            }
            
            if (diaSemama == DayOfWeek.Thursday && OConfiguracao.flagQuinta != true){
                
                OUtilRetorno.flagError = true;
                OUtilRetorno.listaErros.Add("Período indisponível para saque!");

                return OUtilRetorno;
                                
            }
            
            if (diaSemama == DayOfWeek.Friday && OConfiguracao.flagSexta != true){
                
                OUtilRetorno.flagError = true;
                OUtilRetorno.listaErros.Add("Período indisponível para saque!");

                return OUtilRetorno;
                
            }
            
            if (diaSemama == DayOfWeek.Saturday && OConfiguracao.flagSabado != true){
                
                OUtilRetorno.flagError = true;
                OUtilRetorno.listaErros.Add("Período indisponível para saque!");

                return OUtilRetorno;
                
            }
            
            //Validação dos horários
            
            if (diaSemama == DayOfWeek.Sunday && !this.validarHorarioSaque(OConfiguracao.horarioInicioDomingo, OConfiguracao.horarioFimDomingo)){
                
                OUtilRetorno.flagError = true;
                OUtilRetorno.listaErros.Add("Horário indisponível para saque!");
                
                return OUtilRetorno;
                
            }
            
            if (diaSemama == DayOfWeek.Monday && !this.validarHorarioSaque(OConfiguracao.horarioInicioSegunda, OConfiguracao.horarioFimSegunda)){
                
                OUtilRetorno.flagError = true;
                OUtilRetorno.listaErros.Add("Horário indisponível para saque!");
                
                return OUtilRetorno;
                
            }
            
            if (diaSemama == DayOfWeek.Tuesday && !this.validarHorarioSaque(OConfiguracao.horarioInicioTerca, OConfiguracao.horarioFimTerca)){
                
                OUtilRetorno.flagError = true;
                OUtilRetorno.listaErros.Add("Horário indisponível para saque!");
                
                return OUtilRetorno;
                
            }
            
            if (diaSemama == DayOfWeek.Wednesday && !this.validarHorarioSaque(OConfiguracao.horarioInicioQuarta, OConfiguracao.horarioFimQuarta)){
                
                OUtilRetorno.flagError = true;
                OUtilRetorno.listaErros.Add("Horário indisponível para saque!");
                
                return OUtilRetorno;
                
            }
            
            if (diaSemama == DayOfWeek.Thursday && !this.validarHorarioSaque(OConfiguracao.horarioInicioQuinta, OConfiguracao.horarioFimQuinta)){
                
                OUtilRetorno.flagError = true;
                OUtilRetorno.listaErros.Add("Horário indisponível para saque!");
                
                return OUtilRetorno;
                
            }
            
            if (diaSemama == DayOfWeek.Friday && !this.validarHorarioSaque(OConfiguracao.horarioInicioSexta, OConfiguracao.horarioFimSexta)){
                
                OUtilRetorno.flagError = true;
                OUtilRetorno.listaErros.Add("Horário indisponível para saque!");
                
                return OUtilRetorno;
                
            }
            
            if (diaSemama == DayOfWeek.Saturday && !this.validarHorarioSaque(OConfiguracao.horarioInicioSabado, OConfiguracao.horarioFimSabado)){
                
                OUtilRetorno.flagError = true;
                OUtilRetorno.listaErros.Add("Horário indisponível para saque!");
                
                return OUtilRetorno;
                
            }

            return OUtilRetorno;

        }
        
        public bool validarHorarioSaque(string dtInicio, string dtFinal){
            
            if (dtInicio.isEmpty() || dtFinal.isEmpty()){
                return false;    
            }
            
            TimeSpan dtStart = TimeSpan.Parse(dtInicio);
            TimeSpan dtEnd = TimeSpan.Parse(dtFinal);            
            
            if (DateTime.Now.TimeOfDay >= dtStart && DateTime.Now.TimeOfDay <= dtEnd){
                return true;
            }
            
            return false;

        }
    }
}