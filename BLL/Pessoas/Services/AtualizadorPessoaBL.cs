using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Pessoas;
using BLL.Services;
using DAL.Emails;
using DAL.Enderecos;
using DAL.Permissao.Security.Extensions;
using DAL.Telefones;
using EntityFramework.Extensions;

namespace BLL.Pessoas {

    public abstract class AtualizadorPessoaBL : DefaultBL {
        
        public abstract void atualizar(int idPessoa, object OrigemDados);

        //Atributos
        private IPessoaEnderecoCadastroBL _PessoaEnderecoCadastroBL;
        private IPessoaEnderecoExclusaoBL _PessoaEnderecoExclusaoBL;
        private IPessoaEmailCadastroBL _PessoaEmailCadastroBL;
        private IPessoaEmailExclusaoBL _PessoaEmailExclusaoBL;
        private IPessoaTelefoneCadastroBL _PessoaTelefoneCadastroBL;
        private IPessoaTelefoneExclusaoBL _PessoaTelefoneExclusaoBL;
        
        //Propridades
        private IPessoaEnderecoCadastroBL OPessoaEnderecoCadastroBL => this._PessoaEnderecoCadastroBL = this._PessoaEnderecoCadastroBL ?? new PessoaEnderecoCadastroBL();
        private IPessoaEnderecoExclusaoBL OPessoaEnderecoExclusaoBL => this._PessoaEnderecoExclusaoBL = this._PessoaEnderecoExclusaoBL ?? new PessoaEnderecoExclusaoBL();
        private IPessoaEmailCadastroBL OPessoaEmailCadastroBL => this._PessoaEmailCadastroBL = this._PessoaEmailCadastroBL ?? new PessoaEmailCadastroBL();
        private IPessoaEmailExclusaoBL OPessoaEmailExclusaoBL => this._PessoaEmailExclusaoBL = this._PessoaEmailExclusaoBL ?? new PessoaEmailExclusaoBL();
        private IPessoaTelefoneCadastroBL OPessoaTelefoneCadastroBL => this._PessoaTelefoneCadastroBL = this._PessoaTelefoneCadastroBL ?? new PessoaTelefoneCadastroBL();
        private IPessoaTelefoneExclusaoBL OPessoaTelefoneExclusaoBL => this._PessoaTelefoneExclusaoBL = this._PessoaTelefoneExclusaoBL ?? new PessoaTelefoneExclusaoBL();
        
        /// <summary>
        /// 
        /// </summary>
        protected virtual void atualizarEnderecos(Pessoa OPessoa, List<PessoaEndereco> listaEnderecosNovos) {

            if (!listaEnderecosNovos.Any() || OPessoa == null) {
                return;
            }

            var listaEnderecoPessoa = OPessoa.listaEnderecos;

            var OEndereco = listaEnderecoPessoa.FirstOrDefault() ?? new PessoaEndereco();
            
            byte idTipoEndereco = OEndereco.idTipoEndereco.toByte() > 0? OEndereco.idTipoEndereco.toByte(): TipoEnderecoConst.PRINCIPAL;
                
            listaEnderecosNovos.ForEach(x => { x.idTipoEndereco = idTipoEndereco; });
            
            if (listaEnderecoPessoa.Count > 1) {

                var SegundoEndereco = listaEnderecoPessoa.Skip(1).FirstOrDefault() ?? new PessoaEndereco();
                
                SegundoEndereco.idTipoEndereco = SegundoEndereco.idTipoEndereco.toByte() > 0? SegundoEndereco.idTipoEndereco.toByte(): TipoEnderecoConst.PRINCIPAL;
            
                listaEnderecosNovos.Skip(1).ToList().ForEach(x => { x.idTipoEndereco = SegundoEndereco.idTipoEndereco; });
                
            }

            if (listaEnderecoPessoa.Count == listaEnderecosNovos.Count) {
                
                //Exlusao
                OPessoaEnderecoExclusaoBL.excluirPorPessoa(OPessoa.id);
                
            }

            if (listaEnderecoPessoa.Count > listaEnderecosNovos.Count) {

                int cont = 0;
                
                foreach (var OEnderecoExcluir in listaEnderecoPessoa) {

                    if (cont == listaEnderecosNovos.Count) {
                        break;
                    }
                    
                    //Exlusao
                    OPessoaEnderecoExclusaoBL.excluir(OEnderecoExcluir.id);

                    cont++;
                }
                
            }

            foreach (var OPessoaEndereco in listaEnderecosNovos) {

                //Cadastro
                OPessoaEndereco.idPessoa = OPessoa.id;
                OPessoaEnderecoCadastroBL.salvar(OPessoaEndereco);

            }
        }
        
