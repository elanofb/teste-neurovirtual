using System;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Mvc;
using ZXing;
using ZXing.Common;

namespace WEB.Extensions {

	public static class ZxingExtensions {

		public static IHtmlString gerarBarcode39(this HtmlHelper html, string qrValue, int height = 100, int width = 100, bool withText = false, int margin = 0) {

			var barcodeWriter = new BarcodeWriter {
                
				Format = BarcodeFormat.CODE_39,
				Options = new EncodingOptions {
					Height = height,
					Width = width,
					Margin = margin,
					PureBarcode = !withText
				}
			};

			return criarTagImg(barcodeWriter, qrValue);

		}

        public static IHtmlString gerarQrCode(this HtmlHelper html, string qrValue, int height = 100, int width = 100, bool withText = false, int margin = 0) {
            
			var barcodeWriter = new BarcodeWriter {
				Format = BarcodeFormat.QR_CODE,
				Options = new EncodingOptions {
					Height = height,
					Width = width,
					Margin = margin,
					PureBarcode = !withText
				}
			};

            return criarTagImg(barcodeWriter, qrValue);
            			
		}

        private static MvcHtmlString criarTagImg(BarcodeWriter barcodeWriter, string valor) {

            using (var bitmap = barcodeWriter.Write(valor)) {
				using (var stream = new MemoryStream()) {
					bitmap.Save(stream, ImageFormat.Gif);

					var img = new TagBuilder("img");
					img.Attributes.Add("src", String.Format("data:image/gif;base64,{0}",
						Convert.ToBase64String(stream.ToArray())));

					return MvcHtmlString.Create(img.ToString(TagRenderMode.SelfClosing));
				}
			}

        }

	}
}