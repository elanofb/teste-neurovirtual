using System;
using System.Linq;
using BLL.Services;
using DAL.Notificacoes;

namespace BLL.Notificacoes {

    public class TemplateMensagemConsultaBL : DefaultBL, ITemplateMensagemConsultaBL {

        public const string keyCache = "template_mensagem";
        
        // 
        public IQueryable<TemplateMensagem> query(int? idOrganizacaoParam = null) {

            var query = from PA in db.TemplateMensagem
                where PA.dtExclusao == null
                select PA;

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;

        }

        //
        public TemplateMensagem carregar(int id) {

            var query = this.query().condicoesSeguranca();

            return query.FirstOrDefault(x => x.id == id);
        }


        //
        public IQueryable<TemplateMensagem> listar(string valorBusca, bool? ativo = true) {

            var query = this.query().condicoesSeguranca();

            if(!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.titulo.Contains(valorBusca));
            }

            if(ativo != null) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

    }
    
}