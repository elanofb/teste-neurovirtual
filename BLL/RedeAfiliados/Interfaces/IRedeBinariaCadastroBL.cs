using DAL.RedeAfiliados;
using DAL.RedeAfiliados.DTO;

namespace BLL.RedeAfiliados.Services {

    public interface IRedeBinariaCadastroBL {
        RedeBinaria salvar(NovoMembroRede ONovoMembro);
        bool salvar(RedeBinaria ORedeBinaria);
    }

}