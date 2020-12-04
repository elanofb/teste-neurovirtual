using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Associados;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Associados {

	public class AssociadoTituloBL : DefaultBL, IAssociadoTituloBL {

		//Carregamento do registro pelo ID
		public AssociadoTitulo carregar(int id) {

			var query = from PesTit in db.AssociadoTitulo
										.Include(x => x.Associado)
										.Include(x => x.TipoTitulo)
										.Include(x => x.Instituicao)
						where 
							PesTit.id == id && 
							PesTit.flagExcluido == "N"
						select PesTit;
			AssociadoTitulo OPessoaTitulo = query.FirstOrDefault();
			return OPessoaTitulo;
		}

		//Listagem a partir de paramentros
		public IQueryable<AssociadoTitulo> listar(int idAssociado, int idTipoTitulo, string ativo) {

			var query = from AssTit in db.AssociadoTitulo
										.Include(x => x.Associado)
										.Include(x => x.TipoTitulo)
										.Include(x => x.Instituicao)
						where 
							AssTit.flagExcluido == "N"
						select AssTit;

			if (idAssociado > 0) {
				query = query.Where(x => x.idAssociado == idAssociado);
			}

			if (idTipoTitulo > 0) {
				query = query.Where(x => x.idTipoTitulo == idTipoTitulo);
			}

			if (!String.IsNullOrEmpty(ativo)) { 
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}


		//Verificar se já existe registr dentro do mesmo período para evitar duplicidades
		public bool existe(AssociadoTitulo OAssociadoTitulo, int idDesconsiderado) {

			var query = (from AssoTit in db.AssociadoTitulo
						where 
							AssoTit.id != idDesconsiderado && 
							AssoTit.flagExcluido == "N"
						select AssoTit
						).AsNoTracking();

			query = query.Where(x => x.idAssociado == OAssociadoTitulo.idAssociado);
			query = query.Where(x => x.idTipoTitulo == OAssociadoTitulo.idTipoTitulo);
			query = query.Where(x => x.dtAquisicao.Date == OAssociadoTitulo.dtAquisicao.Date);

			var Item = query.Take(1).FirstOrDefault();
			return (Item != null);
		}
		
		//Definir se é um insert ou update e enviar o registro para o banco de dados 
		public bool salvar(AssociadoTitulo OAssociadoTitulo) {

			if (OAssociadoTitulo.id == 0) { 
				return this.inserir(OAssociadoTitulo);
			} else { 
				return this.atualizar(OAssociadoTitulo);
			}
		}

		//Persistir e inserir um novo registro 
		private bool inserir(AssociadoTitulo OAssociadoTitulo) { 

			OAssociadoTitulo.setDefaultInsertValues<AssociadoTitulo>();

			db.AssociadoTitulo.Add(OAssociadoTitulo);
			db.SaveChanges();
			return OAssociadoTitulo.id > 0;
		}

		//Persistir e atualizar um registro existente 
		private bool atualizar(AssociadoTitulo OAssociadoTitulo) { 

			//Localizar existentes no banco
			AssociadoTitulo dbTitulo = this.carregar(OAssociadoTitulo.id);

			//Configurar valores padrão
			OAssociadoTitulo.setDefaultUpdateValues<AssociadoTitulo>();

			//Atualizacao da Empresa
			var TituloEntry = db.Entry(dbTitulo);
			TituloEntry.CurrentValues.SetValues(OAssociadoTitulo);
			TituloEntry.ignoreFields<AssociadoTitulo>(new string[]{"idAssociado"});

			db.SaveChanges();

			return OAssociadoTitulo.id > 0;
		}


		//Remover um registro logicamente
		public UtilRetorno excluir(int id) {
			int idUsuarioLogado = User.id();

			db.AssociadoTitulo.Where(x => x.id == id)
							.Update(x => new AssociadoTitulo{ flagExcluido = "S", dtAlteracao = DateTime.Now, idUsuarioAlteracao = idUsuarioLogado});
			
			UtilRetorno Retorno = UtilRetorno.getInstance();
			Retorno.flagError = false;
			return Retorno;
		}
	}
}