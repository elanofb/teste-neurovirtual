using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using WEB.App_Infrastructure;

namespace WEB.Areas.Associados.Controllers {

    public class AssociadoBuscaController : BaseSistemaController {

        //Atributos
        private IAssociadoBL _IAssociadoBL;
        private AssociadoPesquisaRapidaBL _AssociadoPesquisaRapidaBL;

        //Propriedades
        private IAssociadoBL OAssociadoBL => _IAssociadoBL = _IAssociadoBL ?? new AssociadoBL();
        private AssociadoPesquisaRapidaBL OAssociadoPesquisaBL => _AssociadoPesquisaRapidaBL = _AssociadoPesquisaRapidaBL ?? new AssociadoPesquisaRapidaBL();

        //Construtor
        public AssociadoBuscaController() {

        }

        //Pesquisa Rápida
        [ActionName("pesquisa-rapida-listar")]
        public PartialViewResult pesquisaRapidaListar(string tipoBusca, string valorBusca) {

            valorBusca = valorBusca.Trim();

            var listaAssociados = this.OAssociadoPesquisaBL.listar(valorBusca);


            return PartialView(listaAssociados);
        }

        // 
        [ActionName("carregar-dados-associado")]
        public ActionResult carregarDadosAssociado(int id) {

            var query = this.OAssociadoBL.query().condicoesSeguranca().Where(x => x.id == id);

            var DadosAssociado = query.Select(x => new {
                
                                     x.Pessoa.nome, 
                                     x.Pessoa.nroDocumento,
                                     x.Pessoa.rg,
                
                                     x.Pessoa.idPaisOrigem,
                                     x.Pessoa.passaporte,

                                     emailPrincipal = x.Pessoa.listaEmails.Where(c => !c.email.Equals("") && c.dtExclusao == null).OrderBy(c => c.id).Select(e => e.email).FirstOrDefault(),
                                     emailSecundario = x.Pessoa.listaEmails.Where(c => !c.email.Equals("") && c.dtExclusao == null).OrderBy(c => c.id).Skip(1).Select(e => e.email).FirstOrDefault(),

                                     nroTelPrincipal = x.Pessoa.listaTelefones.Where(c => !c.nroTelefone.Equals("") && c.dtExclusao == null).OrderBy(c => c.id).Select(t => t.nroTelefone).FirstOrDefault(),
                                     nroTelSecundario = x.Pessoa.listaTelefones.Where(c => !c.nroTelefone.Equals("") && c.dtExclusao == null).OrderBy(c => c.id).Skip(1).Select(t => t.nroTelefone).FirstOrDefault(),
                                     nroTelTerciario = x.Pessoa.listaTelefones.Where(c => !c.nroTelefone.Equals("") && c.dtExclusao == null).OrderBy(c => c.id).Skip(2).Select(t => t.nroTelefone).FirstOrDefault(),

                                     x.Pessoa.nroMatriculaEstudante,
                                     x.Pessoa.instituicaoEstudante,
                                     x.Pessoa.dtFormacao,
                                     x.Pessoa.instituicaoFormacao,
                                     x.Pessoa.nroRegistroOrgaoClasse,
                                     ufOrgaoClasse = x.Pessoa.EstadoOrgaoClasse.sigla,
                                     x.Pessoa.profissao,

                                     Endereco = x.Pessoa.listaEnderecos.Where(c => !c.dtExclusao.HasValue).OrderBy(c => c.id)
                                                 .Select(e => new {
                                                             e.id, e.idTipoEndereco, e.cep, e.logradouro, e.numero, 
                                                             e.complemento, e.bairro, idCidade = e.idCidade ?? 0,
                                                             Cidade = new {
                                                                 id = e.idCidade ?? 0, idEstado = (e.Cidade == null ? 0 : e.Cidade.idEstado)
                                                             }, e.nomeCidade, e.idEstado, e.idPais, e.uf
                                                         }).FirstOrDefault()

                                 }).FirstOrDefault();

            if (DadosAssociado == null) {
                return Json(new { error = true, message = "O associado informado não foi encontrado." }, JsonRequestBehavior.AllowGet);
            }
            
            return Json(new { error = false, DadosAssociado }, JsonRequestBehavior.AllowGet);

        }

    }
}
