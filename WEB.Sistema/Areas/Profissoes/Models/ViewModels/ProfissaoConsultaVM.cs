using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Arquivos;
using BLL.Profissoes;
using BLL.Services;
using DAL.Profissoes;
using PagedList;

namespace WEB.Areas.Profissoes.ViewModels {


    public class ProfissaoConsultaVM {

        //Atributos
        private IProfissaoConsultaBL _IProfissaoConsultaBL;
        private IArquivoUploadFotoBL _IArquivoUploadFotoBL;
        
        //Propriedades
        private IProfissaoConsultaBL OProfissaoConsultaBL => _IProfissaoConsultaBL = _IProfissaoConsultaBL ?? new ProfissaoConsultaBL();
        private IArquivoUploadFotoBL OArquivoUploadFotoBL => _IArquivoUploadFotoBL = _IArquivoUploadFotoBL ?? new ArquivoUploadFotoBL();
        
        //Propriedades
        public IPagedList<Profissao> listaProfissoesPaged { get; set; }
        
        public List<Profissao> listaProfissoes { get; set; }

        //
        public ProfissaoConsultaVM() {

            this.listaProfissoesPaged = new List<Profissao>().ToPagedList(1, 20);
            this.listaProfissoes = new List<Profissao>();

        }

        public void carregar() {
            
            this.listaProfissoesPaged = this.listaProfissoes.OrderBy(x => x.descricao).ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());;
            
        }

        public void montarLista() {

            string valorBusca = UtilRequest.getString("valorBusca");
            
            bool? ativo = UtilRequest.getBool("flagAtivo");

            this.listaProfissoes = this.OProfissaoConsultaBL.listar(valorBusca, ativo).Select(x => new {
                x.id,
                x.descricao,
                x.dtCadastro,
                x.ativo
            }).ToListJsonObject<Profissao>();
                
        }

    }
    
}