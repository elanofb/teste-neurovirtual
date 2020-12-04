using System.Linq;
using DAL.Emails;

namespace BLL.Emails{
    
    public interface IMensagemEmailConsultaBL{
        
        IQueryable<MensagemEmail> query(int? idOrganizacaoParam = null);
        
        MensagemEmail carregar(int id);

        IQueryable<MensagemEmail> listar(string codigoIdentificacao, int? idReferencia = null);
        
        bool existe(string codigoIdentificacao, int idOrganizacao);
    }
}