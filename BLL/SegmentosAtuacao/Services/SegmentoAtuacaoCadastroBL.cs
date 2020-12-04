using System;
using DAL.SegmentosAtuacao;

namespace BLL.SegmentosAtuacao {

	public class SegmentoAtuacaoCadastroBL : SegmentoAtuacaoConsultaBL, ISegmentoAtuacaoCadastroBL {

		//
		public SegmentoAtuacaoCadastroBL(){
		}

		//Verificar se deve-se atualizar um registro existente ou criar um novo
		public bool salvar(SegmentoAtuacao OSegmentoAtuacao) {

			if (OSegmentoAtuacao.id == 0) { 
				return this.inserir(OSegmentoAtuacao);
			}

			return this.atualizar(OSegmentoAtuacao);
		}

		//Persistir o objecto e salvar na base de dados
		private bool inserir(SegmentoAtuacao OSegmentoAtuacao) { 

			OSegmentoAtuacao.setDefaultInsertValues<SegmentoAtuacao>();
			db.SegmentoAtuacao.Add(OSegmentoAtuacao);
			db.SaveChanges();

			return (OSegmentoAtuacao.id > 0);
		}

		//Persistir o objecto e atualizar informações
		private bool atualizar(SegmentoAtuacao OSegmentoAtuacao) { 
			OSegmentoAtuacao.setDefaultUpdateValues<SegmentoAtuacao>();

			//Localizar existentes no banco
			SegmentoAtuacao dbSegmentoAtuacao = this.carregar(OSegmentoAtuacao.id);		
			var TipoEntry = db.Entry(dbSegmentoAtuacao);
			TipoEntry.CurrentValues.SetValues(OSegmentoAtuacao);
			TipoEntry.ignoreFields<SegmentoAtuacao>();

			db.SaveChanges();
			return (OSegmentoAtuacao.id > 0);
		}

	}
}