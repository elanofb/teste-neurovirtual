
namespace DAL.Financeiro {

	public static class StatusPagamentoExtensions {

		//
		public static string defaultCssClass(this StatusPagamento OStatus) {

		    if (OStatus == null) {
		        return "";
		    }

		    if (OStatus.id == StatusPagamentoConst.PAGO) {
    		    return "bg-green";
		    }

		    if (OStatus.id == StatusPagamentoConst.CANCELADO || OStatus.id == StatusPagamentoConst.ESTORNADO) {
    		    return "bg-red";
		    }

		    return "bg-yellow";
		}
		

	}
}