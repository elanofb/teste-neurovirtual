using System;
using System.Linq;
using System.Data.Entity;
using DAL.Produtos;
using EntityFramework.Extensions;
using System.Collections.Generic;
using BLL.Services;
using DAL.Permissao.Security.Extensions;

namespace BLL.Produtos {

	public class EstoqueSaidaBL : DefaultBL, IEstoqueSaidaBL {

		//Atributos
        private IProdutoBL _ProdutoBL;

		//Propriedades
        private IProdutoBL OProdutoBL => (this._ProdutoBL = this._ProdutoBL ?? new ProdutoBL() );

	    //Construtor
		public EstoqueSaidaBL(){
		}
		
		//Carregamento de registro pelo Identificador
		public EstoqueSaida carregar(int id) {

			return db.EstoqueSaida.Include(x => x.ProdutoEstoque)
                                .Include(x => x.TipoReferenciaSaida)
								.FirstOrDefault(x => x.id == id);
		}
		
		//montagem de query para listagem de registro de acordo com parametros informados
		public IQueryable<EstoqueSaida> listar(int idTipoReferenciaSaida, int idReferencia, int idProduto, string valorBusca, string ativo) {

			var query = (from oPro in db.EstoqueSaida
										.Include(x => x.ProdutoEstoque)
                                        .Include(x => x.TipoReferenciaSaida)
						where 
							oPro.flagExcluido == "N"
						select oPro
						);

			if (idProduto > 0) { 
				query = query.Where(x => x.ProdutoEstoque.idProduto == idProduto);
			}

			if (idReferencia > 0) { 
				query = query.Where(x => x.idReferencia == idReferencia);
			}

			if (idTipoReferenciaSaida > 0) { 
				query = query.Where(x => x.idTipoReferenciaSaida == idTipoReferenciaSaida);
			}

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.ProdutoEstoque.Produto.nome.Contains(valorBusca) || x.ProdutoEstoque.Produto.descricaoResumida.Contains(valorBusca) );
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query.AsNoTracking();
		}
	
		//Verificar se deve-se atualizar um registro existente ou criar um novo
		public bool salvar(EstoqueSaida OEstoqueSaida) {

			if (OEstoqueSaida.id == 0) {
                Produto OProduto = this.OProdutoBL.carregar(OEstoqueSaida.ProdutoEstoque.idProduto);
                OEstoqueSaida.ProdutoEstoque.saldoAnterior = OProduto.qtde;
				return this.inserir(OEstoqueSaida);
			}

			return this.atualizar(OEstoqueSaida);
		}

		//Persistir o objecto e salvar na base de dados
		private bool inserir(EstoqueSaida OEstoqueSaida) { 
            
			OEstoqueSaida.setDefaultInsertValues<EstoqueSaida>();
			OEstoqueSaida.ProdutoEstoque.setDefaultInsertValues<ProdutoEstoque>();

			db.EstoqueSaida.Add(OEstoqueSaida);
			db.SaveChanges();

            //Atualizo o estoque do produto
            new ProdutoBL().atualizarEstoque(OEstoqueSaida.ProdutoEstoque.idProduto, OEstoqueSaida.ProdutoEstoque.qtdMovimentada,"remover");

			return OEstoqueSaida.id > 0;
		}

		//Persistir o objecto e atualizar informações
		private bool atualizar(EstoqueSaida OEstoqueSaida) { 
			
			//Localizar existentes no banco
			EstoqueSaida dbEstoqueSaida = this.carregar(OEstoqueSaida.id);
			ProdutoEstoque dbProdutoEstoque = db.ProdutoEstoque.FirstOrDefault(x => x.id == dbEstoqueSaida.idProdutoEstoque);

			//Configurar valores padrão
			OEstoqueSaida.setDefaultUpdateValues<EstoqueSaida>();
			OEstoqueSaida.ProdutoEstoque.setDefaultUpdateValues<ProdutoEstoque>();
			OEstoqueSaida.idProdutoEstoque= dbProdutoEstoque.id;
			OEstoqueSaida.ProdutoEstoque.id = dbProdutoEstoque.id;

			//Atualizacao da Funcionario
			var EstoqueSaidaEntry = db.Entry(dbEstoqueSaida);
			EstoqueSaidaEntry.CurrentValues.SetValues(OEstoqueSaida);
			EstoqueSaidaEntry.ignoreFields<EstoqueSaida>();

			//Atualizacao Dados Pessoa
			var ProdutoEstoqueEntry = db.Entry(dbProdutoEstoque);
			ProdutoEstoqueEntry.CurrentValues.SetValues(OEstoqueSaida.ProdutoEstoque);
			ProdutoEstoqueEntry.ignoreFields<ProdutoEstoque>();

			db.SaveChanges();
			return OEstoqueSaida.id > 0;
		}

		//Verificar existencia de EstoqueSaidas com mesmo nome para evitar duplicidades
		public bool existe(DateTime? dtMovimentacao, int idReferencia, int idTipoReferenciaSaida, int idProduto, int qtdMovimentada, int id) {

			var query = from Estoq in db.EstoqueSaida
						where Estoq.ProdutoEstoque.dtMovimentacao == dtMovimentacao && 
                              Estoq.idReferencia == idReferencia && 
                              Estoq.idTipoReferenciaSaida == idTipoReferenciaSaida && 
                              Estoq.ProdutoEstoque.idProduto == idProduto && 
                              Estoq.ProdutoEstoque.qtdMovimentada == qtdMovimentada && 
                              Estoq.id != id && 
                              Estoq.flagExcluido == "N"
						select Estoq;

			var OEstoqueSaida = query.AsNoTracking().Take(1).FirstOrDefault();
			return (OEstoqueSaida != null);
		}

		//Remover o registro do sistema (exclusao lógico - não se remove fiscamente do banco de dados)
		public bool excluir(int id) {

		    var idUsuario = User.id();

			db.EstoqueSaida
				.Where(x => x.id == id)
				.Update(x => new EstoqueSaida{ flagExcluido = "S", dtAlteracao = DateTime.Now, idUsuarioCadastro = idUsuario });

            var OEstoqueEntrada = this.carregar(id);
            this.OProdutoBL.atualizarEstoque(OEstoqueEntrada.ProdutoEstoque.idProduto, OEstoqueEntrada.ProdutoEstoque.qtdMovimentada , "add");

			return true;
		}



        public IQueryable<EstoqueSaida> listarPorId(List<int> ids) {

            var query = db.EstoqueSaida.Include(x => x.ProdutoEstoque)
                                  .Include(x => x.TipoReferenciaSaida)
                                  .Select(n => n)
                                  .Where(x => ids.Contains(x.id));
            return query;
        }
    }
}