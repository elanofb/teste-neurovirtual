using System;
using System.Linq;
using BLL.Services;
using DAL.RamosAtividade;
using  System.Data.Entity;
using System.Json;
using UTIL.Resources;

namespace BLL.RamosAtividade {

	public class SetorAtuacaoBL : DefaultBL, ISetorAtuacaoBL {

        //Atributos

        //Propriedades

		//
		public SetorAtuacaoBL() {
		}

        //Carregamento de registro único pelo ID
		public SetorAtuacao carregar(int id) {
			
			var query = from Item in db.SetorAtuacao
                                    .Include(x => x.RamoAtividade)
						where 
							Item.id == id && 
							Item.flagExcluido == false
						select Item;

            query = query.condicoesSeguranca();

			return query.FirstOrDefault();
		}

        //Carregamento de registro único pelo ramo e setor
		public SetorAtuacao carregar(string descricaoRamoAtividade, string descricaoSetorAtuacao) {
			
			var query = from Item in db.SetorAtuacao
                                    .Include(x => x.RamoAtividade)
						where 
                            Item.RamoAtividade.descricao == descricaoRamoAtividade &&
							Item.descricao == descricaoSetorAtuacao && 
							Item.flagExcluido == false
						select Item;

            query = query.condicoesSeguranca();

			return query.FirstOrDefault();
		}

		// Listagem de Registros
		public IQueryable<SetorAtuacao> listar(int idRamoAtividade, string valorBusca, bool? ativo, int? idOrganizacaoInf = null) {

            if (idOrganizacao > 0 && idOrganizacaoInf == null) {
                idOrganizacaoInf = idOrganizacao;
            }

			var query = from T in db.SetorAtuacao
                                .Include(x => x.RamoAtividade)
						where T.flagExcluido == false
						select T;

            if (idOrganizacaoInf > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoInf);
            }

            if (idOrganizacaoInf == 0) {
                query = query.Where(x => x.idOrganizacao == null);
            }

		    if (idRamoAtividade > 0) {
		        query = query.Where(x => x.idRamoAtividade == idRamoAtividade );   
		    }

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca) );
			}

			if (ativo.HasValue) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		
		// Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
		public bool existe(SetorAtuacao OSetorAtuacao, int id) {
			
			var query = from T in db.SetorAtuacao
						where T.descricao == OSetorAtuacao.descricao && T.id != id && T.flagExcluido == false
						select T;

            query = query.condicoesSeguranca();

			var OItem = query.Take(1).FirstOrDefault();

            return (OItem != null);
		}

		//Realizar os tratamentos necessários
		//Salvar um novo registro
		public bool salvar(SetorAtuacao OSetorAtuacao) {

			if(OSetorAtuacao.id == 0){	
				return this.inserir(OSetorAtuacao);
			}

			return this.atualizar(OSetorAtuacao);
		}

        //Persistir e inserir um novo registro 
		//Inserir SetorAtuacao
        private bool inserir(SetorAtuacao OSetorAtuacao) { 

			OSetorAtuacao.setDefaultInsertValues<SetorAtuacao>();

            OSetorAtuacao.flagSistema = false;

			db.SetorAtuacao.Add(OSetorAtuacao);

			db.SaveChanges();

			return OSetorAtuacao.id > 0;
		}

        //Persistir e atualizar um registro existente 
		//Atualizar dados da SetorAtuacao
		private bool atualizar(SetorAtuacao OSetorAtuacao) { 

			//Localizar existentes no banco
			SetorAtuacao dbSetorAtuacao = this.carregar(OSetorAtuacao.id);

            if(dbSetorAtuacao == null) {
                return false;
            }

			//Configurar valores padrão
			OSetorAtuacao.setDefaultUpdateValues();

			//Atualizacao da SetorAtuacao
			var SetorAtuacaoEntry = db.Entry(dbSetorAtuacao);
			SetorAtuacaoEntry.CurrentValues.SetValues(OSetorAtuacao);
			SetorAtuacaoEntry.ignoreFields();

			db.SaveChanges();

			return OSetorAtuacao.id > 0;
		}

        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
	        var retorno = new JsonMessageStatus();

	        var item = this.carregar(id);

	        if (item == null) {
		        retorno.error = true;
		        retorno.message = NotificationMessages.invalid_register_id;
	        } else {
		        item.ativo = item.ativo != true;
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