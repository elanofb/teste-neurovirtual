using System.Linq;
using DAL.DadosBancarios;
using DAL.Eventos;
using DAL.Historicos;

namespace BLL.Historicos.Interfaces {
    
    public interface IHistoricoAtualizacaoConsultaBL {

        IQueryable<HistoricoAtualizacao> query(int? idOrganizacaoParam = null);

        HistoricoAtualizacao carregar(int id);
        
        IQueryable<HistoricoAtualizacao> listar(int? idAssociado, int? idNaoAssociado, int? idPessoa);                
    }
}