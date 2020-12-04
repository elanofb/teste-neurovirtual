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

	public class ArquivoUploadBL : DefaultBL, IArquivoUploadBL {

		//Construtor
		public ArquivoUploadBL(){
		}

		//Carregar registro único
		public ArquivoUpload carregar(int id) {

            var query = from Arq in db.ArquivoUpload
						where
							Arq.id == id &&
							!Arq.dtExclusao.HasValue &&
                            Arq.ativo == "S"
						orderby Arq.id descending
						select Arq;

			return query.FirstOrDefault();
		}

		//Carregar registro único
		public ArquivoUpload carregar(int idReferencia, string categoria, string entidade) {

            var query = from Arq in db.ArquivoUpload
						where
							Arq.idReferenciaEntidade == idReferencia && Arq.categoria == categoria && Arq.entidade == entidade &&
							!Arq.dtExclusao.HasValue && Arq.ativo == "S"
						orderby Arq.id descending
						select Arq;

			return query.FirstOrDefault();
		}
        
		//Listagem de registro de acordo com parametros informados
		public IQueryable<ArquivoUpload> listar(int idReferencia, string entidade, string categoria, string ativo) {

			var query = from Arq in db.ArquivoUpload
						where 
						    !Arq.dtExclusao.HasValue && 
                            !string.IsNullOrEmpty(Arq.path)
						select Arq;

            if(idReferencia > 0) {
                query = query.Where(x => x.idReferenciaEntidade == idReferencia);
            }
            
			if (!String.IsNullOrEmpty(entidade)) {
				query = query.Where(x => x.entidade == entidade);
			}

			if (!String.IsNullOrEmpty(categoria)) {
				query = query.Where(x => x.categoria == categoria);
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}
			return query;
		}
        
		//Listagem de documentos
		public IQueryable<ArquivoUpload> listarDocumentos(int idReferencia, string entidade) {

			string categoriaDocumento = ArquivoUploadTypes.DOCUMENTO;

            return this.listar(idReferencia, entidade, "", "").Where(x => x.categoria == categoriaDocumento);
		}

        //Listagem de documentos
		public IQueryable<ArquivoUpload> listarAudios(int idReferencia, string entidade) {
			string categoriaAudio = ArquivoUploadTypes.AUDIO;
			return this.listar(idReferencia, entidade, "", "").Where(x => x.categoria == categoriaAudio);
		}

		//Gerar um registro na tabela no banco de dados
		//Gravar o arquivo em disco
		public string salvar(ArquivoUpload OArquivo, HttpPostedFileBase FileUpload, string pathUpload = "") {
			string pathArquivo = "";

			if (FileUpload == null || FileUpload.ContentLength == 0) {
				return pathArquivo;
			}

			OArquivo.extensao = UtilIO.getExtension(FileUpload.FileName);

            OArquivo.contentType = FileUpload.ContentType;

		    OArquivo.setDefaultInsertValues();

		    db.ArquivoUpload.Add(OArquivo);

		    db.SaveChanges();

			if (OArquivo.id > 0) {

				this.upload(ref OArquivo, FileUpload, pathUpload, null);

                pathArquivo = OArquivo.path;
			}

			return pathArquivo;
		}

		//Método Genérico para salvar logotipos
		public bool salvarLogotipo(int idReferencia, string entidade, HttpPostedFileBase Logotipo, List<ThumbDTO> listaThumb = null) {

            var OArquivo = new ArquivoUpload();

            OArquivo.categoria = ArquivoUploadTypes.LOGOTIPO;

            OArquivo.entidade = entidade;

            OArquivo.idReferenciaEntidade = idReferencia;

            OArquivo.extensao = UtilIO.getExtension(Logotipo.FileName);

            OArquivo.contentType = Logotipo.ContentType;

            OArquivo.dtExclusao = DateTime.Now;

            OArquivo.ativo = "S";

		    OArquivo.setDefaultInsertValues();

		    db.ArquivoUpload.Add(OArquivo);

		    db.SaveChanges();

            bool flagUpload = this.upload(ref OArquivo, Logotipo, "", listaThumb);

            return flagUpload;
		}

		//Método Genérico para salvar Documentos
		public bool salvarDocumento(int idReferencia, string entidade, string descricao, HttpPostedFileBase Documento, int idOrganizacaoParam, int idusuarioCadastroParam = 0) {

			if (idOrganizacaoParam == 0)
			{
				idOrganizacaoParam = User.idOrganizacao();
			}

			var OArquivo = new ArquivoUpload();

            OArquivo.categoria = ArquivoUploadTypes.DOCUMENTO;

            OArquivo.entidade = entidade;

            OArquivo.titulo = descricao;

            OArquivo.legenda = descricao;

            OArquivo.idReferenciaEntidade = idReferencia;

            OArquivo.extensao = UtilIO.getExtension(Documento.FileName);

            OArquivo.contentType = Documento.ContentType;

            OArquivo.ativo = "S";

			OArquivo.idOrganizacao = idOrganizacaoParam;
			
			OArquivo.idUsuarioCadastro = idusuarioCadastroParam;

            OArquivo.dtCadastro = DateTime.Now;;

		    db.ArquivoUpload.Add(OArquivo);

		    db.SaveChanges();

            bool flagUpload = this.upload(ref OArquivo, Documento);

            return flagUpload;
		}

        //Método Genérico para salvar Audio
		public bool salvarAudio(int idReferencia, string entidade, string descricao, HttpPostedFileBase Audio) {

            var OArquivo = new ArquivoUpload();

            OArquivo.categoria = ArquivoUploadTypes.AUDIO;

            OArquivo.entidade = entidade;

            OArquivo.titulo = descricao;

            OArquivo.legenda = descricao;

            OArquivo.idReferenciaEntidade = idReferencia;

            OArquivo.extensao = UtilIO.getExtension(Audio.FileName);

            OArquivo.contentType = Audio.ContentType;

            OArquivo.dtExclusao = DateTime.Now;

            OArquivo.ativo = "S";

		    OArquivo.setDefaultInsertValues();

		    db.ArquivoUpload.Add(OArquivo);

		    db.SaveChanges();

            bool flagUpload = this.upload(ref OArquivo, Audio);

            return flagUpload;
		}

		//Salvar o arquivo em disco e configurar os caminhos para buscar posteriormente
		public bool upload(ref ArquivoUpload OArquivo, HttpPostedFileBase FileUpload, string pathUpload = "", List<ThumbDTO> listaThumb = null) {

			string pathBaseAbs = String.IsNullOrEmpty(pathUpload) ? UtilConfig.pathAbsUpload(OArquivo.idOrganizacao.toInt()) : pathUpload;

			string pathPasta = String.Concat(OArquivo.entidade, "/", OArquivo.categoria, "/", OArquivo.idReferenciaEntidade, "/");

            string pathPastaThumb = String.Concat(pathPasta, "thumb/");

            string nomeArquivo = FileUpload.FileName ?? String.Concat(OArquivo.id.ToString(), OArquivo.extensao);

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

			if (!File.Exists(String.Concat(pathBaseAbs, OArquivo.path))){

			    int idArquivo = OArquivo.id;

			    this.db.ArquivoUpload.Where(x => x.id == idArquivo).Delete();

                return false;
			}

			this.db.SaveChanges();

			return true;
		}

		//Registro o log de alteração de informações de uma solicitação
		//Realizar as atualizações solicitadas
		public void atualizarDados(int idArquivo, string nomeCampo, string novoValor) {

			if (nomeCampo.Equals("legenda")) {

                this.db.ArquivoUpload
					.Where(x => x.id == idArquivo)
					.Update(x => new ArquivoUpload { legenda = novoValor });

			} else if (nomeCampo.Equals("titulo")) {

                this.db
					.ArquivoUpload.Where(x => x.id == idArquivo)
					.Update(x => new ArquivoUpload { titulo = novoValor });

			} else if (nomeCampo.Equals("flagPrincipal")) {

				this.db
					.ArquivoUpload.Where(x => x.id == idArquivo)
					.Update(x => new ArquivoUpload { flagPrincipal = novoValor });
			}
		}
		
		//Alteracao de ativo para inativo e vice-versa
		public ArquivoUpload alterarStatus(int id) {

			var Retorno = UtilRetorno.getInstance();

            Retorno.flagError = false;

			ArquivoUpload OArquivo = db.ArquivoUpload.FirstOrDefault(x => x.id == id);

			if (OArquivo == null) { 
				return null;
			}

			OArquivo.ativo = (OArquivo.ativo == "S" ? "N" : "S");

			db.SaveChanges();

			return OArquivo;
		}

		//Excluir um registro através dos parâmetros informados
		public UtilRetorno excluir(int idReferencia, string entidade) {

			UtilRetorno Retorno = UtilRetorno.getInstance();

            Retorno.flagError = false;

			List<ArquivoUpload> listaArquivos = this.listar(idReferencia, entidade, "", "").ToList();

			foreach(ArquivoUpload Item in listaArquivos){
				var flagSucesso = this.excluir(Item.id);

				if (!flagSucesso) { 
					Retorno.flagError = true;
					Retorno.listaErros.Add($"O registro {Item.id} não pôde ser removido.");
				}
			}

			return Retorno;
		}
		
		//Excluir através do ID do registro
		public bool excluir(int id) {

			ArquivoUpload OArquivo = db.ArquivoUpload.FirstOrDefault(x => x.id == id);

            if (OArquivo == null) { 
				return false;
			}

			string pathFile = Path.Combine(UtilConfig.pathAbsUploadFiles, UtilString.notNull(OArquivo.path));
            if(File.Exists(pathFile)) File.Delete(pathFile);

			string folderThumb = Path.Combine(UtilConfig.pathAbsUploadFiles, UtilString.notNull(OArquivo.pathThumb));

            if (Directory.Exists(folderThumb)) { 
                string [] listaDiretorios = Directory.GetDirectories(folderThumb);
            
                foreach (string nomeDiretorio in listaDiretorios) {
                    string diretorioArquivoThumb = Path.Combine(nomeDiretorio, UtilString.notNull(OArquivo.nomeArquivo));
                
                    if(File.Exists(diretorioArquivoThumb)) File.Delete(diretorioArquivoThumb);
                }
            }

			db.ArquivoUpload.Where(x => x.id == id).Delete();

			return true;
		}

        //Salva a ordem de exibição
        public JsonMessage salvarOrder(int[] ids) {

            byte cont = 1;

            foreach(int id in ids){
                
                this.db.ArquivoUpload.Where(x => ( x.id == id)).Update( x => new ArquivoUpload { ordem = cont } );

                cont++;
            }

            return new JsonMessage { error = false, message = "Ordem atualizada." };
        }
	}
}