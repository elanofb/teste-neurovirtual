using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Associados;

namespace BLL.Associados {

	public interface IAssociadoSituacaoBL {

		bool desativar(int idAssociado);
		bool admitir(int idAssociado);

	}
}
