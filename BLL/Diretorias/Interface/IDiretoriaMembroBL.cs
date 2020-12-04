using System;
using System.Json;
using System.Linq;
using DAL.Diretorias;
using System.Web;

namespace BLL.Diretorias {

    public interface IDiretoriaMembroBL {

        DiretoriaMembro carregar(int id);

        IQueryable<DiretoriaMembro> listar(int idDiretoria, bool? ativo);

        bool existe(DiretoriaMembro ODiretoriaMembro, int id);

        bool salvar(DiretoriaMembro ODiretoriaMembro, HttpPostedFileBase OImagem);

        JsonMessageStatus alterarStatus(int id);

        UtilRetorno excluir(int id, int idUsuarioExclusao);

    }
}
