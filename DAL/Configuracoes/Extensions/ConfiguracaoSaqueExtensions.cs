
using System;
using System.Text;

namespace DAL.Configuracoes {

	public static class ConfiguracaoSaqueExtensions {

        /// <summary>
        /// 
        /// </summary>
        public static string exibirPeriodoSaque(this ConfiguracaoSaque OConfiguracaoSaque){

	        StringBuilder texto = new StringBuilder();  
			
	        if (OConfiguracaoSaque.flagDomingo == true && !OConfiguracaoSaque.horarioInicioDomingo.isEmpty() && !OConfiguracaoSaque.horarioFimDomingo.isEmpty()){
		        texto.Append($"Domingo das {OConfiguracaoSaque.horarioInicioDomingo} até {OConfiguracaoSaque.horarioFimDomingo}").Append(Environment.NewLine);
	        }
	        
	        if (OConfiguracaoSaque.flagSegunda == true && !OConfiguracaoSaque.horarioInicioSegunda.isEmpty() && !OConfiguracaoSaque.horarioFimSegunda.isEmpty()){
		        texto.Append($"Segunda-feira das {OConfiguracaoSaque.horarioInicioSegunda} até {OConfiguracaoSaque.horarioFimSegunda}").Append(Environment.NewLine);
	        }
	        
	        if (OConfiguracaoSaque.flagTerca == true && !OConfiguracaoSaque.horarioInicioTerca.isEmpty() && !OConfiguracaoSaque.horarioFimTerca.isEmpty()){
		        texto.Append($"Terça-feira das {OConfiguracaoSaque.horarioInicioTerca} até {OConfiguracaoSaque.horarioFimTerca}").Append(Environment.NewLine);
	        }
	        
	        if (OConfiguracaoSaque.flagQuarta == true && !OConfiguracaoSaque.horarioInicioQuarta.isEmpty() && !OConfiguracaoSaque.horarioFimQuarta.isEmpty()){
		        texto.Append($"Quarta-feira das {OConfiguracaoSaque.horarioInicioQuarta} até {OConfiguracaoSaque.horarioFimQuarta}").Append(Environment.NewLine);
	        }
	        
	        if (OConfiguracaoSaque.flagQuinta == true && !OConfiguracaoSaque.horarioInicioQuinta.isEmpty() && !OConfiguracaoSaque.horarioFimQuinta.isEmpty()){
		        texto.Append($"Quinta-feira das {OConfiguracaoSaque.horarioInicioQuinta} até {OConfiguracaoSaque.horarioFimQuinta}").Append(Environment.NewLine);
	        }
	        
	        if (OConfiguracaoSaque.flagSexta == true && !OConfiguracaoSaque.horarioInicioSexta.isEmpty() && !OConfiguracaoSaque.horarioFimSexta.isEmpty()){
		        texto.Append($"Sexta-feira das {OConfiguracaoSaque.horarioInicioSexta} até {OConfiguracaoSaque.horarioFimSexta}").Append(Environment.NewLine);
	        }
	        
	        if (OConfiguracaoSaque.flagSabado == true && !OConfiguracaoSaque.horarioInicioSabado.isEmpty() && !OConfiguracaoSaque.horarioFimSabado.isEmpty()){
		        texto.Append($"Sábado das {OConfiguracaoSaque.horarioInicioSabado} até {OConfiguracaoSaque.horarioFimSabado}").Append(Environment.NewLine);
	        }
			
	        return texto.ToString();
	        
        }	
        
	}
}