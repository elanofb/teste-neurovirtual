using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using BLL.Permissao;
using DAL.Permissao;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.Permissao.Helpers {
    public class PerfilAcessoHelper {

        //Atributos
        private static PerfilAcessoHelper _instance;
        private static IPerfilAcessoBL _PerfilAcessoBL;

        //Propriedades
        public static PerfilAcessoHelper getInstance => _instance = _instance ?? new PerfilAcessoHelper();
        private static IPerfilAcessoBL OPerfilAcessoBL => _PerfilAcessoBL = _PerfilAcessoBL ?? new PerfilAcessoBL();

        public PerfilAcessoHelper() { }

        //
        public SelectList selectList(int? idOrganizacao, int? selected, List<int> idsExclude = null) {

            var query = OPerfilAcessoBL.listar(UtilNumber.toInt32(idOrganizacao), "", "S");

            if (idsExclude != null) {

                query = query.Where(x => !idsExclude.Contains(x.id));

            }

            if (HttpContextFactory.Current.User.idPerfil() == PerfilAcessoConst.DESENVOLVEDOR) {

                query = query.Where(x => x.id != PerfilAcessoConst.DESENVOLVEDOR);

            }

            var lista = query.Select(x => new { x.id, x.descricao }).ToList();

            return new SelectList(lista, "id", "descricao", selected);
        }

    }
}