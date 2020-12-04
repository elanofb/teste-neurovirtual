using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Contribuicoes;
using EntityFramework.Extensions;

namespace BLL.Contribuicoes {

	public class ContribuicaoPrecoDescontoBL : DefaultBL, IContribuicaoPrecoDescontoBL {

		//Carregamento de registro único pelo ID
		public ContribuicaoPrecoDesconto carregar(int id) {
			
			var query = from ContrPrec in db
									 .ContribuicaoPrecoDesconto
									 .Include(x => x.ContribuicaoPreco)
						where
						 ContrPrec.id == id &&
						 ContrPrec.dtExclusao == null
						select ContrPrec;

			return query.FirstOrDefault();
		}

		//Listagem de Registros
		public IQueryable<ContribuicaoPrecoDesconto> listar(int idContribuicaoPreco) {

			var query = from ContrPrec in db
				          .ContribuicaoPrecoDesconto
				          .Include(x => x.ContribuicaoPreco)
				          .AsNoTracking()
					where
						 ContrPrec.dtExclusao == null
					select ContrPrec;


			if (idContribuicaoPreco > 0) {
				query = query.Where(x => x.idContribuicaoPreco == idContribuicaoPreco);
			}


			return query;
		}

		//Realizar os tratamentos necessários
		//Salvar um novo registro
        public bool salvar(ContribuicaoPrecoDesconto OContribuicaoPrecoDesconto) {

            if(OContribuicaoPrecoDesconto.id == 0){	
				return this.inserir(OContribuicaoPrecoDesconto);
			}

			return this.atualizar(OContribuicaoPrecoDesconto);
		}

        //Remover todos os registros anteriores e salvar os novos
        //Adicionar todos os itens enviados na lista
	    public bool salvarLote(ContribuicaoPreco OContribuicaoPreco, List<ContribuicaoPrecoDesconto> listaDesconto) {

	        this.excluirLote(OContribuicaoPreco.id, UtilNumber.toInt32(OContribuicaoPreco.idUsuarioAlteracao));

	        foreach (var ODesconto in listaDesconto) {

	            ODesconto.setDefaultInsertValues();

                ODesconto.id = 0;

	            ODesconto.idContribuicaoPreco = OContribuicaoPreco.id;

	            ODesconto.idUsuarioCadastro = OContribuicaoPreco.idUsuarioAlteracao;

                ODesconto.idUsuarioAlteracao = OContribuicaoPreco.idUsuarioAlteracao;

	            this.inserir(ODesconto);
	        }

	        return true;
	    }

        //Persistir e inserir um novo registro 
		//Inserir Contribuicao e lista de ContribuicaoPreco vinculados
        private bool inserir(ContribuicaoPrecoDesconto OContribuicaoPrecoDesconto) {

			OContribuicaoPrecoDesconto.setDefaultInsertValues();

            OContribuicaoPrecoDesconto.ContribuicaoPreco = null;

            db.ContribuicaoPrecoDesconto.Add(OContribuicaoPrecoDesconto);

			db.SaveChanges();

			return OContribuicaoPrecoDesconto.id > 0;
		}

        //Persistir e atualizar um registro existente 
		//Atualizar dados da Contribuicao e lista de ContribuicaoPreco
		private bool atualizar(ContribuicaoPrecoDesconto OContribuicaoPrecoDesconto) { 

			//Localizar existentes no banco
			ContribuicaoPrecoDesconto dbContribuicaoPreco = this.carregar(OContribuicaoPrecoDesconto.id);

			//Configurar valores padrão
			OContribuicaoPrecoDesconto.setDefaultUpdateValues();

			//Atualizacao da Contribuição
			var ContribuicaoEntry = db.Entry(dbContribuicaoPreco);

			ContribuicaoEntry.CurrentValues.SetValues(OContribuicaoPrecoDesconto);

			db.SaveChanges();

			return OContribuicaoPrecoDesconto.id > 0;
		}

        //Exclusao de um lote de vencimentos de acordo com o id da contribuicao
	    public UtilRetorno excluirLote(int idContribuicaoPreco, int idUsuarioExclusao) {

	        db.ContribuicaoPrecoDesconto.Where(x => x.idContribuicaoPreco == idContribuicaoPreco)
	            .Update(
	                x =>
	                    new ContribuicaoPrecoDesconto {
	                        idUsuarioExclusao = idUsuarioExclusao,
	                        dtExclusao = DateTime.Now
	                    });

            return UtilRetorno.newInstance(false, "Os registros foram removidos com sucesso.");
	    }
	}
}