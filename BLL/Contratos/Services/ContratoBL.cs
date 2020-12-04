using System;
using System.Data.Entity;
using System.Linq;
using DAL.Contratos;
using DAL.Repository.Base;
using EntityFramework.Extensions;
using System.Web;
using DAL.Entities;
using BLL.Arquivos;

namespace BLL.Contratos {

	public class ContratoBL : TableRepository<Contrato>, IContratoBL {

        //Atributos
		private IArquivoUploadBL _ArquivoUploadBL;

		//Propriedades
		private IArquivoUploadBL OArquivoUploadBL{ get{ return (this._ArquivoUploadBL = this._ArquivoUploadBL ?? new ArquivoUploadBL()); }}

		// Construtor
		public ContratoBL() {

		}

        //
		public Contrato carregar(int id) {
			var db = this.getDataContext();
			var query = from P in db.Contrato.Include(x => x.TipoContrato)
                                             .Include(x => x.ContratoVinculado)
											 .Include(x => x.Fornecedor)
                                             .Include(x => x.StatusContrato)
                                             

						where P.id == id && P.flagExcluido == "N"
						select P;

			return query.FirstOrDefault();
		}

		//
		public IQueryable<Contrato> listar(string valorBusca, string ativo) {
			var db = this.getDataContext();
			var query = from P in db.Contrato.Include(x => x.TipoContrato)
                                             .Include(x => x.ContratoVinculado)
											 .Include(x => x.Fornecedor)
                                             .Include(x => x.StatusContrato)

						where P.flagExcluido == "N"
						select P;

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.titulo.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		/**
		 * Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
		 */

		public bool existe(string descricao, int id) {
			var db = this.getDataContext();
			var query = from P in db.Contrato
						where P.titulo == descricao && P.id != id && P.flagExcluido == "N"
						select P;
			var OContrato = query.Take(1).FirstOrDefault();
			return (OContrato == null ? false : true);
		}

		//
		public bool salvar(Contrato OContrato, HttpPostedFileBase OArquivoContrato) {

            if (OContrato.id == 0) { 
                OContrato.idStatusContrato = Convert.ToInt32(StatusContratoEnum.ACORDADO);
            }

            this.save(OContrato, false);

            if (OArquivoContrato != null) { 
				this.OArquivoUploadBL.salvarDocumento(OContrato.id, EntityTypes.CONTRATO, "", OArquivoContrato, 0);
			}

			return (OContrato.id > 0);
		}

		//
		public bool excluir(int[] ids) {
			this.getDataContext().Contrato.Where(x => ids.Contains(x.id))
				.Update(x => new Contrato { flagExcluido = "S", dtAlteracao = DateTime.Now });

			var listaCheck = this.getDataContext().Contrato.Where(x => ids.Contains(x.id) && x.flagExcluido == "N").ToList();
			return (listaCheck.Count == 0);
		}
	}
}