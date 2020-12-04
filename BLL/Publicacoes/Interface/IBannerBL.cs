using System.Json;
using System.Linq;
using DAL.Publicacoes;
using System.Web;

namespace BLL.Publicacoes {

	public interface IBannerBL{

		Banner carregar(int id);

		IQueryable<Banner> listar(string posicao, string valorBusca, string ativo);

		bool salvar(Banner OBanner, HttpPostedFileBase OImagem);

        JsonMessageStatus alterarStatus(int id);

		bool excluir(int id);

	}
}