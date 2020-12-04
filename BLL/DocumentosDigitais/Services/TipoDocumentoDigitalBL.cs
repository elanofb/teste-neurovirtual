using DAL.DocumentosDigitais;
using DAL.Repository.Base;
using EntityFramework.Extensions;
using System;
using System.Linq;

namespace BLL.DocumentosDigitais {

    public class TipoDocumentoDigitalBL : TableRepository<TipoDocumentoDigital>, ITipoDocumentoDigitalBL {

        //Construtor
        public TipoDocumentoDigitalBL() {
        }

        //Carregar um registro pelo ID
        public TipoDocumentoDigital carregar(int id) {
            var db = this.getDataContext();
            var OTipoDocumentoDigital = db.TipoDocumentoDigital
                            .Where(x => x.id == id && x.flagExcluido == false)
                            .FirstOrDefault();

            return OTipoDocumentoDigital;
        }


        //Listagem dos links úteis
        public IQueryable<TipoDocumentoDigital> listar(string valorBusca, bool? ativo) {
            var db = this.getDataContext();
            var query = from E in db.TipoDocumentoDigital
                        where
                            E.flagExcluido == false
                        select E;

            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca));
            }

            if (ativo.HasValue) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public TipoDocumentoDigital salvar(TipoDocumentoDigital OTipoDocumentoDigital) {

            TipoDocumentoDigital dbTipoDocumentoDigital;

            if (OTipoDocumentoDigital.id == 0) {
                dbTipoDocumentoDigital = this.inserir(OTipoDocumentoDigital);
            }

            dbTipoDocumentoDigital = this.atualizar(OTipoDocumentoDigital);

            return dbTipoDocumentoDigital;
        }

        //Persistir o objecto e salvar na base de dados
        private TipoDocumentoDigital inserir(TipoDocumentoDigital OTipoDocumentoDigital) {
            var db = this.getDataContext();

            OTipoDocumentoDigital.setDefaultInsertValues<TipoDocumentoDigital>();
            db.TipoDocumentoDigital.Add(OTipoDocumentoDigital);
            db.SaveChanges();

            return (OTipoDocumentoDigital);
        }

        //Persistir o objecto e atualizar informações
        private TipoDocumentoDigital atualizar(TipoDocumentoDigital OTipoDocumentoDigital) {
            var db = this.getDataContext();
            OTipoDocumentoDigital.setDefaultUpdateValues<TipoDocumentoDigital>();

            //Localizar existentes no banco
            TipoDocumentoDigital dbTipoDocumentoDigital = this.carregar(OTipoDocumentoDigital.id);
            var TipoDocumentoDigitalEntry = db.Entry(dbTipoDocumentoDigital);
            TipoDocumentoDigitalEntry.CurrentValues.SetValues(OTipoDocumentoDigital);
            TipoDocumentoDigitalEntry.ignoreFields<TipoDocumentoDigital>();

            db.SaveChanges();
            return (OTipoDocumentoDigital);
        }

        //Excluir o registro e os vínculos de imagens
        public bool excluir(int id) {

            var db = this.getDataContext();

            db.TipoDocumentoDigital
                .Where(x => x.id == id)
                .Update(x => new TipoDocumentoDigital { flagExcluido = true, dtAlteracao = DateTime.Now });

            return true;
        }
    }
}