using System;
using System.Linq;
using DAL.Configuracoes;

namespace BLL.Configuracoes {

	public interface IValidacaoSaqueBL {
			
		UtilRetorno validarOperacaoSaque(ConfiguracaoSaque OConfiguracao);
		
		bool validarHorarioSaque(string dtInicio, string dtFinal);		       
		
	}
}