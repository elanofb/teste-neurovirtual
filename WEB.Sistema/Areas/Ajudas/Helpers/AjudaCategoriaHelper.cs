using System.Web.Mvc;
using System.Linq;
using BLL.Ajudas;

namespace WEB.Areas.Ajudas.Helpers{
    public class AjudaCategoriaHelper{

        //Atributos
        private static AjudaCategoriaHelper _instance;
        private IAjudaCategoriaBL _AjudaCategoriaBL;

        //Propriedades
        public static AjudaCategoriaHelper getInstance => _instance = _instance ?? new AjudaCategoriaHelper();
        private IAjudaCategoriaBL OAjudaCategoriaBL => _AjudaCategoriaBL = _AjudaCategoriaBL ?? new AjudaCategoriaBL();


        public SelectList selectList(int? selected){

            var list = OAjudaCategoriaBL.listar("",true).OrderBy(x => x.descricao).ToList();
            return new SelectList(list, "id", "descricao", selected);
        }
    }
}