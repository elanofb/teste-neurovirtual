using System.Web.Mvc;
using System.Linq;
using BLL.Bancos;

namespace WEB.Areas.Bancos.Helpers{
    public class BancoHelper{

        //Atributos
        private static BancoHelper _instance;
        private IBancoBL _BancoBL;

        //Propriedades
        public static BancoHelper getInstance => _instance = _instance ?? new BancoHelper();
        private IBancoBL OBancoBL => _BancoBL = _BancoBL ?? new BancoBL();


        public SelectList selectList(int? selected){

            var list = OBancoBL.listar("","S").OrderBy(x => x.descricao).ToList();
            return new SelectList(list, "id", "nome", selected);
        }
    }
}