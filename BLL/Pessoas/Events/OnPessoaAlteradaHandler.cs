using BLL.Core.Events;
using System;
using System.Collections.Generic;
using DAL.Pessoas;

namespace BLL.Pessoas {

    public class OnPessoaAlteradaHandler : IHandler<object> {

        //Propriedades Serviços
        private IPessoaAtualizacaoBL OPessoaCadastroBL => new PessoaAtualizacaoBL();

        // Propriedades
        private Pessoa OPessoaAtuializacao { get; set; }
        
        private Pessoa dbPessoa { get; set; }

        //
		public void execute(object source) {

            try {

                var listaParametros = source as List<object>;

                this.OPessoaAtuializacao = listaParametros[0] as Pessoa;
            
		        this.dbPessoa = listaParametros[1] as Pessoa;
                
            } catch (Exception ex) {

                UtilLog.saveError(ex, "Erro ao capturar os parâmetros no evento OnPalestranteAlteradoHandler.");

            }
		    
		    try {

		        this.atualizarDadosPessoa();
                
		    } catch (Exception ex) {

		        UtilLog.saveError(ex, "Erro ao executar o método atualizarDadosPessoa() no evento OnPalestranteAlteradoHandler.");

		    }

		}
        
        // Gerar ocorrência de atendimento do pedido
        private void atualizarDadosPessoa() {

            this.OPessoaCadastroBL.atualizarListas(this.OPessoaAtuializacao, this.dbPessoa);

        }
        
	}
}
