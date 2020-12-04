using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Pessoas;
using DAL.Pessoas;
using WEB.App_Infrastructure;
using WEB.Areas.Pessoas.ViewModels;
using System.Json;
using BLL.Services;
using MvcFlashMessages;

namespace WEB.Areas.Pessoas.Controllers {

    public class PessoaContatoController : BaseSistemaController {

        //Atributos
        private IPessoaContatoBL _PessoaContatoBL;

        //Propriedades
        private IPessoaContatoBL OPessoaContatoBL { get { return (this._PessoaContatoBL = this._PessoaContatoBL ?? new PessoaContatoBL()); } }

        //Construtor
        public PessoaContatoController() {

        }

        //Bloco Partial para listagem de contatos de uma pessoa
        [HttpGet, ActionName("partial-listar-contatos")]
        public PartialViewResult partialListarContatos(int idPessoa) {

            var listaContatos = this.OPessoaContatoBL.listar(idPessoa, 0, "S")
                                                      .Select(x => new {
                                                                           x.id, 
                                                                           x.idPessoa,
                                                                           x.nome,
                                                                           x.email,
                                                                           x.telCelular,
                                                                           x.telComercial,
                                                                           x.observacao,
                                                                           x.dtCadastro,
                                                                           TipoContato = new {
                                                                                                 x.TipoContato.descricao
                                                                                             }
                                                                       })
                                                     .ToListJsonObject<PessoaContato>();

            return PartialView(listaContatos);
        }

        //Formulário Parcial para novo contato
        [HttpGet]
        public PartialViewResult editar(int? id, int? idPessoa) {

            var idContato = id.toInt();
            
            var OContato = this.OPessoaContatoBL.listar(idPessoa.toInt(), 0, "")
                           .Where(x => x.id == idContato)
                           .Select(x => new {
                                                x.id, 
                                                x.idTipoContato,
                                                x.idPessoa,
                                                x.ativo,
                                                x.dtCadastro,
                                                x.nome,
                                                x.email,
                                                x.telCelular,
                                                x.telComercial, 
                                                x.observacao
                                                
                                            })
                               .FirstOrDefault()
                               .ToJsonObject<PessoaContato>() ?? new PessoaContato();

            OContato.idPessoa = (OContato.idPessoa > 0 ? OContato.idPessoa : UtilNumber.toInt32(idPessoa));

            PessoaContatoForm ViewModel = new PessoaContatoForm();
            
            ViewModel.PessoaContato = OContato;

            return PartialView(ViewModel);
        }


        //Formulario submetido para novo contato
        [HttpPost]
        public ActionResult editar(PessoaContatoForm ViewModel) {

            if (!ModelState.IsValid) {
                return PartialView(ViewModel);
            }

            bool flagSalvo = this.OPessoaContatoBL.salvar(ViewModel.PessoaContato);


            if (flagSalvo) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados foram salvos com sucesso."));

            } else {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar as informações, tente novamente."));

            }

            return Json(new { flagSucesso = flagSalvo });
        }

        //Excluir determinado registro
        [HttpPost]
        public ActionResult delete(int[] id) {
            JsonMessage Retorno = new JsonMessage();
            Retorno.error = false;

            foreach (int idExclusao in id) {
                UtilRetorno RetornoExclusao = this.OPessoaContatoBL.excluir(idExclusao);

                if (RetornoExclusao.flagError) {
                    Retorno.error = false;
                }
            }

            return Json(new {error = Retorno.error, message = "Registro removido com sucesso."});
        }
    }
}
