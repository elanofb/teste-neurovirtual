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

    public class AssociadoAreaAtuacaoController : BaseSistemaController {

        //Atributos
        private IAssociadoAreaAtuacaoBL _AssociadoAreaAtuacaoBL;

        //Propriedades
        private IAssociadoAreaAtuacaoBL OAssociadoAreaAtuacaoBL { get { return (this._AssociadoAreaAtuacaoBL = this._AssociadoAreaAtuacaoBL ?? new AssociadoAreaAtuacaoBL()); } }

        //Construtor
        public AssociadoAreaAtuacaoController() {

        }

        //Bloco Partial para listagem de AreaAtuacaos de um associado
        [HttpGet, ActionName("partial-listar-areaatuacao")]
        public PartialViewResult partialListarAreaAtuacaos(int idAssociado) {

            var listaAreaAtuacoes = this.OAssociadoAreaAtuacaoBL.listar(idAssociado, "S")
                                                                .Include(x => x.UsuarioCadastro)
                                                                .Select(x => new {
                                                                    x.id,
                                                                    x.dtCadastro,
                                                                    x.idUsuarioCadastro,
                                                                    UsuarioCadastro = new { x.UsuarioCadastro.nome },
                                                                    AreaAtuacao = new { id = x.idAreaAtuacao, x.AreaAtuacao.descricao }
                                                                })
                                                                .ToListJsonObject<AssociadoAreaAtuacao>();

            return PartialView(listaAreaAtuacoes);

        }

        //Formulário Parcial para novo AreaAtuacao
        [HttpGet]
        public PartialViewResult editar(int? id, int? idAssociado) {

            var OAssociadoAreaAtuacao = this.OAssociadoAreaAtuacaoBL.carregar(UtilNumber.toInt32(id)) ?? new AssociadoAreaAtuacao();

            OAssociadoAreaAtuacao.idAssociado = (OAssociadoAreaAtuacao.idAssociado > 0 ? OAssociadoAreaAtuacao.idAssociado : UtilNumber.toInt32(idAssociado));

            AssociadoAreaAtuacaoForm ViewModel = new AssociadoAreaAtuacaoForm();

            ViewModel.AssociadoAreaAtuacao = OAssociadoAreaAtuacao;

            return PartialView(ViewModel);
        }


        //Formulario submetido para novo AreaAtuacao para o associado
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult editar(AssociadoAreaAtuacaoForm ViewModel) {

            if (!ModelState.IsValid) {
                return PartialView(ViewModel);
            }

            bool flagSalvo = this.OAssociadoAreaAtuacaoBL.salvar(ViewModel.AssociadoAreaAtuacao);

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

                UtilRetorno RetornoExclusao = this.OAssociadoAreaAtuacaoBL.excluirPorId(idExclusao);

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
