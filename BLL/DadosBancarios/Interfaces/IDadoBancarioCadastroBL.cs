using System.Json;
using DAL.DadosBancarios;
using DAL.Eventos;

namespace BLL.DadosBancarios.Interfaces {
    
    public interface IDadoBancarioCadastroBL {

        bool salvar(DadoBancario ODadoBancario);

        JsonMessageStatus alterarStatus(int id);

    }
}