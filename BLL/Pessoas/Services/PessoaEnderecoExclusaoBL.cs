using System;
using System.Linq;
using DAL.Pessoas;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Pessoas {

	public class PessoaEnderecoExclusaoBL : PessoaEnderecoConsultaBL, IPessoaEnderecoExclusaoBL {

		//
		public PessoaEnderecoExclusaoBL() {
		}

		//Excluir endereço a partir de um id
        public bool excluir(int idEnderecoPessoa) {

	        var OEndereco = this.carregar(idEnderecoPessoa);

	        if (OEndereco.id <= 0) {
		        return false;
	        }
	        
	        db.PessoaEndereco.Where(x => x.id == OEndereco.id && x.dtExclusao == null)
		        .Update(
			        x =>
				        new PessoaEndereco {
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
	        
	        db.PessoaEndereco.Where(x => x.idPessoa == idPessoa && x.dtExclusao == null)
		        .Update(
			        x =>
				        new PessoaEndereco {
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