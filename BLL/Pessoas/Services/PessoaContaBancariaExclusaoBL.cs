using System;
using System.Linq;
using DAL.Pessoas;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Pessoas {

	public class PessoaContaBancariaExclusaoBL : PessoaContaBancariaConsultaBL, IPessoaContaBancariaExclusaoBL {

		//
		public PessoaContaBancariaExclusaoBL() {
		}
		
		//Excluir endereço a partir de um id
        public bool excluir(int id) {

	        if (id == 0){
		        return false;
	        }
	        
	        db.PessoaContaBancaria.Where(x => x.id == id && x.dtExclusao == null)
		        .Update(
			        x =>
				        new PessoaContaBancaria {
					        dtExclusao = DateTime.Now,
					        idUsuarioExclusao = User.id()					        
				        });

	        db.SaveChanges();
	        
            return true;

        }

		//Excluir endereço a partir de um idPessoa
        public bool excluirPorPessoa(int idPessoa) {

	        if (idPessoa <= 0) {
		        return false;
	        }
	        
	        db.PessoaContaBancaria.Where(x => x.idPessoa == idPessoa && x.dtExclusao == null)
		        .Update(
			        x =>
				        new PessoaContaBancaria {
					        dtExclusao = DateTime.Now,
					        idUsuarioExclusao = User.id()					        
				        });

	        db.SaveChanges();

            return true;

        }

	}
}