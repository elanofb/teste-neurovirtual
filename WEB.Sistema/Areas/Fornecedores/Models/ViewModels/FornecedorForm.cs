using System.Linq;
using System.Collections.Generic;
using DAL.Fornecedores;
using FluentValidation.Attributes;
using DAL.Produtos;
using DAL.Pessoas;
using DAL.Enderecos;

namespace WEB.Areas.Fornecedores.ViewModels {

    [Validator(typeof(FornecedorFormValidator))]
    public class FornecedorForm {

		public Fornecedor Fornecedor { get; set;}
		public List<Produto> listaProdutos { get; set; }

		//Construtor
        public FornecedorForm() {

            this.Fornecedor = new Fornecedor();

			this.listaProdutos = new List<Produto>();
        }

		//Exibir somente o endereço principal da empresa
		//Se nao houver endereco, adicionar 1 item na lista para exibição do formulario
		public void filtrarEnderecoPrincipal() { 

			this.Fornecedor.Pessoa = this.Fornecedor.Pessoa ?? new Pessoa();

			this.Fornecedor.Pessoa.listaEnderecos = this.Fornecedor.Pessoa.listaEnderecos.Where(x => x.idTipoEndereco == TipoEnderecoConst.PRINCIPAL).ToList();

			if(this.Fornecedor.Pessoa.listaEnderecos.Count == 0){
				this.Fornecedor.Pessoa.listaEnderecos.Add(new PessoaEndereco() { idPais = "BRA", idTipoEndereco = TipoEnderecoConst.PRINCIPAL });
			}

		}
    }


}