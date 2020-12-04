using System;
using System.Linq;
using DAL.Mailings;
using BLL.Services;
using System.Data.Entity;
using DAL.Permissao.Security.Extensions;

namespace BLL.Mailings {

	public class MailingBL : DefaultBL, IMailingBL {			

		//Construtor
		public MailingBL(){

		}

		//Carregamento de registro único pelo ID
		public Mailing carregar(int id) {

			var query = from Parc in db.Mailing
						where 
							Parc.id == id && 
							Parc.flagExcluido == "N"
						select Parc;

		    query = query.condicoesSeguranca();

			return query.FirstOrDefault();
		}

		//Listagem de Registros
		public IQueryable<Mailing> listar(string valorBusca, string ativo, int idTipoMailing, int idAssociado, int? idOrganizacaoInf = null) {
			
		    if (idOrganizacaoInf.toInt() == 0) {

		        idOrganizacaoInf = idOrganizacao;
		    }

			var query = from Parc in db.Mailing
                            .Include(x => x.TipoMailing)
                            .Include(x => x.Associado)
						where 
							Parc.flagExcluido == "N"
						select Parc;


            if (idTipoMailing > 0) {
                query = query.Where(x => x.idTipoMailing == idTipoMailing);
            }

            if(idAssociado > 0) {
                query = query.Where(x => x.idAssociado == idAssociado);
            }

            if(!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.nome.Contains(valorBusca) || x.email.Contains(valorBusca));
            }

		    if (idOrganizacaoInf > 0) {
		        query = query.Where(x => x.idOrganizacao == idOrganizacaoInf);
		    }

            return query;
		}

        //
		public bool salvar(Mailing OMailing) {

		    OMailing.TipoMailing = null;

		    OMailing.Associado = null;

			if (OMailing.id == 0) { 
				return this.inserir(OMailing);
			}

            return this.atualizar(OMailing);
            			
		}

		//Persistir o objecto e salvar na base de dados
		private bool inserir(Mailing OMailing) { 

			OMailing.setDefaultInsertValues();

			db.Mailing.Add(OMailing);

			db.SaveChanges();

			return (OMailing.id > 0);
		}

		//Persistir o objecto e atualizar informações
		private bool atualizar(Mailing OMailing) { 

			OMailing.setDefaultUpdateValues();

			//Localizar existentes no banco
			Mailing dbMailing = this.carregar(OMailing.id);		

            if (dbMailing == null) {
                return false;
            }

			var MailingEntry = db.Entry(dbMailing);

			MailingEntry.CurrentValues.SetValues(OMailing);

			MailingEntry.ignoreFields();

			db.SaveChanges();

			return (OMailing.id > 0);
		}

		// Verificar se já existe um registro com o mesmo nome, no entanto, que possua id diferente do informado
		public bool existe(string email, int idTipoMailing, int idAssociado, int id, int? idOrganizacaoInf = null) {

		    if (idOrganizacaoInf.toInt() == 0) {

		        idOrganizacaoInf = idOrganizacao;
		    }


			var query = from Parc in db.Mailing
						where 
							Parc.email == email && 
							Parc.flagExcluido == "N" && 
							Parc.id != id 
						select Parc;

		    if (idTipoMailing > 0) {
		        query = query.Where(x => x.idTipoMailing == idTipoMailing);
		    }

            if(idAssociado > 0) {
                query = query.Where(x => x.idAssociado == idAssociado);
            }

		    if (idOrganizacaoInf > 0) {
		        query = query.Where(x => x.idOrganizacao == idOrganizacaoInf);
		    }

            var OItem = query.Take(1).FirstOrDefault();

            return (OItem != null);
		}
        
		//Exclusao logica de registros
		public UtilRetorno excluir(int id) {

			Mailing OMailing = this.carregar(id);

			if (OMailing == null) {
				return UtilRetorno.newInstance(true, "O registro não foi localizado.");
			}

			OMailing.flagExcluido = "S";
			OMailing.idUsuarioAlteracao = User.id();
			OMailing.dtAlteracao = DateTime.Now;
			db.SaveChanges();

			return UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");
		}
	}
}