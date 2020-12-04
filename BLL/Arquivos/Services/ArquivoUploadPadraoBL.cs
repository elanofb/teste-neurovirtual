using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Web;
using BLL.Services;
using DAL.Arquivos;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;
using ImageResizer;
using UTIL.Upload;

namespace BLL.Arquivos {

	public class ArquivoUploadPadraoBL : DefaultBL, IArquivoUploadPadraoBL {
        
        // Contants
	    private string tipoCategoria { get; }

		//Construtor
	    protected ArquivoUploadPadraoBL(string tipoCategoria) {
            this.tipoCategoria = tipoCategoria;
		}
        
        //
        public IQueryable<ArquivoUpload> query(bool flagFiltroCategoria = true) {
			
            var query = from Arq in db.ArquivoUpload
                        where !Arq.dtExclusao.HasValue                        
                        select Arq;
			
	        if (flagFiltroCategoria ){
		        query = query.Where(x => x.categoria.Equals(tipoCategoria));
	        }
			
            return query;

        }

	    //Carregar registro único
	    public ArquivoUpload carregar(int id) {
            
            var query = this.query().condicoesSeguranca();
            
	        return query.FirstOrDefault(x => x.id == id);

	    }
        
	    //Listagem de registro de acordo com parametros informados
	    public IQueryable<ArquivoUpload> listar(int idReferencia, string entidade, string ativo, int? idOrganizacaoParam = null) {
            
	        var query = this.query().Where(x => !String.IsNullOrEmpty(x.path));

	        if(idReferencia > 0) {
	            query = query.Where(x => x.idReferenciaEntidade == idReferencia);
	        }
            
	        if (!entidade.isEmpty()) {
	            query = query.Where(x => x.entidade == entidade);
	        }
            
	        if (!ativo.isEmpty()) {
	            query = query.Where(x => x.ativo == ativo);
	        }

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

	        return query;

	    }
        
		//Gerar um registro na tabela no banco de dados
		//Gravar o arquivo em disco
		public virtual bool salvar(ArquivoUpload OArquivo, HttpPostedFileBase FileUpload = null, string pathUpload = "", List<ThumbDTO> listaThumb = null) {
            
            if (OArquivo.id > 0) {
                return this.atualizar(OArquivo);
            }
            
		    if (OArquivo.id == 0 && FileUpload == null) {
		        return false;
		    }

            OArquivo.categoria = this.tipoCategoria;
                
            var flagSucesso = this.inserir(OArquivo);

            if (flagSucesso && FileUpload != null) {

                OArquivo.extensao = UtilIO.getExtension(FileUpload.FileName);

                OArquivo.contentType = FileUpload.ContentType;

                this.upload(ref OArquivo, FileUpload, pathUpload, listaThumb);   

            }
            
			return flagSucesso;

		}

	    //
	    private bool inserir(ArquivoUpload OArquivoUpload)
	    {
		    
	        OArquivoUpload.setDefaultInsertValues();

	        db.ArquivoUpload.Add(OArquivoUpload);

	        db.SaveChanges();

	        var flagSucesso = OArquivoUpload.id > 0;
            
	        return flagSucesso;
	    }

	    //
	    private bool atualizar(ArquivoUpload OArquivoUpload) {

	        OArquivoUpload.setDefaultUpdateValues();
            
	        var dbArquivoUpload = this.carregar(OArquivoUpload.id);

	        if (dbArquivoUpload == null) {
	            return false;
	        }

	        var dbEntry = db.Entry(dbArquivoUpload);

	        dbEntry.CurrentValues.SetValues(OArquivoUpload);

	        dbEntry.ignoreFields();

	        db.SaveChanges();

	        return (OArquivoUpload.id > 0);
	    }
        
		//Salvar o arquivo em disco e configurar os caminhos para buscar posteriormente
		public bool upload(ref ArquivoUpload OArquivo, HttpPostedFileBase FileUpload, string pathUpload = "", List<ThumbDTO> listaThumb = null) {

			string pathBaseAbs = String.IsNullOrEmpty(pathUpload) ? UtilConfig.pathAbsUpload(OArquivo.idOrganizacao.toInt()) : pathUpload;

			string pathPasta = String.Concat(OArquivo.entidade, "/", OArquivo.categoria, "/", OArquivo.idReferenciaEntidade, "/");

			string pathPastaThumb = String.Concat(pathPasta, "thumb/");

			string nomeArquivo = String.Concat(OArquivo.id.ToString(), OArquivo.extensao);

			OArquivo.path = String.Concat(pathPasta, nomeArquivo);

			OArquivo.pathThumb = pathPastaThumb;

            OArquivo.nomeArquivo = nomeArquivo;

			//Criar os diretórios necessários.
			UtilIO.createFolder(Path.Combine(pathBaseAbs, pathPasta));
			UtilIO.createFolder(Path.Combine(pathBaseAbs, pathPastaThumb));

			//Salvar o arquivo principal
			FileUpload.SaveAs(String.Concat(pathBaseAbs, OArquivo.path));

			//Caso seja uma imagem, fazer o redimensionamento para o tamanho padrão do sistema.
			if (UploadConfig.validarImagem(FileUpload)) {

                Instructions InstructionsImage = new Instructions { Format = OArquivo.extensao.Replace(".", ""), Mode = FitMode.Max };

                //Tamanho padrão para apresentação no grid do sistema
                InstructionsImage.Height = 100;

                string diretorioThumbSistema = String.Concat(pathPastaThumb, "sistema");

                string diretorioArquivoThumbSistema = String.Concat(diretorioThumbSistema, "/", nomeArquivo);

                UtilIO.createFolder(Path.Combine(pathBaseAbs, diretorioThumbSistema));

			    FileUpload.InputStream.Seek(0, SeekOrigin.Begin);
				ImageBuilder.Current.Build(new ImageJob(FileUpload, Path.Combine(pathBaseAbs, diretorioArquivoThumbSistema), InstructionsImage));

                if (listaThumb == null || listaThumb.Count == 0) { 
					listaThumb = new List<ThumbDTO>();
					listaThumb.Add( new ThumbDTO{ folderName = "sistema", height=50, width = 0});
                }

                foreach (var Item in listaThumb) {
                    InstructionsImage.Width = Item.width;
                    InstructionsImage.Height = Item.height;

                    string diretorioThumb = String.Concat(pathPastaThumb, Item.folderName);
                    string diretorioArquivoThumb = String.Concat(diretorioThumb, "/", nomeArquivo);

                    UtilIO.createFolder(Path.Combine(pathBaseAbs, diretorioThumb));

				    ImageBuilder.Current.Build(new ImageJob(FileUpload, Path.Combine(pathBaseAbs, diretorioArquivoThumb), InstructionsImage));
                }
			}

			if (!File.Exists(String.Concat(pathBaseAbs, OArquivo.path))) {

				this.excluir(OArquivo.id);

				return false;

			}

			this.salvar(OArquivo);

			return true;
		}

