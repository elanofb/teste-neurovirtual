using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;

namespace BLL.Email {

	public interface IEmailBL {

		LogEmail carregar(int id);

		IQueryable<LogEmail> listar(string flagFluxo, string valorBusca, string flagLixeira);

		IQueryable<EmailContatoVW> listarContatos(string emailOrigem);

		bool salvarEmail(LogEmail NovoEmail);

		void registrarLeitura(int id);

		Task<List<LogEmail>> downloadAsync(string host, int nroPorta, bool flagSSL, string usuario, string senha, List<string> emailJaBaixados);

		bool excluir(int id);

		void lixeira(int id);

		void restaurar(int id);
	}
}