using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Associados;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Associados {

	public class AssociadoAreaAtuacaoBL : DefaultBL, IAssociadoAreaAtuacaoBL {

		//Load de um registro a partir do ID
		public AssociadoAreaAtuacao carregar(int id) {

			var query = from PesCar in db.AssociadoAreaAtuacao
										.Include(x => x.Associado)
										.Include(x => x.Associado.Pessoa)
										.Include(x => x.AreaAtuacao)
						where PesCar.id == id && PesCar.flagExcluido == "N"
						select PesCar;

		    query = query.condicoesSeguranca();

            return query.FirstOrDefault();
		}

		//Listagem de registros do banco a partir dos parâmetros informados
		public IQueryable<AssociadoAreaAtuacao> listar(int idAssociado, string ativo) {

			var query = (from PesCar in db.AssociadoAreaAtuacao
										.Include(x => x.Associado)
										.Include(x => x.AreaAtuacao)
						where PesCar.flagExcluido == "N"
						select PesCar).AsNoTracking();

		    query = query.condicoesSeguranca();

			if (idAssociado > 0) {
				query = query.Where(x => x.idAssociado == idAssociado);
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}
		
		//Verificar se já existe registrar dentro do mesmo período para evitar duplicidades
		public bool existe(AssociadoAreaAtuacao OAssociadoAreaAtuacao, int idDesconsiderado) {

			var query = (from PesCar in db.AssociadoAreaAtuacao
						where PesCar.id != idDesconsiderado && PesCar.flagExcluido == "N"
						select PesCar).AsNoTracking();

		    query = query.condicoesSeguranca();

            query = query.Where(x => x.idAssociado == OAssociadoAreaAtuacao.idAssociado);
			query = query.Where(x => x.idAreaAtuacao == OAssociadoAreaAtuacao.idAreaAtuacao);
			query = query.Where(x => x.observacao1 == OAssociadoAreaAtuacao.observacao1);
			query = query.Where(x => x.observacao2 == OAssociadoAreaAtuacao.observacao2);

			var Item = query.FirstOrDefault();

			return (Item != null);
		}
		
        //Realizar os tratamentos necessários
	    //Salvar um novo registro
	    public UtilRetorno salvarLote(List<int> idsAreaAtuacao, int idAssociado){

	        idsAreaAtuacao = idsAreaAtuacao ?? new List<int>();

            var idsAreaAtuacaoDb = this.listar(idAssociado, "S").Select(x => x.idAreaAtuacao).ToList();

            var idsAreaAtuacaoAdd = idsAreaAtuacao.Where(x => !idsAreaAtuacaoDb.Contains(x)).ToList();

            var idsAreaAtuacaoDel = idsAreaAtuacaoDb.Where(x => !idsAreaAtuacao.Contains(x)).ToList();

            if (idsAreaAtuacaoDel.Any()) {
                var idUser = User.id();

                db.AssociadoAreaAtuacao.Where(x => idsAreaAtuacaoDel.Contains(x.idAreaAtuacao) && x.idAssociado == idAssociado).Update(x => new AssociadoAreaAtuacao() {
                    dtAlteracao = DateTime.Now,
                    idUsuarioAlteracao = idUser,
                    flagExcluido = "S"
                });
            }

            if (idsAreaAtuacaoAdd.Any()) {

                var listaAssociadoAreaAtuacao = new List<AssociadoAreaAtuacao>();

                foreach (var idTipoAssociado in idsAreaAtuacaoAdd){
                    listaAssociadoAreaAtuacao.Add(new AssociadoAreaAtuacao() { idAreaAtuacao = idTipoAssociado, idAssociado = idAssociado});
                }

                listaAssociadoAreaAtuacao.ForEach(Item => Item.setDefaultInsertValues());
                
                db.AssociadoAreaAtuacao.AddRange(listaAssociadoAreaAtuacao);
                db.SaveChanges();
            }

	        return UtilRetorno.newInstance(false);
	    }

		//Definir se é um insert ou update e enviar o registro para o banco de dados 
		public bool salvar(AssociadoAreaAtuacao OAssociadoAreaAtuacao) {
			
            OAssociadoAreaAtuacao.AreaAtuacao = null;

			if (OAssociadoAreaAtuacao.id == 0) { 
				return this.inserir(OAssociadoAreaAtuacao);
			} 
			return this.atualizar(OAssociadoAreaAtuacao);
		}

		//Persistir e inserir um novo registro 
		private bool inserir(AssociadoAreaAtuacao OAssociadoAreaAtuacao) { 
			
			OAssociadoAreaAtuacao.setDefaultInsertValues();

			db.AssociadoAreaAtuacao.Add(OAssociadoAreaAtuacao);

			db.SaveChanges();

			return OAssociadoAreaAtuacao.id > 0;
		}

		//Persistir e atualizar um registro existente 
		private bool atualizar(AssociadoAreaAtuacao OAssociadoAreaAtuacao) { 
			
			//Localizar existentes no banco
			AssociadoAreaAtuacao dbAreaAtuacao = this.carregar(OAssociadoAreaAtuacao.id);

		    if (dbAreaAtuacao == null) {
		        return false;
		    }

			//Configurar valores padrão
			OAssociadoAreaAtuacao.setDefaultUpdateValues();

			//Atualização da Empresa
			var AreaAtuacaoEntry = db.Entry(dbAreaAtuacao);
			AreaAtuacaoEntry.CurrentValues.SetValues(OAssociadoAreaAtuacao);
			AreaAtuacaoEntry.ignoreFields(new []{"idAssociado", "ativo"});

			db.SaveChanges();

			return OAssociadoAreaAtuacao.id > 0;
		}

		//Remover um registro logicamente
		public UtilRetorno excluir(int idAreaAtuacao, int idAssociado) {

		    var idUsuario = User.id();

			db.AssociadoAreaAtuacao.Where(x => x.idAreaAtuacao == idAreaAtuacao && x.idAssociado == idAssociado)
					.Update(x => new AssociadoAreaAtuacao(){flagExcluido = "S", idUsuarioAlteracao = idUsuario, dtAlteracao = DateTime.Now });
			
			UtilRetorno Retorno = UtilRetorno.getInstance();
			Retorno.flagError = false;
			return Retorno;
		}

		//Remover um registro logicamente
		public UtilRetorno excluirPorId(int id) {

		    var idUsuario = User.id();

            db.AssociadoAreaAtuacao.Where(x => x.id == id)
			        .Update(x => new AssociadoAreaAtuacao() {  flagExcluido = "S", idUsuarioAlteracao = idUsuario, dtAlteracao = DateTime.Now});

            UtilRetorno Retorno = UtilRetorno.getInstance();
			Retorno.flagError = false;
			return Retorno;
		}
    }
}