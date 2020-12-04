using BLL.Core.Events;
using DAL.Associados;
using System;

namespace BLL.Associados.Events {

    public class OnAreaAssociadoAssociadoCadastradoHandler : OnAssociadoCadastradoHandler, IHandler<object> {

        //Atributos

        //Propridades
       
        //Chamador das ações do evento
        public override void execute(object source) {
            try {

                this.OAssociado = source as Associado;

                this.OAssociado.TipoAssociado = new TipoAssociadoBL().carregar(UtilNumber.toInt32(OAssociado.idTipoAssociado), this.OAssociado.idOrganizacao);

                this.gerarOcorrencia();

                this.gerarTaxaInscricao();

                this.iniciarProcessoAdmissao();

                this.gerarContribuicao();

                this.dispararEmail();

                this.gerarSaldoInicial();

            } catch (Exception ex) {
                UtilLog.saveError(ex, $"Erro no manipulador do evento de cadastro do associado {this.OAssociado.Pessoa.nome}");
            }
        }        
    }
}