using BLL.Associados;
using BLL.Core.Events;
using BLL.NaoAssociados.Events;
using BLL.Pessoas;
using BLL.Services;
using DAL.Relacionamentos;
using System;
using System.Collections.Generic;

namespace BLL.NaoAssociados {

    public class NaoAssociadoEnvioFichaBL : DefaultBL, INaoAssociadoEnvioFichaBL {

		//Atributos
	    private IPessoaRelacionamentoBL _PessoaRelacionamentoBL;

		//Propriedades
	    public IPessoaRelacionamentoBL OPessoaRelacionamentoBL => _PessoaRelacionamentoBL = _PessoaRelacionamentoBL ?? new PessoaRelacionamentoBL();

		//Events
		private readonly EventAggregator onNaoAssociadoEnvioFicha = OnNaoAssociadoEnvioFicha.getInstance;
        
		//Construtor
		public NaoAssociadoEnvioFichaBL() {

		}

        //Enviar 
	    public UtilRetorno enviarPorEmail(int idAssociado, string emails, int idUsuario) {

	        var OAssociado = this.db.Associado.Find(idAssociado);
            
	        if (OAssociado == null) {
	            return UtilRetorno.newInstance(true, "Nenhum cadastro foi localizado com o ID informado.");
	        }
            
	        string obsHistorico = String.Concat("A ficha cadastral do não associado foi enviada por email para: " + emails.Replace(";", "; "));

            db.SaveChanges();

	        this.OPessoaRelacionamentoBL.salvar(OAssociado.idPessoa, OcorrenciaRelacionamentoConst.idEnvioFichaPorEmail, idUsuario, obsHistorico);

            var listaParametros = new List<object>(2);
            listaParametros.Add(idAssociado);
            listaParametros.Add(emails);

            this.onNaoAssociadoEnvioFicha.subscribe( new OnNaoAssociadoEnvioFichaHandler() );
            this.onNaoAssociadoEnvioFicha.publish((listaParametros as object));

            return UtilRetorno.newInstance(false, "A ficha de cadastro do não associado foi enviada com sucesso.");
	    }
        
	}
}