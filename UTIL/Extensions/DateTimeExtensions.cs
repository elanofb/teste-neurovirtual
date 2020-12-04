
namespace System{

    public static class DateTimeExtensions{

        //Exibicao das datas
		public static string exibirData(this DateTime? dtPadrao, bool incluirHorario = false, string dataVazia = "-") {

			string html = dataVazia;

            if (!dtPadrao.isEmpty()) {

				html = dtPadrao.Value.ToShortDateString();

                if (incluirHorario) {
					html = String.Concat(html, " ", dtPadrao.Value.ToShortTimeString());
				}
			}

			return html;
		}

	    /// <summary>
	    /// Converter data em padrao timestamp
	    /// </summary>
	    public static double toUnixTimestamp(this DateTime dateTime){
		    
		    return (TimeZoneInfo.ConvertTimeToUtc(dateTime) - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
	    }
    }
}