using System;
using System.Linq;
using DAL.Pessoas;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Pessoas {

	public class PessoaTelefoneExclusaoBL : PessoaTelefoneConsultaBL, IPessoaTelefoneExclusaoBL {

		//
		public PessoaTelefoneExclusaoBL() {
		}

		//Excluir endereço a partir de um id
        public bool excluir(int idTelefonePessoa) {

	        var OTelefone = this.carregar(idTelefonePessoa);

	        if (OTelefone.id <= 0) {
		        return false;
	        }
	        
	        db.PessoaTelefone.Where(x => x.id == OTelefone.id && x.dtExclusao == null)
		        .Update(
			        x =>
				        new PessoaTelefone {
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
	        
	        db.PessoaTelefone.Where(x => x.idPessoa == idPessoa && x.dtExclusao == null)
		        .Update(
			        x =>
				        new PessoaTelefone {
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