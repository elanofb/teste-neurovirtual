using System;
using System.Data.Entity;
using System.Linq;
using DAL.Associados;

namespace BLL.Associados {
    //preenche um obj do tipo "Tipo associado" com os dados do banco
    public class TipoAssociadoCadastroBL : TipoAssociadoConsultaBL, ITipoAssociadoCadastroBL {

        /*Rotinas de Cadastro*/
        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(TipoAssociado OTipoAssociado) {
            if (OTipoAssociado.id == 0) {
                return this.inserir(OTipoAssociado);
            }

            return this.atualizar(OTipoAssociado);
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(TipoAssociado OTipoAssociado) {
            OTipoAssociado.setDefaultInsertValues<TipoAssociado>();

            db.TipoAssociado.Add(OTipoAssociado);

            db.SaveChanges();

            return (OTipoAssociado.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(TipoAssociado OTipoAssociado) {
            //Localizar existentes no banco
            TipoAssociado dbTipoAssociado = this.carregar(OTipoAssociado.id);

            if (dbTipoAssociado == null) {
                return false;
            }

            OTipoAssociado.setDefaultUpdateValues<TipoAssociado>();

            var TipoEntry = db.Entry(dbTipoAssociado);
            TipoEntry.CurrentValues.SetValues(OTipoAssociado);
            TipoEntry.ignoreFields(new[] {"flagSistema"});

            db.SaveChanges();
            return (OTipoAssociado.id > 0);
        }

        //Verificar se já existe um registro para evitar duplicidades
        private int proximoId() {
            int nroProximoId = db.TipoAssociado.DefaultIfEmpty().Max(x => x.id);

            if (nroProximoId < 100) {
                nroProximoId = 100;
                return nroProximoId;
            }

            nroProximoId = nroProximoId + 1;
            return nroProximoId;
        }

        //Verificar se já existe um registro para evitar duplicidades
        public bool existe(string descricao, int idCategoria, int id, int? idOrganizacaoInf = null) {
            if (idOrganizacao > 0 && idOrganizacaoInf == null) {
                idOrganizacaoInf = idOrganizacao;
            }

            var query = (from T in db.TipoAssociado where T.descricao == descricao && T.id != id && T.flagExcluido == "N" && T.idCategoria == idCategoria select T).AsNoTracking();


            if (idOrganizacaoInf > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoInf);
            }

            if (idOrganizacaoInf == 0) {
                query = query.Where(x => x.idOrganizacao == null);
            }

            var OTipoTitulo = query.Take(1).FirstOrDefault();
            return (OTipoTitulo != null);
        }

        //Verificar se já existe um registro para evitar duplicidades
        public bool ehEstudante(int id) {
            var OTipo = this.carregar(id);

            if (OTipo == null) {
                return false;
            }

            return OTipo.flagEstudante;
        }
    }
}