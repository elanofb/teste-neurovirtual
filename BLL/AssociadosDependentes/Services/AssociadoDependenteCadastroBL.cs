using System;
using System.Data.Entity;
using System.Linq;
using BLL.Core.Events;
using DAL.Associados;
using DAL.Pessoas;
using BLL.AssociadosDependentes.Events;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.AssociadosDependentes {

    public class AssociadoDependenteCadastroBL : DefaultBL, IAssociadoDependenteCadastroBL {



        //Events
        private readonly EventAggregator onAssociadoDependenteCadastrado = OnAssociadoDependenteCadastrado.getInstance;

        //
        public AssociadoDependenteCadastroBL() {

            this.onAssociadoDependenteCadastrado.subscribe(new OnAssociadoDependenteCadastradoHandler());
        }

        /// <summary>
        /// Realizar tratamentos, limpeza e persistências de dados
        /// Fazer o hub para enviar para atualização ou inserção de um novo registro 
        /// </summary>
        public Associado salvar(Associado ODependenteAssociado) {

            ODependenteAssociado.idTipoCadastro = AssociadoTipoCadastroConst.CONSUMIDOR;

            ODependenteAssociado.Pessoa.limparAtributos();

            ODependenteAssociado.TipoAssociado = null;

            if (ODependenteAssociado.id > 0) {
                this.atualizar(ODependenteAssociado);
                return ODependenteAssociado;
            }

            bool flagSalvo = this.inserir(ODependenteAssociado);

            if (flagSalvo) {
                this.onAssociadoDependenteCadastrado.publish((ODependenteAssociado as object));
            }

            return ODependenteAssociado;
        }

        //Inserir os dados para um novo associado
        //Gerar uma senha randômica para enviar para o cadastro do novo associado
        private bool inserir(Associado ODependenteAssociado) {

            ODependenteAssociado.setDefaultInsertValues();

            ODependenteAssociado.Pessoa.setDefaultInsertValues();

            ODependenteAssociado.Pessoa.listaEnderecos.ToList().ForEach(e => { e.setDefaultInsertValues(); });

            ODependenteAssociado.Pessoa.listaEmails.ToList().ForEach(e => { e.setDefaultInsertValues(); });

            ODependenteAssociado.Pessoa.listaTelefones.ToList().ForEach(e => { e.setDefaultInsertValues(); });

            ODependenteAssociado.idTipoAssociado = ODependenteAssociado.idTipoAssociado.toInt();

            ODependenteAssociado.nroAssociado = this.proximoId();

            ODependenteAssociado.idUnidade = User.idUnidade() > 0 ? User.idUnidade() : ODependenteAssociado.idUnidade;

            string senha = UtilString.randomString(8);

            ODependenteAssociado.Pessoa.senha = UtilCrypt.SHA512(senha);

            db.Associado.Add(ODependenteAssociado);

            db.SaveChanges();

            return (ODependenteAssociado.id > 0);
        }

        //Atualizar os dados de um associado e os objetos relacionados
        private bool atualizar(Associado ODependenteAssociado) {

            Associado dbDependenteAssociado = this.db.Associado.FirstOrDefault(x => x.id == ODependenteAssociado.id);

            if (dbDependenteAssociado == null) {
                return false;
            }

            var entryDependenteAssociado = db.Entry(dbDependenteAssociado);
            ODependenteAssociado.setDefaultUpdateValues();
            entryDependenteAssociado.CurrentValues.SetValues(ODependenteAssociado);
            entryDependenteAssociado.State = EntityState.Modified;
            entryDependenteAssociado.ignoreFields(new[] { "idOrganizacao", "idUnidade", "idAssociadoEstipulante", "nroAssociado", "idTipoAssociado", "idPessoa", "idTipoCadastro", "flagSituacaoContribuicao", "dtAdmissao", "idUsuarioAdmissao", "dtDesativacao", "idUsuarioDesativacao", "dtReativacao", "idUsuarioReativacao", "dtExclusao", "idUsuarioExclusao", "idMeioDivulgacao", "idContribuicaoPadrao", "ativo" });

            var entryPessoa = db.Entry(dbDependenteAssociado.Pessoa);
            ODependenteAssociado.Pessoa.setDefaultUpdateValues();
            ODependenteAssociado.Pessoa.id = dbDependenteAssociado.Pessoa.id;
            ODependenteAssociado.Pessoa.idUsuarioAlteracao = UtilNumber.toInt32(ODependenteAssociado.idUsuarioAlteracao);
            entryPessoa.CurrentValues.SetValues(ODependenteAssociado.Pessoa);
            entryPessoa.State = EntityState.Modified;
            entryPessoa.ignoreFields(new[] { "flagTipoPessoa", "ativo", "senha" });

            this.atualizarEnderecos(ODependenteAssociado, dbDependenteAssociado);

            this.atualizarEmails(ODependenteAssociado, dbDependenteAssociado);

            this.atualizarTelefones(ODependenteAssociado, dbDependenteAssociado);

            db.SaveChanges();

            return (ODependenteAssociado.id > 0);
        }

                //Verificar se já existe um registro para evitar duplicidades
        protected int proximoId() {

            int nroProximoId = db.Associado.Max(x => x.nroAssociado) ?? 0;

            if (nroProximoId == 0) {
                return 1;
            }

            nroProximoId = nroProximoId + 1;

            return nroProximoId;
        }

        /// <summary>
        /// Remover endereços do associado e remover anteriores
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
    }
}