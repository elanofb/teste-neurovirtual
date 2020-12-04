using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Pessoas;
using DAL.ProcessosAvaliacoes;
using DAL.Relacionamentos;

namespace BLL.Pessoas {

    public class AtualizadorPessoaProcessoBL : AtualizadorPessoaBL, IAtualizadorPessoaProcessoBL {
        
        //Atributos
        private IPessoaBL _PessoaBL;
        private IPessoaRelacionamentoBL _PessoaRelacionamentoBL;
        
        //Propridades
        private IPessoaBL OPessoaBL => this._PessoaBL = this._PessoaBL ?? new PessoaBL();
        private IPessoaRelacionamentoBL OPessoaRelacionamentoBL => this._PessoaRelacionamentoBL = this._PessoaRelacionamentoBL ?? new PessoaRelacionamentoBL();
        
        public override void atualizar(int idPessoa, object OrigemDados) {

            ProcessoAvaliacaoInscricao OProcessoAvaliacaoInscricao = OrigemDados as ProcessoAvaliacaoInscricao;

            Pessoa OPessoa = OPessoaBL.carregar(idPessoa);

            OPessoa.limparListas();
            
            this.atualizarEnderecos(OPessoa, OProcessoAvaliacaoInscricao);
            
            this.atualizarEmails(OPessoa, OProcessoAvaliacaoInscricao);
            
            this.atualizarTelefones(OPessoa, OProcessoAvaliacaoInscricao);
            
            string ocorrencia = "O cadastro foi atualizado a partir da inscrição no Processo de Tituação \"" + OProcessoAvaliacaoInscricao.ProcessoAvaliacao.titulo + "\".";
                    
            OPessoaRelacionamentoBL.salvar(OPessoa.id, OcorrenciaRelacionamentoConst.idAlteracaoCadastro, 0, ocorrencia);
            
        }

        public void atualizarEnderecos(Pessoa OPessoa, ProcessoAvaliacaoInscricao OProcessoAvaliacaoInscricao) {
            
            var listaEnderecos = new List<PessoaEndereco>();

            string cepInscricao = OProcessoAvaliacaoInscricao.cep;

            if (cepInscricao.isEmpty()) {
                return;
            }

            var OPessoaEndereco = new PessoaEndereco {
                cep = OProcessoAvaliacaoInscricao.cep,
                logradouro = OProcessoAvaliacaoInscricao.logradouro,
                complemento = OProcessoAvaliacaoInscricao.complemento,
                numero = OProcessoAvaliacaoInscricao.numero,
                bairro = OProcessoAvaliacaoInscricao.bairro,
                idEstado = OProcessoAvaliacaoInscricao.idEstado,
                idCidade = OProcessoAvaliacaoInscricao.idCidade
            };
                
            listaEnderecos.Add(OPessoaEndereco);   
            
            this.atualizarEnderecos(OPessoa, listaEnderecos);
            
        }

        public void atualizarEmails(Pessoa OPessoa, ProcessoAvaliacaoInscricao OProcessoAvaliacaoInscricao) {
            
            var listaEmails = new List<PessoaEmail>();

            string emailPrincipal = OProcessoAvaliacaoInscricao.emailPrincipal;
            string emailSecundario = OProcessoAvaliacaoInscricao.emailSecundario;

            if (emailPrincipal.isEmpty() && emailSecundario.isEmpty()) {
                return;
            }

            if (!emailPrincipal.isEmpty()) {
                var OPessoaEmail = new PessoaEmail {
                    email = emailPrincipal
                };
                
                listaEmails.Add(OPessoaEmail);
            }

            if (!emailSecundario.isEmpty()) {
                var OPessoaEmail = new PessoaEmail {
                    email = emailSecundario
                };
                
                listaEmails.Add(OPessoaEmail);
            }
            
            this.atualizarEmails(OPessoa, listaEmails);
        }

        public void atualizarTelefones(Pessoa OPessoa, ProcessoAvaliacaoInscricao OProcessoAvaliacaoInscricao) {

            var listaTelefones = new List<PessoaTelefone>();

            string telPrincipal = OProcessoAvaliacaoInscricao.telPrincipal;
            string telSecundario = OProcessoAvaliacaoInscricao.telSecundario;
            string telTerciario = OProcessoAvaliacaoInscricao.telTerciario;
            
            if (telPrincipal.isEmpty() && telSecundario.isEmpty() && telTerciario.isEmpty()) {
                return;
            }

            if (!telPrincipal.isEmpty()) {
                var OPessoaTelefone = new PessoaTelefone {
                    nroTelefone = telPrincipal
                };
                
                listaTelefones.Add(OPessoaTelefone);
            }

            if (!telSecundario.isEmpty()) {
                var OPessoaTelefone = new PessoaTelefone {
                    nroTelefone = telSecundario
                };
                
                listaTelefones.Add(OPessoaTelefone);
            }

            if (!telTerciario.isEmpty()) {
                var OPessoaTelefone = new PessoaTelefone {
                    nroTelefone = telTerciario
                };
                
                listaTelefones.Add(OPessoaTelefone);
            }
            
            this.atualizarTelefones(OPessoa, listaTelefones);
            
        }
        
    }
}