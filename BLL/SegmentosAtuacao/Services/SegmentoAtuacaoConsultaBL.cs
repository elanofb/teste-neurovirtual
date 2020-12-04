using System;
using System.Linq;
using System.Data.Entity;
using DAL.SegmentosAtuacao;
using BLL.Services;

namespace BLL.SegmentosAtuacao {

	public class SegmentoAtuacaoConsultaBL : DefaultBL, ISegmentoAtuacaoConsultaBL {

		//
		public SegmentoAtuacaoConsultaBL(){
		}

		//Carregamento de registro pelo ID
		public SegmentoAtuacao carregar(int id) { 
			var query = (from Ti in db.SegmentoAtuacao
						 where 
							Ti.dtExclusao == null &&
							Ti.id == id
						 select Ti
						);

			return query.FirstOrDefault();
		}

		//listar registros do banco com base nos parametros
		public IQueryable<SegmentoAtuacao> listar(string valorBusca, bool? ativo) {
			var query = from T in db.SegmentoAtuacao
						where T.dtExclusao == null
						select T;

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (ativo != null) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		//Verificar se já existe um registro para evitar duplicidades
		public bool existe(string descricao, int id) {
			var query = (from T in db.SegmentoAtuacao
						where T.descricao == descricao && T.id != id && T.dtExclusao == null
						select T).AsNoTracking();

			var OTipoTitulo = query.Take(1).FirstOrDefault();
			return (OTipoTitulo != null);
		}		
		
	}
}