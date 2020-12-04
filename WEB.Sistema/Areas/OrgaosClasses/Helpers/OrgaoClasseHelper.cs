using System.Collections;
using System.Linq;
using System.Web.Mvc;
using BLL.Caches;
using BLL.OrgaosClasses;

namespace WEB.Areas.OrgaosClasses.Helpers{

    public class OrgaoClasseHelper{

        //Constantes
        private static OrgaoClasseHelper _instance; 

        //Atributos
         private static IOrgaoClasseBL _OrgaoClasseBL;

        //Propriedades
        public static OrgaoClasseHelper getInstance => _instance = _instance ?? new OrgaoClasseHelper();
        private IOrgaoClasseBL OOrgaoClasseBL   => _OrgaoClasseBL = _OrgaoClasseBL ?? new OrgaoClasseBL();
       

        //Carregar combo de selecao dos tipos de associados
        public SelectList selectList(int? selected) {

            var listaItens = CacheService.getInstance.carregar(CacheService.ORGAO_CLASSE_DD_SIMPLES);

            if (listaItens != null) {

                return new SelectList((IEnumerable)listaItens, "id", "descricao", selected);
            }

            listaItens = OOrgaoClasseBL.listar("", true).OrderBy(x => x.descricao)
                                    .Select(x => new {
                                        x.id,
                                        descricao = x.sigla
                                    }).ToList();

            CacheService.getInstance.adicionar(CacheService.ORGAO_CLASSE_DD_SIMPLES, listaItens);

            return new SelectList((IEnumerable) listaItens, "id", "descricao", selected);
        }

    }
}