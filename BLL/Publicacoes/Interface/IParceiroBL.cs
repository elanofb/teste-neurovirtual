using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using DAL.Publicacoes;
using System.Web;
using DAL.Arquivos;

namespace BLL.Publicacoes {

    public interface IParceiroBL {

        IQueryable<Parceiro> query(int? idOrganizacaoParam = null);

        Parceiro carregar(int id);

        IQueryable<Parceiro> listar(string valorBusca, string ativo, int idTipoParceiro, int? idPortal = 0);

        bool existe(string nome, int id);

        bool salvar(Parceiro OParceiro, HttpPostedFileBase OLogotipo);

        JsonMessageStatus alterarStatus(int id);

        UtilRetorno excluir(int id);

    }
}
