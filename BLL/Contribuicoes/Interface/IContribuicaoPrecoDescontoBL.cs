using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Contribuicoes;

namespace BLL.Contribuicoes {

    public interface IContribuicaoPrecoDescontoBL {

        ContribuicaoPrecoDesconto carregar(int id);

        IQueryable<ContribuicaoPrecoDesconto> listar(int idContribuicao);

        bool salvar(ContribuicaoPrecoDesconto OContribuicaoPrecoDesconto);

        bool salvarLote(ContribuicaoPreco OContribuicaoPreco, List<ContribuicaoPrecoDesconto> listaDescontos);

        UtilRetorno excluirLote(int idContribuicaoPreco, int idUsuarioExclusao);
    }
}