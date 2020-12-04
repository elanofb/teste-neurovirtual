using System;
using DAL.Associados;
using System.Linq;
using System.Security.Principal;
using BLL.Associados;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using FastMember;
using UTIL.Extensions;

namespace WEB.Areas.Associados.ViewModels {
    
    public class AssociadoPreAtualizacaoCadastroPJForm :  AssociadoCadastroPJForm{
        
        //Atributos
        private IAssociadoBL _AssociadoBL;
        
        //Propriedades
        private IAssociadoBL OAssociadoBL => _AssociadoBL = _AssociadoBL ?? new AssociadoBL();
        
        public Associado AssociadoAtual { get; set; }
        
        protected IPrincipal User => HttpContextFactory.Current.User;
        
        //Construtor
        public AssociadoPreAtualizacaoCadastroPJForm(){
            this.flagRetornoAjax = true;
        }
        
        /// <summary>
        /// Atribuir os valores dos campos alterados     
        /// </summary>
        public void carregarValorCampos(AssociadoPreAtualizacaoCadastroPJForm ViewModel) {
            
            var listaCamposNaoFixos = this.listaCampos.Where(x => x.valorFixo.isEmpty()).ToList();
            
            var objectAcessor = ObjectAccessor.Create(ViewModel);
            
            foreach (var OCampo in listaCamposNaoFixos) {

                try {                                        

                    // Valor atualizado pelo associado que será validado 
                    OCampo.valorAtual = objectAcessor.getValueProperty(OCampo.name).stringOrEmpty();

                    string nameValorAntigo = OCampo.name.Replace("Associado", "AssociadoAtual");
                    
                    // Valor atual no banco de dados, que será descartado
                    string valorAntigo = objectAcessor.getValueProperty(nameValorAntigo).stringOrEmpty();
                    
                    if (OCampo.valorAtual != valorAntigo){
                        OCampo.valorPadrao = valorAntigo;
                    }
                    
                } catch (ArgumentOutOfRangeException ex) {
                    
                    UtilLog.saveError(ex, $"Erro ao atribuir valor {OCampo.name}");

                } catch (Exception ex) {

                    UtilLog.saveError(ex, $"Erro ao atribuir valor {OCampo.name}");
                }

            }

        }
        
        public void carregarInformacoesAssociado(int idAssociado){

            if (idAssociado == 0){
                return;
            }
            
            this.AssociadoAtual = this.OAssociadoBL.query(User.idOrganizacao())
                                           .Select(x => new{
                                               x.id, 
                                               x.idOrganizacao,
                                               x.idTipoAssociado,                                             
                                               Pessoa = new{
                                                   x.Pessoa.id,                                                                                                       
                                                   x.Pessoa.flagTipoPessoa,
                                                   x.Pessoa.flagEstrangeiro,
                                                   x.Pessoa.nome,
                                                   x.Pessoa.razaoSocial,
                                                   x.Pessoa.idTipoDocumento,
                                                   x.Pessoa.nroDocumento,
                                                   x.Pessoa.passaporte,
                                                   x.Pessoa.rne,
                                                   x.Pessoa.rg,
                                                   x.Pessoa.orgaoEmissorRg,
                                                   x.Pessoa.idEstadoEmissaoRg,                                                   
                                                   x.Pessoa.nroCTPS,
                                                   x.Pessoa.serieCTPS,
                                                   x.Pessoa.dtEmissaoCTPS,
                                                   x.Pessoa.nroPIS,
                                                   x.Pessoa.nroTituloEleitor,
                                                   x.Pessoa.zonaEleitoral,
                                                   x.Pessoa.sessaoEleitoral,
                                                   x.Pessoa.nroReservista,
                                                   x.Pessoa.serieReservista,
                                                   x.Pessoa.nroCNH,
                                                   x.Pessoa.categoriaCNH,
                                                   x.Pessoa.dtValidadeCNH,
                                                   x.Pessoa.inscricaoEstadual,
                                                   x.Pessoa.inscricaoMunicipal,
                                                   x.Pessoa.cnaeAtividade,
                                                   x.Pessoa.idEmpresaPorte,
                                                   x.Pessoa.flagOptanteSimplesNacional,
                                                   x.Pessoa.flagFinsLucrativos,
                                                   x.Pessoa.qtdeEmpregadosCLT,
                                                   x.Pessoa.qtdeEmpregadosTerceiros,
                                                   x.Pessoa.qtdeEstagiarios,
                                                   x.Pessoa.qtdeMenorAprendiz,
                                                   x.Pessoa.idOrgaoClasse,
                                                   x.Pessoa.nroRegistroOrgaoClasse,
                                                   x.Pessoa.idEstadoOrgaoClasse,
                                                   x.Pessoa.idSetorAtuacao,
                                                   x.Pessoa.idPaisOrigem,
                                                   x.Pessoa.idCidadeOrigem,
                                                   x.Pessoa.nomeCidadeOrigem,
                                                   x.Pessoa.dtNascimento,
                                                   x.Pessoa.flagSexo,
                                                   x.Pessoa.nomePai,
                                                   x.Pessoa.nomeMae,                                                   
                                                   x.Pessoa.enderecoWeb,
                                                   x.Pessoa.idNivelEscolar,
                                                   x.Pessoa.instituicaoFormacao,
                                                   x.Pessoa.anoFormacao,
                                                   x.Pessoa.dtFormacao,
                                                   x.Pessoa.nroMatriculaEstudante,
                                                   x.Pessoa.instituicaoEstudante,
                                                   x.Pessoa.profissao,
                                                   x.Pessoa.localTrabalho,
                                                   x.Pessoa.login,
                                                   x.Pessoa.senha,
                                                   x.Pessoa.observacoes,
                                                   x.Pessoa.idTipoEnderecoCorrespondencia,
                                                   x.Pessoa.nomeResponsavelCadastro,
                                                   x.Pessoa.documentoResponsavelCadastro,
                                                   x.Pessoa.obsResponsavelCadastro,                                                    
                                                   listaEmails = x.Pessoa.listaEmails.Where(e => e.dtExclusao == null).Select(e => new{
                                                       e.idTipoEmail,
                                                       e.email,                                                       
                                                   }).ToList(),
                                                   listaEnderecos = x.Pessoa.listaEnderecos.Where(e => e.dtExclusao == null).Select(e => new{
                                                       e.id,
                                                       e.idTipoEndereco,
                                                       e.idPais,
                                                       e.idEstado,
                                                       e.uf,
                                                       e.idCidade,
                                                       Cidade = new { e.Cidade.idEstado },
                                                       e.nomeCidade,
                                                       e.cep,
                                                       e.logradouro,
                                                       e.numero,
                                                       e.complemento,
                                                       e.zona,
                                                       e.bairro,
                                                       e.flagEnviarCorrespondencia,
                                                       e.flagEntrega,
                                                       e.observacoes                                                       
                                                   }).ToList(),
                                                   listaTelefones = x.Pessoa.listaTelefones.Where(e => e.dtExclusao == null).Select(e => new{
                                                       e.id,
                                                       e.idTipoTelefone,                                                       
                                                       e.ddi,
                                                       e.nroTelefone,
                                                       e.flagTipo,
                                                       e.idOperadoraTelefonia,                                                       
                                                   }).ToList()                                                   
                                               }
                                           })
                                           .FirstOrDefault(x => x.id == idAssociado)
                                           .ToJsonObject<Associado>() ?? new Associado();
        }

    }

}