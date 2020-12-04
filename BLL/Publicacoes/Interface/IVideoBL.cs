using System.Json;
using System.Linq;
using DAL.Publicacoes;
using System.Web;

namespace BLL.Publicacoes {

    public interface IVideoBL {

        IQueryable<Video> listar(string valorBusca, string ativo, int? idOrganizacaoParam = null);

        Video carregar(int id);

        bool existe(string descricao, int id);

        bool salvar(Video OVideo, HttpPostedFileBase OImagem);

        JsonMessageStatus alterarStatus(int id);

        bool excluir(int[] ids);
    }
}
