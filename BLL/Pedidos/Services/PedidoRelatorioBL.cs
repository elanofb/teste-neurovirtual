using System;
using System.Linq;
using BLL.Services;
using DAL.Associados;
using DAL.Pedidos.DTO;

namespace BLL.Pedidos {

    public class PedidoRelatorioBL : DefaultBL, IPedidoRelatorioBL {
        

		//
		public PedidoRelatorioBL() {
            
        }

        //
        public IQueryable<PedidoGeralDTO> query(int? idOrganizacaoParam = null) {

	        var query = from Ped in this.db.Pedido where Ped.flagExcluido == "N"
						select new PedidoGeralDTO {
							id = Ped.id,
							idOrganizacao = Ped.idOrganizacao,
							idUnidade = Ped.idUnidade,
							idPessoa = Ped.idPessoa,
							nomePessoa = Ped.nomePessoa,
							cpf = Ped.cpf,
							rg = Ped.rg,
							email = Ped.email,
							telPrincipal = Ped.telPrincipal,
							telSecundario = Ped.telSecundario,
							valorProdutos = Ped.valorProdutos,
							valorFrete = Ped.valorFrete,
							flagFreteGratis = Ped.flagFreteGratis,
							valorDesconto = Ped.valorDesconto,
							valorDescontoCupom = Ped.valorDescontoCupom,
							idStatusPedido = Ped.idStatusPedido,
							descricaoStatusPedido = Ped.StatusPedido.descricao,
							listaProdutos = Ped.listaProdutos,
							listaEntregas = Ped.listaPedidoEntrega,
							dtCadastro = Ped.dtCadastro,
							dtQuitacao = Ped.dtQuitacao,
							dtAtendimento = Ped.dtAtendimento,
							dtPreparacao = Ped.dtPreparacao,
							dtExpedicao = Ped.dtExpedicao,
							dtCancelamento = Ped.dtCancelamento,
							dtFinalizado = Ped.dtFinalizado,
							flagFaturamentoCadastro = Ped.flagFaturamentoCadastro,
							flagPagamentoNaEntrega = Ped.flagPagamentoNaEntrega,
							dtVencimento = Ped.dtVencimento,
							dtFaturamento = Ped.dtFaturamento,
							idUsuarioCadastro = Ped.idUsuarioCadastro,
							nomeUsuarioCadastro = Ped.UsuarioCadastro.nome,
							idUsuarioAlteracao = Ped.idUsuarioAlteracao,
							idTipoCadastro = Ped.idAssociado > 0 ? AssociadoTipoCadastroConst.CONSUMIDOR : (Ped.idNaoAssociado > 0 ? AssociadoTipoCadastroConst.COMERCIANTE : (byte) 0),
							ativo = Ped.ativo
						};

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;

        }

		//
		public PedidoGeralDTO carregar(int id) {

			var query = this.query().condicoesSeguranca();
            
		    return query.FirstOrDefault(x => x.id == id);
            
		}

		//
		public IQueryable<PedidoGeralDTO> listar(string valorBusca, string ativo, int idStatusPedido) {

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

	}

}