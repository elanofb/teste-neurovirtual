using DAL.Notificacoes;

namespace WEB.Areas.AvisosNotificacoes.Extensions {

	public static class AvisosNotificacoesExtensions {
        
		//
		public static string cssBorderNotificacaoEnvio(this NotificacaoSistemaEnvio OItem) {

		    if(OItem == null || OItem.id == 0){
		        return "";
		    }

		    if (OItem.flagExcluido == true) {
		        return "border-red";
		    }

            if (OItem.dtLeitura.HasValue) {
		        return "border-green";
		    }

		    if (OItem.dtEnvioEmail.HasValue) {
		        return "border-blue";
		    }

			return "border-grey";
		}
	}
}