using System;
using System.Linq;
using System.Data.Entity;
using System.Json;
using BLL.Services;
using DAL.Arquivos;
using DAL.Entities;
using DAL.Permissao.Security.Extensions;
using DAL.Publicacoes;
using EntityFramework.Extensions;
using UTIL.Resources;

namespace BLL.Publicacoes {

	public class TipoGaleriaFotoBL : DefaultBL, ITipoGaleriaFotoBL {

		//Carregamento de registro pelo ID
		public TipoGaleriaFoto carregar(int id) { 
			var query = (from Ti in db.TipoGaleriaFoto
						 where 
							Ti.flagExcluido == false &&
							Ti.id == id
						 select Ti
						);

            query = query.condicoesSeguranca();

			return query.FirstOrDefault();
		}

		//listar registros do banco com base nos parametros
		public IQueryable<TipoGaleriaFoto> listar(string valorBusca, bool? ativo, bool? flagGaleriaAtiva) {
			var query = from T in db.TipoGaleriaFoto
						where T.flagExcluido == false
						select T;

            query = query.condicoesSeguranca();

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (ativo != null) {
				query = query.Where(x => x.ativo == ativo);
			}

		    if (flagGaleriaAtiva == true) {
                query = query.Where(
                    x => db.GaleriaFoto.Any(

                        //Galerias ativas
                        GAL => GAL.idTipoGaleria == x.id
                        && GAL.flagExcluido == "N"
                        && GAL.ativo == "S"

                        //Fotos
                        && db.ArquivoUpload.Any(
                            ARQ => ARQ.idReferenciaEntidade == GAL.id &&
                            !ARQ.dtExclusao.HasValue &&
                            ARQ.ativo == "S" &&
                            ARQ.entidade == EntityTypes.GALERIAFOTO &&
                            ARQ.categoria == ArquivoUploadTypes.FOTO
                        )
                    )
                );
		    }

			return query;
		}

		//Verificar se deve-se atualizar um registro existente ou criar um novo
		public bool salvar(TipoGaleriaFoto OTipoGaleriaFoto) {

			if (OTipoGaleriaFoto.id == 0) { 
				return this.inserir(OTipoGaleriaFoto);
			}

			return this.atualizar(OTipoGaleriaFoto);
		}

		//Persistir o objecto e salvar na base de dados
		private bool inserir(TipoGaleriaFoto OTipoGaleriaFoto) {

			OTipoGaleriaFoto.setDefaultInsertValues();

            db.TipoGaleriaFoto.Add(OTipoGaleriaFoto);

            db.SaveChanges();

			return (OTipoGaleriaFoto.id > 0);
		}

		//Persistir o objecto e atualizar informações
		private bool atualizar(TipoGaleriaFoto OTipoGaleriaFoto) { 
			OTipoGaleriaFoto.setDefaultUpdateValues<TipoGaleriaFoto>();

			//Localizar existentes no banco
			TipoGaleriaFoto dbTipoGaleriaFoto = this.carregar(OTipoGaleriaFoto.id);		

            if (dbTipoGaleriaFoto == null) {
                return false;
            }

			var TipoEntry = db.Entry(dbTipoGaleriaFoto);
			TipoEntry.CurrentValues.SetValues(OTipoGaleriaFoto);
			TipoEntry.ignoreFields(new[]{"flagSistema"});

			db.SaveChanges();
			return (OTipoGaleriaFoto.id > 0);
		}

		//Verificar se já existe um registro para evitar duplicidades
		public bool existe(string descricao, int id) {
			var query = (from T in db.TipoGaleriaFoto
						where T.descricao == descricao && T.id != id && T.flagExcluido == false
						select T).AsNoTracking();

            query = query.condicoesSeguranca();

			var OTipoTitulo = query.Take(1).FirstOrDefault();

			return (OTipoTitulo != null);
		}		

        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
	        var retorno = new JsonMessageStatus();

	        var item = this.carregar(id);

	        if (item == null) {
		        retorno.error = true;
		        retorno.message = NotificationMessages.invalid_register_id;
	        } else {
		        item.ativo = item.ativo != true;
		        db.SaveChanges();
		        retorno.active = item.ativo == true ? "S" : "N";
		        retorno.message = NotificationMessages.updateSuccess;
	        }
	        return retorno;
        }

		//Exclusao logica de registro
		public bool excluir(int id) {

            var idUsuario = User.id();

            this.db.TipoGaleriaFoto
						.Where(x => x.id == id)
						.Update(x => new TipoGaleriaFoto{ flagExcluido = true, idUsuarioAlteracao = idUsuario, dtAlteracao = DateTime.Now });

            var check = this.db.TipoGaleriaFoto.Any(x => x.id == id && x.flagExcluido == true);

			return check;
		}
        
	}
}