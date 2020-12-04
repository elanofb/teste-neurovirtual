using System.Linq;
using DAL.Emails;

namespace BLL.Emails{
    
    public interface IMensagemEmailExclusaoBL{                        
        bool excluir(int id);
    }
}