using System;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using BLL.MateriaisApoio;
using DAL.MateriaisApoio;
using PagedList;
using WEB.Areas.MateriaisApoio.ViewModels;
using System.Collections.Generic;
using DAL.Pessoas;
using MvcFlashMessages;

namespace WEB.Areas.MateriaisApoio.controllers {

    public class MaterialApoioController : Controller {

        //Atributos
        private IMaterialApoioBL _IMaterialApoioBL;
        private IMaterialApoioPessoaBL _IMaterialApoioPessoaBL;

        //Propriedades
        private IMaterialApoioBL OMaterialApoioBL { get { return (this._IMaterialApoioBL = this._IMaterialApoioBL ?? new MaterialApoioBL()); } }
        private IMaterialApoioPessoaBL OMaterialApoioPessoaBL { get { return (this._IMaterialApoioPessoaBL = this._IMaterialApoioPessoaBL ?? new MaterialApoioPessoaBL()); } }

        //Construtor
        public MaterialApoioController() { }

        //Listagem de registros
        public ActionResult listar() {

            string status = UtilRequest.getString("flagAtivo");
            string valorBusca = UtilRequest.getString("valorBusca");

            var listaMaterialApoio = this.OMaterialApoioBL.listar(valorBusca, status).OrderBy(x => x.id);

            return View(listaMaterialApoio.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //Carregamento de formulario para inclusao/edicao
        [HttpGet]
        public ActionResult editar(int? id) {

            MaterialApoioForm ViewModel = new MaterialApoioForm();
            ViewModel.MaterialApoio = this.OMaterialApoioBL.carregar(UtilNumber.toInt32(id)) ?? new MaterialApoio();

            if (ViewModel.MaterialApoio.id > 0) {

                ViewModel.MaterialApoio.listaPessoasPermitidas = ViewModel.MaterialApoio.listaPessoasPermitidas.Where(x => x.flagExcluido == "N").ToList();

                var listaPessoasEspecificas = ViewModel.MaterialApoio.listaPessoasPermitidas.Select(x => x.Pessoa).ToList();
                SessionMateriaisApoio.setListAssociadosEspecificos(listaPessoasEspecificas);
                return View(ViewModel);

            }

            SessionMateriaisApoio.setListAssociadosEspecificos(new List<Pessoa>());
            return View(ViewModel);
        }

        //Salvar dados do formulario no banco de dados
        [HttpPost]
        public ActionResult editar(MaterialApoioForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            bool flagSucesso = this.OMaterialApoioBL.salvar(ViewModel.MaterialApoio, ViewModel.OArquivo);

            if (flagSucesso) {

                this.OMaterialApoioPessoaBL.excluir(ViewModel.MaterialApoio.id);

                if (ViewModel.MaterialApoio.flagDisponibilidadeAssociado == DisponibilidadeAssociadoConst.ASSOCIADOS_ESPECIFICOS) {

                    var listaAssociadosEspecificos = SessionMateriaisApoio.getListAssociadosEspecificos();

                    listaAssociadosEspecificos.ForEach(x => {
                        var OMaterialApoioPessoa = new MaterialApoioPessoa() {
                            idMaterialApoio = ViewModel.MaterialApoio.id,
                            idPessoa = x.id
                        };

                        this.OMaterialApoioPessoaBL.salvar(OMaterialApoioPessoa);
                    });

                    SessionMateriaisApoio.setListAssociadosEspecificos(new List<Pessoa>());

                }

            }

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "Os dados foram salvos com sucesso.");
            return RedirectToAction("editar", new {
                id = ViewModel.MaterialApoio.id
            });
        }

        [HttpPost]
        public ActionResult excluir(int[] id) {

            JsonMessage Retorno = new JsonMessage();
            Retorno.error = false;

            foreach (int idExclusao in id) {
                var RetornoExclusao = this.OMaterialApoioBL.excluir(idExclusao);

                if (RetornoExclusao.flagError) {
                    Retorno.error = true;
                    Retorno.message = "Alguns registros não puderam ser excluídos.";
                }
            }

            return Json(Retorno);

        }
    }
}