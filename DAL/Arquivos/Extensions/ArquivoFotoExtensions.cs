using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DAL.Arquivos.Extensions {

	public static class ArquivoFotoExtensions {

		// Carregar a foto principal da lista
		public static ArquivoUpload fotoPrincipal(this List<ArquivoUpload> listaFotos, int? idReferenciaEntidade = null) {

			if (listaFotos == null) {
                return new ArquivoUpload();
            }

			if (idReferenciaEntidade > 0) {
				listaFotos = listaFotos.Where(x => x.idReferenciaEntidade == idReferenciaEntidade).ToList();
			}
			
			if (!listaFotos.Any()) {
				return new ArquivoUpload();
			}

            var FotoPrincipal = listaFotos.FirstOrDefault(x => x.flagPrincipal == "S") ?? listaFotos.OrderByDescending(x => x.id).FirstOrDefault();

            return FotoPrincipal;

		}
        
	}

}
