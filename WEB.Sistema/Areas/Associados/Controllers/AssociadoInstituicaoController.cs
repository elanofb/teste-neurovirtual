using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using DAL.Associados;
using WEB.App_Infrastructure;
using WEB.Areas.Associados.ViewModels;
using System.Json;
using BLL.Services;
using MvcFlashMessages;

namespace WEB.Areas.Associados.Controllers {

    public class AssociadoInstituicaoController : BaseSistemaController {

        //Atributos
        private IAssociadoInstituicaoBL _AssociadoInstituicaoBL;

        //Propriedades
        private IAssociadoInstituicaoBL OAssociadoInstituicaoBL => this._AssociadoInstituicaoBL = this._AssociadoInstituicaoBL ?? new AssociadoInstituicaoBL();

        //Construtor
        public AssociadoInstituicaoController() {

        }

        //Bloco Partial para listagem de instituicoes de um associado
        [HttpGet, ActionName("partial-listar-instituicoes")]
        public PartialViewResult partialListarInstituicoes(int idAssociado) {

            var listaInstituicoes = this.OAssociadoInstituicaoBL.listar(idAssociado, true)
                                                                .Include(x => x.UsuarioCadastro)
                                                                .Select(x => new {
                                                                    x.id,
                                                                    x.dtCadastro,
                                                                    x.idUsuarioCadastro,
                                                                    UsuarioCadastro = new { x.UsuarioCadastro.nome },
                                                                    Instituicao = new { id = x.idInstituicao, x.Instituicao.descricao }
                                                                })
                                                                .ToListJsonObject<AssociadoInstituicao>();

            return PartialView(listaInstituicoes);

        }

        //Formulário Parcial para novo Instituicao
        [HttpGet]
        public PartialViewResult editar(int? id, int? idAssociado) {

            var OAssociadoInstituicao = this.OAssociadoInstituicaoBL.carregar(UtilNumber.toInt32(id)) ?? new AssociadoInstituicao();

            OAssociadoInstituicao.idAssociado = (OAssociadoInstituicao.idAssociado > 0 ? OAssociadoInstituicao.idAssociado : UtilNumber.toInt32(idAssociado));

            AssociadoInstituicaoForm ViewModel = new AssociadoInstituicaoForm();

            ViewModel.AssociadoInstituicao = OAssociadoInstituicao;

            return PartialView(ViewModel);
        }


        //Formulario submetido para novo Instituicao para o associado
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult editar(AssociadoInstituicaoForm ViewModel) {

            if (!ModelState.IsValid) {
                return PartialView(ViewModel);
            }

            bool flagSalvo = this.OAssociadoInstituicaoBL.salvar(ViewModel.AssociadoInstituicao);

            if (flagSalvo) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados foram salvos com sucesso."));

            } else {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar as informações, tente novamente."));

            }

            return Json(new { flagSucesso = flagSalvo });
        }

        //Excluir determinado registro
        [HttpPost]
        public ActionResult excluir(int[] id) {

            JsonMessage Retorno = new JsonMessage();

            Retorno.error = false;

            foreach (int idExclusao in id) {

                UtilRetorno RetornoExclusao = this.OAssociadoInstituicaoBL.excluirPorId(idExclusao);

                if (RetornoExclusao.flagError) {

                    Retorno.error = false;
                }
            }

            if (Retorno.error) {
                Retorno.listMessage.Add("Alguns itens não puderam ser removidos.");
            } else {
                Retorno.listMessage.Add("Os registros foram removidos com sucesso.");
            }

            return Json(new { error = Retorno.error, message = string.Join("<br />", Retorno.listMessage) });
        }
    }
}
