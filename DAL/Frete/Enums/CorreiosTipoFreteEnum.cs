using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Frete {

	public enum CorreiosTipoFreteEnum { 
		//PAC = 41068, - PAC com contrato

		//SEDEX = 40096 - SEDEX com contrato
	
		PAC = 41106, //PAC sem contrato

		SEDEX = 40010 // SEDEX sem contrato
	}

}