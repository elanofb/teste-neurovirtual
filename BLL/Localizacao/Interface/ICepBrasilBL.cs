using DAL.Localizacao;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Localizacao {

	public interface ICepBrasilBL {

		Task<CepBrasil> buscarEndereco(string cep);

        Task<CepBrasil> carregar(string cep);

        Task<List<CepBrasil>> listarLoteCep(List<string> listaCep);

    }
}
