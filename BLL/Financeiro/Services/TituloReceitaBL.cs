using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Financeiro;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Financeiro {

    public abstract class TituloReceitaBL : DefaultBL, ITituloReceitaBL {

        //Atributos

        //Propriedades

        //eventos

        // Carregar um titulo a partir do seu ID
        public virtual IQueryable<TituloReceita> query(int? idOrganizacaoParam = null) {

            var query = from Tit in db.TituloReceita
                        select Tit;
            
            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;
        }


        // Carregar um titulo a partir do seu ID
        public virtual TituloReceita carregar(int id, bool? flagExcluido = false) {

            var query = from Tit in db.TituloReceita
                                        .Include(x => x.CentroCusto)
                                        .Include(x => x.Pessoa)
                                        .Include(x => x.listaTituloReceitaPagamento)
                        where
                            Tit.id == id
                        select
                            Tit;

            if (flagExcluido == false){
	            query = query.Where(x => x.dtExclusao == null);
	        }

	        if (flagExcluido == true){
	            query = query.Where(x => x.dtExclusao.HasValue);
	        }

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Carregar o titulo dando join nos dados da pessoa
        /// </summary>
        public virtual TituloReceita carregarComPessoa(int id) {

            var query = from Tit in db.TituloReceita
                                        .Include(x => x.Pessoa)
                                        .Include(x => x.Pessoa.listaEnderecos)
                                        .Include(x => x.Pessoa.listaTelefones)
                                        .Include(x => x.Pessoa.listaEmails)
                                        .Include(x => x.listaTituloReceitaPagamento)
                        where
                            Tit.id == id &&
                            Tit.dtExclusao == null
                        select
                            Tit;

            return query.FirstOrDefault();
        }

        //Carregar pelos campos idReceita e CentroCusto
        public abstract TituloReceita carregarPorReceita(int idReceita);
        
        //Listagem de registros conforme parametros informados
        public virtual IQueryable<TituloReceita> listar(int idTipoReceita, int idReceita, int idPessoa, string valorBusca, bool? flagExcluido = false) {

            var query = from Tit in db.TituloReceita
                                    .Include(x => x.CentroCusto)
                                    .Include(x => x.Pessoa)
                        select
                            Tit;

            query = query.condicoesSeguranca();

            if (flagExcluido == false) {
		        query = query.Where(x => x.dtExclusao == null);
		    }

            if (flagExcluido == true) {
                query = query.Where(x => x.dtExclusao.HasValue);
            }

            if(idTipoReceita > 0) {
                query = query.Where(x => x.idTipoReceita == idTipoReceita);
            }

            if(idReceita > 0) {
                query = query.Where(x => x.idReceita == idReceita);
            }

            if(idPessoa > 0) {
                query = query.Where(x => x.idPessoa == idPessoa);
            }

            if(!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.nomePessoa.Contains(valorBusca));
            }

            var lista = query.AsNoTracking();

            return lista;
        }

        public UtilRetorno substituirCategoriaEMacroConta(List<int> ids, int idNovaCategoria, int idNovaMacroConta) {

            var idUsuario = User.id();

            if (ids == null) {
                return UtilRetorno.newInstance(true, "Registros não localizado");
            }

            db.TituloReceita.Where(x => ids.Contains(x.id))
                .Update(x => new TituloReceita { dtAlteracao = DateTime.Now, idUsuarioAlteracao = idUsuario, idCategoria = idNovaCategoria, idMacroConta = idNovaMacroConta});

            db.TituloReceitaPagamento.Where(x => ids.Contains(x.idTituloReceita) && x.dtExclusao == null)
                .Update(x => new TituloReceitaPagamento { dtAlteracao = DateTime.Now, idUsuarioAlteracao = idUsuario, idCategoria = idNovaCategoria, idMacroConta = idNovaMacroConta });

            return UtilRetorno.newInstance(false);
        }

        public UtilRetorno substituirMacroConta(List<int> ids, int idNovaMacroConta) {

            var idUsuario = User.id();

            if (ids == null) {
                return UtilRetorno.newInstance(true, "Registros não localizado");
            }

            db.TituloReceita.Where(x => ids.Contains(x.id))
                .Update(x => new TituloReceita { dtAlteracao = DateTime.Now, idUsuarioAlteracao = idUsuario, idMacroConta = idNovaMacroConta });
            
            return UtilRetorno.newInstance(false);
        }


    }
}