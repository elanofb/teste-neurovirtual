using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Services;
using DAL.AssociadosContribuicoes;

namespace BLL.AssociadosContribuicoes {

    public class AssociadoContribuicaoFilaBoletoGeracaoBL : DefaultBL, IAssociadoContribuicaoFilaBoletoGeracaoBL {

        //Atributos

        //Propriedades

        //Events

        //
        public IQueryable<AssociadoContribuicaoBoletoGeracao> listar(int idContribuicao) {

            var query = from Fil in db.AssociadoContribuicaoBoletoGeracao
                        select Fil;

            if (idContribuicao > 0) {

                query = query.Where(x => x.AssociadoContribuicao.idContribuicao == idContribuicao);

            }

            return query;
        }

        /// <summary>
        /// Salvar itens de uma lista de contribuicoes que precisam ser geradas
        /// </summary>
        public void salvar(List<AssociadoContribuicaoBoletoGeracao> listaBoletosContribuicao) {

            foreach (var OItemFila in listaBoletosContribuicao) {

                OItemFila.setDefaultInsertValues();

            }

            db.Configuration.AutoDetectChangesEnabled = false;

            db.Configuration.ValidateOnSaveEnabled = false;

            db.AssociadoContribuicaoBoletoGeracao.AddRange(listaBoletosContribuicao);

            db.SaveChanges();
        }

        /// <summary>
        /// Salvar itens de uma lista de contribuicoes que precisam ser geradas
        /// </summary>
        public AssociadoContribuicaoBoletoGeracao salvar(AssociadoContribuicaoBoletoGeracao OItemFila) {

            if (OItemFila.id == 0) {

                return this.inserir(OItemFila);

            }

            return this.atualizar(OItemFila);
        }

        /// <summary>
        /// Persistir e inserir um novo registro 
        /// </summary>
        private AssociadoContribuicaoBoletoGeracao inserir(AssociadoContribuicaoBoletoGeracao OItemFila) {

            OItemFila.setDefaultInsertValues();

            db.AssociadoContribuicaoBoletoGeracao.Add(OItemFila);

            db.SaveChanges();

            return OItemFila;
        }

        /// <summary>
        /// Persistir e atualizar um registro existente 
        /// </summary>
        //Atualizar dados da AreaAtuacao
        private AssociadoContribuicaoBoletoGeracao atualizar(AssociadoContribuicaoBoletoGeracao OItemFila) {

            //Localizar existentes no banco
            AssociadoContribuicaoBoletoGeracao dbItem = this.db.AssociadoContribuicaoBoletoGeracao.Find(OItemFila.id);

            //Configurar valores padrão
            dbItem.setDefaultUpdateValues();

            //Atualizacao da AreaAtuacao
            var ItemEntry = db.Entry(dbItem);

            ItemEntry.CurrentValues.SetValues(OItemFila);

            ItemEntry.ignoreFields();

            db.SaveChanges();

            return OItemFila;
        }

    }
}