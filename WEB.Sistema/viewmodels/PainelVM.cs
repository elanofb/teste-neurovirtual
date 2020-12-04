using System.Collections.Generic;
using DAL.Procedures;
using DAL.Associados;
using DAL.Pedidos;
using DAL.Anuidades;
using BLL.Permissao;
using DAL.Permissao;
using System.Linq;

namespace WEB.ViewModels {

    public class PainelVM {

        //Atributos
        public Anuidade AnuidadeAtual { get; set; }
        public SpAnuidadeTotalizadores TotalizadorAnuidade { get; set; }
        public IAcessoRecursoGrupoBL _AcessoRecursoGrupoBL;

        public List<Associado> listaUltimosAssociados { get; set; }
        public List<Pedido> listaUltimosPedidos { get; set; }
        //public bool flagPermissaoNaoAssociados { get; set; }

        //Propriedades Servicos
        private IAcessoRecursoGrupoBL OAcessoRecursoGrupoBL => _AcessoRecursoGrupoBL = _AcessoRecursoGrupoBL ?? new AcessoRecursoGrupoBL();

        //
        public PainelVM() {

            this.listaUltimosAssociados = new List<Associado>();

            this.listaUltimosPedidos = new List<Pedido>();

            this.AnuidadeAtual = new Anuidade();

            //this.flagPermissaoNaoAssociados = (this.OAcessoRecursoGrupoBL.carregar(AcessoRecursoGrupoConst.NAO_ASSOCIADOS).ativo == "S");
        }
    }
}
