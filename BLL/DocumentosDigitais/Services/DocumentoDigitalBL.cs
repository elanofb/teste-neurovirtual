using DAL.DocumentosDigitais;
using EntityFramework.Extensions;
using System;
using System.Data.Entity;
using System.Json;
using System.Linq;
using BLL.Services;
using UTIL.Resources;

namespace BLL.DocumentosDigitais {

    public class DocumentoDigitalBL : DefaultBL, IDocumentoDigitalBL {

        //
        public IQueryable<DocumentoDigital> query(int? idOrganizacaoParam = null) {

            var query = from E in db.DocumentoDigital
                        where E.flagExcluido == false
                        select E;

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;

        }

        //Carregar um registro pelo ID
        public DocumentoDigital carregar(int id) {
            
            var query = this.query().condicoesSeguranca();

            return query.FirstOrDefault(x => x.id == id);

        }


        //Listagem dos links úteis
        public IQueryable<DocumentoDigital> listar(string valorBusca, int idTipoDocumento, string flagTipoPessoa, bool? ativo) {
            
            var query = this.query().condicoesSeguranca()
                                    .Include(x => x.TipoDocumentoDigital);
            
            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.titulo.Contains(valorBusca));
            }

            if (idTipoDocumento > 0) {
                query = query.Where(x => x.idTipoDocumentoDigital == idTipoDocumento);
            }

            if (!String.IsNullOrEmpty(flagTipoPessoa)) {
                query = query.Where(x => x.flagTipoPessoa.Equals(flagTipoPessoa) || x.flagTipoPessoa.Equals("T"));
            }

            if (ativo.HasValue) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;

        }

        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(DocumentoDigital ODocumentoDigital) {
            
            if (ODocumentoDigital.id == 0) {

                return this.inserir(ODocumentoDigital);
            }

            return this.atualizar(ODocumentoDigital);
            
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(DocumentoDigital ODocumentoDigital) {
            
            ODocumentoDigital.setDefaultInsertValues();

            db.DocumentoDigital.Add(ODocumentoDigital);

            db.SaveChanges();

            return (ODocumentoDigital.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(DocumentoDigital ODocumentoDigital) {
            
            ODocumentoDigital.setDefaultUpdateValues();

            //Localizar existentes no banco
            var dbDocumentoDigital = this.carregar(ODocumentoDigital.id);

            if (dbDocumentoDigital == null) {
                return false;
            }

            var DocumentoDigitalEntry = db.Entry(dbDocumentoDigital);

            DocumentoDigitalEntry.CurrentValues.SetValues(ODocumentoDigital);

            DocumentoDigitalEntry.ignoreFields();

            db.SaveChanges();

            return ODocumentoDigital.id > 0;
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

        //Excluir o registro e os vínculos de imagens
        public bool excluir(int id) {

            var ODocumentoDigital = this.carregar(id);
            if (ODocumentoDigital == null) {
                return false;
            }

            db.DocumentoDigital
                .Where(x => x.id == id)
                .Update(x => new DocumentoDigital { flagExcluido = true, dtAlteracao = DateTime.Now });

            return true;
        }
    }
}