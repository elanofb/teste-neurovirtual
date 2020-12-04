using System.Linq;
using DAL.SegmentosAtuacao;

namespace BLL.SegmentosAtuacao {

	public interface ISegmentoAtuacaoConsultaBL {

		SegmentoAtuacao carregar(int id);
		IQueryable<SegmentoAtuacao> listar(string valorBusca, bool? ativo);
		bool existe(string descricao, int idDesconsiderado);

	}
}