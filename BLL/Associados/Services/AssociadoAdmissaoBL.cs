
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Associados;
using BLL.Services;
using BLL.Core.Events;
using BLL.Associados.Events;
using DAL.Permissao.Security.Extensions;

namespace BLL.Associados {

	public class AssociadoAdmissaoBL : DefaultBL, IAssociadoAdmissaoBL {
        
		//Events
		private EventAggregator onAdmissao = OnAdmissao.getInstance;

		//Construtor
		public AssociadoAdmissaoBL() {
		}

		//Admitir um associado que estava em processo de admissao
		public UtilRetorno admitirAssociados(List<int> idsAssociados, DateTime? dtAdmissao, string observacoes) {
            
            var listaAssociados = db.Associado.Where(x => idsAssociados.Contains(x.id)).ToList();

			if (!listaAssociados.Any()) {
                return UtilRetorno.newInstance(true, "Nenhum associado foi encontrado.");
            }

            int idUsuarioLogado = User.id();

            listaAssociados.ForEach(x => {

                x.dtAdmissao = dtAdmissao;
			    x.idUsuarioAdmissao = idUsuarioLogado;
			    x.ativo = "S";

            });
			
			db.SaveChanges();

            var listaParametros = new List<object>();
            listaParametros.Add(listaAssociados);
            listaParametros.Add(observacoes);

            this.onAdmissao.subscribe( new OnAdmissaoHandler() );
			this.onAdmissao.publish( (object) listaParametros );

			return UtilRetorno.newInstance(false, "Associado(s) admitido(s) com sucesso.");
		}

	}
}