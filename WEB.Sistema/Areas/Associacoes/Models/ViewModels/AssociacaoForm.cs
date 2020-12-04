using System.Web;
using FluentValidation.Attributes;
using DAL.Pessoas;
using DAL.Enderecos;
using DAL.Organizacoes;
using PagedList;

namespace WEB.Areas.Associacoes.ViewModels {

    //
    [Validator(typeof(AssociacaoFormValidator))]
    public class AssociacaoForm {

        public Organizacao Associacao { get; set; }

        public HttpPostedFileBase Logotipo { get; set; }

        public string flagTipoSaida { get; set; }

        public IPagedList<Organizacao> listaOrganizacao { get; set; }

        public AssociacaoForm() {

            Associacao = new Organizacao();
        }

        public void tratarEnderecos() {

            if (this.Associacao.Pessoa.listaEnderecos.Count == 1) {
                return;
            }

            this.Associacao.Pessoa.listaEnderecos.Add(new PessoaEndereco { idTipoEndereco = TipoEnderecoConst.PRINCIPAL, idPais = "BRA" });
        }
    }
}