using System;
using System.Linq;
using BLL.Services;
using DAL.Notificacoes;

namespace BLL.Notificacoes {

    public class PessoaDeviceConsultaBL : DefaultBL, IPessoaDeviceConsultaBL {

        // 
        public IQueryable<PessoaDevice> query(int? idOrganizacaoParam = null) {

            var query = from PA in db.PessoaDevice
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
        public PessoaDevice carregar(int id) {

            var query = this.query().condicoesSeguranca();

            return query.FirstOrDefault(x => x.id == id);
        }


        //
        public IQueryable<PessoaDevice> listar(string valorBusca, bool? ativo = true) {

            var query = this.query().condicoesSeguranca();

            if(!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.Pessoa.nome.Contains(valorBusca) || x.idDevice.Equals(valorBusca));
            }

            if(ativo != null) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

    }
    
}