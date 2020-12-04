using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Services;
using DAL.AssociadosContribuicoes;

namespace BLL.AssociadosContribuicoes {

    public class AssociadoContribuicaoFilaGeracaoBL : DefaultBL, IAssociadoContribuicaoFilaGeracaoBL {

        //Atributos

        //Propriedades

        //Events

        //
        public IQueryable<AssociadoContribuicaoFilaGeracao> listar(int idContribuicao) {

            var query = from Fil in db.AssociadoContribuicaoFilaGeracao
                        select Fil;

            if (idContribuicao > 0) {

                query = query.Where(x => x.idContribuicao == idContribuicao);

            }

            return query;
        } 

        /// <summary>
        /// Salvar itens de uma lista de contribuicoes que precisam ser geradas
        /// </summary>
        public void salvar(List<AssociadoContribuicaoFilaGeracao> listaFilaContribuicao) {

            foreach (var OItemFila in listaFilaContribuicao) {

                OItemFila.setDefaultInsertValues();

            }

            db.Configuration.AutoDetectChangesEnabled = false;

            db.Configuration.ValidateOnSaveEnabled = false;

            db.AssociadoContribuicaoFilaGeracao.AddRange(listaFilaContribuicao);

            db.SaveChanges();
        }

        /// <summary>
        /// Salvar itens de uma lista de contribuicoes que precisam ser geradas
        /// </summary>
        public AssociadoContribuicaoFilaGeracao salvar(AssociadoContribuicaoFilaGeracao OItemFila) {

            if (OItemFila.id == 0) {

                return this.inserir(OItemFila);

            }

            return this.atualizar(OItemFila);
        }

        /// <summary>
        /// Persistir e inserir um novo registro 
        /// </summary>
        private AssociadoContribuicaoFilaGeracao inserir(AssociadoContribuicaoFilaGeracao OItemFila) { 

			OItemFila.setDefaultInsertValues();

			db.AssociadoContribuicaoFilaGeracao.Add(OItemFila);

			db.SaveChanges();

			return OItemFila;
		}

        /// <summary>
        /// Persistir e atualizar um registro existente 
        /// </summary>
		//Atualizar dados da AreaAtuacao
		private AssociadoContribuicaoFilaGeracao atualizar(AssociadoContribuicaoFilaGeracao OItemFila) { 

			//Localizar existentes no banco
			AssociadoContribuicaoFilaGeracao dbItem = this.db.AssociadoContribuicaoFilaGeracao.Find(OItemFila.id);

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