using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BLL.Profissoes;

namespace WEB.Areas.Profissoes.Helpers {
    
    public class ProfissaoHelper{

        // 
        private static ProfissaoHelper _instance;
        private IProfissaoConsultaBL _IProfissaoConsultaBL;
        
        //
        public static ProfissaoHelper getInstance => _instance = _instance ?? new ProfissaoHelper();
        public IProfissaoConsultaBL OProfissaoConsultaBL => _IProfissaoConsultaBL = _IProfissaoConsultaBL ?? new ProfissaoConsultaBL();
        
        //
        public SelectList selectList(int? selected, List<int> idsExclude = null){

            var query = this.OProfissaoConsultaBL.listar("", true);

            if (idsExclude != null){
                query = query.Where(x => !idsExclude.Contains(x.id));
            }

            var listaProfissao = query.OrderBy(x => x.id).AsNoTracking().ToList();

            return new SelectList(listaProfissao, "id", "descricao", selected);
        }
    }
}
