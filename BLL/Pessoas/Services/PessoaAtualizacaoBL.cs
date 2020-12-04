using System;
using System.Data.Entity;
using System.Linq;
using DAL.Pessoas;
using EntityFramework.Extensions;
using BLL.Services;
using DAL.Permissao.Security.Extensions;

namespace BLL.Pessoas {

    public class PessoaAtualizacaoBL : DefaultBL, IPessoaAtualizacaoBL {

        //
        public PessoaAtualizacaoBL() {
            
        }

        //
        public void atualizarListas(Pessoa OPessoaAtualizacao, Pessoa dbPessoa) {

            if (OPessoaAtualizacao.listaEnderecos?.Any() == true || dbPessoa.listaEnderecos?.Any() == true) {
                this.atualizarEnderecos(OPessoaAtualizacao, dbPessoa);
            }
            
            if (OPessoaAtualizacao.listaEmails?.Any() == true || dbPessoa.listaEmails?.Any() == true) {
                this.atualizarEmails(OPessoaAtualizacao, dbPessoa);
            }
            
            if (OPessoaAtualizacao.listaTelefones?.Any() == true || dbPessoa.listaTelefones?.Any() == true) {
                this.atualizarTelefones(OPessoaAtualizacao, dbPessoa);
            }

        }

        /// <summary>
        /// Remover endereços do associado e remover anteriores
        /// </summary>
        private void atualizarEnderecos(Pessoa OPessoaAtualizacao, Pessoa dbPessoa) {

            this.db.PessoaEndereco.Where(x => x.idPessoa == dbPessoa.id && x.dtExclusao == null)
                                  .Update(x => new PessoaEndereco {
                                      dtExclusao = DateTime.Now,
                                      idUsuarioExclusao = User.id()
                                  });

            if (OPessoaAtualizacao.listaEnderecos == null) {
                return;
            }

            foreach (var OPessoaEndereco in OPessoaAtualizacao.listaEnderecos) {

                OPessoaEndereco.idPessoa = dbPessoa.id;
                OPessoaEndereco.setDefaultInsertValues();
                db.PessoaEndereco.Add(OPessoaEndereco);
                db.SaveChanges();

            }

        }
        
        /// <summary>
        /// Remover e-mails anteriores e adicionar novos emails do associado
        /// </summary>
        private void atualizarEmails(Pessoa OPessoaAtualizacao, Pessoa dbPessoa) {
            
            this.db.PessoaEmail.Where(x => x.idPessoa == dbPessoa.id && x.dtExclusao == null)
                               .Update(x => new PessoaEmail {
                               	   dtExclusao = DateTime.Now,
                                   idUsuarioExclusao = User.id(),
                                   motivoExclusao = "Alteração de cadastro"
                               });

            if (OPessoaAtualizacao.listaEmails == null) {
                return;
            }
            
            foreach (var OPessoaEmail in OPessoaAtualizacao.listaEmails) {

                OPessoaEmail.idPessoa = dbPessoa.id;
	            
                OPessoaEmail.setDefaultInsertValues();
	            
                db.PessoaEmail.Add(OPessoaEmail);
	            
                db.SaveChanges();

            }
        }

        /// <summary>
        /// Remover contatos anteriores e adicionar os novos
        /// </summary>
        private void atualizarTelefones(Pessoa OPessoaAtualizacao, Pessoa dbPessoa) {

            this.db.PessoaTelefone.Where(x => x.idPessoa == dbPessoa.id && x.dtExclusao == null)
                                  .Update(x => new PessoaTelefone {
                                  	  dtExclusao = DateTime.Now,
                                      idUsuarioExclusao = User.id(),
                                      motivoExclusao = "Alteração de cadastro"
                                  });
            
            if (OPessoaAtualizacao.listaTelefones == null) {
                return;
            }
            
            foreach (var OPessoaTelefone in OPessoaAtualizacao.listaTelefones) {

                var dbTelefone = dbPessoa.listaTelefones.FirstOrDefault(e => e.id == OPessoaTelefone.id);

                if (dbTelefone != null) {
	                
                    var EntryEmail = db.Entry(dbTelefone);
	                
                    OPessoaTelefone.setDefaultUpdateValues();
	                
                    EntryEmail.CurrentValues.SetValues(OPessoaTelefone);
	                
                    EntryEmail.ignoreFields(new[] { "idPessoa" });
	                
                    EntryEmail.State = EntityState.Modified;

                    continue;
                }

                OPessoaTelefone.idPessoa = dbPessoa.id;

                OPessoaTelefone.setDefaultInsertValues();

                db.PessoaTelefone.Add(OPessoaTelefone);

                db.SaveChanges();

            }
	        
        }
        
    }
    
}