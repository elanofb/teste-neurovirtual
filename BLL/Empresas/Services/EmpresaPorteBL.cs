using System;
using System.Linq;
using BLL.Services;
using DAL.Empresas;
using System.Json;
using UTIL.Resources;

namespace BLL.Empresas {

    public class EmpresaPorteBL : DefaultBL, IEmpresaPorteBL {


        /// <summary>
        /// Carregar um registro pelo ID
        /// </summary>
        public EmpresaPorte carregar(int id) {

            var query = from Item in db.EmpresaPorte
                        where Item.id == id && Item.flagExcluido == false
                        select Item;

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Listagem de registros de acordo com parametros informados
        /// </summary>
        public IQueryable<EmpresaPorte> listar(int idOrganizacaoParam, string descricao, bool? ativo = true) {

            var query = from Item in db.EmpresaPorte
                        where Item.flagExcluido == false
                        select Item;

            if (idOrganizacaoParam == 0) {

                query = query.condicoesSeguranca();

            } else {

                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);

            }

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
        public bool salvar(EmpresaPorte OEmpresaPorte) {

            if (OEmpresaPorte.id == 0) {

                return this.inserir(OEmpresaPorte);

            }

            return this.atualizar(OEmpresaPorte);
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(EmpresaPorte OEmpresaPorte) {

            OEmpresaPorte.setDefaultInsertValues();

            db.EmpresaPorte.Add(OEmpresaPorte);

            db.SaveChanges();

            return (OEmpresaPorte.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(EmpresaPorte OEmpresaPorte) {

            OEmpresaPorte.setDefaultUpdateValues();

            EmpresaPorte dbTipoProduto = this.carregar(OEmpresaPorte.id);

            var TipoEntry = db.Entry(dbTipoProduto);

            TipoEntry.CurrentValues.SetValues(OEmpresaPorte);

            TipoEntry.ignoreFields();

            db.SaveChanges();

            return (OEmpresaPorte.id > 0);
        }

        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
            var retorno = new JsonMessageStatus();

            var item = this.carregar(id);

            if (item == null) {
                retorno.error = true;
                retorno.message = NotificationMessages.invalid_register_id;
            } else {
                item.ativo = (item.ativo == true ? false : true);
                db.SaveChanges();
                retorno.active = (item.ativo == true ? "S" : "N");
                retorno.message = NotificationMessages.updateSuccess;
            }
            return retorno;
        }

        // Excluir Registro
        public UtilRetorno excluir(int id, int idUsuarioExclusao) {

            var ORegistro = this.carregar(id);

            if (ORegistro == null) {
                return UtilRetorno.newInstance(true, "O registro informado não pôde ser localizado.");
            }

            ORegistro.flagExcluido = true;

            ORegistro.idUsuarioAlteracao = idUsuarioExclusao;

            ORegistro.dtAlteracao = DateTime.Now;

            db.SaveChanges();

            return UtilRetorno.newInstance(false, "Os dados foram atualizados com sucesso.");
        }

    }
}