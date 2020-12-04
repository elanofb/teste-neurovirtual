using System;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.OrgaosClasses;
using UTIL.Resources;

namespace BLL.OrgaosClasses {

	public class OrgaoClasseBL : DefaultBL, IOrgaoClasseBL {

        //Atributos

        //Propriedades

		//
		public OrgaoClasseBL() {
		}

        //Carregamento de registro único pelo ID
		public OrgaoClasse carregar(int id) {
			
			var query = from Item in db.OrgaoClasse
						where 
							Item.id == id && 
							Item.flagExcluido == false
						select Item;

            query = query.condicoesSeguranca();

			return query.FirstOrDefault();
		}

		// Listagem de Registros
		public IQueryable<OrgaoClasse> listar(string valorBusca, bool? ativo, int? idOrganizacaoInf = null) {

            if (idOrganizacaoInf == null) {
                idOrganizacaoInf = idOrganizacao;
            }

			var query = from T in db.OrgaoClasse
						where T.flagExcluido == false
						select T;
            
            if (idOrganizacaoInf > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoInf);
            }

            if (idOrganizacaoInf == 0) {
                query = query.Where(x => x.idOrganizacao == null);
            }

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca) || x.sigla.Contains(valorBusca));
			}

			if (ativo.HasValue) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		
		// Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
		public bool existe(OrgaoClasse OOrgaoClasse, int id) {
			
			var query = from T in db.OrgaoClasse
						where T.descricao == OOrgaoClasse.descricao && T.id != id && T.flagExcluido == false
						select T;

            query = query.condicoesSeguranca();

			var OItem = query.Take(1).FirstOrDefault();

            return (OItem != null);
		}

		//Realizar os tratamentos necessários
		//Salvar um novo registro
		public bool salvar(OrgaoClasse OOrgaoClasse) {

			if(OOrgaoClasse.id == 0){	
				return this.inserir(OOrgaoClasse);
			}

			return this.atualizar(OOrgaoClasse);
		}

        //Persistir e inserir um novo registro 
		//Inserir OrgaoClasse
        private bool inserir(OrgaoClasse OOrgaoClasse) { 

			OOrgaoClasse.setDefaultInsertValues<OrgaoClasse>();

            OOrgaoClasse.flagSistema = false;

			db.OrgaoClasse.Add(OOrgaoClasse);

			db.SaveChanges();

			return OOrgaoClasse.id > 0;
		}

        //Persistir e atualizar um registro existente 
		//Atualizar dados da OrgaoClasse
		private bool atualizar(OrgaoClasse OOrgaoClasse) { 

			//Localizar existentes no banco
			OrgaoClasse dbOrgaoClasse = this.carregar(OOrgaoClasse.id);

            if(dbOrgaoClasse == null) {
                return false;
            }

			//Configurar valores padrão
			OOrgaoClasse.setDefaultUpdateValues();

			//Atualizacao da OrgaoClasse
			var OrgaoClasseEntry = db.Entry(dbOrgaoClasse);
			OrgaoClasseEntry.CurrentValues.SetValues(OOrgaoClasse);
			OrgaoClasseEntry.ignoreFields();

			db.SaveChanges();

			return OOrgaoClasse.id > 0;
		}

        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
			var retorno = new JsonMessageStatus();

			var item = this.carregar(id);

			if (item == null) {
				retorno.error = true;
				retorno.message = NotificationMessages.invalid_register_id;
			} else {
				item.ativo = (item.ativo != true);
				db.SaveChanges();
				retorno.active = item.ativo == true ? "S" : "N";
				retorno.message = NotificationMessages.updateSuccess;
			}
			return retorno;
		}

		// Excluir Registro
		public UtilRetorno excluir(int id, int idUsuarioExclusao) {

		    var ORegistro = this.carregar(id);

		    if (ORegistro == null) {
		        return UtilRetorno.newInstance(true, "O registro informado não pôde ser localizado.");
		    }

		    ORegistro.flagExcluido = true;

            ORegistro.idUsuarioAlteracao = idUsuarioExclusao;

            ORegistro.dtAlteracao = DateTime.Now;

            db.SaveChanges();

		    return UtilRetorno.newInstance(false, "Os dados foram atualizados com sucesso.");
		}
	}
}