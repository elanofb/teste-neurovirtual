using System;
using System.Data.Entity;
using System.Linq;
using DAL.Financeiro;
using EntityFramework.Extensions;
using System.Collections.Generic;
using BLL.Fornecedores;
using BLL.Funcionarios;
using DAL.Funcionarios;
using System.Json;
using BLL.Pessoas;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using UTIL.Resources;

namespace BLL.Financeiro {

    public class MacroContaBL: DefaultBL, IMacroContaBL {

        public const string keyCache = "macro_conta";

        //Carregamento de registro pelo ID
        public MacroConta carregar(int id) {

            var query = from P in db.MacroConta
                        where P.id == id && P.flagExcluido == false
                        select P;

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();
        }

        //Listagem de registros de acordo com filtros
        public IQueryable<MacroConta> listar(string valorBusca, bool? ativo, int idCentroCusto = 0) {

            var query = from P in db.MacroConta.Include(x => x.UsuarioAprovacao)
                                               .Include(x => x.UsuarioAprovacao.Pessoa)   
                        where P.flagExcluido == false
                        select P;

            query = query.condicoesSeguranca();

            if(!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca));
            }

            if(ativo.HasValue) {
                query = query.Where(x => x.ativo == ativo);
            }

            if (idCentroCusto > 0) {
                query = query.Where(x => db.CentroCustoMacroConta.Any(o => o.idCentroCusto == idCentroCusto && o.idMacroConta == x.id && !o.dtExclusao.HasValue));
            }

            return query;
        }

        //
        public IQueryable<MacroConta> listarPorId(List<int> ids) {

            var query = db.MacroConta.Select(x => x).Where(x => ids.Contains(x.id));

            query = query.condicoesSeguranca();

            return query;
        }

        //Realiza a listagem dos objetos conforme o tipo de refêrencia
        public List<ReferenciaReceitaDTO> listarPorMacroConta(int idMacroConta) {

            List<ReferenciaReceitaDTO> OListaReferenciaReceita = new List<ReferenciaReceitaDTO>();

            switch (idMacroConta) { 

                case (int)MacroContaEnum.ASSOCIACAO:

                    //IFornecedorBL OFornecedorBL = new FornecedorBL();
                    //OListaReferenciaReceita = OFornecedorBL.listar("", "S").OrderBy(x => x.Pessoa.nome).Select(x => new ReferenciaReceitaDTO { id = x.id, nome = x.Pessoa.nome }).ToList();
                    //break;

                case (int)MacroContaEnum.INSCRICAO_EVENTO:

                    IFuncionarioConsultaBL OFuncionarioConsultaBL = new FuncionarioConsultaBL();
                    OListaReferenciaReceita = OFuncionarioConsultaBL.listar("", "S").OrderBy(x => x.Pessoa.nome).Select(x => new ReferenciaReceitaDTO { id = x.id, nome = x.Pessoa.nome }).ToList();
                    break;
            }

            return OListaReferenciaReceita;
        }

        //
        public ReferenciaReceitaDTO getReferenciaReceita(int idMacroConta, int idReferenciaReceita) {

            var OReferenciaReceita = new ReferenciaReceitaDTO();

            switch (idMacroConta) { 

                case (int)MacroContaEnum.ASSOCIACAO:

                    var OFornecedorConsultaBL = new FornecedorConsultaBL();
                    var OFornecedor  = OFornecedorConsultaBL.carregar(idReferenciaReceita);
                    if (OFornecedor != null) {
                        OReferenciaReceita.id = OFornecedor.id;
                        OReferenciaReceita.nome = OFornecedor.Pessoa.nome;
                    }
                    break;

                case (int)MacroContaEnum.INSCRICAO_EVENTO:

                    IFuncionarioConsultaBL OFuncionarioConsultaBL = new FuncionarioConsultaBL();
                    Funcionario OFuncionario  = OFuncionarioConsultaBL.carregar(idReferenciaReceita);
                    if (OFuncionario != null) {
                        OReferenciaReceita.id = OFuncionario.id;
                        OReferenciaReceita.nome = OFuncionario.Pessoa.nome;
                    }
                    break;
            }

            return OReferenciaReceita;
        }

        //Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
        public bool existe(string descricao,int id) {
            
            var query = from P in db.MacroConta
                        where P.descricao == descricao && P.id != id && P.flagExcluido == false
                        select P;

            query = query.condicoesSeguranca();

            var OMacroConta = query.Take(1).FirstOrDefault();
            return (OMacroConta != null);
        }

        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(MacroConta OTipoProduto) {

            if(OTipoProduto.id == 0) {
                return this.inserir(OTipoProduto);
            }

            return this.atualizar(OTipoProduto);
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(MacroConta OMacroConta) {

            OMacroConta.flagSistema = false;

            OMacroConta.setDefaultInsertValues();

            db.MacroConta.Add(OMacroConta);

            db.SaveChanges();

            return (OMacroConta.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(MacroConta OMacroConta) {

            OMacroConta.setDefaultUpdateValues();

            //Localizar existentes no banco
            MacroConta dbMacroConta = this.carregar(OMacroConta.id);

            if (dbMacroConta == null) {
                return false;
            }

            var TipoEntry = db.Entry(dbMacroConta);
            TipoEntry.CurrentValues.SetValues(OMacroConta);
            TipoEntry.ignoreFields();

            db.SaveChanges();
            return (OMacroConta.id > 0);
        }

        //Remover um registro (exclusao lógica - nao remove-se fisicamente)
        public bool excluir(int id) {

            var idUsuario = User.id();

            var query = db.MacroConta.Where(x => x.id == id);

            query = query.condicoesSeguranca();

            query.Update(x => new MacroConta { flagExcluido = true, dtAlteracao = DateTime.Now,idUsuarioAlteracao = idUsuario });

            return true;
        }

        public JsonMessageStatus alterarStatus(int id) {

            var retorno = new JsonMessageStatus();

            var Objeto = this.carregar(id);
            if (Objeto == null) {
                retorno.error = true;
                retorno.message = NotificationMessages.invalid_register_id;
            } else {

                Objeto.ativo = Objeto.ativo != true;

                db.SaveChanges();

                retorno.active = Objeto.ativo == true? "S": "N";

                retorno.message = "Os dados foram alterados com sucesso.";
            }
            return retorno;
        }
    }
}