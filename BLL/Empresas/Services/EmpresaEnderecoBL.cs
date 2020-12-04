using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Empresas;
using DAL.Repository.Base;

namespace BLL.Empresas {

	public class EmpresaEnderecoBL : TableRepository<EmpresaEndereco> {

		//
		public EmpresaEnderecoBL()
			: base(null) {
			this.defaultPredicate = (x => x.flagExcluido == "N");
		}

		//
		public IQueryable<EmpresaEndereco> listar(int idEmpresa, int idTipoEndereco) {
			var db = this.getDataContext();
			var query = from EndEmp in db.EmpresaEndereco
						join Cid in db.Cidade on EndEmp.idCidade equals Cid.id
						join Est in db.Estado on Cid.idEstado equals Est.id
						where
							EndEmp.idTipoEndereco == idTipoEndereco &&
							EndEmp.idEmpresa == idEmpresa &&
							EndEmp.flagExcluido == "N"
						select EndEmp;

			return query;
		}

		//
		public IList<EmpresaEndereco> getAll(int idEmpresa, int idTipoEndereco) {
			var query = this.listar(idEmpresa, idTipoEndereco);
			return query.ToList();
		}

		//
		public EmpresaEndereco getSingle(int idEmpresa, int idTipoEndereco) {
			var query = this.listar(idEmpresa, idTipoEndereco);
			return query.FirstOrDefault();
		}

		//
		public void salvar(EmpresaEndereco OEnderecoEmpresa, bool flagMessage = true) {
			OEnderecoEmpresa.cep = UtilString.onlyNumber(OEnderecoEmpresa.cep);
			base.save(OEnderecoEmpresa, flagMessage);
		}
	}
}