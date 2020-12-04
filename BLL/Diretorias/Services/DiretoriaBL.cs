using System;
using System.Json;
using System.Linq;
using BLL.Services;
using UTIL.Resources;
using DAL.Diretorias;

namespace BLL.Diretorias {

	public class DiretoriaBL : DefaultBL, IDiretoriaBL {

        //Atributos

        //Propriedades

		//
		public DiretoriaBL(){
		}

	    //
	    public IQueryable<Diretoria> query(int? idOrganizacaoParam = null) {
			
	        var query = from C in db.Diretoria
	                    where C.flagExcluido == false
	                    select C;

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;

        }

        //Carregamento de registro único pelo ID
		public Diretoria carregar(int id) {
			
			var query = this.query().condicoesSeguranca();

            return query.FirstOrDefault(x => x.id == id);
		}

		//
		public IQueryable<Diretoria> listar(string valorBusca, bool? ativo) {
			
		    var query = this.query().condicoesSeguranca();

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => valorBusca.Contains(x.anoFimGestao.ToString()) || valorBusca.Contains(x.anoInicioGestao.ToString()));
			}

			if (ativo != null) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		//Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
		public bool existe(Diretoria ODiretoria, int id) {

			var query = from C in db.Diretoria
                        where 
                            C.id != id && 
                            C.flagExcluido == false
						select C;

            query = query.condicoesSeguranca();

			return query.Any();
		}

		//Realizar os tratamentos necessários
		//Salvar um novo registro
		public bool salvar(Diretoria ODiretoria) {

			if(ODiretoria.id == 0){	
				return this.inserir(ODiretoria);
			}

			return this.atualizar(ODiretoria);
		}

        //Persistir e inserir um novo registro 
        //Inserir Diretoria
        private bool inserir(Diretoria ODiretoria) {

            ODiretoria.setDefaultInsertValues<Diretoria>();

			db.Diretoria.Add(ODiretoria);

			db.SaveChanges();

			return ODiretoria.id > 0;
		}

        //Persistir e atualizar um registro existente 
        //Atualizar dados da Diretoria
        private bool atualizar(Diretoria ODiretoria) {

            //Localizar existentes no banco
            var dbDiretoria = this.carregar(ODiretoria.id);

            if (dbDiretoria == null) {
                return false;
            }

            //Configurar valores padrão
            ODiretoria.setDefaultUpdateValues();

            //Atualizacao da Diretoria
            var DiretoriaEntry = db.Entry(dbDiretoria);

            DiretoriaEntry.CurrentValues.SetValues(ODiretoria);

            DiretoriaEntry.ignoreFields();

			db.SaveChanges();

			return ODiretoria.id > 0;
		}

        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
			var retorno = new JsonMessageStatus();

			var item = this.carregar(id);

			if (item == null) {
				retorno.error = true;
				retorno.message = NotificationMessages.invalid_register_id;
			} else {
				item.ativo = (item.ativo == true ? false : true);
				db.SaveChanges();
				retorno.active = (item.ativo == true ? "S" : "N");
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