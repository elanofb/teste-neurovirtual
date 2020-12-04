using DAL.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Associados;
using BLL.Permissao;
using DAL.Permissao;
using DAL.Associados;
using DAL.Notificacoes;
using System.Data.Entity;
using DAL.Pessoas;
using EntityFramework.Extensions;
using BLL.Services;
using DAL.MateriaisApoio;

namespace BLL.MateriaisApoio {
    public class MaterialApoioPessoaBL : DefaultBL, IMaterialApoioPessoaBL {

        // Atributos

        // Propriedades

        public MaterialApoioPessoaBL() {

        }

        public MaterialApoioPessoa carregar(int idMaterialApoio, int idPessoa) {
            var query = from NP in db.MaterialApoioPessoa.Include(x => x.Pessoa).Include(x => x.MaterialApoio)
                        where NP.idMaterialApoio == idMaterialApoio && NP.idPessoa == idPessoa
                        select NP;

            MaterialApoioPessoa OMaterialApoioPessoa = query.FirstOrDefault();
            return OMaterialApoioPessoa;
        }

        public IQueryable<MaterialApoioPessoa> listar(int idMaterialApoio, int idPessoa) {
            var query = from NP in db.MaterialApoioPessoa.Include(x => x.Pessoa).Include(x => x.MaterialApoio)
                        where NP.flagExcluido == "N" && NP.idPessoa > 0
                        select NP;

            if (idMaterialApoio > 0) { 
                query = query.Where(x => x.idMaterialApoio == idMaterialApoio);
            }

            if (idPessoa > 0) { 
                query = query.Where(x => x.idPessoa == idPessoa);
            }

            return query;

        }

        public bool salvar(MaterialApoioPessoa OMaterialApoioPessoa) {

            OMaterialApoioPessoa.MaterialApoio = null;
            OMaterialApoioPessoa.Pessoa = null;

            if(OMaterialApoioPessoa.id == 0) {
                return this.inserir(OMaterialApoioPessoa);
            }

            return this.atualizar(OMaterialApoioPessoa);
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(MaterialApoioPessoa OMaterialApoioPessoa) {
            OMaterialApoioPessoa.setDefaultInsertValues<MaterialApoioPessoa>();
            db.MaterialApoioPessoa.Add(OMaterialApoioPessoa);
            db.SaveChanges();

            return (OMaterialApoioPessoa.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(MaterialApoioPessoa OMaterialApoioPessoa) {
            
            MaterialApoioPessoa dbMaterialApoioPessoa = this.carregar(OMaterialApoioPessoa.idMaterialApoio, OMaterialApoioPessoa.idPessoa);

            var tipoEntry = db.Entry(dbMaterialApoioPessoa);

            OMaterialApoioPessoa.setDefaultUpdateValues<MaterialApoioPessoa>();
            tipoEntry.CurrentValues.SetValues(OMaterialApoioPessoa);
            tipoEntry.ignoreFields<MaterialApoioPessoa>();

            db.SaveChanges();
            return (OMaterialApoioPessoa.id > 0);
        }

        public bool excluir(int idMaterialApoio) {
            db.MaterialApoioPessoa
                .Where(x => x.idMaterialApoio == idMaterialApoio)
                .Delete();

			var listaCheck = db.MaterialApoioPessoa.Where(x => x.idMaterialApoio == idMaterialApoio && x.flagExcluido == "N").ToList();
			return (listaCheck.Count == 0);
        }

        public bool excluirPessoa(int idMaterialApoio, int idPessoa) {
            db.MaterialApoioPessoa
                .Where(x => x.idMaterialApoio == idMaterialApoio && x.idPessoa == idPessoa)
                .Delete();

			var listaCheck = db.MaterialApoioPessoa.Where(x => x.idMaterialApoio == idMaterialApoio && x.flagExcluido == "N").ToList();
			return (listaCheck.Count == 0);
        }

    }

}
