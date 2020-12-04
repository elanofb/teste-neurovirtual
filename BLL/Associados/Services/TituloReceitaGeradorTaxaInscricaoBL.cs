using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Financeiro;
using DAL.Associados;
using DAL.AssociadosContribuicoes;
using DAL.Financeiro;
using DAL.Pessoas;

namespace BLL.Associados {

    public class TituloReceitaGeradorTaxaInscricaoBL : TituloReceitaGeradorBL {

        //Atributos
        private ITipoAssociadoBL _TipoAssociadoBL;
        private ITituloReceitaBL _TituloReceitaBL;

        //Propriedades
		private int idTipoReceita { get; set; }
        private ITipoAssociadoBL OTipoAssociadoBL => _TipoAssociadoBL = _TipoAssociadoBL ?? new TipoAssociadoBL();
        private ITituloReceitaBL OTituloReceitaBL => _TituloReceitaBL = _TituloReceitaBL ?? new TituloReceitaTaxaInscricaoBL();

        //Eventos

		//Construtor
		public TituloReceitaGeradorTaxaInscricaoBL() {

			this.idTipoReceita = TipoReceitaConst.TAXA_INSCRICAO;
		}

        //
        public override UtilRetorno gerarLote(object OrigemTitulo) {
			throw new NotImplementedException();
		}

		//Metodo para geracao do titulo de receita
		public override UtilRetorno gerar(object OrigemTitulo){

			var OAssociado = (OrigemTitulo as Associado);

			if (OAssociado == null) {
                return UtilRetorno.newInstance(true, "O Objeto (associado) para gerar o título de taxa de inscrição não foi informado");
			}

		    var OTipoAssociado = this.OTipoAssociadoBL.carregar(UtilNumber.toInt32(OAssociado.idTipoAssociado));

			if (OTipoAssociado == null) {
                return UtilRetorno.newInstance(true, "O Objeto (tipo) para gerar o título de taxa de inscrição não foi informado");
			}

			//Verificar se o titulo já existe
			TituloReceita OTituloReceita = this.OTituloReceitaBL.carregarPorReceita(OAssociado.id);

            if (OTituloReceita != null) {

                return UtilRetorno.newInstance(false, "O título já foi gerado antes.", OTituloReceita);
            }

			OTituloReceita = new TituloReceita();

			OTituloReceita.idPessoa = OAssociado.idPessoa;

			OTituloReceita.idTipoReceita = (byte)idTipoReceita;

			OTituloReceita.idReceita = OAssociado.id;

            OTituloReceita.idOrganizacao = OAssociado.idOrganizacao;

            OTituloReceita.idUnidade = OAssociado.idUnidade;

            OTituloReceita.qtdeRepeticao = 1;

            OTituloReceita.mesCompetencia = (byte)DateTime.Today.Month;

            OTituloReceita.anoCompetencia = (short)DateTime.Today.Year;

            if (OTituloReceita.mesCompetencia > 0 && OTituloReceita.anoCompetencia > 0){

                byte? diaCompetencia = DateTime.Today.Day.toByte();

                diaCompetencia = diaCompetencia.toByte() > 0 ? diaCompetencia.toByte() : (byte)1;

                OTituloReceita.dtCompetencia = new DateTime(OTituloReceita.anoCompetencia.toInt(), OTituloReceita.mesCompetencia.toInt(), diaCompetencia.toInt());

            }

		    OTituloReceita.idContaBancaria = OTipoAssociado.idContaBancariaInscricao;

		    OTituloReceita.idCentroCusto = OTipoAssociado.idCentroCustoInscricao;

            OTituloReceita.idMacroConta = OTipoAssociado.idMacroContaInscricao;

            OTituloReceita.idCategoria = null;


			OTituloReceita.descricao = string.Concat("Taxa de Inscrição", " - ", OAssociado.Pessoa.nome).abreviar(100);

    		OTituloReceita.valorTotal = Math.Round(UtilNumber.toDecimal(OTipoAssociado.valorTaxaInscricao), 2);

            OTituloReceita.dtVencimentoOriginal = DateTime.Today.AddDays(OTipoAssociado.diasPrazoTaxaInscricao ?? 5);

            OTituloReceita.dtVencimento = OTituloReceita.dtVencimentoOriginal;

            this.preencherRecibo(ref OTituloReceita, OAssociado.Pessoa);

			this.salvar(OTituloReceita);

		    return UtilRetorno.newInstance(false, "O título foi gerado com sucesso.", OTituloReceita);
		}


    }
}
