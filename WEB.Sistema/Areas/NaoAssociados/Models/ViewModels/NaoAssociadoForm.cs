using FluentValidation.Attributes;
using DAL.Associados;
using DAL.Pessoas;
using DAL.Enderecos;
using DAL.Configuracoes;
using BLL.AreasAtuacao;
using BLL.Configuracoes;
using System.Collections.Generic;
using System.Linq;
using System;
using DAL.Associados.Config;
using MoreLinq;
using WEB.ViewModels;

namespace WEB.Areas.NaoAssociados.ViewModels {

    [Validator(typeof(NaoAssociadoFormValidator))]
    public class NaoAssociadoForm {

        //Atributos
        private AreaAtuacaoBL _AreaAtuacaoBL;

        //Propriedades
        public Associado NaoAssociado { get; set; }

        public ConfiguracaoPortal ConfiguracaoPortal { get; set; }

        public byte? idTipoEstipulante { get; set; }

        public IList<CheckBoxItens> listaAreaAtuacao { get; set; }

        //Servicos
        private AreaAtuacaoBL OAreaAtuacaoBL => (this._AreaAtuacaoBL = this._AreaAtuacaoBL ?? new AreaAtuacaoBL());

        //Construtor
        public NaoAssociadoForm() {

            this.listaAreaAtuacao = new List<CheckBoxItens>();

            this.idTipoEstipulante = 0;// ConfiguracaoContribuicaoBL.getInstance.carregar().idTipoEstipulanteContribuicao;
        }

        //Atribuir valores padrão para quando estiverem em branco
        public void configurarValoresPadrao() {

            this.NaoAssociado.Pessoa.idPaisOrigem = !String.IsNullOrEmpty(this.NaoAssociado.Pessoa.idPaisOrigem) ? this.NaoAssociado.Pessoa.idPaisOrigem : "BRA";

            this.NaoAssociado.Pessoa.idTipoEnderecoCorrespondencia = NaoAssociado.Pessoa.idTipoEnderecoCorrespondencia > 0 ? this.NaoAssociado.Pessoa.idTipoEnderecoCorrespondencia : TipoEnderecoConst.PRINCIPAL;

        }

        //Tratamento para os endereços do associado
        public void tratarEnderecos() {

            if (this.NaoAssociado.Pessoa.listaEnderecos.Count == 2) {
                return;
            }

            if (this.NaoAssociado.Pessoa.listaEnderecos.Count == 1) {
                this.NaoAssociado.Pessoa.listaEnderecos.Add(new PessoaEndereco { idTipoEndereco = TipoEnderecoConst.COMERCIAL, idPais = "BRA" });
                return;
            }

            this.NaoAssociado.Pessoa.listaEnderecos.Add(new PessoaEndereco { idTipoEndereco = TipoEnderecoConst.PRINCIPAL, idPais = "BRA" });
            this.NaoAssociado.Pessoa.listaEnderecos.Add(new PessoaEndereco { idTipoEndereco = TipoEnderecoConst.COMERCIAL, idPais = "BRA" });
        }

        //Buscar todas as areas de atuacao cadastradas
        //Carregar áreas de atuação para exibição no formulario em checkboxes
        //Definir quais serao marcadas por já haver cadastro para o não associado.
        public void carregarAreasAtuacao() {

            this.listaAreaAtuacao = this.OAreaAtuacaoBL.listar("", "S")
                                                        .Select(x => new CheckBoxItens {
                                                            descricao = x.descricao,
                                                            id = x.id,
                                                            isChecked = false
                                                        })
                                                        .ToList();

            this.listaAreaAtuacao.ForEach(item => {
                item.isChecked = this.NaoAssociado.listaAreaAtuacao.Any(a => a.idAreaAtuacao == item.id);
            });
        }

        //Verificar as áreas de atuacao que foram marcadas e preencher para salvar o associado
        public void mapearAreasAtuacao() {
            this.NaoAssociado.listaAreaAtuacao = this.NaoAssociado.listaAreaAtuacao ?? new List<AssociadoAreaAtuacao>();

            this.listaAreaAtuacao.Where(x => x.isChecked).ForEach(item => {
                this.NaoAssociado.listaAreaAtuacao.Add(new AssociadoAreaAtuacao { idAreaAtuacao = item.id });
            });
        }
    }
}
