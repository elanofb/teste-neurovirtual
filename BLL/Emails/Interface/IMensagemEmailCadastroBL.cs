using System.Linq;
using DAL.Emails;

namespace BLL.Emails{
    public interface IMensagemEmailCadastroBL{
        /// <summary>
        /// Persistir e salvar os dados
        /// </summary>
        bool salvar(MensagemEmail OMensagemEmail);
    }
}