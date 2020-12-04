using System;
using System.Linq;
using BLL.Services;
using DAL.Telefones;

namespace BLL.Telefones {

    public class OperadoraTelefoniaBL : DefaultBL, IOperadoraTelefoniaBL {


        /// <summary>
        /// Carregar um registro pelo ID
        /// </summary>
        public OperadoraTelefonia carregar(int id) {

            var query = from Item in db.OperadoraTelefonia
                        where Item.id == id && Item.dtExclusao == null
                        select Item;
            OperadoraTelefonia OOperadoraTelefonia = query.FirstOrDefault();
            return OOperadoraTelefonia;
        }

        /// <summary>
        /// Listagem de registros de acordo com parametros informados
        /// </summary>
        public IQueryable<OperadoraTelefonia> listar(string descricao, bool? ativo = true) {

            var query = from Item in db.OperadoraTelefonia
                        where Item.dtExclusao == null
                        select Item;

            if (!String.IsNullOrEmpty(descricao)) {
                query = query.Where(x => x.nome.Contains(descricao));
            }

            if (ativo.HasValue) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

        /// <summary>
        /// Persistir e salvar os dados
        /// </summary>
        public bool salvar(OperadoraTelefonia OOperadoraTelefonia) {

            if (OOperadoraTelefonia.id == 0) {

                return this.inserir(OOperadoraTelefonia);

            }

            return this.atualizar(OOperadoraTelefonia);
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(OperadoraTelefonia OOperadoraTelefonia) {

            OOperadoraTelefonia.setDefaultInsertValues();

            db.OperadoraTelefonia.Add(OOperadoraTelefonia);

            db.SaveChanges();

            return (OOperadoraTelefonia.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(OperadoraTelefonia OOperadoraTelefonia) {

            OOperadoraTelefonia.setDefaultUpdateValues();

            OperadoraTelefonia dbTipoProduto = this.carregar(OOperadoraTelefonia.id);

            var TipoEntry = db.Entry(dbTipoProduto);

            TipoEntry.CurrentValues.SetValues(OOperadoraTelefonia);

            TipoEntry.ignoreFields();

            db.SaveChanges();

            return (OOperadoraTelefonia.id > 0);
        }


    }
}