using System;
using System.Linq;
using BLL.Services;
using DAL.Compras;
using EntityFramework.Extensions;

namespace BLL.Compras {

	public class CarrinhoResumoBL : DefaultBL, ICarrinhoResumoBL {

		//Atributos

		//Propriedades

		//Carregar registro
		public CarrinhoResumo carregarExistente(int idOrganizacaoParam, int? idPessoa, string idSessao) {
			
			var query = from Car in db.CarrinhoResumo 
						where 
                            Car.idOrganizacao == idOrganizacaoParam && 
							Car.idSessao == idSessao && 
							Car.dtExclusao == null
						select 
							Car;

			if (idPessoa > 0) {
				query = query.Where(x => x.idPessoa == idPessoa);
			}

			return query.OrderByDescending(x => x.id).FirstOrDefault();
		}

		//Salvar registro
		public void salvar(CarrinhoResumo OCarrinhoResumo) {

			OCarrinhoResumo.cepDestino = UtilString.onlyNumber(OCarrinhoResumo.cepDestino);

			var CarrinhoExistente = this.carregarExistente(OCarrinhoResumo.idOrganizacao, OCarrinhoResumo.idPessoa, OCarrinhoResumo.idSessao) ?? new CarrinhoResumo();

			if (CarrinhoExistente.id == 0) {

				this.inserir(OCarrinhoResumo);

			} else {

				CarrinhoExistente.valorFrete = OCarrinhoResumo.valorFrete;

				CarrinhoExistente.idTipoFrete = OCarrinhoResumo.idTipoFrete;

				CarrinhoExistente.valorDesconto = OCarrinhoResumo.valorDesconto;

				CarrinhoExistente.cepDestino = OCarrinhoResumo.cepDestino.onlyNumber().abreviar(8);

				CarrinhoExistente.prazoEntrega = OCarrinhoResumo.prazoEntrega;

                CarrinhoExistente.logradouroEntrega = OCarrinhoResumo.logradouroEntrega.abreviar(100);

                CarrinhoExistente.numeroEntrega = OCarrinhoResumo.numeroEntrega.abreviar(20);

                CarrinhoExistente.complementoEntrega = OCarrinhoResumo.complementoEntrega.abreviar(50);

                CarrinhoExistente.bairroEntrega = OCarrinhoResumo.bairroEntrega.abreviar(80);

                CarrinhoExistente.nomeCidadeEntrega = OCarrinhoResumo.nomeCidadeEntrega.abreviar(80);

                CarrinhoExistente.siglaEstadoEntrega = OCarrinhoResumo.siglaEstadoEntrega.abreviar(2);

                CarrinhoExistente.idCidadeEntrega = OCarrinhoResumo.idCidadeEntrega;

                CarrinhoExistente.idEstadoEntrega = OCarrinhoResumo.idEstadoEntrega;

				CarrinhoExistente.dtAlteracao = DateTime.Now;

				db.SaveChanges();

			}
		}

		//Persistir e inserir um novo registro 
		private bool inserir(CarrinhoResumo OCarrinhoResumo) { 

			db.CarrinhoResumo.Add(OCarrinhoResumo);

			db.SaveChanges();

			return OCarrinhoResumo.id > 0;
		}

		//Limpar o carrinho de compras do usuário
		// 1 - Limpar os dados do resumo do carrinho
		// 2 - Limpar os itens adicionados no carrinho
		public void limpar(int idOrganizacaoParam, int? idPessoa, string idSessao) {

			var OCarrinhoResumo = this.carregarExistente(idOrganizacaoParam, idPessoa, idSessao);

			if (OCarrinhoResumo == null) {
				return;
			}

			OCarrinhoResumo.dtExclusao = DateTime.Now;
			
			this.db.SaveChanges();

		    this.db.CarrinhoItem.Where( x => x.idOrganizacao == idOrganizacaoParam && x.idPessoa == idPessoa && x.idSessao == idSessao && x.dtExclusao == null)
		                        .Update(x => new CarrinhoItem {dtExclusao = DateTime.Now});
		}

		//Vincular um carrinho existente ao ID da pessoa logada
		public UtilRetorno limparFrete(int idOrganizacaoParam, int? idPessoaLogada, string idSessao) {
			
			var OCarrinhoResumo = this.carregarExistente(idOrganizacaoParam, idPessoaLogada, idSessao);

			if (OCarrinhoResumo == null) {
				return UtilRetorno.newInstance(true, "Registro não localizado");
			}

			OCarrinhoResumo.valorFrete = new decimal(0);
			
			this.db.SaveChanges();

            return UtilRetorno.newInstance(false);

		}

		//Vincular um carrinho existente ao ID da pessoa logada
		public void vincular(int? idPessoaLogada, string idSessao) {
			
			var OCarrinhoResumo = this.carregarExistente(0, 0, idSessao);

			if (OCarrinhoResumo == null) {
				return;
			}

			OCarrinhoResumo.idPessoa = idPessoaLogada;
			
			this.db.SaveChanges();

		}
	}
}
