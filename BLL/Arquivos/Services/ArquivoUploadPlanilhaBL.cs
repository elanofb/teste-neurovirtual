using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Web;
using DAL.Arquivos;

namespace BLL.Arquivos {

	public class ArquivoUploadPlanilhaBL : ArquivoUploadPadraoBL {
        
		//Construtor
		public ArquivoUploadPlanilhaBL() : base(ArquivoUploadTypes.PLANILHA) {

		}
        
	}

}