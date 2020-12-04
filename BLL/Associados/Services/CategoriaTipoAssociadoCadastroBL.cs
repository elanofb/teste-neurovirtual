using System;
using System.Linq;
using System.Data.Entity;
using DAL.Associados;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Associados {
	public class CategoriaTipoAssociadoCadastroBL : CategoriaTipoAssociadoConsultaBL, ICategoriaTipoAssociadoCadastroBL {

		//Verificar se deve-se atualizar um registro existente ou criar um novo
		public bool salvar(CategoriaTipoAssociado OCategoriaTipoAssociado) {

			if (OCategoriaTipoAssociado.id == 0) { 
				return this.inserir(OCategoriaTipoAssociado);
			}

			return this.atualizar(OCategoriaTipoAssociado);
		}
		//Persistir o objeto e salvar na base de dados
		private bool inserir(CategoriaTipoAssociado OCategoriaTipoAssociado) { 

			OCategoriaTipoAssociado.setDefaultInsertValues<CategoriaTipoAssociado>();
			db.CategoriaTipoAssociado.Add(OCategoriaTipoAssociado);
			db.SaveChanges();

			return (OCategoriaTipoAssociado.id > 0);
		}
		//Persistir o objecto e atualizar informações
		private bool atualizar(CategoriaTipoAssociado OCategoriaTipoAssociado) { 
			OCategoriaTipoAssociado.setDefaultUpdateValues<CategoriaTipoAssociado>();

			//Localizar existentes no banco
			CategoriaTipoAssociado dbCategoriaTipoAssociado = this.carregar(OCategoriaTipoAssociado.id);		
			var TipoEntry = db.Entry(dbCategoriaTipoAssociado);
			TipoEntry.CurrentValues.SetValues(OCategoriaTipoAssociado);
			TipoEntry.ignoreFields<CategoriaTipoAssociado>();

			db.SaveChanges();
			return (OCategoriaTipoAssociado.id > 0);
		}
		//Verificar se já existe um registro para evitar duplicidades
		public bool existe(string descricao, int id, int? idOrganizacaoInf = null) {

            if (idOrganizacao > 0 && idOrganizacaoInf == null) {
                idOrganizacaoInf = idOrganizacao;
            }

            var query = (from T in db.CategoriaTipoAssociado
						where T.descricao == descricao && T.id != id && T.flagExcluido == "N"
                         select T).AsNoTracking();

            if (idOrganizacaoInf > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoInf);
            }

            if (idOrganizacaoInf == 0) {
                query = query.Where(x => x.idOrganizacao == null);
            }

            var OTipoTitulo = query.FirstOrDefault();

			return (OTipoTitulo != null);
		}

	}

}