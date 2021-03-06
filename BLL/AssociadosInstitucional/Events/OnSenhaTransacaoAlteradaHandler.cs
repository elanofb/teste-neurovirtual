﻿using BLL.Core.Events;
using DAL.Associados;
using System;
using DAL.Pessoas;
using BLL.Pessoas;
using DAL.Relacionamentos;

namespace BLL.AssociadosInstitucional.Events {

	public class OnSenhaTransacaoAlteradaHandler : IHandler<object> {

		//Atributos
		private IPessoaRelacionamentoBL _PessoaRelacionamentoBL;

		//Propridades
		private IPessoaRelacionamentoBL OPessoaRelacionamentoBL{ get{ return (this._PessoaRelacionamentoBL = this._PessoaRelacionamentoBL ?? new PessoaRelacionamentoBL() ); }}
		private Associado OAssociado { get; set;}


		//Chamador das ações do evento
		public void execute(object source) {
			try {

				this.OAssociado = (source as Associado);

				this.gerarOcorrencia();

			} catch (Exception ex) {
				UtilLog.saveError(ex, String.Format("Erro no manipulador do evento de alteracao de senha para transações do membro {0}", this.OAssociado.Pessoa.nome));
			}
		}

		//Gerar Ocorrencia para histórico do associado
		public void gerarOcorrencia() { 
			PessoaRelacionamento Ocorrencia = new PessoaRelacionamento();
			Ocorrencia.dtOcorrencia = DateTime.Now;
			Ocorrencia.idPessoa = OAssociado.idPessoa;
			Ocorrencia.idOcorrenciaRelacionamento = OcorrenciaRelacionamentoConst.idAlteracaoSenha;
			Ocorrencia.observacao = "Senha para transações alterada através da área do membro.";

			this.OPessoaRelacionamentoBL.salvar(Ocorrencia);
		}
	}
}