using System;
using System.Linq;
using BLL.Services;
using DAL.Emails;

namespace BLL.Emails {

    public class TipoEmailBL : DefaultBL, ITipoEmailBL {


        /// <summary>
        /// Carregar um registro pelo ID
        /// </summary>
        public TipoEmail carregar(int id) {

            var query = from Item in db.TipoEmail
                        where Item.id == id && Item.dtExclusao == null
                        select Item;
            TipoEmail OTipoEmail = query.FirstOrDefault();
            return OTipoEmail;
        }

        /// <summary>
        /// Listagem de registros de acordo com parametros informados
        /// </summary>
        public IQueryable<TipoEmail> listar(string descricao, bool? ativo = true) {

            var query = from Item in db.TipoEmail
                        where Item.dtExclusao == null
                        select Item;

            if (!String.IsNullOrEmpty(descricao)) {
                query = query.Where(x => x.descricao.Contains(descricao));
            }

            if (ativo.HasValue) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

        /// <summary>
        /// Persistir e salvar os dados
        /// </summary>
        public bool salvar(TipoEmail OTipoEmail) {

            if (OTipoEmail.id == 0) {

                return this.inserir(OTipoEmail);

            }

            return this.atualizar(OTipoEmail);
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(TipoEmail OTipoEmail) {

            OTipoEmail.setDefaultInsertValues();

            db.TipoEmail.Add(OTipoEmail);

            db.SaveChanges();

            return (OTipoEmail.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(TipoEmail OTipoEmail) {

            OTipoEmail.setDefaultUpdateValues();

            TipoEmail dbTipoProduto = this.carregar(OTipoEmail.id);

            var TipoEntry = db.Entry(dbTipoProduto);

            TipoEntry.CurrentValues.SetValues(OTipoEmail);

            TipoEntry.ignoreFields();

            db.SaveChanges();

            return (OTipoEmail.id > 0);
        }


    }
}