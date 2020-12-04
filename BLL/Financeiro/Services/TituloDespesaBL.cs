using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Financeiro;
using System.Data.Entity;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Financeiro {

	public abstract class TituloDespesaBL : DefaultBL, ITituloDespesaBL {

	    // Carregar um titulo a partir do seu ID
	    public virtual IQueryable<TituloDespesa> query(int? idOrganizacaoParam = null) {

	        var query = from Tit in db.TituloDespesa
	            select Tit;
            
	        if (idOrganizacaoParam == null) {
	            idOrganizacaoParam = idOrganizacao;
	        }

	        if (idOrganizacaoParam > 0) {
	            query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
	        }

	        return query;
	    }

        //
		public TituloDespesa carregar(int id, bool? flagExcluido = false) {			

			var query = (from OTituloDespesa in 
							 db.TituloDespesa
                             .Include(x => x.CentroCusto)
                             .Include(x => x.MacroConta)
                             .Include(x => x.Categoria)
						 where 
							OTituloDespesa.id == id select OTituloDespesa);

		    query = query.condicoesSeguranca();

		    if (flagExcluido == false) {
		        query = query.Where(x => x.dtExclusao == null);
		    }

            if (flagExcluido == true) {
                query = query.Where(x => x.dtExclusao.HasValue);
            }

            return query.FirstOrDefault();
		}

	    //Carregar pelos campos idReceita e CentroCusto
	    public abstract TituloDespesa carregarPorDespesa(int idDespesa);

        //
		public IQueryable<TituloDespesa> listar(string valorBusca, bool? flagExcluido = false) {
            
			var query = (from OTituloDespesa in 
							 db.TituloDespesa 
                             .Include(x => x.CentroCusto)
                             .Include(x => x.Categoria)
						 select OTituloDespesa);

            query = query.condicoesSeguranca();

            if (flagExcluido == false) {
		        query = query.Where(x => x.dtExclusao == null);
		    }

            if (flagExcluido == true) {
                query = query.Where(x => x.dtExclusao.HasValue);
            }

            if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			return query;
		}

		public UtilRetorno substituirCategoriaEMacroConta(List<int> ids, int idNovaCategoria, int idNovaMacroConta) {

		    var idUsuario = User.id();
            
		    if (ids == null) {
		        return UtilRetorno.newInstance(true, "Registros não localizado");
		    }

            db.TituloDespesa.Where(x => ids.Contains(x.id))
                .Update(x => new TituloDespesa { dtAlteracao = DateTime.Now, idUsuarioAlteracao = idUsuario, idCategoria = idNovaCategoria, idMacroConta = idNovaMacroConta});

            db.TituloDespesaPagamento.Where(x => ids.Contains(x.idTituloDespesa) && x.dtExclusao == null)
				.Update(x => new TituloDespesaPagamento { dtAlteracao = DateTime.Now, idUsuarioAlteracao = idUsuario, idCategoria = idNovaCategoria, idMacroConta = idNovaMacroConta});

		    return UtilRetorno.newInstance(false);
        }

		public UtilRetorno substituirMacroConta(List<int> ids, int idNovaMacroConta) {

		    var idUsuario = User.id();
            
		    if (ids == null) {
		        return UtilRetorno.newInstance(true, "Registros não localizado");
		    }

            db.TituloDespesa.Where(x => ids.Contains(x.id))
                .Update(x => new TituloDespesa { dtAlteracao = DateTime.Now, idUsuarioAlteracao = idUsuario, idMacroConta = idNovaMacroConta });
            
		    return UtilRetorno.newInstance(false);
        }

		public UtilRetorno excluir(int id, string motivoExclusao) {

		    var idUsuario = User.id();

		    var OTituloDespesa = this.carregar(id);
		    if (OTituloDespesa == null) {
		        return UtilRetorno.newInstance(true, "Registro não localizado");
		    }

            db.TituloDespesa.Where(x => x.id == id)
                .Update(x => new TituloDespesa { dtExclusao = DateTime.Now, idUsuarioExclusao = idUsuario, motivoExclusao = motivoExclusao });

            db.TituloDespesaPagamento.Where(x => x.idTituloDespesa == id && x.dtExclusao == null)
				.Update(x => new TituloDespesaPagamento { dtExclusao = DateTime.Now, idUsuarioExclusao = idUsuario, motivoExclusao = motivoExclusao});

		    return UtilRetorno.newInstance(false);
        }		
	}
}