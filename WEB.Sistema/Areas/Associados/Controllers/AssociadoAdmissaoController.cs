using BLL.Associados;
using System;
using System.Linq;
using System.Web.Mvc;
using DAL.Associados.DTO;

namespace WEB.Areas.Associados.Controllers {

    public class AssociadoAdmissaoController : Controller {

        //Atributos
        private IAssociadoBL _AssociadoBL;

        //Propriedades
        private IAssociadoBL OAssociadoBL => this._AssociadoBL = this._AssociadoBL ?? new AssociadoBL();

        //Listagem dos associados do sistema
        public ActionResult listar() {

            var dtCadastroInicio = UtilRequest.getDateTime("dtCadastroInicio");
            var dtCadastroFim = UtilRequest.getDateTime("dtCadastroFim");

            var flagSituacaoContribuicao = UtilRequest.getString("flagSituacaoContribuicao");

            var idTipoAssociado = UtilRequest.getInt32("idTipoAssociado");

            var valorBusca = UtilRequest.getString("valorBusca");

            var queryAssociados = this.OAssociadoBL.listar(idTipoAssociado, flagSituacaoContribuicao, valorBusca, "E");
            
            if (dtCadastroInicio.HasValue) {
                queryAssociados = queryAssociados.Where(x => x.dtCadastro >= dtCadastroInicio.Value);
            }

            if (dtCadastroFim.HasValue) {
                var dtFiltro = dtCadastroFim.Value.AddDays(1);
                queryAssociados = queryAssociados.Where(x => x.dtCadastro < dtFiltro);
            }

            var query = queryAssociados.Select(x => new ItemListaAssociado {
                id = x.id,
                nroAssociado = x.nroAssociado,
                descricaoTipoAssociado = x.TipoAssociado.nomeDisplay,
                flagTipoPessoa = x.Pessoa.flagTipoPessoa,
                nome = x.Pessoa.nome,
                razaoSocial = x.Pessoa.razaoSocial,
                nroDocumento = x.Pessoa.nroDocumento,
                nroTelefone = x.Pessoa.listaTelefones.FirstOrDefault().nroTelefone,
                email = x.Pessoa.listaEmails.FirstOrDefault().email,
                dtCadastro = x.dtCadastro,
                ativo = x.ativo
            });
            
            var lista = query.OrderBy(x => x.nome).ToList();

            return View(lista);
        }
        
    }
}