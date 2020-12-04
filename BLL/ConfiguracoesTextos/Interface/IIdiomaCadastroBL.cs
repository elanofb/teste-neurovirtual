using System.Json;
using DAL.ConfiguracoesTextos;
using DAL.Contatos;

namespace BLL.ConfiguracoesTextos {

	public interface IIdiomaCadastroBL {

		bool salvar(Idioma OIdioma);

		JsonMessageStatus alterarStatus(int id);

	}
	
}