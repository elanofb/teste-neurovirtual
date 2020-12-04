using System;
using System.Linq;
using System.Json;
using System.Collections.Generic;
using DAL.ContasBancarias;

namespace BLL.ContasBancarias {

    public interface IContaBancariaMovimentacaoBL {

        ContaBancariaMovimentacao carregar(int id);

        IQueryable<ContaBancariaMovimentacao> listar(string valorBusca, string ativo, int idContaBancaria, int idTipoOperacao, DateTime? dtInicio, DateTime? dtFim);

        bool existe(ContaBancariaMovimentacao OContaMovimentacao, bool descricao);

        bool salvar(ContaBancariaMovimentacao OContaMovimentacao);

        bool excluir(int id);

        JsonMessageStatus alterarStatus(int id);

        IQueryable<ContaBancariaMovimentacao> listarPorId(List<int> ids);

        IQueryable<ContaBancariaMovimentacao> listarPorContaBancaria(int idContaBancariaBeneficiada);

        void conciliarMovimentacao(int id, bool flagConciliado);

    }
}