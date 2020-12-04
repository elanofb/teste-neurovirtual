using DAL.Localizacao;
using System.Threading.Tasks;

namespace BLL.Localizacao {

	public interface ICepBrasil {

		Task<CepBrasil> buscarEndereco(string cep);
	}
}
