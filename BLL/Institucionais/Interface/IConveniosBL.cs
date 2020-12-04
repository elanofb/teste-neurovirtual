using System.Linq;
using DAL.Institucionais;
using System.Json;
using System;
using System.Collections.Generic;
using System.Web;

namespace BLL.Institucionais {

    public interface IConvenioBL {

        IQueryable<Convenio> query(int? idOrganizacaoParam = null);

        Convenio carregar(int id);

        IQueryable<Convenio> listar(string valorBusca, bool? ativo, int? idTipoConvenio);

        bool existe(string titulo, string chaveUrl, int id);

        bool salvar(Convenio OConvenio, HttpPostedFileBase OArquivo);

        JsonMessageStatus alterarStatus(int id);

        UtilRetorno excluir(int id, int idUsuarioExclusao);
    }
}
