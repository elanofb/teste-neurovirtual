using System;
using DAL.CuponsDesconto;

namespace BLL.CuponsDesconto {

	public interface IEnvioCupom {

		UtilRetorno enviar(CupomDesconto OCupomDesconto);

	}
}