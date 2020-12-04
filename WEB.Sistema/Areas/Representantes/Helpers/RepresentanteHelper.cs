using System.Web.Mvc;
using System.Linq;
using BLL.Representantes;

namespace WEB.Areas.Representantes.Helpers {
    
    public class RepresentanteHelper {

        //Atributos
        private IRepresentanteConsultaBL _IRepresentanteConsultaBL;
        private static RepresentanteHelper _helper;

        //Propriedades
        private IRepresentanteConsultaBL ORepresentanteConsultaBL => _IRepresentanteConsultaBL = _IRepresentanteConsultaBL ?? new RepresentanteConsultaBL();
        public static RepresentanteHelper getInstance => _helper = _helper ?? new RepresentanteHelper();

        //
        public SelectList selectList(int selected){
            
            var list = ORepresentanteConsultaBL.query().Select(x => new {
                x.id,
                x.Pessoa.nome
            }).OrderBy(x => x.nome).ToList();
            
            return new SelectList(list, "id", "nome", selected);
        }
        
    }
    
}