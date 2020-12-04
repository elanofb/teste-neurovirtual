using System;
using System.Linq;
using DAL.Contribuicoes;
using DAL.Repository.Base;
using EntityFramework.Extensions;

namespace BLL.Contribuicoes {

	public class PeriodoContribuicaoBL : TableRepository<PeriodoContribuicao>, IPeriodoContribuicaoBL {

		//Carregamento de registro único pelo ID
		public PeriodoContribuicao carregar(int id) {

			var db = this.getDataContext();
			var query = from Tipo in db.PeriodoContribuicao
						where 
							Tipo.id == id && 
							Tipo.flagExcluido == false
						select Tipo;

			return query.FirstOrDefault();
		}

		//Listagem de Registros
		public IQueryable<PeriodoContribuicao> listar(bool? ativo) {

			var db = this.getDataContext();
			var query = from Tipo in db.PeriodoContribuicao.AsNoTracking()
						where 
							Tipo.flagExcluido == false
						select Tipo;

			if (ativo.HasValue) { 
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		//Realizar os tratamentos necessários
		//Salvar um novo registro
		public bool salvar(PeriodoContribuicao OPeriodoContribuicao) {

            this.save(OPeriodoContribuicao);

            return (OPeriodoContribuicao.id > 0);
		}

		//Excluir registro
		public UtilRetorno excluir(int id) {
			UtilRetorno Retorno = UtilRetorno.getInstance();
			Retorno.flagError = true;

			var db = this.getDataContext();
			int qtdeRelacionamentos = (from Cont in db.Contribuicao where Cont.idPeriodoContribuicao == id select Cont).Count();

			if (qtdeRelacionamentos > 0) { 
				Retorno.flagError = true;
				Retorno.listaErros.Add("Não é possível remover o registro, existem contribuições vinculadas.");
				return Retorno;
			}
			
			db.PeriodoContribuicao
			    .Where(x => x.id == id)
			    .Delete();

			return Retorno;
		}
	}
}