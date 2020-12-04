using System.Web.Mvc;

namespace WEB.Areas.Associados.Helpers{

	/**
	 * 
	 */
    public class TitulacaoHelper{

		public static string tituloSr = "Sr.";
		public static string tituloSra = "Sra.";
		public static string tituloDr = "Dr.";
		public static string tituloDra = "Dra.";
		public static string tituloProf = "Prof. Dr.";
		public static string tituloProfa = "Prof. Dra.";

        //
        public static SelectList getCombo(string selected){
            var list = new[] { 
                    new{value = "Sr.", text = "Sr."},
                    new{value = "Sra.", text = "Sra."},
                    new{value = "Dr.", text = "Dr."},
                    new{value = "Dra.", text = "Dra."},
                    new{value = "Prof. Dr.", text = "Prof. Dr."},
                    new{value = "Prof. Dra.", text = "Prof. Dra."}
            };
            return new SelectList(list, "value", "text", selected);
        }


    }
}