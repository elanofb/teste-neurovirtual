using System;
using System.Data.Entity;
using System.Linq;
using BLL.Configuracoes;
using BLL.Core.Events;
using BLL.Services;
using DAL.Associados;
using EntityFramework.Extensions;
using BLL.NaoAssociadosInstitucional.Events;
using DAL.Documentos;
using MoreLinq;
using BLL.Localizacao;
using DAL.Localizacao;
using DAL.Pessoas;

namespace BLL.NaoAssociadosInstitucional {

    public class NaoAssociadoInstitucionalBL : DefaultBL, INaoAssociadoInstitucionalBL {

        //Constantes

        //Atributos
        private ICidadeBL _CidadeBL;

        //Propriedades
        private ICidadeBL OCidadeBL => this._CidadeBL = this._CidadeBL ?? new CidadeBL();

        //Events
        private readonly EventAggregator onNaoAssociadoCadastro = OnNaoAssociadoInstitucionalCadastrado.getInstance;

        //
        public NaoAssociadoInstitucionalBL() {
            this.onNaoAssociadoCadastro.subscribe(new OnNaoAssociadoInstitucionalCadastradoHandler());
        }

        //Carregar o associado fazendo join com as tabelas necessarias através do ID
        public Associado carregar(int idNaoAssociado) {
            var query = from Ass in db.Associado
                            .Include(x => x.Pessoa)
                            .Include(x => x.Pessoa.CidadeOrigem)
                            .Include(x => x.Pessoa.listaEnderecos)
                        where Ass.id == idNaoAssociado && !Ass.dtExclusao.HasValue
                        select Ass;
            Associado OAssociado = query.FirstOrDefault();
            return OAssociado;
        }

        //Carregar o associado e os dados complementares através do código de Pessoa
        public Associado carregarAssociadoPessoa(int idPessoa) {
            var query = from Ass in db.Associado
                            .Include(x => x.Pessoa)
                            .Include(x => x.Pessoa.CidadeOrigem)
                            .Include(x => x.Pessoa.listaEnderecos)
                        where Ass.idPessoa == idPessoa && !Ass.dtExclusao.HasValue
                        select Ass;
            Associado OAssociado = query.FirstOrDefault();
            return OAssociado;
        }

        //Listar os associado considerando os parametros informados
        public IQueryable<Associado> listar(int idTipoAssociado, string flagSituacao, string valorBusca, string ativo) {

            var query = from Ass in db.Associado.AsNoTracking()
                                .Include(x => x.Pessoa)
                                .Include(x => x.TipoAssociado)
                        where !Ass.dtExclusao.HasValue
                        select Ass;

            if(!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.Pessoa.nome.Contains(valorBusca) || x.Pessoa.nroDocumento.Contains(valorBusca));
            }

            if(!String.IsNullOrEmpty(ativo)) {
                query = query.Where(x => x.ativo == ativo);
            }


            if(idTipoAssociado > 0) {
                query = query.Where(x => x.idTipoAssociado == idTipoAssociado);
            }

            return query;
        }


        //Verificar se já existe um registro com o documento/email informado, que possua id diferente do informado
        public bool existe(int idTipoDocumento, string nroDocumento, string email, string login, int idDesconsiderado) {
            nroDocumento = UtilString.onlyNumber(nroDocumento);

            var query = from Ass in db.Associado.Include(x => x.Pessoa)
                        where Ass.id != idDesconsiderado && !Ass.dtExclusao.HasValue
                        select Ass;

            if(!String.IsNullOrEmpty(nroDocumento)) {
                query = query.Where(x => x.Pessoa.nroDocumento == nroDocumento && x.Pessoa.idTipoDocumento == idTipoDocumento);
            }

            if(!String.IsNullOrEmpty(email))
            {
                query = query.Where(x => x.Pessoa.ToEmailList().Contains(email));
            }

            if(!String.IsNullOrEmpty(login)) {
                query = query.Where(x => x.Pessoa.login == login);
            }

            var OAssociado = query.Take(1).FirstOrDefault();
            return (OAssociado != null);
        }

