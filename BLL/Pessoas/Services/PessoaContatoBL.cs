using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using DAL.Pessoas;
using EntityFramework.Extensions;

namespace BLL.Pessoas {

	public class PessoaContatoBL : DefaultBL, IPessoaContatoBL {

		//Construtor
		public PessoaContatoBL(){
		}


		//Carregamento de um registro específico
		public PessoaContato carregar(int id) { 

			var query = (	from PesCon in db.PessoaContato
											.Include(x => x.Pessoa)
											.Include(x => x.TipoContato)
							where 
								PesCon.id == id &&
								PesCon.flagExcluido == "N"
							select
								PesCon
						);

			return query.FirstOrDefault();
		}

		//Listagem de registros de acordo com parametros informados
		public IQueryable<PessoaContato> listar(int idPessoa, int idTipoContato, string ativo) {

			var query = from Cont in db.PessoaContato
									.Include(x => x.TipoContato)
									.Include(x => x.Pessoa)
									.AsNoTracking()
						where Cont.flagExcluido == "N"
						select Cont;

			if (idPessoa > 0) {
				query = query.Where(x => x.idPessoa == idPessoa);
			}

			if (idTipoContato > 0) {
				query = query.Where(x => x.idTipoContato == idTipoContato);
			}

			if (!String.IsNullOrEmpty(ativo)) { 
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		//Definir se é um insert ou update e enviar o registro para o banco de dados 
		public bool salvar(PessoaContato OContato) {

			OContato.telCelular = UtilString.onlyNumber(OContato.telCelular);

			OContato.telComercial = UtilString.onlyNumber(OContato.telComercial);

			if (OContato.id == 0) { 
				return this.inserir(OContato);
			}

            return this.atualizar(OContato);
			
		}

		//Persistir e inserir um novo registro 
		private bool inserir(PessoaContato OContato) { 

			OContato.setDefaultInsertValues<PessoaContato>();

			db.PessoaContato.Add(OContato);
			db.SaveChanges();
			return OContato.id > 0;
		}

		//Persistir e atualizar um registro existente 
		private bool atualizar(PessoaContato OContato) { 
			
			//Localizar existentes no banco
			PessoaContato dbContato = this.carregar(OContato.id);

			//Configurar valores padrão
			OContato.setDefaultUpdateValues<PessoaContato>();

			//Atualizacao da Empresa
			var ContatoEntry = db.Entry(dbContato);
			ContatoEntry.CurrentValues.SetValues(OContato);
			ContatoEntry.ignoreFields<PessoaContato>(new string[]{"idPessoa"});

			db.SaveChanges();

			return OContato.id > 0;
		}

		//Carregamento de um registro específico
		public bool existe(string nome, string email, int idPessoa, int idDesconsiderar) { 

			var query = (	from PesCon in db.PessoaContato.AsNoTracking()
							where 
								PesCon.id != idDesconsiderar &&
                                PesCon.idPessoa == idPessoa &&
								PesCon.flagExcluido == "N"
							select
								PesCon
						);

			if (!String.IsNullOrEmpty(nome)) { 
				query = query.Where(x => x.nome == nome);
			}

			if (!String.IsNullOrEmpty(email)) { 
				query = query.Where(x => x.email == email);
			}

			bool flagExiste = query.Any();

			return flagExiste;

		}

		//Remover um registro logicamente
		public UtilRetorno excluir(int id) {

			int idUsuarioLogado = User.id();

			db.PessoaContato.Where(x => x.id == id)
							.Update(x => new PessoaContato{ flagExcluido = "S", idUsuarioAlteracao = idUsuarioLogado, dtAlteracao = DateTime.Now});
			
			UtilRetorno Retorno = UtilRetorno.getInstance();
			Retorno.flagError = false;
			return Retorno;
		}
	}
}