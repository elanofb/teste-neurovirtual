using System;
using System.Text;
using BLL.Core.Events;
using BLL.Request;
using DAL.Associados;
using Newtonsoft.Json;

namespace BLL.Associados {

    public class OnAssociadoAlteradoHandler : IHandler<object> {

        public void execute(object source) {

            try {
                Associado OAssociado = (source as Associado);

                this.postBack(OAssociado);

            } catch (Exception ex) {
                UtilLog.saveError(ex, "Erro no manipulador de evento: OnAssociadoAlteradoHandler");
            }
        }

        private void postBack(Associado OAssociado) {

            try {

                string json = JsonConvert.SerializeObject(OAssociado, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                byte[] data = Encoding.ASCII.GetBytes("associado=" + json);

                var ORequestPost = new RequestPost();

            } catch (Exception ex) {
                UtilLog.saveError(ex, "Erro ao realizar postback: OnAssociadoAlteradoHandler");
            }
        }
    }
}