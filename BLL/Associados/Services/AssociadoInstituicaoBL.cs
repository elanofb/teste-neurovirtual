using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Associados;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Associados {

    public class AssociadoInstituicaoBL : DefaultBL, IAssociadoInstituicaoBL {

        //Load de um registro a partir do ID
        public AssociadoInstituicao carregar(int id) {

            var query = from PesCar in db.AssociadoInstituicao
                                        .Include(x => x.Associado)
                                        .Include(x => x.Associado.Pessoa)
                                        .Include(x => x.Instituicao)
                        where PesCar.id == id && PesCar.dtExclusao == null
                        select PesCar;

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();
        }

        //Listagem de registros do banco a partir dos parâmetros informados
        public IQueryable<AssociadoInstituicao> listar(int idAssociado, bool? ativo) {

            var query = (from PesCar in db.AssociadoInstituicao
                                        .Include(x => x.Associado)
                                        .Include(x => x.Instituicao)
                         where
                             PesCar.dtExclusao == null
                         select PesCar).AsNoTracking();

            query = query.condicoesSeguranca();

            if (idAssociado > 0) {
                query = query.Where(x => x.idAssociado == idAssociado);
            }

            if (ativo.HasValue) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

        //Verificar se já existe registrar dentro do mesmo período para evitar duplicidades
        public bool existe(AssociadoInstituicao OAssociadoInstituicao, int idDesconsiderado) {

            var query = (from PesCar in db.AssociadoInstituicao
                         where
                             PesCar.id != idDesconsiderado &&
                             PesCar.dtExclusao == null
                         select PesCar).AsNoTracking();

            query = query.condicoesSeguranca();

            query = query.Where(x => x.idAssociado == OAssociadoInstituicao.idAssociado);
            query = query.Where(x => x.idInstituicao == OAssociadoInstituicao.idInstituicao);

            var Item = query.FirstOrDefault();

            return (Item != null);
        }

        //Realizar os tratamentos necessários
        //Salvar um novo registro
        public UtilRetorno salvarLote(List<int> idsInstituicao, int idAssociado) {

            idsInstituicao = idsInstituicao ?? new List<int>();

            var idsInstituicaoDb = this.listar(idAssociado, true).Select(x => x.idInstituicao).ToList();

            var idsInstituicaoAdd = idsInstituicao.Where(x => !idsInstituicaoDb.Contains(x)).ToList();

            var idsInstituicaoDel = idsInstituicaoDb.Where(x => !idsInstituicao.Contains(x)).ToList();

            if (idsInstituicaoDel.Any()) {

                var idUser = User.id();

                db.AssociadoInstituicao
                    .Where(x => idsInstituicaoDel.Contains(x.idInstituicao) && x.idAssociado == idAssociado)
                    .Update(x => new AssociadoInstituicao() {
                        dtExclusao = DateTime.Now,
                        idUsuarioExclusao = idUser,
                        motivoExclusao = "Inclusão de novos itens"
                    });
            }

            if (idsInstituicaoAdd.Any()) {

                var listaAssociadoInstituicao = new List<AssociadoInstituicao>();

                foreach (var idTipoAssociado in idsInstituicaoAdd) {
                    listaAssociadoInstituicao.Add(new AssociadoInstituicao() { idInstituicao = idTipoAssociado, idAssociado = idAssociado });
                }

                listaAssociadoInstituicao.ForEach(Item => Item.setDefaultInsertValues());

                db.AssociadoInstituicao.AddRange(listaAssociadoInstituicao);
                db.SaveChanges();
            }

            return UtilRetorno.newInstance(false);
        }

        //Definir se é um insert ou update e enviar o registro para o banco de dados 
        public bool salvar(AssociadoInstituicao OAssociadoInstituicao) {

            OAssociadoInstituicao.Instituicao = null;

            OAssociadoInstituicao.observacao1 = OAssociadoInstituicao.observacao1.abreviar(100);

            OAssociadoInstituicao.observacao2 = OAssociadoInstituicao.observacao2.abreviar(100);

            if (OAssociadoInstituicao.id == 0) {
                return this.inserir(OAssociadoInstituicao);
            }

            return this.atualizar(OAssociadoInstituicao);
        }

        //Persistir e inserir um novo registro 
        private bool inserir(AssociadoInstituicao OAssociadoInstituicao) {

            OAssociadoInstituicao.setDefaultInsertValues();

            db.AssociadoInstituicao.Add(OAssociadoInstituicao);

            db.SaveChanges();

            return OAssociadoInstituicao.id > 0;
        }

        //Persistir e atualizar um registro existente 
        private bool atualizar(AssociadoInstituicao OAssociadoInstituicao) {

            //Localizar existentes no banco
            AssociadoInstituicao dbInstituicao = this.carregar(OAssociadoInstituicao.id);

            if (dbInstituicao == null) {
                return false;
            }

            //Configurar valores padrão
            OAssociadoInstituicao.setDefaultUpdateValues();

            //Atualização da Empresa
            var InstituicaoEntry = db.Entry(dbInstituicao);
            InstituicaoEntry.CurrentValues.SetValues(OAssociadoInstituicao);

            InstituicaoEntry.ignoreFields(new[] { "idAssociado", "idInstituicao", "idOrganizacao", "ativo" });

            db.SaveChanges();

            return OAssociadoInstituicao.id > 0;
        }

        //Remover um registro logicamente
        public UtilRetorno excluir(int idInstituicao, int idAssociado) {

            var idUsuario = User.id();

            db.AssociadoInstituicao
                    .Where(x => x.idInstituicao == idInstituicao && x.idAssociado == idAssociado)
                    .Update(x => new AssociadoInstituicao() { dtExclusao = DateTime.Now, idUsuarioExclusao = idUsuario });

            UtilRetorno Retorno = UtilRetorno.getInstance();

            Retorno.flagError = false;

            return Retorno;
        }

        //Remover um registro logicamente
        public UtilRetorno excluirPorId(int id) {

            var idUsuario = User.id();

            db.AssociadoInstituicao.Where(x => x.id == id)
                    .Update(x => new AssociadoInstituicao() { dtExclusao = DateTime.Now, idUsuarioExclusao = idUsuario });

            UtilRetorno Retorno = UtilRetorno.getInstance();

            Retorno.flagError = false;

            return Retorno;
        }
    }
}