                //Realizar tratamentos, limpeza e persistências de dados
        //Fazer o hub para enviar para atualização ou inserção de um novo registro
        public Associado salvar(Associado ONaoAssociado) {

            ONaoAssociado.idTipoCadastro = AssociadoTipoCadastroConst.COMERCIANTE;

            ONaoAssociado.idTipoAssociado = ONaoAssociado.idTipoAssociado > 0?  ONaoAssociado.idTipoAssociado : TipoAssociadoConst.NAO_ASSOCIADO;

            //Tratar valores
            ONaoAssociado.Pessoa.nroDocumento = UtilString.onlyAlphaNumber(ONaoAssociado.Pessoa.nroDocumento);

            ONaoAssociado.Pessoa.rg = UtilString.onlyAlphaNumber(ONaoAssociado.Pessoa.rg);

            ONaoAssociado.Pessoa.nroTelPrincipal = UtilString.onlyNumber(ONaoAssociado.Pessoa.nroTelPrincipal);

            if (string.IsNullOrEmpty(ONaoAssociado.Pessoa.dddTelPrincipal) && ONaoAssociado.Pessoa.nroTelPrincipal.Length > 2) {
                ONaoAssociado.Pessoa.dddTelPrincipal = ONaoAssociado.Pessoa.nroTelPrincipal.Substring(0, 2);
                ONaoAssociado.Pessoa.nroTelPrincipal = ONaoAssociado.Pessoa.nroTelPrincipal.Substring( 2);
            }
            
            ONaoAssociado.Pessoa.nroTelSecundario = UtilString.onlyNumber(ONaoAssociado.Pessoa.nroTelSecundario);

            if (string.IsNullOrEmpty(ONaoAssociado.Pessoa.dddTelSecundario) && ONaoAssociado.Pessoa.nroTelSecundario.Length > 2) {
                ONaoAssociado.Pessoa.dddTelSecundario = ONaoAssociado.Pessoa.nroTelSecundario.Substring(0, 2);
                ONaoAssociado.Pessoa.nroTelSecundario = ONaoAssociado.Pessoa.nroTelSecundario.Substring( 2);
            }

            ONaoAssociado.Pessoa.nroTelTerciario = UtilString.onlyNumber(ONaoAssociado.Pessoa.nroTelTerciario);

            if (string.IsNullOrEmpty(ONaoAssociado.Pessoa.dddTelTerciario) && ONaoAssociado.Pessoa.nroTelTerciario.Length > 2) {
                ONaoAssociado.Pessoa.dddTelTerciario = ONaoAssociado.Pessoa.nroTelTerciario.Substring(0, 2);
                ONaoAssociado.Pessoa.nroTelTerciario = ONaoAssociado.Pessoa.nroTelTerciario.Substring( 2);
            }

            ONaoAssociado.Pessoa.emailPrincipal = ONaoAssociado.Pessoa.emailPrincipal().ToLower();

            ONaoAssociado.Pessoa.emailSecundario = (!String.IsNullOrEmpty(ONaoAssociado.Pessoa.emailSecundario()) ? ONaoAssociado.Pessoa.emailSecundario().ToLower() : "");

            if (ONaoAssociado.Pessoa.flagTipoPessoa == "F") { 
                ONaoAssociado.Pessoa.idTipoDocumento = Convert.ToInt32(TipoDocumentoEnum.CPF);
            }

            if (ONaoAssociado.Pessoa.flagTipoPessoa == "J") { 
                ONaoAssociado.Pessoa.idTipoDocumento = Convert.ToInt32(TipoDocumentoEnum.CNPJ);
            }

            //Anular relacionamentos que nao se deseja inserções
            ONaoAssociado.TipoAssociado = null;
            ONaoAssociado.Pessoa.CidadeOrigem = null;
            ONaoAssociado.Pessoa.NivelEscolar = null;
            ONaoAssociado.Pessoa.PaisOrigem = null;
            ONaoAssociado.Pessoa.TipoDocumento = null;

            var idsCidades = ONaoAssociado.Pessoa.listaEnderecos.Select(x => x.idCidade).ToArray();
            var listaCidades = this.OCidadeBL.listar(0, "", "S").Where(x => idsCidades.Contains(x.id));

            ONaoAssociado.Pessoa.listaEnderecos.ForEach(e => {
                var OCidade = listaCidades.Where(x => x.id == e.idCidade).FirstOrDefault() ?? new Cidade();
                var siglaUF = OCidade != null && OCidade.Estado != null ? OCidade.Estado.sigla : "-";

                e.nomeCidade = OCidade.nome;
                e.uf = siglaUF;
                e.TipoEndereco = null;
                e.Pais = null;
                e.Cidade = null;
                e.Estado = null;
                e.cep = UtilString.onlyNumber(e.cep);
                e.idPais = !string.IsNullOrEmpty(e.idPais) ? e.idPais : "BRA";
            });

            if(ONaoAssociado.id > 0) {

                this.atualizar(ONaoAssociado);

                return ONaoAssociado;
            }

            bool flagSalvo = this.inserir(ONaoAssociado);

            if(flagSalvo) {
                this.onNaoAssociadoCadastro.publish((ONaoAssociado as object));
            }

            return ONaoAssociado;
        }

