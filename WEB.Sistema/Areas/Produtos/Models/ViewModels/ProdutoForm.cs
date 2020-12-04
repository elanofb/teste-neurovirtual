using System.Collections.Generic;
using FluentValidation.Attributes;
using DAL.Produtos;
using System.Web;

namespace WEB.Areas.Produtos.ViewModels {
    [Validator(typeof(ProdutoFormValidator))]
    public class ProdutoForm {
        public Produto Produto { get; set; }

        public List<int> listaIdsTipoAssociado { get; set; }

        public List<string> listaProdutoSituacao { get; set; }

        public string flagQuites { get; set; }

        public int idSituacao2 { get; set; }
        //public Produto Produto { get; set; }

        public HttpPostedFileBase OImagem { get; set; }

        //Construtor
        public ProdutoForm() {
            this.listaIdsTipoAssociado = new List<int>();

            this.listaProdutoSituacao = new List<string>();
        }
    }
}