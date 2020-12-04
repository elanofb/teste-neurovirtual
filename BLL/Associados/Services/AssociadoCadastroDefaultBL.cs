using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Associados;
using DAL.Permissao.Security.Extensions;
using DAL.Pessoas;
using EntityFramework.Extensions;

namespace BLL.Associados {

    public class AssociadoCadastroDefaultBL : DefaultBL {

        /// <summary>
        /// Atualizar lista de dependentes
        /// </summary>
        protected void atualizarDependentes(Associado OAssociado, Associado dbAssociado) {

        }

        
        /// <summary>
        /// Atualizar lista de contatos 
        /// </summary>
        protected void atualizarEnderecos(Associado OAssociado, Associado dbAssociado) {

            this.db.PessoaEndereco.Where(x => x.idPessoa == dbAssociado.idPessoa && x.dtExclusao == null)
                                .Update(
                                    x =>
                                        new PessoaEndereco {
                                            dtExclusao = DateTime.Now,
                                            idUsuarioExclusao = User.id(),
                                            dtAlteracao = DateTime.Now,
                                            idUsuarioAlteracao = User.id()
                                        });

            if (OAssociado.Pessoa.listaEnderecos == null) {
                return;
            }

            foreach (var OPessoaEndereco in OAssociado.Pessoa.listaEnderecos) {

                OPessoaEndereco.idPessoa = dbAssociado.idPessoa;
                OPessoaEndereco.setDefaultInsertValues();
                db.PessoaEndereco.Add(OPessoaEndereco);
                db.SaveChanges();

            }
        }

        /// <summary>
        /// Remover e-mails anteriores e adicionar novos emails do associado
        /// </summary>
        protected void atualizarEmails(Associado OAssociado, Associado dbAssociado) {

            this.db.PessoaEmail.Where(x => x.idPessoa == dbAssociado.idPessoa && x.dtExclusao == null)
                                .Update(
                                    x =>
                                        new PessoaEmail {
                                            dtExclusao = DateTime.Now,
                                            idUsuarioExclusao = User.id(),
                                            dtAlteracao = DateTime.Now,
                                            idUsuarioAlteracao = User.id()
                                        });

            if (OAssociado.Pessoa.listaEmails == null) {
                return;
            }
            
            foreach (var OPessoaEmail in OAssociado.Pessoa.listaEmails) {

                OPessoaEmail.idPessoa = dbAssociado.idPessoa;
                OPessoaEmail.setDefaultInsertValues();
                db.PessoaEmail.Add(OPessoaEmail);
                db.SaveChanges();

            }
        }

        /// <summary>
        /// Remover contatos anteriores e adicionar os novos
        /// </summary>
        protected void atualizarTelefones(Associado OAssociado, Associado dbAssociado) {

            this.db.PessoaTelefone.Where(x => x.idPessoa == dbAssociado.idPessoa && x.dtExclusao == null)
                                .Update(
                                    x =>
                                        new PessoaTelefone {
                                            dtExclusao = DateTime.Now,
                                            idUsuarioExclusao = User.id(),
                                            dtAlteracao = DateTime.Now,
                                            idUsuarioAlteracao = User.id()
                                        });
            
            if (OAssociado.Pessoa.listaTelefones == null) {
                return;
            }
            
            foreach (var OPessoaTelefone in OAssociado.Pessoa.listaTelefones) {

                OPessoaTelefone.idUsuarioAlteracao = OAssociado.idUsuarioAlteracao.toInt();

                var dbTelefone = dbAssociado.Pessoa.listaTelefones.FirstOrDefault(e => e.id == OPessoaTelefone.id);

                if (dbTelefone != null) {
                    var EntryEmail = db.Entry(dbTelefone);
                    OPessoaTelefone.setDefaultUpdateValues();
                    EntryEmail.CurrentValues.SetValues(OPessoaTelefone);
                    EntryEmail.ignoreFields(new[] { "idPessoa" });
                    EntryEmail.State = EntityState.Modified;

                    continue;
                    ;
                }

                OPessoaTelefone.idPessoa = dbAssociado.idPessoa;

                OPessoaTelefone.setDefaultInsertValues();

                db.PessoaTelefone.Add(OPessoaTelefone);

                db.SaveChanges();

            }
        }

        //Verificar se já existe um registro para evitar duplicidades
        protected int proximoId(int idOrganizacaoParam) {

            int nroProximoId = db.Associado.Where(x => x.idOrganizacao == idOrganizacaoParam).Max(x => x.nroAssociado) ?? 0;

            if (nroProximoId == 0) {
                return 1;
            }

            nroProximoId = nroProximoId + 1;

            return nroProximoId;
        }


    }
}