		//Registro o log de alteração de informações de uma solicitação
		//Realizar as atualizações solicitadas
		public void atualizarDados(int idArquivo, string nomeCampo, string novoValor) {

			if (nomeCampo.Equals("legenda")) {

				this.db.ArquivoUpload.Where(x => x.id == idArquivo)
					.Update(x => new ArquivoUpload { legenda = novoValor });

			} else if (nomeCampo.Equals("titulo")) {

				this.db.ArquivoUpload.Where(x => x.id == idArquivo)
					.Update(x => new ArquivoUpload { titulo = novoValor });

			} else if (nomeCampo.Equals("flagPrincipal")) {

				this.db.ArquivoUpload.Where(x => x.id == idArquivo)
					.Update(x => new ArquivoUpload { flagPrincipal = novoValor });
			}

		}
		
	    /// <summary>
	    /// Alteracao de status
	    /// </summary>
	    public JsonMessageStatus alterarStatus(int id) {

	        var ORetorno = new JsonMessageStatus();

	        var Objeto = this.carregar(id);

	        if (Objeto == null) {

	            ORetorno.error = true;

	            ORetorno.message = "O registro informado não foi encontrado.";

	            return ORetorno;
	        }

	        Objeto.ativo = Objeto.ativo == "S" ? "N" : "S";

	        db.SaveChanges();

	        ORetorno.active = Objeto.ativo;

	        ORetorno.message = "Os dados foram alterados com sucesso.";

	        return ORetorno;

	    }

		//Excluir um registro através dos parâmetros informados
		public UtilRetorno excluir(int idReferencia, string entidade) {

			var Retorno = UtilRetorno.newInstance(false);

			var listaArquivos = this.listar(idReferencia, entidade, "").ToList();

			foreach(ArquivoUpload Item in listaArquivos){

				var ORetorno = this.excluir(Item.id);

				if (ORetorno.error) { 

					Retorno.flagError = true;

					Retorno.listaErros.Add($"O registro { Item.id } não pôde ser removido.");

				}

			}

			return Retorno;
		}
		
		//Excluir através do ID do registro
		public virtual JsonMessageStatus excluir(int id) {
			
		    var ORetorno = new JsonMessageStatus();
			
		    var Objeto = this.carregar(id);
			
			if (Objeto == null) { 

			    ORetorno.error = true;

			    ORetorno.message = "O registro informado não foi encontrado.";

			    return ORetorno;

			}
			
			string basePath = Objeto.idOrganizacao > 0 ? UtilConfig.pathAbsUpload(Objeto.idOrganizacao.toInt()) : UtilConfig.pathAbsUploadFiles;
			
			string pathFile = Path.Combine(basePath, UtilString.notNull(Objeto.path));
			
            if(File.Exists(pathFile)) {

                File.Delete(pathFile);
            }
			
			string folderThumb = Path.Combine(basePath, UtilString.notNull(Objeto.pathThumb));

            if (Directory.Exists(folderThumb)) {
	            
                string [] listaDiretorios = Directory.GetDirectories(folderThumb);
	            
                foreach (string nomeDiretorio in listaDiretorios) {
	                
                    string diretorioArquivoThumb = Path.Combine(nomeDiretorio, UtilString.notNull(Objeto.nomeArquivo));
	                
                    if(File.Exists(diretorioArquivoThumb)) {
	                    
                        File.Delete(diretorioArquivoThumb);
	                    
                    }

                }

            }
			
		    Objeto.dtExclusao = DateTime.Now;
			
		    Objeto.idUsuarioExclusao = HttpContextFactory.Current.User.id();
			
            db.SaveChanges();
			
			//db.ArquivoUpload.Where(x => x.id == id).Delete();
			
		    ORetorno.error = false;
			
		    ORetorno.message = "O arquivo foi removido com sucesso.";
			
            return ORetorno;
			
		}

	}

}