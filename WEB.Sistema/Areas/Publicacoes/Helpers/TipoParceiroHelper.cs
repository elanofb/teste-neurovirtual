using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Publicacoes;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.Produtos.Helpers {

    public class TipoParceiroHelper {

        //Constanctes
        private static TipoParceiroHelper _instance;
        private ITipoParceiroBL _TipoParceiroBL;

        //Atributos

        //Propriedades
        public static TipoParceiroHelper getInstance => _instance = _instance ?? new TipoParceiroHelper();
        private ITipoParceiroBL OTipoParceiroBL => _TipoParceiroBL = _TipoParceiroBL ?? new TipoParceiroBL();

        //
        public SelectList selectList(int? selected) {

            int idOrganizacao = HttpContextFactory.Current.User.idOrganizacao();

            var lista = this.OTipoParceiroBL.listar(idOrganizacao, "", true)
                                        .ToList()
                                        .Select(x => new { id = x.id, descricao = x.descricao })
                                        .ToList();

            return new SelectList(lista, "id", "descricao", selected);
        }
    }
}