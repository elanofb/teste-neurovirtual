using System;
using System.Linq;
using DAL.Entities;
using DAL.Publicacoes;
using BLL.Services;
using BLL.Arquivos;
using System.Web;
using DAL.Arquivos;
using System.Collections.Generic;
using System.Data.Entity;
using System.Json;
using DAL.Permissao.Security.Extensions;
using UTIL.Resources;

namespace BLL.Publicacoes {

	public class ParceiroBL : DefaultBL, IParceiroBL {
		
        //Atributos
		private IArquivoUploadPadraoBL _IArquivoUploadPadraoBL;

		//Propriedades
		private IArquivoUploadPadraoBL OArquivoUploadPadraoBL => _IArquivoUploadPadraoBL = _IArquivoUploadPadraoBL ?? new ArquivoUploadLogotipoBL();

	    //Construtor
		public ParceiroBL(){
		}

        //
	    public IQueryable<Parceiro> query(int? idOrganizacaoParam = null) {

	        var query = from Parc in db.Parceiro
	                    where Parc.flagExcluido == "N"
	                    select Parc;

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;

        }

		//Carregamento de registro único pelo ID
		public Parceiro carregar(int id) {

			var query = this.query().condicoesSeguranca();
            
			return query.FirstOrDefault(x => x.id == id);

		}

		//Listagem de Registros
		public IQueryable<Parceiro> listar(string valorBusca, string ativo, int idTipoParceiro, int? idPortal = 0) {
			
			var query = this.query().condicoesSeguranca().AsNoTracking();
                
            if (idPortal > 0) {
                query = query.Where(x => x.idPortal == idPortal);
            }

			if (!String.IsNullOrEmpty(valorBusca)) { 
				query = query.Where(x => x.nome.Contains(valorBusca) );
			}

			if (!String.IsNullOrEmpty(ativo)) { 
				query = query.Where(x => x.ativo == ativo);
			}

            if (idTipoParceiro > 0) {
                query = query.Where(x => x.idTipoParceiro == idTipoParceiro);
            }

			return query;
		}

		//Realizar os tratamentos necessários
		//Salvar um novo registro
		//Salvar o logotipo do parceiro no banco de dados
		//Verificar se deve-se atualizar um registro existente ou criar um novo
		public bool salvar(Parceiro OParceiro, HttpPostedFileBase OFoto) {

            bool flagSucesso = false;
			
		    if (OParceiro.id > 0) { 
		        flagSucesso = this.atualizar(OParceiro);
		    }

			if (OParceiro.id == 0) { 
				flagSucesso = this.inserir(OParceiro);
			}
            
            if (flagSucesso && OFoto != null) { 

                var OArquivo = new ArquivoUpload();

                OArquivo.idReferenciaEntidade = OParceiro.id;

                OArquivo.entidade = EntityTypes.PARCEIRO;

                var listathumbs = new List<ThumbDTO> {
                    new ThumbDTO { folderName = "destaque", width = 250, height = 167 }
                };

				this.OArquivoUploadPadraoBL.salvar(OArquivo, OFoto, "", listathumbs);

			}

			return flagSucesso;
		}

		//Persistir o objecto e salvar na base de dados
		private bool inserir(Parceiro OParceiro) { 

			OParceiro.setDefaultInsertValues<Parceiro>();
			db.Parceiro.Add(OParceiro);
			db.SaveChanges();

			return (OParceiro.id > 0);
		}

		//Persistir o objecto e atualizar informações
		private bool atualizar(Parceiro OParceiro) { 
			OParceiro.setDefaultUpdateValues<Parceiro>();

			//Localizar existentes no banco
			Parceiro dbParceiro = this.carregar(OParceiro.id);		

            if (dbParceiro == null) {
                return false;
            }

			var ParceiroEntry = db.Entry(dbParceiro);
			ParceiroEntry.CurrentValues.SetValues(OParceiro);
			ParceiroEntry.ignoreFields<Parceiro>();

			db.SaveChanges();
			return (OParceiro.id > 0);
		}

		// Verificar se já existe um registro com o mesmo nome, no entanto, que possua id diferente do informado
		public bool existe(string nome, int id) {

			var query = from Parc in db.Parceiro
						where 
							Parc.nome == nome && 
							Parc.flagExcluido == "N" && 
							Parc.id != id 
						select Parc;

            query = query.condicoesSeguranca();

			var OItem = query.Take(1).FirstOrDefault();
			return (OItem != null);
		}

        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
	        var retorno = new JsonMessageStatus();

	        var item = this.carregar(id);

	        if (item == null) {
		        retorno.error = true;
		        retorno.message = NotificationMessages.invalid_register_id;
	        } else {
		        item.ativo = (item.ativo == "S" ? "N" : "S");
		        db.SaveChanges();
		        retorno.active = item.ativo;
		        retorno.message = NotificationMessages.updateSuccess;
	        }
	        return retorno;
        }

		//Exclusao logica de registros
		public UtilRetorno excluir(int id) {

			Parceiro OParceiro = this.carregar(id);

			if (OParceiro == null) {
				return UtilRetorno.newInstance(true, "O registro não foi localizado.");
			}

			OParceiro.flagExcluido = "S";
			OParceiro.idUsuarioAlteracao = User.id();
			OParceiro.dtAlteracao = DateTime.Now;
			db.SaveChanges();

			return UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");
		}
	}
}