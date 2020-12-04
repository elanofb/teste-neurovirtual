﻿using System;
using System.Data.Entity;
using System.Linq;
using BLL.Associados.Events;
using BLL.Core.Events;
using BLL.NaoAssociados;
using BLL.Services;
using DAL.Associados;
using DAL.Permissao.Security.Extensions;
using DAL.Pessoas;

namespace BLL.Associados {

    public class AssociadoCadastroBL : AssociadoCadastroDefaultBL, IAssociadoCadastroBL {
        
        //Atributos
        private IAssociadoConsultaBL _AssociadoConsultaBL;        

        //Propriedades
        private IAssociadoConsultaBL OAssociadoConsultaBL => _AssociadoConsultaBL = _AssociadoConsultaBL ?? new AssociadoConsultaBL();        
        
        
        //Events
        private readonly EventAggregator onAssociadoCadastro = OnAssociadoCadastrado.getInstance;
        private readonly EventAggregator onAssociadoAlterado = OnAssociadoAlterado.getInstance;

        //
        public AssociadoCadastroBL() {
            this.onAssociadoCadastro.subscribe(new OnAssociadoCadastradoHandler());
            this.onAssociadoAlterado.subscribe(new OnAssociadoAlteradoHandler());
        }

        /// <summary>
        /// Realizar tratamentos, limpeza e persistências de dados
        /// Fazer o hub para enviar para atualização ou inserção de um novo registro 
        /// </summary>
        public Associado salvar(Associado OAssociado) {

            OAssociado.idTipoCadastro = AssociadoTipoCadastroConst.CONSUMIDOR;
            
            OAssociado.idTipoAssociado = 1;

            OAssociado.limparAtributosDependentes();
            
            OAssociado.Pessoa.limparAtributos();

            OAssociado.TipoAssociado = null;

            OAssociado.observacoes = OAssociado.observacoes.abreviar(1000);
            
            OAssociado.outrasInformacoes = OAssociado.outrasInformacoes.abreviar(1000);
            
            OAssociado.rotaConta = UtilString.onlyUrlChars(OAssociado.rotaConta.abreviar(20).stringOrEmptyLower());
            
            OAssociado.codigoIndicador = OAssociado.codigoIndicador.stringOrEmptyLower();
            
            bool flagSalvo = false;

            if (OAssociado.id > 0) {

                flagSalvo = this.atualizar(OAssociado);
                if (flagSalvo) {
                    this.onAssociadoAlterado.publish((OAssociado as object));
                }
                return OAssociado;
            }

            flagSalvo = this.inserir(OAssociado);

            if (flagSalvo) {
                this.onAssociadoCadastro.publish((OAssociado as object));
            }

            return OAssociado;
        }

        //Inserir os dados para um novo associado
        //Gerar uma senha randômica para enviar para o cadastro do novo associado
        private bool inserir(Associado OAssociado) {

            OAssociado.setDefaultInsertValues();

            OAssociado.Pessoa.setDefaultInsertValues();

            OAssociado.Pessoa.listaEnderecos?.ForEach(e => { e.setDefaultInsertValues(); });

            OAssociado.Pessoa.listaEmails?.ForEach(e => { e.setDefaultInsertValues(); });

            OAssociado.Pessoa.listaTelefones?.ForEach(e => { e.setDefaultInsertValues(); });

            OAssociado.idTipoAssociado = OAssociado.idTipoAssociado.toInt();

            OAssociado.nroAssociado = !OAssociado.nroAssociado.isEmpty() ? OAssociado.nroAssociado : this.proximoId(OAssociado.idOrganizacao);

            OAssociado.nroDocumentoIndicador = UtilString.onlyNumber(OAssociado.nroDocumentoIndicador);

            OAssociado.idUnidade = User.idUnidade() > 0 ? User.idUnidade() : OAssociado.idUnidade;

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

            if (OAssociado.id == 1) {
                return false;
            }

            Associado dbAssociado = this.db.Associado.condicoesSeguranca()
                                    .FirstOrDefault(x => x.id == OAssociado.id && x.idTipoCadastro == AssociadoTipoCadastroConst.CONSUMIDOR);

            if (dbAssociado == null) {
                return false;
            }
            
            var entryAssociado = db.Entry(dbAssociado);
            OAssociado.setDefaultUpdateValues();
            entryAssociado.CurrentValues.SetValues(OAssociado);
            entryAssociado.State = EntityState.Modified;
            entryAssociado.ignoreFields( AssociadoBL.ignoreUpdateFields );
                
            var entryPessoa = db.Entry(dbAssociado.Pessoa);
            OAssociado.Pessoa.setDefaultUpdateValues();
            OAssociado.Pessoa.id = dbAssociado.Pessoa.id;
            OAssociado.Pessoa.idUnidade = OAssociado.idUnidade; 
            OAssociado.Pessoa.idUsuarioAlteracao = UtilNumber.toInt32(OAssociado.idUsuarioAlteracao);
            entryPessoa.CurrentValues.SetValues(OAssociado.Pessoa);
            entryPessoa.State = EntityState.Modified;
            entryPessoa.ignoreFields(new[] { "idOrganizacao", "nroDocumento", "flagTipoPessoa", "ativo", "senha" });
            
            this.atualizarEnderecos(OAssociado, dbAssociado);

            this.atualizarEmails(OAssociado, dbAssociado);

            this.atualizarTelefones(OAssociado, dbAssociado);
            
            this.atualizarDependentes(OAssociado, dbAssociado);
            
            var flagSave = db.SaveChanges();

            return (OAssociado.id > 0);
        }

    }
}