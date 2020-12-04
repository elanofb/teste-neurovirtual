using System.Json;
using DAL.Planos;
using DAL.Produtos;

namespace BLL.Planos {

	public interface IPlanoCarreiraCadastroBL{
        
	    bool salvar(PlanoCarreira OPlanoCarreira);
        
	    JsonMessageStatus alterarStatus(int id);
        
	}
}