using System.Collections.Generic;
using DAL.Permissao;

namespace WEB.Areas.Associacoes.ViewModels {
    
    public class AssociacaoModulosContratadosVM {

        //
        public int idOrganizacao { get; set; }

        public List<AcessoRecursoGrupo> listaModulosAtivos { get; set; }

        public List<AcessoRecursoGrupo> listaModulosInativos { get; set; }


        public AssociacaoModulosContratadosVM() {
            
            this.listaModulosAtivos = new List<AcessoRecursoGrupo>();

            this.listaModulosInativos = new List<AcessoRecursoGrupo>();

        }
        
    }
}