        //Inserir os dados para um novo não associado
        //Gerar uma senha randômica para enviar para o cadastro do novo não associado
        private bool inserir(Associado ONaoAssociado) {

            ONaoAssociado.setDefaultInsertValues();

            ONaoAssociado.Pessoa.login = ONaoAssociado.Pessoa.login.ToLower();

            ONaoAssociado.Pessoa.setDefaultInsertValues();

            ONaoAssociado.Pessoa.listaEnderecos.ForEach(e => { e.setDefaultInsertValues(); });

            ONaoAssociado.idTipoAssociado = UtilNumber.toInt32(ONaoAssociado.idTipoAssociado);

            ONaoAssociado.ativo = "S";

            string senha = ONaoAssociado.Pessoa.senha;

            ONaoAssociado.Pessoa.senha = UtilCrypt.SHA512(senha);

            db.Associado.Add(ONaoAssociado);
            db.SaveChanges();

            return (ONaoAssociado.id > 0);
        }

        //Atualizar os dados de um não associado e os objetos relacionados
        private bool atualizar(Associado ONaoAssociado) {

            var dbAssociado = this.carregar(ONaoAssociado.id);

            var entryAssociado = db.Entry(dbAssociado);
            ONaoAssociado.setDefaultUpdateValues();
            entryAssociado.CurrentValues.SetValues(ONaoAssociado);
            entryAssociado.State = EntityState.Modified;
            entryAssociado.ignoreFields(new[] { "idPessoa", "idTipoAssociado", "nroAssociado", "nroSegundaCOCEP", "idEstadoSegundaCOCEP", "flagTituloEspecialista", "flagTituloOutrasSociedades", "descOutrasSociedades", "nomeInstituicao", "flagSituacaoContribuicao", "observacoes", "flagInformativosOnline", "idEmpresaEstipulante", "idAssociadoEstipulante", "idUltimaContribuicao", "ativo", "idUsuarioAdmissao", "dtAdmissao", "idUsuarioDesativacao", "dtReativacao", "dtDesativacao", "idUsuarioReativacao", "dtExclusao", "idUsuarioExclusao", "motivoExclusao" });

            var entryPessoa = db.Entry(dbAssociado.Pessoa);
            ONaoAssociado.Pessoa.setDefaultUpdateValues();
            ONaoAssociado.Pessoa.id = dbAssociado.Pessoa.id;
            entryPessoa.CurrentValues.SetValues(ONaoAssociado.Pessoa);
            entryPessoa.State = EntityState.Modified;
            entryPessoa.ignoreFields(new[] { "nroDocumento", "idSegmentoAtuacao", "orgaoEmissorRg", "nomePai", "nomeMae", "idNivelEscolar", "idTipoEnderecoCorrespondencia", "senha", "observacoes", "ativo" });

            this.atualizarEnderecos(ONaoAssociado, dbAssociado);

            db.SaveChanges();

            return (ONaoAssociado.id > 0);
        }

        //Atualizacao dos enderecos do associado
        private void atualizarEnderecos(Associado ONaoAssociado, Associado dbAssociado) {

            foreach(var OPessoaEndereco in ONaoAssociado.Pessoa.listaEnderecos) {

                var dbEndereco = dbAssociado.Pessoa.listaEnderecos.FirstOrDefault(e => e.id == OPessoaEndereco.id);

                if(dbEndereco != null) {

                    var EntryEndereco = db.Entry(dbEndereco);
                    OPessoaEndereco.setDefaultUpdateValues();
                    EntryEndereco.CurrentValues.SetValues(OPessoaEndereco);
                    EntryEndereco.ignoreFields(new[] { "idPessoa" });
                    EntryEndereco.State = EntityState.Modified;

                } else {

                    OPessoaEndereco.idPessoa = dbAssociado.idPessoa;
                    OPessoaEndereco.setDefaultInsertValues();
                    db.PessoaEndereco.Add(OPessoaEndereco);
                }
            }
        }

    }
}
