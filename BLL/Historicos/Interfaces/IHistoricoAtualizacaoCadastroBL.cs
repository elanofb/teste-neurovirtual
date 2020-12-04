using System.Json;
using DAL.DadosBancarios;
using DAL.Eventos;
using DAL.Historicos;

namespace BLL.Historicos.Interfaces {
    
    public interface IHistoricoAtualizacaoCadastroBL {

        bool salvar(HistoricoAtualizacao OHistoricoAtualizacao);
       
    }
}