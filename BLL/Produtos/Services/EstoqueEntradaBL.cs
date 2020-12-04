using System;
using System.Json;
using System.Linq;
using System.Data.Entity;
using DAL.Produtos;
using DAL.Repository.Base;
using EntityFramework.Extensions;
using System.Web;
using System.Collections.Generic;
using BLL.Services;
using DAL.Permissao.Security.Extensions;

namespace BLL.Produtos {

	public class EstoqueEntradaBL : DefaultBL, IEstoqueEntradaBL {

		//Atributos
        private IProdutoBL _ProdutoBL;

		//Propriedades
        private IProdutoBL OProdutoBL => (this._ProdutoBL = this._ProdutoBL ?? new ProdutoBL() );

	    //Construtor
		public EstoqueEntradaBL(){
		}
		
		//Carregamento de registro pelo Identificador
		public EstoqueEntrada carregar(int id) {
			return db.EstoqueEntrada.Include(x => x.ProdutoEstoque)
                                    .Include(x => x.Fornecedor)
									.FirstOrDefault(x => x.id == id);
		}
		
		//montagem de query para listagem de registro de acordo com parametros informados
		public IQueryable<EstoqueEntrada> listar(int idFornecedor, int idProduto, string valorBusca, string ativo) {

			var query = (from oPro in db.EstoqueEntrada
										.Include(x => x.ProdutoEstoque)
                                        .Include(x => x.Fornecedor)
						where 
							oPro.flagExcluido == "N"
						select oPro
						);

			if (idFornecedor > 0) { 
				query = query.Where(x => x.idFornecedor == idFornecedor);
			}

			if (idProduto > 0) { 
				query = query.Where(x => x.ProdutoEstoque.idProduto == idProduto);
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
		public bool salvar(EstoqueEntrada OEstoqueEntrada) {

			if (OEstoqueEntrada.id == 0) {
                Produto OProduto = this.OProdutoBL.carregar(OEstoqueEntrada.ProdutoEstoque.idProduto);
                OEstoqueEntrada.ProdutoEstoque.saldoAnterior = OProduto.qtde;
				return this.inserir(OEstoqueEntrada);
			}

			return this.atualizar(OEstoqueEntrada);
		}

		//Persistir o objecto e salvar na base de dados
		private bool inserir(EstoqueEntrada OEstoqueEntrada) { 
			
			OEstoqueEntrada.setDefaultInsertValues<EstoqueEntrada>();
			OEstoqueEntrada.ProdutoEstoque.setDefaultInsertValues<ProdutoEstoque>();

			db.EstoqueEntrada.Add(OEstoqueEntrada);
			db.SaveChanges();

            //Atualizo o estoque do produto
            new ProdutoBL().atualizarEstoque(OEstoqueEntrada.ProdutoEstoque.idProduto, OEstoqueEntrada.ProdutoEstoque.qtdMovimentada,"add");

			return OEstoqueEntrada.id > 0;
		}

		//Persistir o objecto e atualizar informações
		private bool atualizar(EstoqueEntrada OEstoqueEntrada) { 
			
			//Localizar existentes no banco
			EstoqueEntrada dbEstoqueEntrada = this.carregar(OEstoqueEntrada.id);
			ProdutoEstoque dbProdutoEstoque = db.ProdutoEstoque.FirstOrDefault(x => x.id == dbEstoqueEntrada.idProdutoEstoque);

			//Configurar valores padrão
			OEstoqueEntrada.setDefaultUpdateValues<EstoqueEntrada>();
			OEstoqueEntrada.ProdutoEstoque.setDefaultUpdateValues<ProdutoEstoque>();
			OEstoqueEntrada.idProdutoEstoque= dbProdutoEstoque.id;
			OEstoqueEntrada.ProdutoEstoque.id = dbProdutoEstoque.id;

			//Atualizacao da Funcionario
			var EstoqueEntradaEntry = db.Entry(dbEstoqueEntrada);
			EstoqueEntradaEntry.CurrentValues.SetValues(OEstoqueEntrada);
			EstoqueEntradaEntry.ignoreFields<EstoqueEntrada>();

			//Atualizacao Dados Pessoa
			var ProdutoEstoqueEntry = db.Entry(dbProdutoEstoque);
			ProdutoEstoqueEntry.CurrentValues.SetValues(OEstoqueEntrada.ProdutoEstoque);
			ProdutoEstoqueEntry.ignoreFields<ProdutoEstoque>();

			db.SaveChanges();
			return OEstoqueEntrada.id > 0;
		}

		//Verificar existencia de EstoqueEntradas com mesmo nome para evitar duplicidades
		public bool existe(DateTime? dtMovimentacao, int idFornecedor, int idProduto, int qtdMovimentada, int id) {

			var query = from Estoq in db.EstoqueEntrada
						where Estoq.ProdutoEstoque.dtMovimentacao == dtMovimentacao && 
                              Estoq.idFornecedor == idFornecedor && 
                              Estoq.ProdutoEstoque.idProduto == idProduto && 
                              Estoq.ProdutoEstoque.qtdMovimentada == qtdMovimentada && 
                              Estoq.id != id && 
                              Estoq.flagExcluido == "N"
						select Estoq;

			var OEstoqueEntrada = query.AsNoTracking().Take(1).FirstOrDefault();
			return (OEstoqueEntrada != null);
		}

		//Remover o registro do sistema (exclusao lógico - não se remove fiscamente do banco de dados)
		public bool excluir(int id) {

		    var idUsuario = User.id();

			db.EstoqueEntrada
				.Where(x => x.id == id)
				.Update(x => new EstoqueEntrada{ flagExcluido = "S", dtAlteracao = DateTime.Now, idUsuarioCadastro = idUsuario });

            var OEstoqueEntrada = db.EstoqueEntrada.FirstOrDefault(x => x.id == id);
            this.OProdutoBL.atualizarEstoque(OEstoqueEntrada.ProdutoEstoque.idProduto, OEstoqueEntrada.ProdutoEstoque.qtdMovimentada , "remover");

			return true;
		}

        public IQueryable<EstoqueEntrada> listarPorId(List<int> ids) {

            return db.EstoqueEntrada.Include(x => x.Fornecedor)
                                    .Include(x => x.ProdutoEstoque)
                                    .Select(x => x)
                                    .Where(x => ids.Contains(x.id));

        }
	}
}