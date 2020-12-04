using System;
using System.IO;
using System.Net;
using System.Web.Mvc;
using NReco.PdfGenerator;


namespace UTIL.Pdf {
	public class UtilPDF {

		//
		public byte[] gerarBoletoPDF(string htmlCode) {
			
			try {
				//var ODocumento = new HtmlToPdfDocument{
				//	GlobalSettings = {
				//		DocumentTitle = "CBCD",
				//		PaperSize = PaperKind.A4, // Implicit conversion to PechkinPaperSize
				//		Orientation = TuesPechkin.GlobalSettings.PaperOrientation.Portrait,
						
				//		Margins =
				//		{
				//			All = 1,
				//			Unit = Unit.Centimeters
				//		}
				//	},
				//	Objects = {
				//		new ObjectSettings { HtmlText = htmlCode, LoadSettings = new LoadSettings{ ZoomFactor = 1.5}}
				//	}
				//};
				
				//IConverter converter =
				//	new ThreadSafeConverter(
				//		new RemotingToolset<PdfToolset>(
				//			new Win32EmbeddedDeployment(
				//				new TempFolderDeployment())));

				//byte[] pdfBuffer = converter.Convert(ODocumento );
				//return pdfBuffer;				
				return null;

			} catch (Exception ex) {
				UtilLog.saveError(ex,"Erro ao salvar PDF ");
				return null;
			}

		}
		
		public static bool gerarPDF(string htmlContent, string filename){
			
			string pathPDF = filename;
			
			var htmlToPdf = new HtmlToPdfConverter();
			var margins = new PageMargins();
			margins.Bottom = 10; // margins.Left = 5;margins.Right = 5;
			margins.Top = 10;
			htmlToPdf.Margins = margins;
			htmlToPdf.CustomWkHtmlPageArgs = "--enable-smart-shrinking  --encoding <encoding>";
			htmlToPdf.PageFooterHtml = "";
			//htmlToPdf.Orientation = NReco.PdfGenerator.PageOrientation.Portrait;
			htmlToPdf.Zoom = 1.0f;
			
			var conteudo = htmlToPdf.GeneratePdf(htmlContent, null);
			
			UtilIO.saveBytesToFile(pathPDF, conteudo, FileMode.Create);
			
			if (File.Exists(pathPDF)){
				return true;
			}
			
			return false;

		}

	}
}
