using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace UTIL.Upload {
	public static class UploadFileValidation {

		private static readonly List<string> _allowExtensionsImage = new List<string> { ".png", ".jpg", ".jpeg", ".gif" };
		private static readonly List<string> _extensoesPermitidas = new List<string> { ".png", ".jpg", ".gif", ".doc", ".docx", ".xls", ".xlsx", ".pdf", ".txt", ".mp3", ".mp4", ".zip", ".rar", ".msg" };
		private static readonly byte[] BMP = { 66, 77 };
		private static readonly byte[] DOC = { 208, 207, 17, 224, 161, 177, 26, 225 };
		private static readonly byte[] EXE_DLL = { 77, 90 };
		private static readonly byte[] GIF = { 71, 73, 70, 56 };
		private static readonly byte[] ICO = { 0, 0, 1, 0 };
		private static readonly byte[] JPG = { 255, 216, 255 };
		private static readonly byte[] MP3 = { 255, 251, 48 };
		private static readonly byte[] OGG = { 79, 103, 103, 83, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0 };
		private static readonly byte[] PDF = { 37, 80, 68, 70, 45, 49, 46 };
		private static readonly byte[] PNG = { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82 };
		private static readonly byte[] RAR = { 82, 97, 114, 33, 26, 7, 0 };
		private static readonly byte[] SWF = { 70, 87, 83 };
		private static readonly byte[] TIFF = { 73, 73, 42, 0 };
		private static readonly byte[] TORRENT = { 100, 56, 58, 97, 110, 110, 111, 117, 110, 99, 101 };
		private static readonly byte[] TTF = { 0, 1, 0, 0, 0 };
		private static readonly byte[] WAV_AVI = { 82, 73, 70, 70 };
		private static readonly byte[] WMV_WMA = { 48, 38, 178, 117, 142, 102, 207, 17, 166, 217, 0, 170, 0, 98, 206, 108 };
		private static readonly byte[] ZIP_DOCX = { 80, 75, 3, 4 };

		//
		public static bool isImageType(Stream streamFile) {
			
			try {
				
				var OImage = Image.FromStream(streamFile);

				//Move the pointer back to the beginning of the stream
				streamFile.Seek(0, SeekOrigin.Begin);

				if (ImageFormat.Jpeg.Equals(OImage.RawFormat)) {
					return true;
				}

				if (ImageFormat.Gif.Equals(OImage.RawFormat)) {
					return true;
				}
				
				if (ImageFormat.Png.Equals(OImage.RawFormat)) {
					return true;
				}

			} catch (Exception) {
				return false;
			}

			return false;
		}

		//
		public static bool isPDFType(HttpPostedFileBase file) {
			
			try {

				byte[] data = new Byte[file.ContentLength];
				
				file.InputStream.Read(data, 0, file.ContentLength);
				
				
				if (data.Take(7).SequenceEqual(PDF)) {
					return true;
				}

			} catch (Exception) {
				return false;
			}

			return false;
		}

		//
		public static bool isExcelType(HttpPostedFileBase file) {
			
			try {

				byte[] data = new Byte[file.ContentLength];
				
				file.InputStream.Read(data, 0, file.ContentLength);
				
				
				if (data.Take(16).SequenceEqual(EXE_DLL)) {
					return true;
				}

			} catch (Exception) {
				return false;
			}

			return false;
		}

		//
		public static string getExtension(HttpPostedFileBase FileUpload) {
			return FileUpload.FileName.Substring(FileUpload.FileName.LastIndexOf(".", StringComparison.Ordinal));
		}

		//
		public static bool isImageExtension(string extension) {
			return _allowExtensionsImage.Contains(extension.ToLower());
		}

		//
		public static bool isImageExtension(HttpPostedFileBase FileUpload) {
			string extension = getExtension(FileUpload);

			if (!_allowExtensionsImage.Contains(extension.ToLower())) {
				return false;
			}
			return true;
		}

		//
		public static bool isAllowedExtension(HttpPostedFileBase FileUpload) {
			string extension = getExtension(FileUpload);

			if (!_extensoesPermitidas.Contains(extension)) {
				return false;
			}
			return true;
		}

		//
		public static bool isPDFExtension(HttpPostedFileBase FileUpload) {
			string extension = getExtension(FileUpload).ToLower();

			if (extension == ".pdf") {
				return true;
			}
	
			return false;
		}

		//
		public static bool isTextExtenion(HttpPostedFileBase FileUpload) {
			string extension = getExtension(FileUpload).ToLower();
			if (extension == ".txt") {
				return true;
			}

			return false;
		}


	}
}