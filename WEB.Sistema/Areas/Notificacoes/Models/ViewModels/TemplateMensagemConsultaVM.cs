using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using BLL.Notificacoes;
using BLL.Services;
using DAL.Notificacoes;

namespace WEB.Areas.Notificacoes.ViewModels {


    public class TemplateMensagemConsultaVM {

        // Atributos Serviços
        private ITemplateMensagemConsultaBL _ITemplateMensagemConsultaBL;

        // Propriedades Serviços
        private ITemplateMensagemConsultaBL OTemplateMensagemConsultaBL => _ITemplateMensagemConsultaBL = _ITemplateMensagemConsultaBL ?? new TemplateMensagemConsultaBL();

        //Propriedades
        public List<TemplateMensagem> listaTodos { get; set; }
        
        public List<TemplateMensagem> listaAtivos { get; set; }
        
        public List<TemplateMensagem> listaDesativados { get; set; }
        
        // Constants
        private IPrincipal User => HttpContextFactory.Current.User;

        //
        public TemplateMensagemConsultaVM() {

            this.listaTodos = new List<TemplateMensagem>();
            
            this.listaAtivos = new List<TemplateMensagem>();
            
            this.listaDesativados = new List<TemplateMensagem>();
            
        }

        public void carregarInformacoes() {
            
            var query = this.montarQuery();
            
            this.listaTodos = this.filtrarCampos(query);

            if (!this.listaTodos.Any()) {
                return;
            }
            
            this.filtrarListas();
            
        }

        private IQueryable<TemplateMensagem> montarQuery() {

            string valorBusca = UtilRequest.getString("valorBusca");
            
            var query = this.OTemplateMensagemConsultaBL.listar(valorBusca, null);

            return query;

        }

        private List<TemplateMensagem> filtrarCampos(IQueryable<TemplateMensagem> query) {
            
            var listaFiltrada = query.Select(x => new {
                x.id,
                x.titulo,
                x.ativo,
                x.dtCadastro,
                x.dtExclusao
            }).OrderByDescending(x => x.id).ToListJsonObject<TemplateMensagem>();

            return listaFiltrada;

        }
        
        private void filtrarListas() {
            
            this.listaAtivos = this.listaTodos.Where(x => x.ativo == true).ToList();
            
            this.listaDesativados = this.listaTodos.Where(x => x.ativo == false).ToList();

        }

    }
    
}