using System;
using System.Linq;
using DAL.Pessoas;
using DAL.Relacionamentos;
using DAL.Repository.Base;
using EntityFramework.Extensions;

namespace BLL.Associados {

	public class AssociadoRelacionamentoBL : TableRepository<PessoaRelacionamento>, IAssociadoRelacionamentoBL {

		//
		public AssociadoRelacionamentoBL() {
		}

		//
		public PessoaRelacionamento carregar(int id) {
			var db = this.getDataContext();
			var query = from PesRel in db.PessoaRelacionamento.Include("Pessoa").Include("OcorrenciaRelacionamento")
						where PesRel.id == id && PesRel.flagExcluido == "N"
						select PesRel;
			PessoaRelacionamento OPessoaRelacionamento = query.FirstOrDefault();
			return OPessoaRelacionamento;
		}

		//
		public IQueryable<PessoaRelacionamento> listar(int idPessoa, int idOcorrenciaRelacionamento) {
			var db = this.getDataContext();
			var query = from PesRel in db.PessoaRelacionamento.Include("Pessoa").Include("OcorrenciaRelacionamento")
						where PesRel.flagExcluido == "N"
						select PesRel;

			if (idPessoa > 0) {
				query = query.Where(x => x.idPessoa == idPessoa);
			}

			if (idOcorrenciaRelacionamento > 0) {
				query = query.Where(x => x.idOcorrenciaRelacionamento == idOcorrenciaRelacionamento);
			}

			return query;
		}

		//Salvar Dados Principais
		public bool salvar(PessoaRelacionamento OPessoaRelacionamento) {
			this.save(OPessoaRelacionamento, false);

			if (OPessoaRelacionamento.idOcorrenciaRelacionamento == Convert.ToInt32(OcorrenciaRelacionamentoEnum.ADMISSAO_ASSOCIADO)) {
				AssociadoBL OAssociadoBL = new AssociadoBL();
				//OAssociadoBL.registrarAdmissao(OPessoaRelacionamento.idPessoa);
			}

			if (OPessoaRelacionamento.idOcorrenciaRelacionamento == Convert.ToInt32(OcorrenciaRelacionamentoEnum.DESATIVACAO_ASSOCIADO)) {
				AssociadoBL OAssociadoBL = new AssociadoBL();
				//OAssociadoBL.registrarDesativacao(OPessoaRelacionamento.idPessoa);
			}

			if (OPessoaRelacionamento.idOcorrenciaRelacionamento == Convert.ToInt32(OcorrenciaRelacionamentoEnum.REATIVACAO_ASSOCIADO)) {
				AssociadoBL OAssociadoBL = new AssociadoBL();
				//OAssociadoBL.registrarReativacao(OPessoaRelacionamento.idPessoa);
			}
			return (OPessoaRelacionamento.id > 0);
		}

		/**
		 * Registrar Ocorrência de cadastro do associado
		 */

		public bool registrarCadastro(int idPessoa) {
			int idOcorrenciaCadastro = Convert.ToInt32(OcorrenciaRelacionamentoEnum.REALIZACAO_CADASTRO);
			PessoaRelacionamento OPessoaRelacionamento = new PessoaRelacionamento();
			OPessoaRelacionamento.idPessoa = idPessoa;
			OPessoaRelacionamento.idOcorrenciaRelacionamento = idOcorrenciaCadastro;

			this.save(OPessoaRelacionamento, false);

			return (OPessoaRelacionamento.id > 0);
		}

		/**
		 *
		 */

		public bool excluir(int[] ids) {
			this.getDataContext().PessoaRelacionamento.Where(x => ids.Contains(x.id))
				.Update(x => new PessoaRelacionamento { flagExcluido = "S", dtAlteracao = DateTime.Now });

			var listaCheck = this.getDataContext().PessoaRelacionamento.Where(x => ids.Contains(x.id) && x.flagExcluido == "N").ToList();
			return (listaCheck.Count == 0);
		}
	}
}