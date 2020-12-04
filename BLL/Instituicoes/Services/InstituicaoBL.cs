using System;
using System.Linq;
using BLL.Services;
using DAL.Instituicoes;
using DAL.Permissao.Security.Extensions;

namespace BLL.Instituicoes {

    public class InstituicaoBL : DefaultBL, IInstituicaoBL {


        //
        public InstituicaoBL() {
        }

        //Carregamento de registro único pelo ID
        public Instituicao carregar(int id) {

            var query = from Inst in db.Instituicao
                        where
                            Inst.id == id &&
                            Inst.flagExcluido == false
                        select Inst;

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();
        }

        // Listagem de Registros
        public IQueryable<Instituicao> listar(int idOrganizacaoParam, string valorBusca, bool? ativo) {

            var query = from T in db.Instituicao
                        where T.flagExcluido == false
                        select T;

            query = query.condicoesSeguranca();

            if (idOrganizacaoParam > 0){
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca) || x.observacao.Contains(valorBusca));
            }

            if (ativo.HasValue) {

                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }


        // Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
        public bool existe(string descricao, int idOrganizacaoParam, int id) {

            var query = from T in db.Instituicao
                        where
                            T.descricao == descricao &&
                            T.idOrganizacao == idOrganizacaoParam &&
                            T.id != id &&
                            T.flagExcluido == false
                        select T;

            query = query.condicoesSeguranca();

            var OInstituicao = query.Take(1).FirstOrDefault();

            return (OInstituicao != null);
        }

        //Realizar os tratamentos necessários
        //Salvar um novo registro
        public bool salvar(Instituicao OInstituicao) {

            OInstituicao.sigla = OInstituicao.sigla.stringOrEmptyUpper().abreviar(20);

            OInstituicao.descricao = OInstituicao.descricao.toUppercaseWords().abreviar(100);

            OInstituicao.observacao = OInstituicao.observacao.stringOrEmpty().abreviar(1000);

            if (OInstituicao.id == 0) {
                return this.inserir(OInstituicao);
            }

            return this.atualizar(OInstituicao);
        }

        //Persistir e inserir um novo registro 
        //Inserir Instituicao
        private bool inserir(Instituicao OInstituicao) {

            OInstituicao.setDefaultInsertValues<Instituicao>();

            db.Instituicao.Add(OInstituicao);

            db.SaveChanges();

            return OInstituicao.id > 0;
        }

        //Persistir e atualizar um registro existente 
        //Atualizar dados da Instituicao
        private bool atualizar(Instituicao OInstituicao) {

            //Localizar existentes no banco
            Instituicao dbInstituicao = this.carregar(OInstituicao.id);

            if (dbInstituicao == null) {
                return false;
            }

            //Configurar valores padrão
            OInstituicao.setDefaultUpdateValues<Instituicao>();

            //Atualizacao da Instituicao
            var InstituicaoEntry = db.Entry(dbInstituicao);
            InstituicaoEntry.CurrentValues.SetValues(OInstituicao);
            InstituicaoEntry.ignoreFields<Instituicao>();

            db.SaveChanges();
            return OInstituicao.id > 0;
        }

        //Alteracao de status
        public UtilRetorno alterarStatus(int id) {

            var OEntidade = this.carregar(id);

            if (OEntidade == null) {
                return UtilRetorno.newInstance(true, "O registro informado não pôde ser localizado.");
            }

            OEntidade.ativo = OEntidade.ativo != true;

            OEntidade.dtAlteracao = DateTime.Now;

            OEntidade.idUsuarioAlteracao = User.id();

            this.db.SaveChanges();

            return UtilRetorno.newInstance(false, "O registro informado foi alterado com sucesso.");
        }

        // Excluir Registro
        public UtilRetorno excluir(int id) {

            var OEntidade = this.carregar(id);

            if (OEntidade == null) {
                return UtilRetorno.newInstance(true, "O registro informado não pôde ser localizado.");
            }

            OEntidade.flagExcluido = true;

            OEntidade.dtAlteracao = DateTime.Now;

            OEntidade.idUsuarioAlteracao = User.id();

            this.db.SaveChanges();

            return UtilRetorno.newInstance(false, "O registro informado foi removido com sucesso.");

        }
    }
}