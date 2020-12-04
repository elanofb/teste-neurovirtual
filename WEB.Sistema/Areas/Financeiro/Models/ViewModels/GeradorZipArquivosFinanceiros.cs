using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Principal;
using DAL.Financeiro;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.Financeiro.ViewModels{

	public class GeradorZipArquivosFinanceiros {
		
		// Propriedades
		public List<int> idsArquivos { get; set; }

		// Constantes
		private IPrincipal User { get; set; }

		//
		public GeradorZipArquivosFinanceiros() {
			
			this.idsArquivos = new List<int>();
		}

		public string gerarZip(List<ReceitaDespesaArquivoVW> listaArquivos) {
			
			if (!listaArquivos.Any()) {
				return "";
			}

			string baseName = String.Concat("Arquivos_Financeiros_", User.id(), "_", UtilString.onlyNumber(DateTime.Now.ToString()));
			string nomePathTempArquivos = String.Concat(UtilConfig.pathAbsTempFiles, "/", baseName);

			if (!Directory.Exists(nomePathTempArquivos)) {
				Directory.CreateDirectory(nomePathTempArquivos);
			}

			foreach (var OArquivo in listaArquivos) {
              
				string pathFile = Path.Combine(UtilConfig.pathAbsUpload(OArquivo.idOrganizacao.toInt()), OArquivo.path);
				
				var OFileInfo = new FileInfo(pathFile);

				if (!OFileInfo.Exists) {
					continue;
				}

				UtilIO.copiarArquivo(OFileInfo, nomePathTempArquivos, OFileInfo.Name);
			}

			string nomeArquivoZip = String.Concat(baseName, ".zip");
			
			string nomeFullArquivoZip = string.Concat(UtilConfig.pathAbsTempFiles, "/", nomeArquivoZip);

			ZipFile.CreateFromDirectory(nomePathTempArquivos, nomeFullArquivoZip, CompressionLevel.Optimal, true);

			return nomeArquivoZip;
			
		}

	}
}