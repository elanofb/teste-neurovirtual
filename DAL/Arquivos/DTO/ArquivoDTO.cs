using System.Web;

namespace DAL.Arquivos {

	public class ArquivoDTO {
		
		public HttpPostedFileBase FileUpload { get; set; }
		
		public string legenda { get; set; }
		
	}
}