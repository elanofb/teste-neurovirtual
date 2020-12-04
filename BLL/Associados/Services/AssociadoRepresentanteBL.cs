using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Associados;
using DAL.Permissao.Security.Extensions;
using DAL.Repository.Base;
using EntityFramework.Extensions;

namespace BLL.Associados {

	public class AssociadoRepresentanteBL : DefaultBL, IAssociadoRepresentanteBL {

		//
		public AssociadoRepresentanteBL() {
		}

		//Load de um registro a partir do ID
		public AssociadoRepresentante carregar(int id) {

			var query = from PesCar in db.AssociadoRepresentante
										.Include(x => x.Associado)
										.Include(x => x.Associado.Pessoa)
						where PesCar.id == id && PesCar.flagExcluido == "N"
						select PesCar;
			AssociadoRepresentante OAssociadoRepresentante = query.FirstOrDefault();
			return OAssociadoRepresentante;
		}

		//Listagem de registros do banco a partir dos paramentros informados
		public IQueryable<AssociadoRepresentante> listar(int idAssociado, string ativo) {

			var query = (from PesCar in db.AssociadoRepresentante
										.Include(x => x.Associado)
						where PesCar.flagExcluido == "N"
						select PesCar).AsNoTracking();

			if (idAssociado > 0) {
				query = query.Where(x => x.idAssociado == idAssociado);
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}
		
		//Verificar se já existe registr dentro do mesmo período para evitar duplicidades
		public bool existe(AssociadoRepresentante OAssociadoRepresentante, int idDesconsiderado) {

			var query = (from PesCar in db.AssociadoRepresentante
						where PesCar.id != idDesconsiderado && PesCar.flagExcluido == "N"
						select PesCar
						).AsNoTracking();

			query = query.Where(x => x.idAssociado == OAssociadoRepresentante.idAssociado);
			query = query.Where(x => x.cpf == OAssociadoRepresentante.cpf);

			var Item = query.Take(1).FirstOrDefault();
			return (Item != null);
		}
		
		//Definir se é um insert ou update e enviar o registro para o banco de dados 
		public bool salvar(AssociadoRepresentante OAssociadoRepresentante) {

            OAssociadoRepresentante.cpf = UtilString.onlyNumber(OAssociadoRepresentante.cpf);

		    OAssociadoRepresentante.TipoAssociadoRepresentante = null;

			if (OAssociadoRepresentante.id == 0) { 
				return this.inserir(OAssociadoRepresentante);
			} else { 
				return this.atualizar(OAssociadoRepresentante);
			}
		}

		//Persistir e inserir um novo registro 
		private bool inserir(AssociadoRepresentante OAssociadoRepresentante) { 

			OAssociadoRepresentante.setDefaultInsertValues<AssociadoRepresentante>();

			db.AssociadoRepresentante.Add(OAssociadoRepresentante);
			db.SaveChanges();
			return OAssociadoRepresentante.id > 0;
		}

		//Persistir e atualizar um registro existente 
		private bool atualizar(AssociadoRepresentante OAssociadoRepresentante) { 

			//Localizar existentes no banco
			var dbRepresentante = this.carregar(OAssociadoRepresentante.id);

			//Configurar valores padrão
			OAssociadoRepresentante.setDefaultUpdateValues<AssociadoRepresentante>();

			//Atualizacao da Empresa
			var RepresentanteEntry = db.Entry(dbRepresentante);
			RepresentanteEntry.CurrentValues.SetValues(OAssociadoRepresentante);
			RepresentanteEntry.ignoreFields<AssociadoRepresentante>(new string[]{"idAssociado", "ativo"});

			db.SaveChanges();

			return OAssociadoRepresentante.id > 0;
		}

		//Remover um registro logicamente
		public UtilRetorno excluir(string cpf, int idAssociado) {

			int idUsuarioLogado = User.id();

			db.AssociadoRepresentante.Where(x => x.idAssociado == idAssociado && x.cpf == cpf)
							.Update(x => new AssociadoRepresentante{ flagExcluido = "S", dtAlteracao = DateTime.Now, idUsuarioAlteracao = idUsuarioLogado});
			
			UtilRetorno Retorno = UtilRetorno.getInstance();
			Retorno.flagError = false;
			return Retorno;
		}

		public UtilRetorno excluirPorId(int id) {

			int idUsuarioLogado = User.id();

			db.AssociadoRepresentante.Where(x => x.id == id)
							.Update(x => new AssociadoRepresentante{ flagExcluido = "S", dtAlteracao = DateTime.Now, idUsuarioAlteracao = idUsuarioLogado});
			
			UtilRetorno Retorno = UtilRetorno.getInstance();
			Retorno.flagError = false;
			return Retorno;
		}

	}
}