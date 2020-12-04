using System;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using DAL.Produtos;
using UTIL.Resources;

namespace BLL.Produtos {

	public class TipoProdutoBL : DefaultBL, ITipoProdutoBL {

		//
		public TipoProdutoBL() {
		}

	    //
	    public IQueryable<TipoProduto> query(int? idOrganizacaoParam = null) {

	        var query = from Obj in db.TipoProduto
	                    where Obj.flagExcluido == false
	                    select Obj;
            
	        if (idOrganizacaoParam == null) {
	            idOrganizacaoParam = idOrganizacao;
	        }

	        if (idOrganizacaoParam > 0) {
	            query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
	        }

	        return query;

	    }

		//Carregamento de registro pelo ID
		public TipoProduto carregar(int id) { 

		    var query = this.query().condicoesSeguranca();
            
		    return query.FirstOrDefault(x => x.id == id);

		}

		//Listagem de registros de acordo com filtros
		public IQueryable<TipoProduto> listar(string valorBusca, bool? ativo) {

			var query = this.query().condicoesSeguranca();

			if (!string.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (ativo != null) {
				query = query.Where(x => x.ativo == ativo);
			}
            
			return query;
		}

		//Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
		public bool existe(string descricao, int id) {

		    var query = this.query();

		    query = query.Where(x => x.descricao == descricao && x.id != id);

		    query = query.condicoesSeguranca();
            
			var OTipoProduto = query.Take(1).FirstOrDefault();

			return (OTipoProduto != null);

		}

		//Verificar se deve-se atualizar um registro existente ou criar um novo
		public bool salvar(TipoProduto OTipoProduto) {

			OTipoProduto.descricao = OTipoProduto.descricao.toUppercaseWords(); 

			if (OTipoProduto.id == 0) { 
				return this.inserir(OTipoProduto);
			}

			return this.atualizar(OTipoProduto);
		}

		//Persistir o objecto e salvar na base de dados
		private bool inserir(TipoProduto OTipoProduto) { 

			OTipoProduto.setDefaultInsertValues();

			db.TipoProduto.Add(OTipoProduto);

			db.SaveChanges();

			return (OTipoProduto.id > 0);

		}

		//Persistir o objecto e atualizar informações
		private bool atualizar(TipoProduto OTipoProduto) { 
            
			//Localizar existentes no banco
			var dbTipoProduto = this.carregar(OTipoProduto.id);		

		    if (dbTipoProduto == null) {
		        return false;
		    }

			var dbEntry = db.Entry(dbTipoProduto);

		    OTipoProduto.setDefaultUpdateValues();

		    dbEntry.CurrentValues.SetValues(OTipoProduto);

		    dbEntry.ignoreFields();

			db.SaveChanges();

			return (OTipoProduto.id > 0);
		}

	    //Alteracao de status
	    public JsonMessageStatus alterarStatus(int id) {
	        var retorno = new JsonMessageStatus();

	        var item = this.carregar(id);

	        if (item == null) {
	            retorno.error = true;
	            retorno.message = NotificationMessages.invalid_register_id;
	        } else {
	            item.ativo = (item.ativo != true);
	            db.SaveChanges();
	            retorno.active = item.ativo == true ? "S" : "N";
	            retorno.message = NotificationMessages.updateSuccess;
	        }
	        return retorno;
	    }

	    //Remover o registro do sistema (exclusao lógico - não se remove fiscamente do banco de dados)
	    public UtilRetorno excluir(int id) {

	        var Objeto = this.carregar(id);

	        if (Objeto == null) {
	            return UtilRetorno.newInstance(true, "O registro informado não foi localizado.");
	        }

	        Objeto.flagExcluido = true;

	        Objeto.dtAlteracao = DateTime.Now;;

	        Objeto.idUsuarioAlteracao = User.id();
            
	        db.SaveChanges();

	        return UtilRetorno.newInstance(false, "Registro removido com sucesso.");

	    }

	}
}