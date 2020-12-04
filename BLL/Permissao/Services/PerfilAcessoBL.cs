using System;
using System.Json;
using System.Linq;
using DAL.Permissao;
using UTIL.Resources;
using BLL.Services;
using DAL.Permissao.Security.Extensions;

namespace BLL.Permissao {

    public class PerfilAcessoBL : DefaultBL, IPerfilAcessoBL {

        public PerfilAcessoBL() {

        }

        //Carregar registro pelo ID
        public PerfilAcesso carregar(int id) {

            var query = from Perfil in db.PerfilAcesso
                        where
                            Perfil.flagExcluido == "N" &&
                            Perfil.id == id
                        select Perfil;

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();
        }


        //Listar
        public IQueryable<PerfilAcesso> listar(int idOrganizacaoParam, string valorBusca, string ativo) {

            if (idOrganizacao > 0){
                idOrganizacaoParam = idOrganizacao;
            }

            var idAdministradorCliente = PerfilAcessoConst.ADMINISTRADOR;

            var query = from Perfil in db.PerfilAcesso
                        where Perfil.flagExcluido == "N"
                        select Perfil;

            if (User.idPerfil() != PerfilAcessoConst.DESENVOLVEDOR) {
                idOrganizacaoParam = User.idOrganizacao();
            }

            if (idOrganizacao > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam || x.id == idAdministradorCliente);
            }

            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca));
            }

            if (!String.IsNullOrEmpty(ativo)) {
                query = query.Where(x => x.ativo == ativo);
            }

            query = query.condicoesSeguranca();

            return query;
        }

        //Inserir ou atualizar um novo registro
        //Salvar um novo registro
        public bool salvar(PerfilAcesso OPerfilAcesso) {

            if (OPerfilAcesso.id == 0) {
                return this.inserir(OPerfilAcesso);
            }

            return this.atualizar(OPerfilAcesso);
        }

        //Persistir e inserir um novo registro
        private bool inserir(PerfilAcesso OPerfilAcesso) {

            OPerfilAcesso.setDefaultInsertValues();
            this.db.PerfilAcesso.Add(OPerfilAcesso);
            this.db.SaveChanges();
            return OPerfilAcesso.id > 0;
        }

        //Persistir e atualizar um registro existente
        private bool atualizar(PerfilAcesso OPerfilAcesso) {

            //Localizar existentes no banco
            var PerfilAcesso = this.carregar(OPerfilAcesso.id);

            //Configurar valores padrão
            OPerfilAcesso.setDefaultUpdateValues();

            //Atualizacao do Perfil Acesso
            var PerfilEntry = this.db.Entry(PerfilAcesso);
            PerfilEntry.CurrentValues.SetValues(OPerfilAcesso);
            PerfilEntry.ignoreFields();

            this.db.SaveChanges();
            return OPerfilAcesso.id > 0;
        }

        //Alterar status
        public JsonMessageStatus alterarStatus(int id) {
            var retorno = new JsonMessageStatus();

            PerfilAcesso item = this.carregar(id);
            if (item == null) {
                retorno.error = true;
                retorno.message = NotificationMessages.invalid_register_id;
            } else {
                item.ativo = (item.ativo == "S" ? "N" : "S");
                this.db.SaveChanges();
                retorno.active = item.ativo;
                retorno.message = NotificationMessages.updateSuccess;
            }
            return retorno;
        }

        //Excluir registros
        public UtilRetorno excluir(int id, int idUsuarioExclusao) {

            var OPerfilAcesso = this.carregar(id);

            if (OPerfilAcesso == null) {
                return UtilRetorno.newInstance(true, "O perfil informado não foi localizado.");
            }

            OPerfilAcesso.flagExcluido = "S";

            OPerfilAcesso.idUsuarioAlteracao = idUsuarioExclusao;

            db.SaveChanges();

            return UtilRetorno.newInstance(false, "Registro removido com sucesso.");
        }

        //Auto Completar Perfil
        public object getAutoComplete(string term) {

            int idOrganizacaoLogada = User.idOrganizacao();

            var query = from p in db.PerfilAcesso
                        where
                            p.descricao.Contains(term) &&
                            !p.flagExcluido.Equals("S") &&
                            p.ativo.Equals("S") &&
                            p.idOrganizacao == idOrganizacaoLogada
                        select new {
                            value = p.descricao,
                            nome = p.descricao,
                            id = p.id,
                        };

            return query.ToList();
        }
    }
}