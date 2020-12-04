using System;
using System.Linq;
using System.Web;
using BLL.Arquivos;
using BLL.Services;
using DAL.Entities;
using DAL.MateriaisApoio;
using System.Data.Entity;
using DAL.Arquivos;
using DAL.Permissao.Security.Extensions;
using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Crypto;

namespace BLL.MateriaisApoio {

    public class MaterialApoioBL : DefaultBL, IMaterialApoioBL {

        //Atributos
        private IArquivoUploadPadraoBL _IArquivoUploadDocumentoBL;

        //Propriedades
        private IArquivoUploadPadraoBL OArquivoUploadDocumentoBL => _IArquivoUploadDocumentoBL = _IArquivoUploadDocumentoBL ?? new ArquivoUploadDocumentoBL();

        //Construtor
        public MaterialApoioBL() {
        }

        //
        public IQueryable<MaterialApoio> query(int? idOrganizacaoParam = null) {

            var query = from Tipo in db.MaterialApoio.Include(x => x.TipoMaterialApoio)
                        where
                            Tipo.flagExcluido == "N"
                        select Tipo;

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;
        }
        
        //Carregamento de registro único pelo ID
        public MaterialApoio carregar(int id) {

            var query = this.query().condicoesSeguranca()
                            .Include(x => x.listaPessoasPermitidas);

            return query.FirstOrDefault(x => x.id == id);
        }

        //Listagem de Registros
        public IQueryable<MaterialApoio> listar(string valorBusca, string ativo) {

            var query = this.query().condicoesSeguranca()
                            .Include(x => x.TipoMaterialApoio).AsNoTracking();

            if(!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca) || x.titulo.Contains(valorBusca));
            }

            if(!String.IsNullOrEmpty(ativo)) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

        //Realizar os tratamentos necessários
        //Salvar um novo registro
        //Salvar o arquivo no banco de dados
        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(MaterialApoio OMaterial, HttpPostedFileBase OArquivo) {

            bool flagSucesso = false;

            if(OMaterial.id > 0) {
                flagSucesso = this.atualizar(OMaterial);
            }

            if(OMaterial.id == 0) {
                flagSucesso = this.inserir(OMaterial);
            }

            if(flagSucesso && OArquivo != null) {

                var OArquivoUpload = new ArquivoUpload();

                OArquivoUpload.idReferenciaEntidade = OMaterial.id;

                OArquivoUpload.entidade = EntityTypes.MATERIAL_APOIO;

                OArquivoUpload.titulo = OMaterial.titulo;

                this.OArquivoUploadDocumentoBL.salvar(OArquivoUpload, OArquivo);

            }

            return flagSucesso;

        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(MaterialApoio OMaterialApoio) {

            OMaterialApoio.idUnidade = User.idUnidade();

            OMaterialApoio.setDefaultInsertValues<MaterialApoio>();

            db.MaterialApoio.Add(OMaterialApoio);

            db.SaveChanges();

            return (OMaterialApoio.id > 0);

        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(MaterialApoio OMaterialApoio) {

            //Localizar existentes no banco
            var dbMaterialApoio = this.carregar(OMaterialApoio.id);

            if (dbMaterialApoio == null) {
                return false;
            }
            
            OMaterialApoio.setDefaultUpdateValues();

            var MaterialEntry = db.Entry(dbMaterialApoio);

            MaterialEntry.CurrentValues.SetValues(OMaterialApoio);

            MaterialEntry.ignoreFields<MaterialApoio>();

            db.SaveChanges();

            return (OMaterialApoio.id > 0);

        }

        //Exclusao logica de registros
        public UtilRetorno excluir(int id) {

            MaterialApoio OMaterialApoio = this.carregar(id);

            if(OMaterialApoio == null) {
                return UtilRetorno.newInstance(true, "O registro não foi localizado.");
            }

            OMaterialApoio.flagExcluido = "S";

            OMaterialApoio.idUsuarioAlteracao = User.id();

            OMaterialApoio.dtAlteracao = DateTime.Now;

            db.SaveChanges();

            return UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");
        }
    }
}