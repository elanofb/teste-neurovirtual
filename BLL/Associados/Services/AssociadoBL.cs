using System;
using System.Data.Entity;
using System.Linq;

using BLL.Services;

using DAL.Associados;
using DAL.Emails;
using DAL.Pessoas;

using EntityFramework.Extensions;

namespace BLL.Associados {

    public class AssociadoBL : DefaultBL, IAssociadoBL {
        public static string[] ignoreUpdateFields = {
            "idOrigem", "idOrganizacao", "idUnidade", "nroAssociado", 
            "idTipoAssociado", "idPessoa", "idTipoCadastro", "ativo", 
            "dtAdmissao", "idUsuarioAdmissao", "dtDesativacao", 
            "idUsuarioDesativacao", "observacaoDesativacao", 
            "dtReativacao", "idUsuarioReativacao", "dtExclusao", 
            "idUsuarioExclusao", "idMotivoDesligamento", "observacaoDesligamento", 
            "idMeioDivulgacao", "nroDocumentoIndicador", "codigoIndicador", 
            "senhaTransacao", "idIndicador", "idIndicadorSegundoNivel", 
            "idIndicadorTerceiroNivel", "idUsuarioAprovacaoDocumento"
        };
        
        //Carregar o associado fazendo join com as tabelas necessárias através do ID
        public IQueryable<Associado> query(int? idOrganizacaoParam = null) {
            var query = from Ass in db.Associado.Include(x => x.Pessoa)
                        where Ass.idTipoCadastro == AssociadoTipoCadastroConst.CONSUMIDOR && !Ass.dtExclusao.HasValue
                        select Ass;

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;
        }

        //Carregar o associado fazendo join com as tabelas necessárias através do ID
        public Associado carregar(int id) {
            var query = this.query()
                            .condicoesSeguranca()
                            .Include(x => x.Pessoa)
                            .Include(x => x.Indicador)
                            .Include(x => x.Pessoa.CidadeOrigem)
                            .Include(x => x.Pessoa.listaEnderecos)
                            .Include(x => x.Pessoa.listaEmails)
                            .Include(x => x.Pessoa.listaTelefones);

            return query.FirstOrDefault(x => x.id == id);
        }

        //Carregar o associado e os dados complementares através do código de Pessoa
        public Associado carregarAssociadoPessoa(int idPessoa) {
            var query = this.query()
                            .condicoesSeguranca()
                            .Include(x => x.Pessoa)
                            .Include(x => x.Pessoa.CidadeOrigem)
                            .Include(x => x.Pessoa.listaEnderecos)
                            .Include(x => x.Pessoa.listaEmails)
                            .Include(x => x.Pessoa.listaTelefones);

            return query.FirstOrDefault(x => x.idPessoa == idPessoa);
        }

        //Listar os associado considerando os parametros informados
        public IQueryable<Associado> listar(int idTipoAssociado, string flagSituacao, string valorBusca, string ativo) {
            var query = this.query()
                            .condicoesSeguranca()
                            .AsNoTracking()
                            .Include(x => x.Pessoa)
                            .Include(x => x.Pessoa.listaEnderecos)
                            .Include(x => x.Pessoa.listaEmails)
                            .Include(x => x.Pessoa.listaTelefones)
                            .Include(x => x.TipoAssociado)
                            .Include(x => x.TipoAssociado.Categoria);

            if (!String.IsNullOrEmpty(valorBusca)) {
                string valorBuscaSoNumeros = UtilString.onlyNumber(valorBusca);
                int    intValorBusca       = UtilNumber.toInt32(valorBuscaSoNumeros);

                query = query.Where(
                    x => x.id == intValorBusca 
                         || x.Pessoa.nome.Contains(valorBusca) 
                         || x.Pessoa.razaoSocial.Contains(valorBusca) 
                         || (x.Pessoa.nroDocumento == valorBuscaSoNumeros 
                             && !string.IsNullOrEmpty(x.Pessoa.nroDocumento)) 
                         || (x.nroAssociado == intValorBusca && intValorBusca > 0)
                         
                    //  x.Pessoa.listaEmails.Any(y => y.email.Contains(valorBusca))
                );
            }

            if (!String.IsNullOrEmpty(ativo)) {
                query = query.Where(x => x.ativo == ativo);
            }

            if (idTipoAssociado > 0) {
                query = query.Where(x => x.idTipoAssociado == idTipoAssociado);
            }

            return query;
        }

