using System;
using System.Linq;
using DAL.SegmentosAtuacao;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.SegmentosAtuacao {

	public class SegmentoAtuacaoExclusaoBL : SegmentoAtuacaoConsultaBL, ISegmentoAtuacaoExclusaoBL {

		//
		public SegmentoAtuacaoExclusaoBL(){
		}
		
		//Exclusao logica de registro
		public UtilRetorno excluir(int id) {
			UtilRetorno Retorno = UtilRetorno.getInstance();
			Retorno.flagError = false;

		    var idUsuario = User.id();

			this.db.SegmentoAtuacao
						.Where(x => x.id == id)
						.Update(x => new SegmentoAtuacao{ dtExclusao = DateTime.Now, idUsuarioExclusao = idUsuario });

			return Retorno;
		}
	}
}