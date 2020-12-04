using System.Web.Mvc;
using BLL.DocumentosDigitais;
using System.Linq;
using UTIL.UtilClasses;

namespace WEB.Areas.DocumentosDigitais.Helpers{

    public class TipoDocumentoDigitalHelper{

        //Constanctes
        private static TipoDocumentoDigitalHelper _instance;
        private ITipoDocumentoDigitalBL _TipoDocumentoDigitalBL;

        //Atributos

        //Propriedades
        public static TipoDocumentoDigitalHelper getInstance => _instance = _instance ?? new TipoDocumentoDigitalHelper();
        private ITipoDocumentoDigitalBL OTipoDocumentoDigitalBL => _TipoDocumentoDigitalBL = _TipoDocumentoDigitalBL ?? new TipoDocumentoDigitalBL();

        //
        public SelectList selectList(int? selected) {

            var lista = this.OTipoDocumentoDigitalBL.listar("", true)
                .Select(x => new OptionSelect { value = x.id.ToString(), text = x.descricao })
                .ToList();

            lista = lista.OrderBy(x => x.text).ToList();

            return new SelectList(lista, "value", "text", selected);
        }
    }
}