        //Listar Associados para função de autocompletar
        //IdPessoa usado para recarregar um associado que eventualmente foi selecionado antes
        public IQueryable<AssociadoAutoComplete> autocompletar(string valorBusca, int idPessoa) {
            var query = from Asso in db.Associado.Include(x => x.Pessoa).Include(x => x.Pessoa.listaEnderecos)
                        where (Asso.Pessoa.nome.Contains(valorBusca) 
                               || Asso.Pessoa.razaoSocial.Contains(valorBusca) 
                               || Asso.Pessoa.nroDocumento.Contains(valorBusca)) 
                               && (Asso.idTipoCadastro == AssociadoTipoCadastroConst.CONSUMIDOR) 
                               && (Asso.idPessoa == idPessoa || idPessoa == 0) 
                               && (!Asso.dtExclusao.HasValue && Asso.ativo.Equals("S"))
                        select 
                            new AssociadoAutoComplete {
                                value = Asso.Pessoa.nome, 
                                id = Asso.id, 
                                idOrganizacao = Asso.idOrganizacao, 
                                idUnidade = Asso.idUnidade, 
                                idPessoa = Asso.idPessoa, 
                                flagTipoPessoa = Asso.Pessoa.flagTipoPessoa, 
                                label = Asso.Pessoa.nome, 
                                telPrincipal = String.Concat(Asso.Pessoa.dddTelPrincipal, Asso.Pessoa.nroTelPrincipal), 
                                telSecundario = String.Concat(Asso.Pessoa.dddTelSecundario, Asso.Pessoa.nroTelSecundario), 
                                nroDocumento = Asso.Pessoa.nroDocumento, 
                                emailPrincipal = Asso.Pessoa.listaEmails.FirstOrDefault(x => x.idTipoEmail == TipoEmailConst.PESSOAL && x.dtExclusao == null).email,
                                emailSecundario = Asso.Pessoa.listaEmails.FirstOrDefault(x => x.idTipoEmail == TipoEmailConst.COMERCIAL&& x.dtExclusao == null).email, 
                                flagAtivo = Asso.ativo, 
                                flagSituacaoContribuicao = "", 
                                cep = Asso.Pessoa.listaEnderecos.FirstOrDefault(e => (e.idTipoEndereco == Asso.Pessoa.idTipoEnderecoCorrespondencia) || (e.idTipoEndereco == 0)).cep, 
                                numero = Asso.Pessoa.listaEnderecos.FirstOrDefault(e => (e.idTipoEndereco == Asso.Pessoa.idTipoEnderecoCorrespondencia) || (e.idTipoEndereco == 0)).numero, 
                                complemento = Asso.Pessoa.listaEnderecos.FirstOrDefault(e => (e.idTipoEndereco == Asso.Pessoa.idTipoEnderecoCorrespondencia) || (e.idTipoEndereco == 0)).complemento
                            };

            query = query.condicoesSeguranca();

            return query;
        }

        //Verificar se já existe um registro com o documento/email informado, que possua id diferente do informado
        public bool existe(int idTipoDocumento, string nroDocumento, string email, string login, byte idTipoCadastro, int idDesconsiderado, int? idOrganizacaoParam = null) {
            if (idOrganizacao         > 0
                && idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            nroDocumento = UtilString.onlyNumber(nroDocumento);

            var query = from Ass in db.Associado.Include(x => x.Pessoa)
                        where Ass.id != idDesconsiderado && Ass.dtExclusao == null
                        select Ass;

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            if (!String.IsNullOrEmpty(nroDocumento)) {
                query = query.Where(x => x.Pessoa.nroDocumento == nroDocumento && x.Pessoa.idTipoDocumento == idTipoDocumento);
            }

            if (!String.IsNullOrEmpty(email)) {
                query = query.Where(x => x.Pessoa.ToEmailList().Contains(email));
            }

            if (!String.IsNullOrEmpty(login)) {
                query = query.Where(x => x.Pessoa.login == login && !string.IsNullOrEmpty(login));
            }

            if (idTipoCadastro > 0) {
                query = query.Where(x => x.idTipoCadastro == idTipoCadastro);
            }

            var OAssociado = query.FirstOrDefault();

            return (OAssociado != null);
        }

        public bool existeLogin(string login, int id, int? idOrganizacaoParam = null) {
            if (idOrganizacao         > 0
                && idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            var query = from P in db.Associado.Include(x => x.Pessoa)
                        where P.Pessoa.login == login && P.id != id && P.dtExclusao == null
                        select P;

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            var OPessoa = query.FirstOrDefault();

            return (OPessoa != null);
        }

        public bool existeRota(string rota, int id, int? idOrganizacaoParam = null) {
            rota = UtilString.onlyUrlChars(rota?.Trim());

            if (idOrganizacao         > 0
                && idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            var query = from A in db.Associado
                        where A.rotaConta == rota && A.id != id && A.dtExclusao == null
                        select A;

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            var OAssociado = query.FirstOrDefault();

            return (OAssociado != null);
        }

        public bool existeCodigo(string codigo, int id, int? idOrganizacaoParam = null) {
            codigo = codigo?.Trim();

            if (idOrganizacao         > 0
                && idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            var query = from A in db.Associado
                        where A.rotaConta == codigo && A.id != id && A.dtExclusao == null
                        select A;

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            var OAssociado = query.FirstOrDefault();

            return (OAssociado != null);
        }

        public bool cryptSenha(int id, string senhaClean) {
            if (id == 0) {
                return false;
            }

            if (senhaClean.isEmpty()) {
                return false;
            }

            db.Associado.Where(x => id == x.id)
                        .Update(
                            x => new Associado {
                                senhaTransacao = UtilCrypt.SHA512(senhaClean)
                            }
                        );

            return true;
        }
    }

}