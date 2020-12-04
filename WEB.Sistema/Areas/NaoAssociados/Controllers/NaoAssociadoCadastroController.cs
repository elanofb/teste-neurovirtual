using System;
using System.Linq;
using System.Web.Mvc;
using BLL.ConfiguracoesAssociados;
using BLL.NaoAssociados;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.NaoAssociados.Controllers {
    public class NaoAssociadoCadastroController : Controller {

        //Atributos
        private INaoAssociadoBL _NaoAssociadoBL;

        //Servicos
        private INaoAssociadoBL ONaoAssociadoBL => _NaoAssociadoBL = _NaoAssociadoBL ?? new NaoAssociadoBL();

        //
        public ActionResult cadastrar() {

            var ConfiguracaoPF = ConfiguracaoAssociadoPFBL.getInstance.carregar(User.idOrganizacao());

            var ConfiguracaoPJ = ConfiguracaoAssociadoPJBL.getInstance.carregar(User.idOrganizacao());

            if (ConfiguracaoPF.flagHabilitado == true && ConfiguracaoPJ.flagHabilitado == true) {
                return View("definir-tipo-cadastro");
            }

            if (ConfiguracaoPF.flagHabilitado == true) {
                return RedirectToAction("editar", "NaoAssociadoCadastroPF");
            }

            return RedirectToAction("editar", "NaoAssociadoCadastroPJ");

        }

        /// <summary>
        /// Hub para direcionar o usuário para a tela de edição correta
        /// </summary>
        [ActionName("editar")]
        public ActionResult editar(int? id) {

            var OAssociado = this.ONaoAssociadoBL.carregar(id.toInt()).condicoesSeguranca().FirstOrDefault();

            if (OAssociado == null) {
                return RedirectToAction("listar", "NaoAssociado");
            }

            if (OAssociado.Pessoa.flagTipoPessoa == "J") {
                return RedirectToAction("editar", "NaoAssociadoCadastroPJ", new {OAssociado.id});
            }

            return RedirectToAction("editar", "NaoAssociadoCadastroPF", new {OAssociado.id});
        }
    }
}