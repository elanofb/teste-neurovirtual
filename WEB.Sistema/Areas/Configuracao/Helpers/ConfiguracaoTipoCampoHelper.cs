using System.Linq;
using System.Web.Mvc;
using BLL.Configuracoes;

namespace WEB.Areas.Configuracao.Helpers {

    public class ConfiguracaoTipoCampoHelper {

        //Atributos
        private static ConfiguracaoTipoCampoHelper _instance;
        private IConfiguracaoTipoCampoBL _ConfiguracaoTipoCampoBL;

        //Services
        public static ConfiguracaoTipoCampoHelper getInstance => _instance = _instance ?? new ConfiguracaoTipoCampoHelper();
        private IConfiguracaoTipoCampoBL OConfiguracaoTipoCampoBL => _ConfiguracaoTipoCampoBL = _ConfiguracaoTipoCampoBL ?? new ConfiguracaoTipoCampoBL();

        //
        public SelectList selectList(int? selected) {

            var query = this.OConfiguracaoTipoCampoBL.listar("", true);

            var lista = query.Select(x => new { value = x.id, text = x.descricao })
                            .OrderBy(x => x.text)
                            .ToList();

            return new SelectList(lista, "value", "text", selected);
        }

    }
}