using System;
using System.Collections;
using System.Web.Mvc;
using System.Linq;
using BLL.Instituicoes;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.Instituicoes.Helpers {

    public class InstituicaoHelper {

        // Atributos
        private static IInstituicaoBL _InstituicaoBL;

        //Propriedades
        private static IInstituicaoBL OInstituicaoBL => _InstituicaoBL = _InstituicaoBL ?? new InstituicaoBL();

        //Select List
        public static SelectList selectList(int? selected) {

            int idOrganizacao = HttpContextFactory.Current.User.idOrganizacao();

            var list = OInstituicaoBL.listar(idOrganizacao, null, true).OrderBy(x => x.descricao).ToList();

            return new SelectList(list, "id", "descricao", selected);
        }

        public static MultiSelectList multiSelectList(int[] selected, bool flagCache = true, int[] excludeItens = null) {

            int idOrganizacao = HttpContextFactory.Current.User.idOrganizacao();

            var query = OInstituicaoBL.listar(idOrganizacao, "", true);

            if (excludeItens != null && excludeItens.Length > 0) {
                query = query.Where(x => !excludeItens.Contains(x.id));
            }

            var listaItens = query.OrderBy(x => x.descricao)
                                    .ToList()
                                    .Select(x => new { x.id, x.descricao })
                                    .ToList();

            return new MultiSelectList((IEnumerable)listaItens, "id", "descricao", selected);
        }
    }
}