        protected virtual void atualizarEmails(Pessoa OPessoa, List<PessoaEmail> listaEmailsNovos) {

            if (!listaEmailsNovos.Any() || OPessoa == null) {
                return;
            }

            var listaEmailPessoa = OPessoa.listaEmails;

            var OEmail = listaEmailPessoa.FirstOrDefault() ?? new PessoaEmail();
            
            byte idTipoEmail = OEmail.idTipoEmail.toByte() > 0? OEmail.idTipoEmail.toByte(): TipoEmailConst.PESSOAL;
                
            listaEmailsNovos.ForEach(x => { x.idTipoEmail = idTipoEmail; });
            
            if (listaEmailPessoa.Count > 1) {

                var SegundoEmail = listaEmailPessoa.Skip(1).FirstOrDefault() ?? new PessoaEmail();
                
                SegundoEmail.idTipoEmail = SegundoEmail.idTipoEmail.toByte() > 0? SegundoEmail.idTipoEmail.toByte(): TipoEmailConst.PESSOAL;
            
                listaEmailsNovos.Skip(1).ToList().ForEach(x => { x.idTipoEmail = SegundoEmail.idTipoEmail; });
                
            }

            if (listaEmailPessoa.Count == listaEmailsNovos.Count) {
                
                //Exlusao
                OPessoaEmailExclusaoBL.excluirPorPessoa(OPessoa.id);
                
            }

            if (listaEmailPessoa.Count > listaEmailsNovos.Count) {

                int cont = 0;
                
                foreach (var OEmailExcluir in listaEmailPessoa) {

                    if (cont == listaEmailsNovos.Count) {
                        break;
                    }
                    
                    //Exlusao
                    OPessoaEmailExclusaoBL.excluir(OEmailExcluir.id);

                    cont++;
                }
                
            }

            foreach (var OPessoaEmail in listaEmailsNovos) {

                //Cadastro
                OPessoaEmail.idPessoa = OPessoa.id;
                OPessoaEmailCadastroBL.salvar(OPessoaEmail);

            }
            
        }

        protected virtual void atualizarTelefones(Pessoa OPessoa, List<PessoaTelefone> listaTelefonesNovos) {
            
            if (!listaTelefonesNovos.Any() || OPessoa == null) {
                return;
            }

            var listaTelefonePessoa = OPessoa.listaTelefones;

            var OTelefone = listaTelefonePessoa.FirstOrDefault() ?? new PessoaTelefone();
            
            byte idTipoTelefone = OTelefone.idTipoTelefone.toByte() > 0? OTelefone.idTipoTelefone.toByte(): TipoTelefoneConst.PESSOAL;
                
            listaTelefonesNovos.ForEach(x => { x.idTipoTelefone = idTipoTelefone; });
            
            if (listaTelefonePessoa.Count > 1) {

                var SegundoTelefone = listaTelefonePessoa.Skip(1).FirstOrDefault() ?? new PessoaTelefone();
                
                SegundoTelefone.idTipoTelefone = SegundoTelefone.idTipoTelefone.toByte() > 0? SegundoTelefone.idTipoTelefone.toByte(): TipoTelefoneConst.PESSOAL;
            
                listaTelefonesNovos.Skip(1).ToList().ForEach(x => { x.idTipoTelefone = SegundoTelefone.idTipoTelefone; });
                
            }

            if (listaTelefonePessoa.Count == listaTelefonesNovos.Count) {
                
                //Exlusao
                OPessoaTelefoneExclusaoBL.excluirPorPessoa(OPessoa.id);
                
            }

            if (listaTelefonePessoa.Count > listaTelefonesNovos.Count) {

                int cont = 0;
                
                foreach (var OTelefoneExcluir in listaTelefonePessoa) {

                    if (cont == listaTelefonesNovos.Count) {
                        break;
                    }
                    
                    //Exlusao
                    OPessoaTelefoneExclusaoBL.excluir(OTelefoneExcluir.id);

                    cont++;
                }
                
            }

            foreach (var OPessoaTelefone in listaTelefonesNovos) {

                //Cadastro
                OPessoaTelefone.idPessoa = OPessoa.id;
                OPessoaTelefoneCadastroBL.salvar(OPessoaTelefone);

            }
            
        }

    }
}