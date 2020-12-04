using System.Data.Entity;
using System.Linq;
using BLL.Financeiro;
using DAL.Financeiro;

namespace BLL.Planos {

    public class TituloReceitaPlanoContratacaoBL : TituloReceitaBL, ITituloReceitaBL {

		//Atributos
        private IPlanoContratacaoBL _IPlanoContratacaoBL;

		//Propriedades
		private int idCentroCusto { get; set; }
        private IPlanoContratacaoBL OPlanoContratacaoBL { get { return (this._IPlanoContratacaoBL = this._IPlanoContratacaoBL ?? new PlanoContratacaoBL()); } }

		//Construtor
		public TituloReceitaPlanoContratacaoBL() {
			this.idCentroCusto = CentroCustoConst.PLANO;
		}

		// Carregar um titulo a partir do tipo da receita e da receita
		public override TituloReceita carregarPorReceita(int id) {
			
			var query = from Tit in db.TituloReceita
										.Include(x => x.CentroCusto)
										.Include(x => x.Pessoa)
						where
							Tit.idReceita == id && 
							Tit.idCentroCusto == idCentroCusto &&
							Tit.dtExclusao == null
						select
							Tit;

			return query.FirstOrDefault();
		}

		//Metodo para geracao do titulo de receita
		//public override TituloReceita gerar(object OrigemTitulo){

		//	var OPlanoContratacao = (OrigemTitulo as PlanoContratacao);
  //          var dbPlanoContratacao = this.OPlanoContratacaoBL.carregar(OPlanoContratacao.id);
			
		//	//Verificar se o titulo já existe
		//	TituloReceita OTituloReceita = this.listar(TipoReceitaConst.PLANO, OPlanoContratacao.id, UtilNumber.toInt32(OPlanoContratacao.idPessoa), "").FirstOrDefault() ?? new TituloReceita();

		//	OTituloReceita.listaTituloReceitaPagamento = new List<TituloReceitaPagamento>();

		//	if (OTituloReceita.id == 0) { 

		//		OTituloReceita.idPessoa = dbPlanoContratacao.idPessoa;
		//		OTituloReceita.idTipoReceita = TipoReceitaConst.PLANO;
		//		OTituloReceita.idReceita = dbPlanoContratacao.id;                
  //              OTituloReceita.ativo = true;
  //              OTituloReceita.dtVencimento = DateTime.Today.AddDays(5);
  //              OTituloReceita.descricao = dbPlanoContratacao.Plano.nome +" - "+ dbPlanoContratacao.Pessoa.exibirNome();
  //              OTituloReceita.qtdeRepeticao = 1;
  //              OTituloReceita.valorTotal = dbPlanoContratacao.valor;

  //              //Seleciono uma conta bancária

  //              var OContaBancaria =  this.db.ContaBancaria.FirstOrDefault(x => x.flagExcluido == false && x.ativo == true);

  //              if(OContaBancaria != null) OTituloReceita.idContaBancaria =  OContaBancaria.id;

  //              //Adiciono um registro de pagamento da receita, ainda em aberto
  //              TituloReceitaPagamento OTituloReceitaPagamento = new TituloReceitaPagamento();
  //              OTituloReceitaPagamento.descricaoParcela = OTituloReceita.descricao;
  //              OTituloReceitaPagamento.valorOriginal = OTituloReceita.valorTotal.Value;
  //              OTituloReceitaPagamento.dtVencimento = OTituloReceita.dtVencimento.Value;
  //              OTituloReceita.listaTituloReceitaPagamento.Add(OTituloReceitaPagamento);
		//	}

		//	OTituloReceita.diaCompetencia = Convert.ToByte(dbPlanoContratacao.dtCadastro.Day);
		//	OTituloReceita.mesCompetencia = Convert.ToByte(dbPlanoContratacao.dtCadastro.Month);
		//	OTituloReceita.anoCompetencia = Convert.ToInt16(dbPlanoContratacao.dtCadastro.Year);
		//	OTituloReceita.valorTotal = dbPlanoContratacao.valor;

		//	OTituloReceita.nomePessoa = dbPlanoContratacao.Pessoa.exibirNome();
		//	OTituloReceita.documentoPessoa = dbPlanoContratacao.Pessoa.nroDocumento;
		//	OTituloReceita.nroTelPrincipal = dbPlanoContratacao.Pessoa.nroTelPrincipal;
		//	OTituloReceita.nroTelSecundario = dbPlanoContratacao.Pessoa.nroTelSecundario;
		//	OTituloReceita.emailPrincipal = dbPlanoContratacao.Pessoa.emailPrincipal;
			
		//	//Recibo em nome do proprio associado por padrão
		//	OTituloReceita.nomeRecibo = dbPlanoContratacao.Pessoa.exibirNome();
		//	OTituloReceita.documentoRecibo = dbPlanoContratacao.Pessoa.nroDocumento;

		//	this.salvar(OTituloReceita);

		//    return OTituloReceita;
		//}

  //      // Gera títulos em lote
  //      public override void gerarLote(object OrigemTitulo) {
  //          throw new NotImplementedException();
  //      }

		////Salvar uma receita no banco de dados
		//public override TituloReceita salvar(TituloReceita OTituloReceita) {
		//	base.salvar(OTituloReceita);

		//	return OTituloReceita;
		//}

		////Registrar a liquidacao do titulo (pagamento total)
		//public override TituloReceita liquidar(TituloReceita OTituloReceita) {
		//	base.liquidar(OTituloReceita);

		//	return OTituloReceita;
		//}

		////
		//public override TituloReceita liquidar(int idReferencia, List<TituloReceitaPagamento> listaPagamentos) {
		//	throw new NotImplementedException();
		//}
    }
}