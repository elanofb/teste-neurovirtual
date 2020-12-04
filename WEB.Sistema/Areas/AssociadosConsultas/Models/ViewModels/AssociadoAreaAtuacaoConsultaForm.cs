using System;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using DAL.Associados;
using DAL.Pessoas;

namespace WEB.Areas.AssociadosConsultas.ViewModels {

    public class AssociadoAreaAtuacaoConsultaForm {
        
        public int? idTipoCadastro { get; set; }

        public List<int> idsTipoAssociado { get; set; }

        public string ativo { get; set; }

        public string flagSituacaoContribuicao { get; set; }

        public string valorBusca { get; set; }
                
        public string flagTipoSaida { get; set; }
        
        public List<int> idsAreaAtuacao { get; set; }
        
        
        public List<AssociadoAreaAtuacaoVW> listaResultados { get; set; }
        
        public IPagedList<AssociadoAreaAtuacaoDTO> listaFiltrada { get; set; }
        
        //
        public AssociadoAreaAtuacaoConsultaForm() {
            this.idsAreaAtuacao = new List<int>();
            this.listaResultados = new List<AssociadoAreaAtuacaoVW>(); 
            this.listaFiltrada = new List<AssociadoAreaAtuacaoDTO>().ToPagedList(1, 20);

        }
        
        public void carregarResultados(){

            if (!this.listaResultados.Any()){
                return;
            }

            var listaPessoas = this.listaResultados.Select(x => new{ x.idAssociado, x.nome, x.descricaoTipoAssociado, x.nroDocumento }).Distinct().ToList();
            var lista = new List<AssociadoAreaAtuacaoDTO>();
            
            foreach (var OPessoa in listaPessoas){
                AssociadoAreaAtuacaoDTO OAssociadoAreaAtuacaoDTO = new AssociadoAreaAtuacaoDTO();
                
                OAssociadoAreaAtuacaoDTO.idAssociado = OPessoa.idAssociado;
                OAssociadoAreaAtuacaoDTO.nome = OPessoa.nome;
                OAssociadoAreaAtuacaoDTO.nroDocumento = OPessoa.nroDocumento;
                OAssociadoAreaAtuacaoDTO.tipoAssociado = OPessoa.descricaoTipoAssociado;
                
                OAssociadoAreaAtuacaoDTO.listaAreaAtuacao = this.listaResultados.Where(x => x.idAssociado == OPessoa.idAssociado).Where(x => !x.descricaoAreaAtuacao.isEmpty())
                    .Select(x => x.descricaoAreaAtuacao)
                    .Distinct()
                    .ToList();
                
                OAssociadoAreaAtuacaoDTO.listaTelefones = this.listaResultados.Where(x => x.idAssociado == OPessoa.idAssociado).Where(x => !x.nroTelefone.isEmpty())
                    .Select(x => x.nroTelefone)
                    .Distinct().ToList();
                
                
                var listaEmails = this.listaResultados.Where(x => x.idAssociado == OPessoa.idAssociado).Where(x => !x.email.isEmpty())
                    .Select(x => new { x.email, x.descricaoTipoEmail } )
                    .Distinct().ToList();
                
                foreach (var OEmail in listaEmails){
                    PessoaEmailResumoDTO OPessoaEmailResumoDTO = new PessoaEmailResumoDTO{ email = OEmail.email, tipoEmail  = OEmail.descricaoTipoEmail };
                    OAssociadoAreaAtuacaoDTO.listaEmails.Add(OPessoaEmailResumoDTO);
                }                
                
                lista.Add(OAssociadoAreaAtuacaoDTO);
                
            }
                                                
            this.listaFiltrada = lista.ToPagedList(1, 20);
        
        }

    }
}