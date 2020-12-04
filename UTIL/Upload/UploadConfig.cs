using System;
using System.Web;
using System.Collections.Generic;

namespace UTIL.Upload {
	public class UploadConfig {

		private List<string> _allowExtensionsImage = new List<string> { ".png", ".jpg", ".jpeg", ".gif" };
		private List<string> _extensoesPermitidas = new List<string> { ".png", ".jpg", ".gif", ".doc", ".docx", ".xls", ".xlsx", ".pdf", ".txt", ".mp3", ".mp4", ".zip", ".rar", ".msg" };

		//
		public static string getExtension(HttpPostedFileBase FileUpload) {
			
			if (FileUpload == null) {
				return "";
			}

			string fileName = FileUpload.FileName;

			if (String.IsNullOrEmpty(fileName) || !fileName.Contains(".")) {
				return "";
			}

			return FileUpload.FileName.Substring(fileName.LastIndexOf(".")).ToLower();
		}

		//
		public static bool isImageExtension(string extension) {
			UploadConfig UploadConfig = new UploadConfig();
			return UploadConfig._allowExtensionsImage.Contains(extension.ToLower());
		}

		//
		public bool validarExtensaoImagem(HttpPostedFileBase FileUpload) {
			string extension = getExtension(FileUpload);

			if (!this._allowExtensionsImage.Contains(extension.ToLower())) {
				return false;
			}
			return true;
		}

		//
		public bool validarExtensaoArquivo(HttpPostedFileBase FileUpload) {
			string extension = getExtension(FileUpload);

			if (!this._extensoesPermitidas.Contains(extension)) {
				return false;
			}
			return true;
		}

		//
		public static bool validarArquivo(HttpPostedFileBase FileUpload) {
			if (FileUpload == null) {
				return false;
			}
			UploadConfig Config = new UploadConfig();
			return Config.validarExtensaoArquivo(FileUpload);
		}

		//
		public static bool validarImagem(HttpPostedFileBase FileUpload) {
			if (FileUpload == null) {
				return false;
			}
			UploadConfig Config = new UploadConfig();
			return Config.validarExtensaoImagem(FileUpload);
		}

		//
		public static bool validateExcel(HttpPostedFileBase FileUpload) {
			string extension = getExtension(FileUpload).ToLower();
			if (extension == ".xls" || extension == ".xlsx") {
				return true;
			}

			return false;
		}

		//
		public static bool validateCsv(HttpPostedFileBase FileUpload) {
			string extension = getExtension(FileUpload).ToLower();
			if (extension == ".csv") {
				return true;
			}

			return false;
		}

		//
		public static bool validatePDF(HttpPostedFileBase FileUpload) {
			string extension = getExtension(FileUpload).ToLower();
			if (extension == ".pdf") {
				return true;
			} else {
				return false;
			}
		}

		//
		public static bool validateTextFile(HttpPostedFileBase FileUpload) {
			string extension = getExtension(FileUpload).ToLower();
			if (extension == ".txt") {
				return true;
			} else {
				return false;
			}
		}

		public static bool validarArquivoRetorno(HttpPostedFileBase FileUpload) {
			if (FileUpload == null) {
				return false;
			}

			string extension = getExtension(FileUpload).ToLower();
			if (extension == ".ret") {
				return true;
			}

			return false;
		}
	}
}