using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Contribuicoes;

namespace BLL.Contribuicoes {

    public interface IContribuicaoVencimentoBL {

        ContribuicaoVencimento carregar(int id);

        IQueryable<ContribuicaoVencimento> listar(int idContribuicao);

        bool salvar(ContribuicaoVencimento OContribuicaoVencimento);

        bool salvarLote(Contribuicao OContribuicao, List<ContribuicaoVencimento> listaVencimento);

        UtilRetorno excluirLote(int idContribuicao, int idUsuarioExclusao);
    }
}