using System;
using System.Data.Entity;
using System.Linq;
using BLL.Core.Events;
using BLL.Services;
using DAL.Pedidos;
using DAL.Pessoas;

namespace BLL.Pedidos {

    public class PedidoBL : DefaultBL, IPedidoBL {
        
		// Events
        private EventAggregator onPedidoCadastro => OnPedidoCadastrado.getInstance;

		//
		public PedidoBL() {
            
        }

        //
        public IQueryable<Pedido> query(int? idOrganizacaoParam = null) {

            var query = from Obj in this.db.Pedido
                        where Obj.flagExcluido == "N"
                        select Obj;

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;

        }

		//
		public Pedido carregar(int id) {

		    var query = this.query().condicoesSeguranca()
		                    .Include(x => x.Pessoa)
		                    .Include(x => x.StatusPedido)
		                    .Include(x => x.CupomDesconto)
		                    .Include(x => x.listaPedidoEntrega);
            
		    return query.FirstOrDefault(x => x.id == id);
            
		}

		//
		public IQueryable<Pedido> listar(string valorBusca, string ativo, int idStatusPedido) {

			var query = this.query().condicoesSeguranca();

			if (!String.IsNullOrEmpty(valorBusca)) {

                var idPedido = valorBusca.toInt();

				query = query.Where(x => x.nomePessoa.Contains(valorBusca) || x.id == idPedido);
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}
            
			if (idStatusPedido > 0) {
				query = query.Where(x => x.idStatusPedido == idStatusPedido);
			}
            
			return query;
		}

		//Listagem de Registros
		public IQueryable<Pedido> listarPorPessoa(int idPessoa) {

			var query = this.query().condicoesSeguranca();

			query = query.Where(x => x.idPessoa == idPessoa);
            
			return query;

		}

		//Salvar um novo pedido ou atualizar um pedido existente
		public bool salvar(Pedido OPedido) {

		    if (OPedido.Pessoa != null) {

		        OPedido.nomePessoa = OPedido.Pessoa.nome;

                OPedido.cpf = UtilString.onlyNumber(OPedido.Pessoa.nroDocumento);

		        OPedido.rg = OPedido.Pessoa.rg;

			    OPedido.telPrincipal = UtilString.onlyNumber(OPedido.Pessoa.formatarTelPrincipal());
			
			    OPedido.telSecundario = UtilString.onlyNumber(OPedido.Pessoa.formatarTelSecundario());

		        OPedido.email = OPedido.Pessoa.emailPrincipal();
		        
		    }

            var flagSucesso = true;

			try {

                OPedido.Pessoa = null;

			    OPedido.CupomDesconto = null;

			    OPedido.CentroCusto = null;

			    OPedido.MacroConta = null;

			    OPedido.CategoriaTitulo = null;

			    OPedido.ContaBancaria = null;

			    flagSucesso = this.inserir(OPedido);

			} catch (Exception ex) {

				UtilLog.saveError(ex, String.Format("Erro ao processar o pedido {0}, pessoa: {1}.", OPedido.id, OPedido.idPessoa));

			    flagSucesso = false;

			}

			return flagSucesso;
		}

        private bool inserir(Pedido OPedido) {

            OPedido.idStatusPedido = StatusPedidoConst.EM_ABERTO;

            OPedido.listaProdutos.ForEach(x => {

                x.Produto = null;

                x.setDefaultInsertValues();
            });

            OPedido.listaPedidoEntrega.ForEach(x => {

                x.Cidade = null;

                x.Estado = null;

                x.Pais = null;

                x.setDefaultInsertValues();
            });

            OPedido.setDefaultInsertValues();

            this.db.Pedido.Add(OPedido);

            this.db.SaveChanges();
                
            var flagSucesso = OPedido.id > 0;

            if (flagSucesso) {

                this.onPedidoCadastro.subscribe(new PedidoCadastradoHandler());

                this.onPedidoCadastro.publish( (OPedido as object) );

            }

            return flagSucesso;

        }

	}

}