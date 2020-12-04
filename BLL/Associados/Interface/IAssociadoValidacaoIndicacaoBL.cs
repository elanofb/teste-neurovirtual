using System;
using System.Linq;
using DAL.Associados;

namespace BLL.Associados {

    public interface IAssociadoValidacaoIndicacaoBL {
        
        UtilRetorno carregarMembroIndicacao(string rotaConta, int? idOrganizacaoParam = null);
        
        UtilRetorno carregarMembroIndicacao(int id, int? idOrganizacaoParam = null);

    }
}