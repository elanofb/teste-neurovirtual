using BLL.Core.Events;
using DAL.Associados;
using System.Linq;
using System;
using BLL.Associados;
using DAL.Pessoas;
using BLL.Pessoas;
using BLL.Services;
using DAL.Relacionamentos;

namespace BLL.AssociadosDependentes.Events {

	public class OnAssociadoDependenteCadastradoHandler : DefaultBL, IHandler<object> {

        //Atributos
	    private IAssociadoBL _AssociadoBL;
        private ITipoAssociadoBL _TipoAssociadoBL;
	    private IPessoaRelacionamentoBL _PessoaRelacionamentoBL;
        
        //Propridades
        private IAssociadoBL OAssociadoBL => this._AssociadoBL = this._AssociadoBL ?? new AssociadoBL();
        private ITipoAssociadoBL OTipoAssociadoBL => this._TipoAssociadoBL = this._TipoAssociadoBL ?? new TipoAssociadoBL();
        private IPessoaRelacionamentoBL OPessoaRelacionamentoBL => this._PessoaRelacionamentoBL = this._PessoaRelacionamentoBL ?? new PessoaRelacionamentoBL();


        private Associado OAssociado { get; set; }
	    private TipoAssociado OTipoAssociado { get; set; }

        //Chamador das ações do evento
        public void execute(object source) {
			try {

				this.OAssociado = source as Associado;

    	        this.OTipoAssociado = this.OTipoAssociadoBL.carregar(UtilNumber.toInt32(OAssociado.idTipoAssociado));

                this.gerarOcorrencia();

			    this.gerarOcorrenciaEstipulante();

            } catch (Exception ex) {
				UtilLog.saveError(ex,$"Erro no manipulador do evento de cadastro do associado {this.OAssociado.Pessoa.nome}");
			}
		}

		//Gerar Ocorrencia para histórico do associado
		private void gerarOcorrencia() {

		    try {

			    PessoaRelacionamento Ocorrencia = new PessoaRelacionamento();

                Ocorrencia.dtOcorrencia = DateTime.Now;

                Ocorrencia.idPessoa = OAssociado.idPessoa;

                Ocorrencia.idOcorrenciaRelacionamento = OcorrenciaRelacionamentoConst.idRealizacaoCadastro;

                Ocorrencia.observacao = this.OAssociado.observacoes;

			    this.OPessoaRelacionamentoBL.salvar(Ocorrencia);

		    } catch (Exception ex) {
		        UtilLog.saveError(ex, "Erro ao salvar ocorrencia apos cadastro de associado pelo sistema");
		    }
		}

        		//Gerar Ocorrencia para histórico do associado
		private void gerarOcorrenciaEstipulante() {

		    try {

		        var idAssociadoPessoaEstipulante = OAssociadoBL.listar(0, "", "", "").Where(x => x.id == 0).Select(x => x.idPessoa).FirstOrDefault();

			    PessoaRelacionamento Ocorrencia = new PessoaRelacionamento();

                Ocorrencia.dtOcorrencia = DateTime.Now;

                Ocorrencia.idPessoa = idAssociadoPessoaEstipulante;

                Ocorrencia.idOcorrenciaRelacionamento = OcorrenciaRelacionamentoConst.idInclusaoDependente;

                Ocorrencia.observacao = this.OAssociado.observacoes;

			    this.OPessoaRelacionamentoBL.salvar(Ocorrencia);

		    } catch (Exception ex) {
		        UtilLog.saveError(ex, "Erro ao salvar ocorrencia apos cadastro de associado pelo sistema");
		    }
		}
	}
}