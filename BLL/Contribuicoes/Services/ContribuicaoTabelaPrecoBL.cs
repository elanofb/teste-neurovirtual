using System;
using System.Linq;
using BLL.Services;
using DAL.Contribuicoes;
using EntityFramework.Extensions;

namespace BLL.Contribuicoes {

    public class ContribuicaoTabelaPrecoBL : DefaultBL, IContribuicaoTabelaPrecoBL {

        //Carregamento de registro único pelo ID
        public ContribuicaoTabelaPreco carregar(int id) {

            var query = from Tipo in db.ContribuicaoTabelaPreco
                        where
                            Tipo.id == id &&
                            Tipo.flagExcluido == false
                        select Tipo;

            return query.FirstOrDefault();
        }

        //Listagem de Registros
        public IQueryable<ContribuicaoTabelaPreco> listar(int idContribuicao, bool? ativo) {

            var query = from Tipo in db.ContribuicaoTabelaPreco.AsNoTracking()
                        where
                            Tipo.flagExcluido == false
                        select Tipo;

            if (idContribuicao > 0) {
                query = query.Where(x => x.idContribuicao == idContribuicao);
            }

            if (ativo.HasValue) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

        //Realizar os tratamentos necessários
        //Salvar um novo registro
        public bool salvar(ContribuicaoTabelaPreco OContribuicaoTabelaPreco) {

            OContribuicaoTabelaPreco.Contribuicao = null;

            OContribuicaoTabelaPreco.ativo = true;

            if (OContribuicaoTabelaPreco.id == 0) {
                return this.inserir(OContribuicaoTabelaPreco);
            }

            return this.atualizar(OContribuicaoTabelaPreco);

        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(ContribuicaoTabelaPreco OContribuicaoTabelaPreco) {

            OContribuicaoTabelaPreco.setDefaultInsertValues();
            db.ContribuicaoTabelaPreco.Add(OContribuicaoTabelaPreco);
            db.SaveChanges();

            return (OContribuicaoTabelaPreco.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(ContribuicaoTabelaPreco OContribuicaoTabelaPreco) {

            OContribuicaoTabelaPreco.setDefaultUpdateValues();

            //Localizar existentes no banco
            ContribuicaoTabelaPreco dbContribuicaoTabelaPreco = this.carregar(OContribuicaoTabelaPreco.id);
            var ContribuicaoTabelaPrecoEntry = db.Entry(dbContribuicaoTabelaPreco);
            ContribuicaoTabelaPrecoEntry.CurrentValues.SetValues(OContribuicaoTabelaPreco);
            ContribuicaoTabelaPrecoEntry.ignoreFields();

            db.SaveChanges();
            return (OContribuicaoTabelaPreco.id > 0);
        }

        //Excluir registro
        public UtilRetorno excluir(int id, int idUsuario) {

            db.ContribuicaoTabelaPreco
                        .Where(x => x.id == id)
                        .Update(x => new ContribuicaoTabelaPreco { flagExcluido = true, idUsuarioAlteracao = idUsuario, dtAlteracao = DateTime.Now});

            return UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");
        }
    }
}