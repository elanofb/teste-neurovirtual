using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Pessoas;
using BLL.Services;
using DAL.Associados.DTO;
using DAL.Pessoas;

namespace WEB.Areas.AssociadosConsultas.ViewModels {

    public class AssociadoConsultaForm {
        
        //Atributos
        private IPessoaEmailConsultaBL _PessoaEmailConsultaBL;
        private IPessoaTelefoneConsultaBL _PessoaTelefoneConsultaBL;
        
        //Servicos
        private IPessoaEmailConsultaBL OPessoaEmailConsultaBL => _PessoaEmailConsultaBL = _PessoaEmailConsultaBL ?? new PessoaEmailConsultaBL(); 
        private IPessoaTelefoneConsultaBL OPessoaTelefoneConsultaBL => _PessoaTelefoneConsultaBL = _PessoaTelefoneConsultaBL ?? new PessoaTelefoneConsultaBL(); 
        
        //Propriedades
        public string valorBuscaLote { get; set; }

        public string valorBusca { get; set; }
        
        public List<int> idsTipoAssociado { get; set; }

        public DateTime? dtCadastroInicio { get; set; }
        public DateTime? dtCadastroFim { get; set; }

        public DateTime? dtAdmissaoInicio { get; set; }
        public DateTime? dtAdmissaoFim { get; set; }

        public List<int> idsUnidades { get; set; }

        public string flagSituacaoContribuicao { get; set; }

        public string ativo { get; set; }

        public string flagTipoPessoa { get; set; }

        public string flagSexo { get; set; }

        public string flagTipoSaida { get; set; }
        
        public bool? flagDocumentosAprovados { get; set; }

        public bool flagTemCarteirinha { get; set; }
        public bool? flagComissao { get; set; }

        //
        public IPagedList<ItemListaAssociado> listaAssociados { get; set; }

        //
        public AssociadoConsultaForm() {

            this.idsTipoAssociado = new List<int>();

            this.idsUnidades = new List<int>();

            this.listaAssociados = new List<ItemListaAssociado>().ToPagedList(1, 20);
        }

        /// <summary>
        /// 
        /// </summary>
        public void carregarTelefones() {

            var idsPessoas = this.listaAssociados.Select(x => x.idPessoa).ToList();

            var listaFones = this.OPessoaTelefoneConsultaBL.listar(0)
                                                            .Where(x => idsPessoas.Contains(x.idPessoa))
                                                            .Select(x => new {x.id, x.idPessoa, x.nroTelefone, x.ddi})
                                                            .ToListJsonObject<PessoaTelefone>();
            
            foreach (var item in listaAssociados) {

                var Fone = listaFones.FirstOrDefault(x => x.idPessoa == item.idPessoa);

                if (Fone == null) {
                    continue;
                }

                item.nroTelefone = Fone.nroTelefone; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void carregarEmails() {
            
            var idsPessoas = this.listaAssociados.Select(x => x.idPessoa).ToList();

            var listaEmail = this.OPessoaEmailConsultaBL.listar(0)
                                                        .Where(x => idsPessoas.Contains(x.idPessoa))
                                                        .Select(x => new {x.id, x.idPessoa, x.email})
                                                        .ToListJsonObject<PessoaEmail>();            
            
            foreach (var item in listaAssociados) {

                var Email = listaEmail.FirstOrDefault(x => x.idPessoa == item.idPessoa);

                if (Email == null) {
                    continue;
                }

                item.email = Email.email; 
                
                var EmailSecundario = listaEmail.Skip(1).FirstOrDefault(x => x.idPessoa == item.idPessoa);
                
                if (EmailSecundario == null) {
                    continue;
                }

                item.emailSecundario = EmailSecundario.email;                
            }            
        }

    }
}