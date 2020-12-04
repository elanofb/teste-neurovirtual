using System;
using System.Linq;
using EntityFramework.Extensions;
using BLL.Services;
using DAL.MateriaisApoio;
using DAL.Permissao.Security.Extensions;

namespace BLL.MateriaisApoio {

    public class TipoMaterialApoioBL : DefaultBL, ITipoMaterialApoioBL {

        //
        public TipoMaterialApoioBL() {
        }

        //
        public IQueryable<TipoMaterialApoio> query(int? idOrganizacaoParam = null) {

            var query = from Obj in db.TipoMaterialApoio
                where Obj.flagExcluido == "N"
                select Obj;

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;

        }

        //Carregamento de registro único pelo ID
        public TipoMaterialApoio carregar(int id) {

            var query = from Tipo in db.TipoMaterialApoio
                        where
                            Tipo.id == id &&
                            Tipo.flagExcluido == "N"
                        select Tipo;

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();
        }


        //
        public IQueryable<TipoMaterialApoio> listar(string valorBusca, string ativo) {

            var query = from Tip in db.TipoMaterialApoio
                        where Tip.flagExcluido == "N"
                        select Tip;

            query = query.condicoesSeguranca();

            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca));
            }

            if (!String.IsNullOrEmpty(ativo)) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

        //Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
        public bool existe(string descricao, int id) {

            var query = from C in db.TipoMaterialApoio
                        where C.descricao == descricao && C.id != id && C.flagExcluido == "N"
                        select C;

            query = query.condicoesSeguranca();

            var OTipoMaterialApoio = query.Take(1).FirstOrDefault();
            return (OTipoMaterialApoio == null ? false : true);
        }

        //
        public bool salvar(TipoMaterialApoio OTipoMaterialApoio) {

            if (OTipoMaterialApoio.id == 0) {
                return this.inserir(OTipoMaterialApoio);
            }
                
           return this.atualizar(OTipoMaterialApoio);
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(TipoMaterialApoio OTipoMaterialApoio) {

            OTipoMaterialApoio.setDefaultInsertValues<TipoMaterialApoio>();
            db.TipoMaterialApoio.Add(OTipoMaterialApoio);
            db.SaveChanges();

            return (OTipoMaterialApoio.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(TipoMaterialApoio OTipoMaterialApoio) {
            OTipoMaterialApoio.setDefaultUpdateValues<TipoMaterialApoio>();

            //Localizar existentes no banco
            TipoMaterialApoio dbTipoMaterialApoio = this.carregar(OTipoMaterialApoio.id);
            var MaterialEntry = db.Entry(dbTipoMaterialApoio);
            MaterialEntry.CurrentValues.SetValues(OTipoMaterialApoio);
            MaterialEntry.ignoreFields<TipoMaterialApoio>();

            db.SaveChanges();
            return (OTipoMaterialApoio.id > 0);
        }

        //Excluir registro logicamente do sistema
        public UtilRetorno excluir(int id) {
            UtilRetorno Retorno = UtilRetorno.getInstance();
            Retorno.flagError = false;

            var idUsuarioLogado = User.id();

            db.TipoMaterialApoio
                        .Where(x => x.id == id)
                        .Update(x => new TipoMaterialApoio { flagExcluido = "S", dtAlteracao = DateTime.Now, idUsuarioAlteracao = idUsuarioLogado });

            return Retorno;
        }
    }
}