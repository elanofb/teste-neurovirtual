using System.Linq;
using System.Web;
using DAL.Empresas;
using DAL.Enderecos;
using DAL.Pessoas;
using FluentValidation.Attributes;

namespace WEB.Areas.Empresas.ViewModels {

	[Validator(typeof(EmpresaValidator))]
	public class EmpresaForm {

		//Propriedades
		public Empresa Empresa { get; set; }

        public HttpPostedFileBase Arquivo { get; set; }

        //Construtor
        public EmpresaForm() { 
		}

		//Exibir somente o endereço principal da empresa
		//Se nao houver endereco, adicionar 1 item na lista para exibição do formulario
		public void filtrarEnderecoPrincipal() { 
			this.Empresa.Pessoa = this.Empresa.Pessoa ?? new Pessoa();
			this.Empresa.Pessoa.listaEnderecos = this.Empresa.Pessoa.listaEnderecos.Where(x => x.idTipoEndereco == TipoEnderecoConst.PRINCIPAL).ToList();

			if(this.Empresa.Pessoa.listaEnderecos.Count == 0){
				this.Empresa.Pessoa.listaEnderecos.Add(new PessoaEndereco());
			}
		}
	}
}