using System;
using System.Data.Entity;
using System.Linq;
using BLL.Associados;
using BLL.Core.Events;
using BLL.Services;
using DAL.Associados;
using EntityFramework.Extensions;
using BLL.NaoAssociados.Events;
using BLL.Pessoas;
using DAL.Permissao.Security.Extensions;
using DAL.Pessoas;
using DAL.Relacionamentos;

namespace BLL.NaoAssociados {

    public class NaoAssociadoCadastroBL : DefaultBL, INaoAssociadoCadastroBL {
        
        //Atributos
        private IAssociadoConsultaBL _AssociadoConsultaBL;
        private IPessoaRelacionamentoBL _PessoaRelacionamentoBL;

        //Propriedades
        private IAssociadoConsultaBL OAssociadoConsultaBL => _AssociadoConsultaBL = _AssociadoConsultaBL ?? new AssociadoConsultaBL();
        private IPessoaRelacionamentoBL OPessoaRelacionamentoBL => _PessoaRelacionamentoBL = _PessoaRelacionamentoBL ?? new PessoaRelacionamentoBL();

        //Events
        private readonly EventAggregator onNaoAssociadoCadastro = OnNaoAssociadoCadastrado.getInstance;


        //Realizar tratamentos, limpeza e persistências de dados
        //Fazer o hub para enviar para atualização ou inserção de um novo registro
        public Associado salvar(Associado OAssociado) {

            OAssociado.idTipoCadastro = AssociadoTipoCadastroConst.COMERCIANTE;

            OAssociado.idTipoAssociado = 1;

            OAssociado.Pessoa.limparAtributos();

            OAssociado.TipoAssociado = null;
                       
            OAssociado.rotaConta = UtilString.onlyUrlChars(OAssociado.rotaConta.abreviar(20).stringOrEmptyLower());

            if (OAssociado.id > 0) {
                this.atualizar(OAssociado);

                return OAssociado;
            }

	        bool flagSalvo = this.inserir(OAssociado);

	        if (flagSalvo) {
                this.onNaoAssociadoCadastro.subscribe(new OnNaoAssociadoCadastradoHandler());
		        this.onNaoAssociadoCadastro.publish((OAssociado as object));
	        }

	        return OAssociado;
        }

        //Inserir os dados para um novo associado
        //Gerar uma senha randômica para enviar para o cadastro do novo associado
        private bool inserir(Associado OAssociado) {

            OAssociado.setDefaultInsertValues();

            OAssociado.Pessoa.setDefaultInsertValues();

            OAssociado.Pessoa.listaEnderecos?.ToList().ForEach(e => { e.setDefaultInsertValues(); });

            OAssociado.Pessoa.listaEmails?.ToList().ForEach(e => { e.setDefaultInsertValues(); });

            OAssociado.Pessoa.listaTelefones?.ToList().ForEach(e => { e.setDefaultInsertValues(); });
            
            OAssociado.idTipoAssociado = UtilNumber.toInt32(OAssociado.idTipoAssociado);

            OAssociado.nroAssociado = !OAssociado.nroAssociado.isEmpty() ? OAssociado.nroAssociado : this.proximoId();

            OAssociado.idUnidade = User.idUnidade() > 0 ? User.idUnidade() : OAssociado.idUnidade;
            
            OAssociado.nroDocumentoIndicador = UtilString.onlyNumber(OAssociado.nroDocumentoIndicador);

            string senha = OAssociado.Pessoa.senha.isEmpty()? UtilString.randomString(8): OAssociado.Pessoa.senha;
            string senhaTransacao = OAssociado.senhaTransacao.isEmpty()? UtilString.randomString(8): OAssociado.senhaTransacao;

            OAssociado.Pessoa.senha = UtilCrypt.SHA512(senha);
            OAssociado.senhaTransacao = UtilCrypt.SHA512(senhaTransacao);
            
            if (!OAssociado.codigoIndicador.isEmpty()){
                
                Associado Indicador = this.OAssociadoConsultaBL.queryNoFilter(OAssociado.idOrganizacao).Select(x => new{
                    x.id,
                    x.idPessoa,
                    x.idIndicador,
                    x.idIndicadorSegundoNivel,
                    x.idIndicadorTerceiroNivel,
                    x.rotaConta,
                    Pessoa = new{
                        x.Pessoa.nroDocumento
                    }
                })
                .FirstOrDefault(x => x.rotaConta == OAssociado.codigoIndicador)
                .ToJsonObject<Associado>() ?? new Associado();                                
                
                OAssociado.idIndicador = Indicador.id;
                OAssociado.idIndicadorSegundoNivel = Indicador.idIndicador;
                OAssociado.idIndicadorTerceiroNivel = Indicador.idIndicadorSegundoNivel;
                
            }

            db.Associado.Add(OAssociado);
            db.SaveChanges();

            return (OAssociado.id > 0);
        }

