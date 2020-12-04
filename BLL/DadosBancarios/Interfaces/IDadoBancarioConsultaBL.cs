using System.Linq;
using DAL.DadosBancarios;
using DAL.Eventos;

namespace BLL.DadosBancarios.Interfaces {
    
    public interface IDadoBancarioConsultaBL {

        IQueryable<DadoBancario> query(int? idOrganizacaoParam = null);

        DadoBancario carregar(int id);
        
        IQueryable<DadoBancario> listar(string valorBusca, bool? ativo);                
    }
}