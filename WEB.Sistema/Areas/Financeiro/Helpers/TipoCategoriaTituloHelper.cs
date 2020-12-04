using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;

namespace WEB.Areas.Financeiro.Helpers{
    public class TipoCategoriaTituloHelper{

        //Atributos
        private static ITipoCategoriaTituloBL _TipoCategoriaTituloBL;
        //Propriedades
        private static ITipoCategoriaTituloBL OTipoCategoriaTituloBL {get{ return (_TipoCategoriaTituloBL = _TipoCategoriaTituloBL ?? new TipoCategoriaTituloBL());}}
        //Construtor

        public static SelectList selectList(int? selected){
        
            var list = OTipoCategoriaTituloBL.listar(0, 0, "","S").OrderBy(x => x.descricao);
            return new SelectList(list, "id", "descricao", selected);
        }

        public static SelectList selectList(int? idCategoria, int? selected){
        
            var list = OTipoCategoriaTituloBL.listar(0, 0, "","S").Where(x => x.idCategoria == idCategoria).OrderBy(x => x.descricao);
            return new SelectList(list, "id", "descricao", selected);
        }
    }
}