using System.Linq;
using DAL.Contribuicoes;

namespace BLL.Contribuicoes {

	public class MensalidadeBL : ContribuicaoBL, IContribuicaoBL {

		//Atributos

		//Propriedades

		//Listagem de Registros
		public override IQueryable<Contribuicao> listar(string valorBusca, string ativo) {

			int idTipoContribuicao = TipoContribuicaoConst.MENSALIDADES;

			var query = base.listar(valorBusca, ativo);
			
			query = query.Where(x => x.idTipoContribuicao == idTipoContribuicao);

			return query;
		}

		//Realizar os tratamentos necessários
		//Salvar um novo registro
		public override bool salvar(Contribuicao OContribuicao) {
			OContribuicao.idTipoContribuicao = TipoContribuicaoConst.MENSALIDADES;

			return base.salvar(OContribuicao);
		}

		//Verificar se já existe uma anuidade com para o ano informado
        public override bool existe(Contribuicao OContribuicao) {
            var db = this.getDataContext();

			var query = from Contr in db.Contribuicao.AsNoTracking()
						where
							Contr.id != OContribuicao.id && 
							Contr.descricao == OContribuicao.descricao &&
							Contr.dtCancelamento == null
						select 
							Contr;

			bool flagExiste = (query.Count() > 0);
			return flagExiste;
        }
	}
}