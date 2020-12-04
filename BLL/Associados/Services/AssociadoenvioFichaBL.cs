using BLL.Associados.Events;
using BLL.Core.Events;
using BLL.Pessoas;
using BLL.Services;
using DAL.Relacionamentos;
using System;
using System.Collections.Generic;

namespace BLL.Associados {

    public class AssociadoEnvioFichaBL : DefaultBL, IAssociadoEnvioFichaBL {

		//Atributos
	    private IPessoaRelacionamentoBL _PessoaRelacionamentoBL;

		//Propriedades
	    public IPessoaRelacionamentoBL OPessoaRelacionamentoBL => _PessoaRelacionamentoBL = _PessoaRelacionamentoBL ?? new PessoaRelacionamentoBL();

		//Events
		private EventAggregator onAssociadoEnvioFicha = OnAssociadoEnvioFicha.getInstance;
        
		//Construtor
		public AssociadoEnvioFichaBL() {

		}

        //Enviar 
	    public UtilRetorno enviarPorEmail(int idAssociado, string emails, int idUsuario) {
            this.onAssociadoEnvioFicha.subscribe( new OnAssociadoEnvioFichaHandler() );

	        var OAssociado = this.db.Associado.Find(idAssociado);
            
	        if (OAssociado == null) {
	            return UtilRetorno.newInstance(true, "Nenhum associado foi localizado com o ID informado.");
	        }
            
	        string obsHistorico = String.Concat("A ficha cadastral do associado foi enviada por email para: " + emails.Replace(";", "; "));

            db.SaveChanges();

	        this.OPessoaRelacionamentoBL.salvar(OAssociado.idPessoa, OcorrenciaRelacionamentoConst.idEnvioFichaPorEmail, idUsuario, obsHistorico);

            var listaParametros = new List<object>(2);
            listaParametros.Add(idAssociado);
            listaParametros.Add(emails);

            this.onAssociadoEnvioFicha.publish((listaParametros as object));

            return UtilRetorno.newInstance(false, "A ficha de cadastro do associado foi enviada com sucesso.");
	    }
        
	}
}