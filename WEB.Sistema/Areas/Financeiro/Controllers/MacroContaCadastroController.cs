using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Caches;
using BLL.Financeiro;
using DAL.Financeiro;
using MvcFlashMessages;
using WEB.Areas.Financeiro.ViewModels;
using WEB.ViewModels;

namespace WEB.Areas.Financeiro.Controllers
{
    public class MacroContaCadastroController : Controller
    {
        //Atributos        
        private IMacroContaBL _MacroContaBL;
        private ICentroCustoMacroContaBL _CentroCustoMacroContaBL;
        private ICentroCustoBL _CentroCustoBL;

        //Propriedades
        private IMacroContaBL OMacroContaBL => this._MacroContaBL = this._MacroContaBL ?? new MacroContaBL();
        private ICentroCustoMacroContaBL OCentroCustoMacroContaBL => this._CentroCustoMacroContaBL = this._CentroCustoMacroContaBL ?? new CentroCustoMacroContaBL();
        private ICentroCustoBL OCentroCustoBL => this._CentroCustoBL = this._CentroCustoBL ?? new CentroCustoBL();

        //
        [HttpGet]
        public ActionResult editar(int? id) {
            
            var ViewModel = new MacroContaForm();
            var OMacroConta = this.OMacroContaBL.carregar(UtilNumber.toInt32(id)) ?? new MacroConta();

            ViewModel.listaCentroCusto = this.OCentroCustoBL.listar("", true).OrderBy(x => x.descricao).ToList();

            ViewModel.MacroConta = OMacroConta;

            ViewModel.carregarCheckboxes();

            return View(ViewModel);
        }

        //
        [HttpPost]
        public ActionResult editar(MacroContaForm ViewModel) {

            ViewModel.listaCentroCusto = this.OCentroCustoBL.listar("", true).ToList();

            if (!ModelState.IsValid) {
                ViewModel.carregarCheckboxes();

                return View(ViewModel);
            }

            bool flagSucesso = this.OMacroContaBL.salvar(ViewModel.MacroConta);

            if (flagSucesso) {

                var idsSelecionados = ViewModel.listaSelecionados.Where(x => x.isChecked)
                    .Select(x => x.id)
                    .ToArray();

                var listaMacroContaCentroCusto = ViewModel.listaCentroCusto
                    .Where(x => idsSelecionados.Contains(x.id))
                    .Select(o => new CentroCustoMacroConta() { idMacroConta = ViewModel.MacroConta.id, idCentroCusto = o.id })
                    .ToList();

                this.OCentroCustoMacroContaBL.salvar(ViewModel.MacroConta.id, 0, listaMacroContaCentroCusto);

                CacheService.getInstance.limparCacheOrganizacao(null, MacroContaBL.keyCache);

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

                return RedirectToAction("editar", new { ViewModel.MacroConta.id });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View(ViewModel);

        }

        //
        [ActionName("alterar-status"), HttpPost]
        public ActionResult alterarStatus(int id) {

            var ORetorno = this.OMacroContaBL.alterarStatus(id);

            if (!ORetorno.error) {
                CacheService.getInstance.limparCacheOrganizacao(null, MacroContaBL.keyCache);
            }

            return Json(ORetorno);
        }

        //
        [HttpGet, ActionName("modal-cadastrar-macro-conta")]
        public ActionResult modalCadastrarMacroConta() {

            var ViewModel = new MacroContaForm();

            var id = UtilRequest.getInt32("id");

            if (id > 0) {
                ViewModel.MacroConta = OMacroContaBL.carregar(id) ?? new MacroConta();
            }

            ViewModel.listaCentroCusto = this.OCentroCustoBL.listar("", true).OrderBy(x => x.descricao).ToList();
            ViewBag.modalId = UtilRequest.getString("modalId");
            ViewModel.group = UtilRequest.getString("group");

            if (ViewModel.MacroConta.id > 0) {
                var listIdsSelected = OCentroCustoMacroContaBL.listar(ViewModel.MacroConta.id, 0).Select(x => x.idCentroCusto).ToList();

                foreach (var OCentroCusto in ViewModel.listaCentroCusto) {
                    var Item = new CheckBoxItens { id = OCentroCusto.id, isChecked = listIdsSelected.Contains(OCentroCusto.id), descricao = OCentroCusto.descricao };
                    ViewModel.listaSelecionados.Add(Item);
                }
                ViewModel.listaSelecionados = ViewModel.listaSelecionados.OrderBy(x => x.descricao).ToList();

                return View(ViewModel);
            }

            ViewModel.MacroConta.descricao = UtilRequest.getString("descricao");
            var idCentroCusto = UtilRequest.getInt32("idCentroCusto");

            foreach (var OCentroCusto in ViewModel.listaCentroCusto) {
                var Item = new CheckBoxItens { id = OCentroCusto.id, isChecked = OCentroCusto.id == idCentroCusto, descricao = OCentroCusto.descricao };
                ViewModel.listaSelecionados.Add(Item);
            }
            ViewModel.listaSelecionados = ViewModel.listaSelecionados.OrderBy(x => x.descricao).ToList();

            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("salvar-macro-conta")]
        public ActionResult salvarMacroConta(MacroContaForm ViewModel) {

            ViewModel.listaCentroCusto = this.OCentroCustoBL.listar("", true).ToList();

            if (!ModelState.IsValid) {
                foreach (var OCentroCusto in ViewModel.listaCentroCusto) {
                    var Item = new CheckBoxItens { id = OCentroCusto.id, isChecked = false, descricao = OCentroCusto.descricao };
                    ViewModel.listaSelecionados.Add(Item);
                }
                ViewModel.listaSelecionados = ViewModel.listaSelecionados.OrderBy(x => x.descricao).ToList();

                return View("modal-cadastrar-macro-conta", ViewModel);
            }

            bool flagSucesso = this.OMacroContaBL.salvar(ViewModel.MacroConta);

            if (flagSucesso) {

                var idsSelecionados = ViewModel.listaSelecionados.Where(x => x.isChecked)
                    .Select(x => x.id).ToArray();

                var listaMacroContaCentroCusto = ViewModel.listaCentroCusto
                    .Where(x => idsSelecionados.Contains(x.id))
                    .Select(o => new CentroCustoMacroConta() { idMacroConta = ViewModel.MacroConta.id, idCentroCusto = o.id })
                    .ToList();

                this.OCentroCustoMacroContaBL.salvar(ViewModel.MacroConta.id, 0, listaMacroContaCentroCusto);

                CacheService.getInstance.limparCacheOrganizacao(null, MacroContaBL.keyCache);

                return Json(new { error = false, ViewModel.MacroConta.id, ViewModel.MacroConta.descricao, ViewModel.group });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View("modal-cadastrar-macro-conta", ViewModel);
        }
    }
}