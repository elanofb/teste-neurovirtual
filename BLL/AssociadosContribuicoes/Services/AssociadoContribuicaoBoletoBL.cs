using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Services;
using DAL.AssociadosContribuicoes;

namespace BLL.AssociadosContribuicoes {

    public class AssociadoContribuicaoBoletoBL : DefaultBL, IAssociadoContribuicaoBoletoBL {

        //Atributos

        //Propriedades

        //Events

        //
        public IQueryable<AssociadoContribuicaoBoleto> listar(int idContribuicao) {

            var query = from Fil in db.AssociadoContribuicaoBoleto.AsNoTracking()
                        select Fil;

            if (idContribuicao > 0) {

                query = query.Where(x => x.idContribuicao == idContribuicao);

            }

            query = query.condicoesSeguranca();

            return query;
        }

        /// <summary>
        /// Salvar itens de uma lista de contribuicoes que precisam ser geradas
        /// </summary>
        public void salvar(List<AssociadoContribuicaoBoleto> listaFilaContribuicao) {

            foreach (var OItemFila in listaFilaContribuicao) {

                OItemFila.setDefaultInsertValues();

            }

            db.Configuration.AutoDetectChangesEnabled = false;

            db.Configuration.ValidateOnSaveEnabled = false;

            db.AssociadoContribuicaoBoleto.AddRange(listaFilaContribuicao);

            db.SaveChanges();
        }



    }
}