using System.Web.Mvc;
using BLL.Documentos;

namespace WEB.Helpers{
    public class CategoriaDocumentoHelper{

        private static CategoriaDocumentoBL _CategoriaDocumentoBL;

        public static CategoriaDocumentoBL getService(){
            if(_CategoriaDocumentoBL == null){
                _CategoriaDocumentoBL = new CategoriaDocumentoBL();
            }
            return _CategoriaDocumentoBL;
        }

        //
        public static SelectList selectList(int? selected){
            var list = getService().listar("");
            return new SelectList(list, "id", "descricao", selected);
        }

    }
}