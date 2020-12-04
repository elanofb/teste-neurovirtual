using System;
using System.Linq;
using BLL.Services;
using DAL.Telefones;

namespace BLL.Telefones {

    public class TipoTelefoneBL : DefaultBL, ITipoTelefoneBL {


        /// <summary>
        /// Carregar um registro pelo ID
        /// </summary>
        public TipoTelefone carregar(int id) {

            var query = from Item in db.TipoTelefone
                        where Item.id == id && Item.dtExclusao == null
                        select Item;
            TipoTelefone OTipoTelefone = query.FirstOrDefault();
            return OTipoTelefone;
        }

        /// <summary>
        /// Listagem de registros de acordo com parametros informados
        /// </summary>
        public IQueryable<TipoTelefone> listar(string descricao, bool? ativo = true) {

            var query = from Item in db.TipoTelefone
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
        public bool salvar(TipoTelefone OTipoTelefone) {

            if (OTipoTelefone.id == 0) {

                return this.inserir(OTipoTelefone);

            }

            return this.atualizar(OTipoTelefone);
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(TipoTelefone OTipoTelefone) {

            OTipoTelefone.setDefaultInsertValues();

            db.TipoTelefone.Add(OTipoTelefone);

            db.SaveChanges();

            return (OTipoTelefone.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(TipoTelefone OTipoTelefone) {

            OTipoTelefone.setDefaultUpdateValues();

            TipoTelefone dbTipoProduto = this.carregar(OTipoTelefone.id);

            var TipoEntry = db.Entry(dbTipoProduto);

            TipoEntry.CurrentValues.SetValues(OTipoTelefone);

            TipoEntry.ignoreFields();

            db.SaveChanges();

            return (OTipoTelefone.id > 0);
        }


    }
}