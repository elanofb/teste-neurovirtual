using System;
using System.Data.Entity;
using System.Linq;
using BLL.Core.Events;
using BLL.Services;
using BLL.AssociadosInstitucional.Events;
using DAL.Associados;
using DAL.Documentos;
using EntityFramework.Extensions;
using BLL.Localizacao;
using DAL.Localizacao;
using DAL.Pessoas;

namespace BLL.AssociadosInstitucional {

    public class AssociadoInstitucionalBL : DefaultBL, IAssociadoInstitucionalBL {

        //Constantes

        //Atributos
        private ICidadeBL _CidadeBL;

        //Propriedades
        private ICidadeBL OCidadeBL => this._CidadeBL = this._CidadeBL ?? new CidadeBL();

        //Events
        private readonly EventAggregator onAssociadoCadastro = OnAssociadoInstitucionalCadastrado.getInstance;

        //
        public AssociadoInstitucionalBL() {
            this.onAssociadoCadastro.subscribe(new OnAssociadoInstitucionalCadastradoHandler());
        }

        //Carregar o associado fazendo join com as tabelas necessarias através do ID
        public Associado carregar(int idAssociado) {
            var query = from Ass in db.Associado
                            .Include(x => x.Pessoa)
                            .Include(x => x.Pessoa.CidadeOrigem)
                            .Include(x => x.Pessoa.listaEnderecos)
                        where Ass.id == idAssociado && !Ass.dtExclusao.HasValue
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

            if(!String.IsNullOrEmpty(email)) {
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
        public Associado salvar(Associado OAssociado) {

            OAssociado.idTipoCadastro = AssociadoTipoCadastroConst.CONSUMIDOR;

            //Tratar valores
            OAssociado.Pessoa.nroDocumento = UtilString.onlyAlphaNumber(OAssociado.Pessoa.nroDocumento);

            OAssociado.Pessoa.rg = UtilString.onlyAlphaNumber(OAssociado.Pessoa.rg);

            OAssociado.Pessoa.nroTelPrincipal = UtilString.onlyNumber(OAssociado.Pessoa.nroTelPrincipal);

            if (string.IsNullOrEmpty(OAssociado.Pessoa.dddTelPrincipal) && OAssociado.Pessoa.nroTelPrincipal.Length > 2) {
                OAssociado.Pessoa.dddTelPrincipal = OAssociado.Pessoa.nroTelPrincipal.Substring(0, 2);
                OAssociado.Pessoa.nroTelPrincipal = OAssociado.Pessoa.nroTelPrincipal.Substring( 2);
            }
            
            OAssociado.Pessoa.nroTelSecundario = UtilString.onlyNumber(OAssociado.Pessoa.nroTelSecundario);

            if (string.IsNullOrEmpty(OAssociado.Pessoa.dddTelSecundario) && OAssociado.Pessoa.nroTelSecundario.Length > 2) {
                OAssociado.Pessoa.dddTelSecundario = OAssociado.Pessoa.nroTelSecundario.Substring(0, 2);
                OAssociado.Pessoa.nroTelSecundario = OAssociado.Pessoa.nroTelSecundario.Substring( 2);
            }

            OAssociado.Pessoa.nroTelTerciario = UtilString.onlyNumber(OAssociado.Pessoa.nroTelTerciario);

            if (string.IsNullOrEmpty(OAssociado.Pessoa.dddTelTerciario) && OAssociado.Pessoa.nroTelTerciario.Length > 2) {
                OAssociado.Pessoa.dddTelTerciario = OAssociado.Pessoa.nroTelTerciario.Substring(0, 2);
                OAssociado.Pessoa.nroTelTerciario = OAssociado.Pessoa.nroTelTerciario.Substring( 2);
            }

            OAssociado.Pessoa.emailPrincipal = OAssociado.Pessoa.emailPrincipal().ToLower();

            OAssociado.Pessoa.emailSecundario = (!String.IsNullOrEmpty(OAssociado.Pessoa.emailSecundario()) ? OAssociado.Pessoa.emailSecundario().ToLower() : "");

            if (OAssociado.Pessoa.flagTipoPessoa == "F") { 
                OAssociado.Pessoa.idTipoDocumento = Convert.ToInt32(TipoDocumentoEnum.CPF);
            }

            if (OAssociado.Pessoa.flagTipoPessoa == "J") { 
                OAssociado.Pessoa.idTipoDocumento = Convert.ToInt32(TipoDocumentoEnum.CNPJ);
            }

            //Anular relacionamentos que nao se deseja inserções
            OAssociado.TipoAssociado = null;
            OAssociado.Pessoa.CidadeOrigem = null;
            OAssociado.Pessoa.NivelEscolar = null;
            OAssociado.Pessoa.PaisOrigem = null;
            OAssociado.Pessoa.TipoDocumento = null;

            var idsCidades = OAssociado.Pessoa.listaEnderecos.Select(x => x.idCidade).ToArray();
            var listaCidades = this.OCidadeBL.listar(0, "", "S").Where(x => idsCidades.Contains(x.id));

            OAssociado.Pessoa.listaEnderecos.ToList().ForEach(e => {
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

            if(OAssociado.id > 0) {

                this.atualizar(OAssociado);

                return OAssociado;
            }

            bool flagSalvo = this.inserir(OAssociado);

            if(flagSalvo) {
                this.onAssociadoCadastro.publish((OAssociado as object));
            }

            return OAssociado;
        }

        //Inserir os dados para um novo associado
        //Gerar uma senha randômica para enviar para o cadastro do novo associado
        private bool inserir(Associado OAssociado) {

            OAssociado.setDefaultInsertValues();

            OAssociado.Pessoa.login = OAssociado.Pessoa.login.ToLower();

            OAssociado.Pessoa.setDefaultInsertValues();

            OAssociado.Pessoa.listaEnderecos.ToList().ForEach(e => { e.setDefaultInsertValues(); });

            OAssociado.idTipoAssociado = UtilNumber.toInt32(OAssociado.idTipoAssociado);

            OAssociado.nroAssociado = this.proximoId();

            OAssociado.ativo = "E";

            string senha = OAssociado.Pessoa.senha;
            OAssociado.Pessoa.senha = UtilCrypt.SHA512(senha);

            db.Associado.Add(OAssociado);
            db.SaveChanges();

            return (OAssociado.id > 0);
        }

        //Atualizar os dados de um associado e os objetos relacionados
        private bool atualizar(Associado OAssociado) {

            var dbAssociado = this.carregar(OAssociado.id);

            var entryAssociado = db.Entry(dbAssociado);
            OAssociado.setDefaultUpdateValues();
            entryAssociado.CurrentValues.SetValues(OAssociado);
            entryAssociado.State = EntityState.Modified;
            entryAssociado.ignoreFields(new[] { "idPessoa", "idTipoAssociado", "nroAssociado", "nroSegundaCOCEP", "idEstadoSegundaCOCEP", "flagTituloEspecialista", "flagTituloOutrasSociedades", "descOutrasSociedades", "nomeInstituicao", "flagSituacaoContribuicao", "observacoes", "flagInformativosOnline", "idEmpresaEstipulante", "idAssociadoEstipulante", "idUltimaContribuicao", "ativo", "idUsuarioAdmissao", "dtAdmissao", "idUsuarioDesativacao", "dtReativacao", "dtDesativacao", "idUsuarioReativacao", "dtExclusao", "idUsuarioExclusao", "motivoExclusao" });

            var entryPessoa = db.Entry(dbAssociado.Pessoa);
            OAssociado.Pessoa.setDefaultUpdateValues();
            OAssociado.Pessoa.id = dbAssociado.Pessoa.id;
            entryPessoa.CurrentValues.SetValues(OAssociado.Pessoa);
            entryPessoa.State = EntityState.Modified;
            entryPessoa.ignoreFields(new[] { "nroDocumento", "idSegmentoAtuacao", "orgaoEmissorRg", "nomePai", "nomeMae", "idNivelEscolar", "idTipoEnderecoCorrespondencia", "senha", "observacoes", "ativo" });

            this.atualizarEnderecos(OAssociado, dbAssociado);

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

        //Atualizacao dos enderecos do associado
        private void atualizarEnderecos(Associado OAssociado, Associado dbAssociado) {

            foreach(var OPessoaEndereco in OAssociado.Pessoa.listaEnderecos) {

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