using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Financeiro;
using DAL.AssociadosContribuicoes;
using DAL.Financeiro;
using DAL.Financeiro.Entities;
using EntityFramework.Extensions;

namespace BLL.AssociadosContribuicoes {

    public class TituloReceitaGeradorContribuicaoDependenteBL : TituloReceitaGeradorBL {

        //Atributos
        private ITituloReceitaBL _TituloReceitaBL;

        //Propriedades
        private int idTipoReceita { get; set; }

        private ITituloReceitaBL OTituloReceitaBL => _TituloReceitaBL = _TituloReceitaBL ?? new TituloReceitaContribuicaoBL();


        //Construtor
        public TituloReceitaGeradorContribuicaoDependenteBL() {

            this.idTipoReceita = TipoReceitaConst.CONTRIBUICAO;

        }

        //Metodo para geracao do titulo de receita
        public override UtilRetorno gerarLote(object OrigemTitulo) {

            List<AssociadoContribuicao> listaOrigemTitulo = (OrigemTitulo as List<AssociadoContribuicao>);

            if (listaOrigemTitulo == null) {
                return UtilRetorno.newInstance(true, "A lista informada é nula.");
            }

            foreach (var Item in listaOrigemTitulo) {

                this.gerar(Item);

            }

            return UtilRetorno.newInstance(false, "Geração de títulos processada com sucesso.");
        }

        /// <summary>
        /// Atualizar o título de receita da cobrança principal para contemplar o valor do dependente
        /// </summary>
        public override UtilRetorno gerar(object OrigemTitulo) {

            AssociadoContribuicao AssociadoContribuicao = (OrigemTitulo as AssociadoContribuicao);

            if (AssociadoContribuicao == null) {
                return UtilRetorno.newInstance(true, "Não é possível gerar um título com o objeto nulo (AssociadoContribuicao).");
            }

            if (AssociadoContribuicao.Contribuicao == null) {
                return UtilRetorno.newInstance(true, "Não é possível gerar um título com o objeto nulo (Contribuicao).");
            }

            //Capturar o título da cobrança principal
            var OTituloReceita = this.OTituloReceitaBL.carregarPorReceita(AssociadoContribuicao.idAssociadoContribuicaoPrincipal.toInt());

            if (OTituloReceita == null) {

                return UtilRetorno.newInstance(true, "O título da cobrança principal não foi encontrado");
            }

            decimal valorTitulo = OTituloReceita.valorTotal.toDecimal();

            decimal valorDependente = AssociadoContribuicao.valorAtual;

            decimal valorAtualizado = decimal.Add(valorTitulo, valorDependente);

            int idTituloReceita = OTituloReceita.id;

            string descricaoTitulo = OTituloReceita.descricao;

            if (!OTituloReceita.descricao.Contains("dependente")){

                descricaoTitulo = string.Concat(descricaoTitulo, " Incluindo cobrança dependentes");
            }

            db.TituloReceita.Where(x => x.id == idTituloReceita)
                            .Update(x => new TituloReceita {
                                valorTotal = valorAtualizado,
                                descricao = descricaoTitulo,
                                dtAlteracao = DateTime.Now
                            });


            return UtilRetorno.newInstance(false, "O título foi atualizado com sucesso", OTituloReceita);
        }

    }
}