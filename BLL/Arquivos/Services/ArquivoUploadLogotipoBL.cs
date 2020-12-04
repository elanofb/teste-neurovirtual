using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Web;
using DAL.Arquivos;

namespace BLL.Arquivos {

	public class ArquivoUploadLogotipoBL : ArquivoUploadPadraoBL {
        
		//Construtor
		public ArquivoUploadLogotipoBL() : base(ArquivoUploadTypes.LOGOTIPO) {

		}
        
	}

}