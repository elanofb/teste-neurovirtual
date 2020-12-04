using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using BLL.ConfiguracoesAssociados;
using DAL.Associados;
using DAL.Permissao.Security.Extensions;
using WEB.Areas.Associados.ViewModels;

namespace WEB.Areas.Associados.Controllers {
    public class AssociadoCadastroController : Controller {

        //Atributos
        private IAssociadoConsultaBL _AssociadoBL;

        //Servicos
        private IAssociadoConsultaBL OAssociadoBL => _AssociadoBL = _AssociadoBL ?? new AssociadoConsultaBL();

        // GET: Associados/AssociadoCadastro
        [ActionName("definir-tipo-cadastro")]
        public ActionResult definirTipoCadastro() {

            var ConfiguracaoPF = ConfiguracaoAssociadoPFBL.getInstance.carregar(User.idOrganizacao());

            var ConfiguracaoPJ = ConfiguracaoAssociadoPJBL.getInstance.carregar(User.idOrganizacao());

            if (ConfiguracaoPF.flagHabilitado == true && ConfiguracaoPJ.flagHabilitado == true) {
                return View();
            }

            if (ConfiguracaoPF.flagHabilitado == true) {
                return RedirectToAction("editar", "AssociadoCadastroPF");
            }

            return RedirectToAction("editar", "AssociadoCadastroPJ");
        }

        /// <summary>
        /// Hub para direcionar o usuário para a tela de edição correta
        /// </summary>
        [ActionName("editar")]
        public ActionResult editar(int? id) {

            int idAssociado = id.toInt();
            
            var OAssociado = this.OAssociadoBL.queryNoFilter(1)
                                 .Where(x => x.id == idAssociado)
                                 .Select(x => new {
                                                      x.id, 
                                                      x.idTipoCadastro,
                                                      x.idPessoa,
                                                      Pessoa = new {
                                                                       x.Pessoa.id,
                                                                       x.Pessoa.flagTipoPessoa
                                                                   }
                                                  })
                                 .FirstOrDefault();

            if (OAssociado == null) {

                return RedirectToAction("index", "AssociadoConsulta", new { area="AssociadosConsultas"});

            }

            if (OAssociado.idTipoCadastro == AssociadoTipoCadastroConst.COMERCIANTE) {
                
                if (OAssociado.Pessoa.flagTipoPessoa == "J") {

                    return RedirectToAction("editar", "NaoAssociadoCadastroPJ", new {area="NaoAssociados", OAssociado.id});

                }
                
                return RedirectToAction("editar", "NaoAssociadoCadastroPF", new {area="NaoAssociados", OAssociado.id});
            }

            if (OAssociado.Pessoa.flagTipoPessoa == "J") {

                return RedirectToAction("editar", "AssociadoCadastroPJ", new {OAssociado.id});

            }

            return RedirectToAction("editar", "AssociadoCadastroPF", new {OAssociado.id});


        }

        /// <summary>
        /// Criar um arquivo padrao
        /// </summary>
        /// <returns></returns>
        [ActionName("criar-arquivo-padrao")]
        public ActionResult criarArquivoPadrao() {

            var ViewModel = new AssociadoCadastroPFForm();

            ViewModel.criarArquivoPadrao(User.idOrganizacao());

            return Json(new { error = false }, JsonRequestBehavior.AllowGet);

        }
    }
}