        //Atualizar os dados de um associado e os objetos relacionados
        private bool atualizar(Associado OAssociado) {

            Associado dbAssociado = this.db.Associado.condicoesSeguranca()
                                    .FirstOrDefault(x => x.id == OAssociado.id && x.idTipoCadastro == AssociadoTipoCadastroConst.COMERCIANTE);

            if (dbAssociado == null) {
                return false;
            }
            
            Associado dbAssociadoData = dbAssociado.ToJsonObject<Associado>();
            
            var entryAssociado = db.Entry(dbAssociado);
            OAssociado.setDefaultUpdateValues();
            entryAssociado.CurrentValues.SetValues(OAssociado);
            entryAssociado.State = EntityState.Modified;
            entryAssociado.ignoreFields(new[] { "idOrigem", "idOrganizacao", "idUnidade", "nroAssociado", "idTipoAssociado", "idPessoa", "idTipoCadastro", "ativo", "dtAdmissao", "idUsuarioAdmissao", "dtDesativacao", "idUsuarioDesativacao", "observacaoDesativacao", "dtReativacao", "idUsuarioReativacao", "dtExclusao", "idUsuarioExclusao", "idMotivoDesligamento", "observacaoDesligamento", "idMeioDivulgacao", "codigoIndicador", "nroDocumentoIndicador", "idIndicador", "idIndicadorSegundoNivel", "idIndicadorTerceiroNivel", "senhaTransacao"});
            
            var entryPessoa = db.Entry(dbAssociado.Pessoa);
            OAssociado.Pessoa.setDefaultUpdateValues();
            OAssociado.Pessoa.id = dbAssociado.Pessoa.id;
			OAssociado.Pessoa.idUsuarioAlteracao = UtilNumber.toInt32(OAssociado.idUsuarioAlteracao);
            entryPessoa.CurrentValues.SetValues(OAssociado.Pessoa);
            entryPessoa.State = EntityState.Modified;
            entryPessoa.ignoreFields(new[] { "idOrganizacao", "idUnidade", "nroDocumento", "flagTipoPessoa", "ativo", "senha" });

            this.atualizarEnderecos(OAssociado, dbAssociado);

            this.atualizarEmails(OAssociado, dbAssociado);

            this.atualizarTelefones(OAssociado, dbAssociado);
            
            this.registarHistorico(OAssociado, dbAssociado);
            
            this.registarHistorico(OAssociado, dbAssociadoData);
            
            db.SaveChanges();

            return (OAssociado.id > 0);
        }

		//Verificar se já existe um registro para evitar duplicidades
		private int proximoId() {

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
        private void atualizarEnderecos(Associado OAssociado, Associado dbAssociado) {

            this.db.PessoaEndereco.Where(x => x.idPessoa == dbAssociado.idPessoa && x.dtExclusao == null).Update(x => new PessoaEndereco {
                dtExclusao = DateTime.Now,
                idUsuarioExclusao = User.id(),
                dtAlteracao = DateTime.Now,
                idUsuarioAlteracao = User.id()
            });


            if (OAssociado.Pessoa.listaEnderecos != null){
                
                foreach (var OPessoaEndereco in OAssociado.Pessoa.listaEnderecos) {
                    OPessoaEndereco.idPessoa = dbAssociado.idPessoa;
                    OPessoaEndereco.setDefaultInsertValues();
                    db.PessoaEndereco.Add(OPessoaEndereco);
                    db.SaveChanges();
                }
                
            }

            
        }

        /// <summary>
        /// Remover e-mails anteriores e adicionar novos e-mails do associado
        /// </summary>
        private void atualizarEmails(Associado OAssociado, Associado dbAssociado) {

            this.db.PessoaEmail.Where(x => x.idPessoa == dbAssociado.idPessoa && x.dtExclusao == null).Update( x => new PessoaEmail {
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
        private void atualizarTelefones(Associado OAssociado, Associado dbAssociado) {


            this.db.PessoaTelefone.Where(x => x.idPessoa == dbAssociado.idPessoa && x.dtExclusao == null).Update(x => new PessoaTelefone {
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
                }

                OPessoaTelefone.idPessoa = dbAssociado.idPessoa;
                OPessoaTelefone.setDefaultInsertValues();

                db.PessoaTelefone.Add(OPessoaTelefone);
                db.SaveChanges();
            }
        }
        
        /// <summary>
        /// Registrar histórico de alterações de campos importantes
        /// </summary>
        private void registarHistorico(Associado OAssociado, Associado dbAssociado) {
            
            if (OAssociado.percentualDesconto == dbAssociado.percentualDesconto){
                return;
            }
            
            var Ocorrencia = new PessoaRelacionamento();

            Ocorrencia.dtOcorrencia = DateTime.Now;

            Ocorrencia.idPessoa = dbAssociado.idPessoa;

            Ocorrencia.idOcorrenciaRelacionamento = OcorrenciaRelacionamentoConst.idAlteracaoCadastro;
            
            Ocorrencia.observacao = $"Alteração do percentual de desconto de { dbAssociado.percentualDesconto } para { OAssociado.percentualDesconto }";

            this.OPessoaRelacionamentoBL.salvar(Ocorrencia);
            
        }

    }
}