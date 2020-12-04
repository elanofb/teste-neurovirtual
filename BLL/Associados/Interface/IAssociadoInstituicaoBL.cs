using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Associados;

namespace BLL.Associados
{
    public interface IAssociadoInstituicaoBL
    {
        AssociadoInstituicao carregar(int id);
        IQueryable<AssociadoInstituicao> listar(int idAssociado, bool? ativo);
        bool existe(AssociadoInstituicao OAssociadoInstituicao, int idDesconsiderado);
        UtilRetorno salvarLote(List<int> idsInstituicao, int idAssociado);
        bool salvar(AssociadoInstituicao OAssociadoInstituicao);
        UtilRetorno excluir(int idInstituicao, int idAssociado);
        UtilRetorno excluirPorId(int id);
    }
}