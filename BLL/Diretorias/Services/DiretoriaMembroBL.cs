using System;
using System.Json;
using System.Linq;
using BLL.Services;
using UTIL.Resources;
using DAL.Diretorias;
using BLL.Arquivos;
using DAL.Entities;
using DAL.Arquivos;
using System.Collections.Generic;
using System.Web;
using System.Data.Entity;

namespace BLL.Diretorias {

	public class DiretoriaMembroBL : DefaultBL, IDiretoriaMembroBL {

        //Atributos
        private IArquivoUploadFotoBL _ArquivoUploadFotoBL;

        //Propriedades
        private IArquivoUploadFotoBL OArquivoUploadFotoBL => _ArquivoUploadFotoBL = _ArquivoUploadFotoBL ?? new ArquivoUploadFotoBL();

        //
        public DiretoriaMembroBL(){
		}

        //Carregamento de registro único pelo ID
		public DiretoriaMembro carregar(int id) {
			
			var query = from Item in db.DiretoriaMembro
                        where 
							Item.id == id && 
							Item.flagExcluido == false
						select Item;

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();
		}

		//
		public IQueryable<DiretoriaMembro> listar(int idDiretoria, bool? ativo) {

            var query = from C in db.DiretoriaMembro.Include(x => x.Associado).Include(x => x.Cargo).Include(x => x.Diretoria)
                        where C.flagExcluido == false
                        where C.idDiretoria == idDiretoria
                        select C;

            query = query.condicoesSeguranca();

			//if (!String.IsNullOrEmpty(valorBusca)) {
			//	query = query.Where(x => x.descricao.Contains(valorBusca));
			//}

			if (ativo != null) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		//Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
		public bool existe(DiretoriaMembro ODiretoriaMembro, int id) {

			var query = from C in db.DiretoriaMembro
                        where 
                            C.id != id && 
                            C.flagExcluido == false
						select C;

            query = query.condicoesSeguranca();

			return query.Any();
		}

		//Realizar os tratamentos necessários
		//Salvar um novo registro
		public bool salvar(DiretoriaMembro ODiretoriaMembro, HttpPostedFileBase OImagem) {

            var flagSucesso = false;

		    if (ODiretoriaMembro.id > 0) { 
		        flagSucesso = this.atualizar(ODiretoriaMembro);
		    }

            if (ODiretoriaMembro.id == 0) {
                flagSucesso = this.inserir(ODiretoriaMembro);
            }

            if (flagSucesso && OImagem != null) {

                var listaThumbs = new List<ThumbDTO>();

                listaThumbs.Add(new ThumbDTO { folderName = "sistema", height = 100, width = 0 });

                var OArquivo = new ArquivoUpload();

                OArquivo.idReferenciaEntidade = ODiretoriaMembro.id;

                OArquivo.entidade = EntityTypes.DIRETORIA_MEMBRO;
                
                this.OArquivoUploadFotoBL.salvar(OArquivo, OImagem, "", listaThumbs);

            }

            return flagSucesso;
        }

        //Persistir e inserir um novo registro 
        //Inserir Diretoria Membro
        private bool inserir(DiretoriaMembro ODiretoriaMembro) {

            ODiretoriaMembro.setDefaultInsertValues<DiretoriaMembro>();

			db.DiretoriaMembro.Add(ODiretoriaMembro);

			db.SaveChanges();

			return ODiretoriaMembro.id > 0;
		}

        //Persistir e atualizar um registro existente 
        //Atualizar dados da Diretoria Membro
        private bool atualizar(DiretoriaMembro ODiretoriaMembro) {

            //Localizar existentes no banco
            DiretoriaMembro dbDiretoriaMembro = this.carregar(ODiretoriaMembro.id);

            //Configurar valores padrão
            ODiretoriaMembro.setDefaultUpdateValues();

            //Atualizacao da Diretoria Membro
            var DiretoriaMembroEntry = db.Entry(dbDiretoriaMembro);
            DiretoriaMembroEntry.CurrentValues.SetValues(ODiretoriaMembro);
            DiretoriaMembroEntry.ignoreFields();

			db.SaveChanges();

			return ODiretoriaMembro.id > 0;
		}

        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
			var retorno = new JsonMessageStatus();

			var item = this.carregar(id);

			if (item == null) {
				retorno.error = true;
				retorno.message = NotificationMessages.invalid_register_id;
			} else {
				item.ativo = (item.ativo == true ? false : true);
				db.SaveChanges();
				retorno.active = (item.ativo == true ? "S" : "N");
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