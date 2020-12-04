using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Contribuicoes;
using DAL.Repository.Base;
using EntityFramework.Extensions;

namespace BLL.Contribuicoes {

	public class ContribuicaoVencimentoBL : DefaultBL, IContribuicaoVencimentoBL {

		//Carregamento de registro único pelo ID
		public ContribuicaoVencimento carregar(int id) {
			
			var query = from ContrPrec in db
									 .ContribuicaoVencimento
									 .Include(x => x.Contribuicao)
						where
						 ContrPrec.id == id &&
						 ContrPrec.dtExclusao == null
						select ContrPrec;

			return query.FirstOrDefault();
		}

		//Listagem de Registros
		public IQueryable<ContribuicaoVencimento> listar(int idContribuicao) {

			var query = from ContrPrec in db
				          .ContribuicaoVencimento
				          .Include(x => x.Contribuicao)
				          .AsNoTracking()
					where
						 ContrPrec.dtExclusao == null
					select ContrPrec;


			if (idContribuicao > 0) {
				query = query.Where(x => x.idContribuicao == idContribuicao);
			}


			return query;
		}

		//Realizar os tratamentos necessários
		//Salvar um novo registro
        public bool salvar(ContribuicaoVencimento OContribuicaoVencimento) {

            if(OContribuicaoVencimento.id == 0){	
				return this.inserir(OContribuicaoVencimento);
			}

			return this.atualizar(OContribuicaoVencimento);
		}

        //Remover todos os registros anteriores e salvar os novos
        //Adicionar todos os itens enviados na lista
	    public bool salvarLote(Contribuicao OContribuicao, List<ContribuicaoVencimento> listaVencimento) {

	        this.excluirLote(OContribuicao.id, OContribuicao.idUsuarioAlteracao);

	        foreach (var OVencimento in listaVencimento) {

	            OVencimento.setDefaultInsertValues();

                OVencimento.id = 0;

	            OVencimento.idContribuicao = OContribuicao.id;

	            OVencimento.idUsuarioCadastro = OContribuicao.idUsuarioAlteracao;

                OVencimento.idUsuarioAlteracao = OContribuicao.idUsuarioAlteracao;

	            this.inserir(OVencimento);
	        }

	        return true;
	    }

        //Persistir e inserir um novo registro 
		//Inserir Contribuicao e lista de ContribuicaoPreco vinculados
        private bool inserir(ContribuicaoVencimento OContribuicaoVencimento) {

			OContribuicaoVencimento.setDefaultInsertValues();

            OContribuicaoVencimento.Contribuicao = null;

            db.ContribuicaoVencimento.Add(OContribuicaoVencimento);

			db.SaveChanges();

			return OContribuicaoVencimento.id > 0;
		}

        //Persistir e atualizar um registro existente 
		//Atualizar dados da Contribuicao e lista de ContribuicaoPreco
		private bool atualizar(ContribuicaoVencimento OContribuicaoVencimento) { 

			//Localizar existentes no banco
			ContribuicaoVencimento dbContribuicao = this.carregar(OContribuicaoVencimento.id);

			//Configurar valores padrão
			OContribuicaoVencimento.setDefaultUpdateValues();

			//Atualizacao da Contribuição
			var ContribuicaoEntry = db.Entry(dbContribuicao);

			ContribuicaoEntry.CurrentValues.SetValues(OContribuicaoVencimento);

			db.SaveChanges();

			return OContribuicaoVencimento.id > 0;
		}

        //Exclusao de um lote de vencimentos de acordo com o id da contribuicao
	    public UtilRetorno excluirLote(int idContribuicao, int idUsuarioExclusao) {

	        db.ContribuicaoVencimento.Where(x => x.idContribuicao == idContribuicao)
	            .Update(
	                x =>
	                    new ContribuicaoVencimento {
	                        idUsuarioExclusao = idUsuarioExclusao,
	                        dtExclusao = DateTime.Now
	                    });

            return UtilRetorno.newInstance(false, "Os registros foram removidos com sucesso.");
	    }
	}
}