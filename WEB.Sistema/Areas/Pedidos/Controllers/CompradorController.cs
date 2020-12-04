using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using BLL.NaoAssociados;
using BLL.Pessoas;
using DAL.Pessoas;
using WEB.Areas.Associados.Extensions;

namespace WEB.Areas.Pedidos.Controllers {

    public class CompradorController : Controller {

        //Constantes

        //Atributos
        private IPessoaVWBL _IPessoaVWBL;
        private IAssociadoBL _AssociadoBL;
        private INaoAssociadoBL _NaoAssociadoBL;

        //Propriedades
        private IPessoaVWBL OPessoaVWBL => _IPessoaVWBL = _IPessoaVWBL ?? new PessoaVWBL();
        private IAssociadoBL OAssociadoBL => this._AssociadoBL = this._AssociadoBL ?? new AssociadoBL();
        private INaoAssociadoBL ONaoAssociadoBL => this._NaoAssociadoBL = this._NaoAssociadoBL ?? new NaoAssociadoBL();

        
        //
        public ActionResult autocomplete(string term) {
            
            var listaMembros = this.OPessoaVWBL.listar(term)
                                   .Where(x => x.flagCategoriaPessoa == CategoriaPessoaConst.ASSOCIADO)
                                   .OrderBy(x => x.nome).Select(x => new { id = x.idPessoa, text = x.nome }).ToList();

            return Json(listaMembros, JsonRequestBehavior.AllowGet);

        }

        //Inicio de um novo pedido ou visualização de um pedido já realizado
        [HttpGet, ActionName("carregar-comprador-associado")]
        public ActionResult carregarCompradorAssociado(int idPessoa) {

            var OAssociado = this.OAssociadoBL.carregarAssociadoPessoa(idPessoa);
        
            if (OAssociado == null) {

                return Json(new { error = true, message = "Nenhum cadastro foi localizado com esses dados."}, JsonRequestBehavior.AllowGet);
            }
            
            var Retorno = new {
                error = false,
                idAssociado = OAssociado.id,
                OAssociado.idPessoa,
                OAssociado.Pessoa.nome,
                nroDocumento = UtilString.formatCPFCNPJ(OAssociado.Pessoa.nroDocumento),
                telPrincipal = OAssociado.Pessoa.formatarTelPrincipal(),
                telSecundario = OAssociado.Pessoa.formatarTelSecundario(),
                emailPrincipal = OAssociado.Pessoa.emailPrincipal(),
                emailSecundario = OAssociado.Pessoa.emailSecundario(),
                OAssociado.ativo,
                descricaoStatus = OAssociado.exibirStatus(),
                descricaoSituacao = OAssociado.exibirSituacao(),
                tipo = "Membro"
            };
            
            return Json(Retorno, JsonRequestBehavior.AllowGet);
        }

        //Inicio de um novo pedido ou visualização de um pedido já realizado
        [HttpGet, ActionName("carregar-comprador-naoassociado")]
        public ActionResult carregarCompradorNaoAssociado(int idPessoa) {

            var ONaoAssociado = this.ONaoAssociadoBL.carregarPorPessoa(idPessoa);

            if (ONaoAssociado == null) {
                return Json(new {error = true, message = "Nenhum cadastro foi localizado com esses dados."}, JsonRequestBehavior.AllowGet);
            }

            var Retorno = new {
                error = false,
                idNaoAssociado = ONaoAssociado.id,
                ONaoAssociado.idPessoa,
                ONaoAssociado.Pessoa.nome,
                nroDocumento = UtilString.formatCPFCNPJ(ONaoAssociado.Pessoa.nroDocumento),
                telPrincipal = ONaoAssociado.Pessoa.formatarTelPrincipal(),
                telSecundario = ONaoAssociado.Pessoa.formatarTelSecundario(),
                emailPrincipal = ONaoAssociado.Pessoa.emailPrincipal(),
                emailSecundario = ONaoAssociado.Pessoa.emailSecundario(),
                ONaoAssociado.ativo,
                descricaoStatus = ONaoAssociado.exibirStatus(),
                descricaoSituacao = "",
                tipo = "Comerciante"
            };

            return Json(Retorno, JsonRequestBehavior.AllowGet);
        }

    }
}