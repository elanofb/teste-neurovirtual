using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Web;
using DAL.Arquivos;

namespace BLL.Arquivos {

	public class ArquivoUploadDocumentoBL : ArquivoUploadPadraoBL {
        
		//Construtor
		public ArquivoUploadDocumentoBL() : base(ArquivoUploadTypes.DOCUMENTO) {

		}
        
	}

}