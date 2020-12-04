using BLL.DocumentosDigitais;
using System.Linq;
using System.Web.Mvc;
using UTIL.UtilClasses;

namespace WEB.Areas.DocumentosDigitais.Helpers {

    public class DocumentoDigitalHelper {

        //Constanctes
	    private static DocumentoDigitalHelper _instance;
		private IDocumentoDigitalBL _DocumentoDigitalBL;

        //Atributos

        //Propriedades
	    public static DocumentoDigitalHelper getInstance => _instance = _instance ?? new DocumentoDigitalHelper();
        private IDocumentoDigitalBL ODocumentoDigitalBL => _DocumentoDigitalBL = _DocumentoDigitalBL ?? new DocumentoDigitalBL();

		//
		public SelectList selectList(int? selected, string tipoPessoa, int? idTipoDocumento = null, int?[] idsExclude = null) {
            
		    var query = this.ODocumentoDigitalBL.listar("", 0, tipoPessoa, true);
            
            if (idTipoDocumento > 0) {
                query = query.Where(x => x.idTipoDocumentoDigital == idTipoDocumento);
            }

            if (idsExclude?.Length > 0) {
                query = query.Where(x => !idsExclude.Contains(x.id));
            }

		    var lista = query.Select(x => new OptionSelect{ value = x.id.ToString(), text = x.titulo })
                             .OrderBy(x => x.text).ToList();

            return new SelectList(lista, "value", "text", selected);
		}
    }
}