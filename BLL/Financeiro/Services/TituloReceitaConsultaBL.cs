using System;
using System.Linq;
using BLL.Services;
using DAL.Financeiro;

namespace BLL.Financeiro {

    public class TituloReceitaConsultaBL : DefaultBL, ITituloReceitaConsultaBL {

        //Atributos

        //Propriedades

        // Carregar um titulo a partir do seu ID
        public TituloReceita carregar(int id, bool? flagExcluido = false) {

            var query = from Tit in db.TituloReceita
                        where
                            Tit.id == id
                        select
                            Tit;

            if (flagExcluido == false) {
                query = query.Where(x => x.dtExclusao == null);
            }

            if (flagExcluido == true) {
                query = query.Where(x => x.dtExclusao.HasValue);
            }

            return query.condicoesSeguranca().FirstOrDefault();
        }

        // Carregar um titulo a partir do seu ID
        public IQueryable<TituloReceita> query(int? idOrganizacaoParam = null, bool? flagExcluido = false) {

            var query = from Tit in db.TituloReceita
                        select
                            Tit;
            
            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            if (flagExcluido == false) {
                query = query.Where(x => x.dtExclusao == null);
            }

            if (flagExcluido == true) {
                query = query.Where(x => x.dtExclusao.HasValue);
            }

            return query;
        }

    }
}