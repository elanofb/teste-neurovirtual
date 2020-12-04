using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using DAL.Permissao;
using DAL.Repository.Base;
using EntityFramework.Extensions;
using UTIL.Resources;
using BLL.Services;
using System.Data.Entity;

namespace BLL.Permissao {

    public class AcessoRecursoBL : DefaultBL, IAcessoRecursoBL {

        //Atributos

        //Propriedades

        //
        public AcessoRecursoBL() {
        }

        //Carregar registro a partir do ID
        public AcessoRecurso carregar(int id) {

            var query = from Rec in db.AcessoRecurso
                        where
                            Rec.id == id &&
                            Rec.flagExcluido == "N"
                        select Rec;

            return query.FirstOrDefault();
        }

        //Listagem dos registros conforme parametros informados
        public IQueryable<AcessoRecurso> listar(int idRecursoGrupo, int idRecursoPai, string ativo) {

            var query = from oAcessoRecurso in db.AcessoRecurso
                        where
                            oAcessoRecurso.flagExcluido == "N"
                        select oAcessoRecurso;

            if (idRecursoGrupo > 0) {
                query = query.Where(x => x.idRecursoGrupo == idRecursoGrupo);
            }

            if (idRecursoPai > 0) {
                query = query.Where(x => x.idRecursoPai == idRecursoPai);
            }

            if (!String.IsNullOrEmpty(ativo)) {
                query = query.Where(x => x.ativo == "S");
            }

            return query.AsNoTracking();
        }


        //Realizar os tratamentos necessarios
        //Salvar um novo registro
        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(AcessoRecurso OAcessoRecurso) {
            bool flagSucesso = false;

            if (OAcessoRecurso.id == 0) {
                flagSucesso = this.inserir(OAcessoRecurso);
            } else {
                flagSucesso = this.atualizar(OAcessoRecurso);
            }

            return flagSucesso;
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(AcessoRecurso OAcessoRecurso) {

            OAcessoRecurso.setDefaultInsertValues<AcessoRecurso>();

            if (OAcessoRecurso.idRecursoPai == 0) {

                OAcessoRecurso.idRecursoPai = null;
            }

            db.AcessoRecurso.Add(OAcessoRecurso);

            db.SaveChanges();

            return (OAcessoRecurso.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(AcessoRecurso OAcessoRecurso) {
            OAcessoRecurso.setDefaultUpdateValues<AcessoRecurso>();

            //Localizar existentes no banco
            AcessoRecurso dbAcessoRecurso = this.carregar(OAcessoRecurso.id);
            var AcessoRecursoEntry = db.Entry(dbAcessoRecurso);
            AcessoRecursoEntry.CurrentValues.SetValues(OAcessoRecurso);
            AcessoRecursoEntry.ignoreFields<AcessoRecurso>();

            db.SaveChanges();
            return (OAcessoRecurso.id > 0);
        }

        //Atualizar ordem dos menus
        public void reordenarRecurso(int idRecurso, int idRecursoPai, int idRecursoGrupo, int ordemExibicao) {

            AcessoRecurso OAcessoRecurso = this.carregar(idRecurso);
            OAcessoRecurso.idRecursoPai = idRecursoPai;
            this.db.SaveChanges();

            if (OAcessoRecurso == null) {
                throw new InvalidOperationException(UTIL.Resources.NotificationMessages.invalid_register_id);
            }

            int cont = 0;
            var itens = db.AcessoRecurso.Where(x => x.ativo == "S" &&
                                                    (x.idRecursoPai == idRecursoPai || (x.idRecursoPai == null && idRecursoPai == 0)) &&
                                                    x.idRecursoGrupo == idRecursoGrupo &&
                                                    x.flagExcluido == "N")
                                        .OrderBy(x => x.ordemExibicao)
                                        .ToList();

            foreach (var x in itens) {

                if (x.id == OAcessoRecurso.id) {

                    db.AcessoRecurso
                            .Where(rec => rec.id == x.id)
                            .Update(rec => new AcessoRecurso {ordemExibicao = ordemExibicao});

                    continue;
                }

                if (cont == ordemExibicao){

                    cont++;
                    
                }

                var cont1 = cont;

                db.AcessoRecurso
                        .Where(rec => rec.id == x.id)
                        .Update(rec => new AcessoRecurso {ordemExibicao = cont1});

                cont++;
            }

            db.SaveChanges();
        }

        //Exclusao logica
        public UtilRetorno excluir(int id) {

            AcessoRecurso OAcessoRecurso = this.carregar(id);

            if (OAcessoRecurso == null) {
                return UtilRetorno.newInstance(true, "O registro informado não foi localizado.");
            }

            OAcessoRecurso.flagExcluido = "S";
            OAcessoRecurso.dtAlteracao = DateTime.Now;
            db.SaveChanges();

            return UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");
        }
    }
}