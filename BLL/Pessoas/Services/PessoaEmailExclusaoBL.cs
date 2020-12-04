using System;
using System.Linq;
using DAL.Pessoas;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Pessoas {

	public class PessoaEmailExclusaoBL : PessoaEmailConsultaBL, IPessoaEmailExclusaoBL {

		//
		public PessoaEmailExclusaoBL() {
		}

		//Excluir endereço a partir de um id
        public bool excluir(int idEmailPessoa) {

	        var OEmail = this.carregar(idEmailPessoa);

	        if (OEmail.id <= 0) {
		        return false;
	        }
	        
	        db.PessoaEmail.Where(x => x.id == OEmail.id && x.dtExclusao == null)
		        .Update(
			        x =>
				        new PessoaEmail {
					        dtExclusao = DateTime.Now,
					        idUsuarioExclusao = User.id(),
					        dtAlteracao = DateTime.Now,
					        idUsuarioAlteracao = User.id()
				        });

	        db.SaveChanges();
	        
            return true;

        }

		//Excluir endereço a partir de um idPessoa
        public bool excluirPorPessoa(int idPessoa) {

	        if (idPessoa <= 0) {
		        return false;
	        }
	        
	        db.PessoaEmail.Where(x => x.idPessoa == idPessoa && x.dtExclusao == null)
		        .Update(
			        x =>
				        new PessoaEmail {
					        dtExclusao = DateTime.Now,
					        idUsuarioExclusao = User.id(),
					        dtAlteracao = DateTime.Now,
					        idUsuarioAlteracao = User.id()
				        });

	        db.SaveChanges();

            return true;

        }

	}
}