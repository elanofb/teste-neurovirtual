using System.Web.Mvc;

namespace WEB.App_Infrastructure{

    public class BaseSistemaController : Controller {


        protected string partialViewSemPermissao = "~/Areas/Erros/Views/erro/partial-sem-permissao.cshtml";

        protected string partialViewSemRegistro = "~/Areas/Erros/Views/erro/sem-registro.cshtml";
    }
}
