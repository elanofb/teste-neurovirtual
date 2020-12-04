using System;
using System.Collections.Generic;
using System.Text;
using BLL.Associados;
using BLL.Core.Events;
using BLL.Request;
using DAL.Associados;
using Newtonsoft.Json;

namespace BLL.AssociadosOperacoes {

    public class AssociadoExcluidoHandler : IHandler<object> {

        public void execute(object source) {

            try {
                List<Associado> listaAssociado = (source as List<Associado>);

                foreach (var OAssociado in listaAssociado) {
                }

            } catch (Exception ex) {

                UtilLog.saveError(ex, "Erro no manipulador de evento: AssociadoExcluidoHandler");

            }

        }


    }
}