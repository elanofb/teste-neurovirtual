using System;
using System.Linq;
using System.Web.Mvc;
using BLL.ConfiguracoesAssociados;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.Configuracao.Helpers {

    public class AssociadoCampoGrupoHelper {

        //Atributos
        private static AssociadoCampoGrupoHelper _instance;
        private IConfiguracaoAssociadoCampoGrupoBL _ConfiguracaoAssociadoCampoGrupoBL;

        //Services
        public static AssociadoCampoGrupoHelper getInstance => _instance = _instance ?? new AssociadoCampoGrupoHelper();
        private IConfiguracaoAssociadoCampoGrupoBL OCampoGrupoBL => _ConfiguracaoAssociadoCampoGrupoBL = _ConfiguracaoAssociadoCampoGrupoBL ?? new ConfiguracaoAssociadoCampoGrupoBL();

        //
        public SelectList selectList(int? selected, int idTipoCampoCadastro, int? idOrganizacao = 0) {

            if (HttpContextFactory.Current.User.idOrganizacao() > 0) {
                idOrganizacao = HttpContextFactory.Current.User.idOrganizacao();
            }

            var query = this.OCampoGrupoBL.listar(UtilNumber.toInt32(idOrganizacao)).Where(x => x.idTipoCampoCadastro == idTipoCampoCadastro);

            var lista = query.Select(x => new { value = x.id, text = x.descricao, x.ordemExibicao })
                            .OrderBy(x => x.ordemExibicao ?? 1000)
                            .ToList();

            return new SelectList(lista, "value", "text", selected);
        }
    }
}