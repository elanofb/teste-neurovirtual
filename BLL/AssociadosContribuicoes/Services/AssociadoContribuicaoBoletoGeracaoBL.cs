using System;
using System.Linq;
using BLL.Financeiro;
using BLL.Services;
using DAL.AssociadosContribuicoes;
using DAL.Financeiro;
using DAL.Permissao.Security.Extensions;

namespace BLL.AssociadosContribuicoes {

    public class AssociadoContribuicaoBoletoGeracaoBL : DefaultBL {

        //Atributos
        private ITituloReceitaGeradorBL _TituloReceitaGeradorBL;
        private ITituloReceitaBL _TituloReceitaBL;
        private ITituloReceitaPagamentoBL _TituloReceitaPagamentoBL;

        //Servicos
        private ITituloReceitaGeradorBL OTituloReceitaGeradorBL => _TituloReceitaGeradorBL = _TituloReceitaGeradorBL ?? new TituloReceitaGeradorContribuicaoBL();
        private ITituloReceitaBL OTituloReceitaBL => _TituloReceitaBL = _TituloReceitaBL ?? new TituloReceitaContribuicaoBL();
        private ITituloReceitaPagamentoBL OTituloReceitaPagamentoBL => _TituloReceitaPagamentoBL = _TituloReceitaPagamentoBL ?? new TituloReceitaPagamentoBL();

        //Propriedades

        //Events

        /// <summary>
        /// Gerar boleto bancario para uma contribuicao
        /// </summary>
        public UtilRetorno gerarPagamentoBoleto(AssociadoContribuicao OAssociadoContribuicao) {

            var OTituloReceita = OTituloReceitaBL.carregarPorReceita(OAssociadoContribuicao.id);

            if (OTituloReceita == null) {

                var RetornoGerador = this.OTituloReceitaGeradorBL.gerar(OAssociadoContribuicao as object);

                OTituloReceita = RetornoGerador.info as TituloReceita;
            }


            string nroDocumento = OTituloReceita.documentoRecibo;

            string cep = OTituloReceita.cepRecibo;

            if (!UtilValidation.isCPF(nroDocumento) && !UtilValidation.isCNPJ(nroDocumento)) {

                return UtilRetorno.newInstance(true, $"O documento do associado é inválido para geração de boleto bancário: {nroDocumento}");

            }

            if (!UtilValidation.isCEP(cep)) {

                return UtilRetorno.newInstance(true, $"O CEP do sacado informado é inválido para geração de boleto bancário: {cep}");

            }
            var OPagamento = new TituloReceitaPagamento();

            OPagamento.transferirDadosTitulo(OTituloReceita);

            OPagamento.idMeioPagamento = MeioPagamentoConst.BOLETO_BANCARIO;

            OPagamento.idFormaPagamento = FormaPagamentoConst.BOLETO_BANCARIO;

            OPagamento.idStatusPagamento = StatusPagamentoConst.AGUARDANDO_PAGAMENTO;

            OTituloReceitaPagamentoBL.salvar(OPagamento);

            //Essas atribuicoes sao feitas pois apos a geracao do registro é enviado para geracao de boletos e os objetos abaixo serao necessarios, assim economiza-se uma nova ida ao banco de dados
            OPagamento.TituloReceita = OTituloReceita;

            return UtilRetorno.newInstance(false, "Os dados para geração do boleto foram configurados com sucesso.", OPagamento);
        }

        /// <summary>
        /// Excluir um item da fila de geracao de boletos
        /// </summary>
        public UtilRetorno excluir(int id) {
            
            var ORegistro = db.AssociadoContribuicaoBoletoGeracao.condicoesSeguranca().FirstOrDefault(x => x.id == id);

            if (ORegistro == null) {
                return UtilRetorno.newInstance(true, "Não foi possível localizar o registro.");
            }

            if (ORegistro.dtGeracao.HasValue) {
                return UtilRetorno.newInstance(true, "Não é possível remover o registro com a data de geração preenchida.");
            }

            ORegistro.dtExclusao = DateTime.Now;
            ORegistro.idUsuarioExclusao = User.id();

            db.SaveChanges();

            return UtilRetorno.newInstance(false, "Registro removido com sucesso.");
        }
    }
}