using System;
using BLL.Services;
using DAL.Arquivos;

namespace BLL.Arquivos {

	public class ArquivoUploadCadastroBL : DefaultBL, IArquivoUploadCadastroBL {

		/// <summary>
		/// Construtor
		/// </summary>
		public ArquivoUploadCadastroBL(){
		}

		//Gerar um registro na tabela no banco de dados
		//Gravar o arquivo em disco
		public ArquivoUpload salvar(ArquivoUpload OArquivo) {
			
			OArquivo.titulo = OArquivo.titulo.abreviar(255);
			
			OArquivo.legenda = OArquivo.legenda.abreviar(255);
			
			OArquivo.entidade = OArquivo.entidade.abreviar(255);

			OArquivo.nomeArquivo = OArquivo.nomeArquivo.abreviar(255);
			
			OArquivo.path = OArquivo.path.abreviar(255);
			
			OArquivo.pathThumb = OArquivo.pathThumb.abreviar(255);

			if (OArquivo.id == 0) {
				
				OArquivo.setDefaultInsertValues();

				db.ArquivoUpload.Add(OArquivo);

				db.SaveChanges();
				
				return OArquivo;
			}

			return this.atualizar(OArquivo);

		}

		/// <summary>
		/// Persistir o objecto e atualizar informações
		/// </summary>
		private ArquivoUpload atualizar(ArquivoUpload OArquivoUpload) { 

			OArquivoUpload.setDefaultUpdateValues();

			//Localizar existentes no banco
			ArquivoUpload dbArquivoUpload = this.db.ArquivoUpload.Find(OArquivoUpload.id);		

			var ArquivoUploadEntry = db.Entry(dbArquivoUpload);

			ArquivoUploadEntry.CurrentValues.SetValues(OArquivoUpload);

			ArquivoUploadEntry.ignoreFields();

			db.SaveChanges();

			return OArquivoUpload;

		}		

	}
}
