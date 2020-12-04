using System;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.MeiosDivulgacao;
using UTIL.Resources;

namespace BLL.MeiosDivulgacao {

    public class MeioDivulgacaoBL : DefaultBL, IMeioDivulgacaoBL {

        //Atributos

        //Propriedades

        //
        public MeioDivulgacaoBL() {
        }

        //Carregamento de registro único pelo ID
        public MeioDivulgacao carregar(int id) {

            var query = from Item in db.MeioDivulgacao
                        where
                            Item.id == id &&
                            Item.flagExcluido == false
                        select Item;

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();
        }

        // Listagem de Registros
        public IQueryable<MeioDivulgacao> listar(int idOrganizacaoParam, string valorBusca, bool? ativo) {

            var query = from T in db.MeioDivulgacao
                        where T.flagExcluido == false
                        select T;

            if (idOrganizacaoParam == 0) {

                query = query.condicoesSeguranca();

            } else {

                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca));
            }

            if (ativo.HasValue) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }


        // Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
        public bool existe(MeioDivulgacao OMeioDivulgacao, int id) {

            var query = from T in db.MeioDivulgacao
                        where T.descricao == OMeioDivulgacao.descricao && T.id != id && T.flagExcluido == false
                        select T;

            query = query.condicoesSeguranca();

            var OItem = query.Take(1).FirstOrDefault();

            return (OItem != null);
        }

        //Realizar os tratamentos necessários
        //Salvar um novo registro
        public bool salvar(MeioDivulgacao OMeioDivulgacao) {

            if (OMeioDivulgacao.id == 0) {
                return this.inserir(OMeioDivulgacao);
            }

            return this.atualizar(OMeioDivulgacao);
        }

        //Persistir e inserir um novo registro 
        //Inserir MeioDivulgacao
        private bool inserir(MeioDivulgacao OMeioDivulgacao) {

            OMeioDivulgacao.setDefaultInsertValues<MeioDivulgacao>();

            OMeioDivulgacao.flagSistema = false;

            db.MeioDivulgacao.Add(OMeioDivulgacao);

            db.SaveChanges();

            return OMeioDivulgacao.id > 0;
        }

        //Persistir e atualizar um registro existente 
        //Atualizar dados da MeioDivulgacao
        private bool atualizar(MeioDivulgacao OMeioDivulgacao) {

            //Localizar existentes no banco
            MeioDivulgacao dbMeioDivulgacao = this.carregar(OMeioDivulgacao.id);

            if (dbMeioDivulgacao == null) {
                return false;
            }

            //Configurar valores padrão
            OMeioDivulgacao.setDefaultUpdateValues();

            //Atualizacao da MeioDivulgacao
            var MeioDivulgacaoEntry = db.Entry(dbMeioDivulgacao);
            MeioDivulgacaoEntry.CurrentValues.SetValues(OMeioDivulgacao);
            MeioDivulgacaoEntry.ignoreFields();

            db.SaveChanges();

            return OMeioDivulgacao.id > 0;
        }

        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
            var retorno = new JsonMessageStatus();

            var item = this.carregar(id);

            if (item == null) {
                retorno.error = true;
                retorno.message = NotificationMessages.invalid_register_id;
            } else {
                item.ativo = (item.ativo != true);
                db.SaveChanges();
                retorno.active = item.ativo == true ? "S" : "N";
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