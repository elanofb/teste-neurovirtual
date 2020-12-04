using System;
using System.Linq;
using DAL.MateriaisApoio;
using System.Threading.Tasks;
using System.Web;
using DAL.Notificacoes;

namespace BLL.MateriaisApoio {

	public interface IMaterialApoioPessoaBL {

		MaterialApoioPessoa carregar(int idMaterialApoio, int idPessoa);
        IQueryable<MaterialApoioPessoa> listar(int idMaterialApoio, int idPessoa);
        bool salvar(MaterialApoioPessoa OMaterialApoioPessoa);
        bool excluir(int idMaterialApoio);

	}
}
