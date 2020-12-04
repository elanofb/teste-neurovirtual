using System.Json;
using System.Linq;
using DAL.Popups;

namespace BLL.Popups {

    public interface IHomePopupBL {

        IQueryable<HomePopup> listar(string valorBusca = "", bool? ativo = true, int? idPortal = 0);

        HomePopup carregar(int id);

        HomePopup carregarPopupDisponivel();

        bool salvar(HomePopup OHomePopup);
        
        JsonMessageStatus alterarStatus(int id);

        bool excluir(int[] ids);

        bool existe(string descricao, int id);

	}

}
