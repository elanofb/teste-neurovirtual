using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BLL.Services;
using DAL.Associados;
using EntityFramework.Extensions;

namespace BLL.Membros {
    
    public class MembroSaldoCadastroBL : DefaultBL, IMembroSaldoCadastroBL {
        

        /// <summary>
        /// Verificar se deve-se atualizar um registro existente ou criar um novo
        /// </summary>
        public bool salvar(MembroSaldo OMembroSaldo) {

            if (OMembroSaldo.idPessoa.toInt() == 0) {

                OMembroSaldo.idPessoa = null;

            }

            if (OMembroSaldo.id == 0) {
                
                return this.inserir(OMembroSaldo);
            }

            return this.atualizar(OMembroSaldo);
        }


        //Persistir o objecto e salvar na base de dados
        private bool inserir(MembroSaldo OMembroSaldo) {
            
            OMembroSaldo.setDefaultInsertValues();

            db.MembroSaldo.Add(OMembroSaldo);

            db.SaveChanges();

            return OMembroSaldo.id > 0;
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(MembroSaldo OMembroSaldo) {

            //Localizar existentes no banco
            MembroSaldo dbMembroSaldo = this.db.MembroSaldo.Find(OMembroSaldo.id);

            if (dbMembroSaldo == null) {
                return false;
            }

            OMembroSaldo.setDefaultUpdateValues();

            var TipoEntry = db.Entry(dbMembroSaldo);
            
            TipoEntry.CurrentValues.SetValues(OMembroSaldo);
            
            TipoEntry.ignoreFields();

            db.SaveChanges();
            
            return (OMembroSaldo.id > 0);
        }


        /// <summary>
        /// 
        /// </summary>
        public void atualizarOuInserir(int[] ids, Expression<Func<MembroSaldo, MembroSaldo>> updateExpression) {

            var listaIdsSaldos = db.MembroSaldo.Where(x => ids.Contains(x.idMembro))
                                   .Select(x => x.idMembro)
                                   .ToList();

            this.atualizar(listaIdsSaldos.ToArray(), updateExpression);

            var idsNaoEncontrados = ids.Except(listaIdsSaldos).ToList();

            if (!idsNaoEncontrados.Any()) {
                return;
            }

            this.inserir(idsNaoEncontrados.ToArray());
            
            this.atualizar(idsNaoEncontrados.ToArray(), updateExpression);
            
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void atualizar(int[] ids, Expression<Func<MembroSaldo, MembroSaldo>> updateExpression) {

            if (!ids.Any()) {
                return;
            }

            this.db.MembroSaldo.Where(x => ids.Contains(x.idMembro)).Update(updateExpression);
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void inserir(int[] idsMembros) {

            var listaMembros = db.Associado.Where(x => idsMembros.Contains(x.id))
                                 .Select(x => new {x.id, x.nroAssociado, x.idPessoa})
                                 .ToListJsonObject<Associado>();
            

            if (!listaMembros.Any()) {
                return;
            }            

            var listaSaldos = new List<MembroSaldo>();
            
            foreach (var idMembro in idsMembros) {

                var OMembro = listaMembros.FirstOrDefault(x => x.id == idMembro);

                if (OMembro == null) {
                    continue;
                }
                
                var NovoSaldo = new MembroSaldo();

                NovoSaldo.idOrganizacao = 1;
                
                NovoSaldo.idMembro = idMembro;
                
                NovoSaldo.idPessoa = OMembro.idPessoa;

                NovoSaldo.saldoAtual = new decimal(0);

                listaSaldos.Add(NovoSaldo);
            }
            
            db.MembroSaldo.AddRange(listaSaldos);

            db.SaveChanges();

        }        
    }
}