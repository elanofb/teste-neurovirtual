using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Associados;

namespace BLL.NaoAssociadosInstitucional {

	public class NaoAssociadoAcessoBL : DefaultBL, INaoAssociadoAcessoBL {
		

		//Login para o associado na área institucional
		public Associado login(string login, string senha, int? idOrganizacaoParam = null) {

            if (idOrganizacao > 0 && idOrganizacaoParam == null) {
		        idOrganizacaoParam = idOrganizacao;
		    }

			string cryptSenha = UtilCrypt.SHA512(senha);

			var query = from Ass in db.Associado
									.Include(x => x.Pessoa)
						where 
							Ass.Pessoa.login == login && 
							Ass.Pessoa.senha == cryptSenha && 
							Ass.dtExclusao == null && 
                            Ass.idTipoCadastro == AssociadoTipoCadastroConst.COMERCIANTE
                        select
							Ass;

            if (idOrganizacaoParam > 0) {
		        query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
		    }

			var OAssociado = query.FirstOrDefault();
			return OAssociado;
		}
	}
}