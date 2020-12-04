using DAL.LogsPermissao;
using DAL.Permissao;
using PagedList;

namespace WEB.Areas.LogsPermissao.ViewModels {

    //
    public class LogAcessoVM {

        public UsuarioSistema UsuarioSistema { get; set; }

        public IPagedList<LogUsuarioSistemaAcesso> listaLogUsuarioSistemaAcesso { get; set; }
    }
}