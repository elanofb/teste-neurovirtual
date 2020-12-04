using System.Configuration;

namespace Textos {

    public static class RsAssociado {

		public static string labelEmailPrincipal => ConfigurationManager.AppSettings["labelEmailPrincipal"];

        public static string labelEmailSecundario => ConfigurationManager.AppSettings["labelEmailSecundario"];

        public static string legendaEmailPrincipal => ConfigurationManager.AppSettings["legendaEmailPrincipal"];

		public static string legendaEmailSecundario => ConfigurationManager.AppSettings["legendaEmailSecundario"];

		public static string labelEnderecoWEB => ConfigurationManager.AppSettings["labelEnderecoWEB"];

		public static string legendaEnderecoWEB => ConfigurationManager.AppSettings["legendaEnderecoWEB"];

        public static string labelEnderecoPrincipal => ConfigurationManager.AppSettings["labelEnderecoPrincipal"];

        public static string labelEnderecoSecundario => ConfigurationManager.AppSettings["labelEnderecoSecundario"];

        public static string labelAreaAtuacaoSingular => ConfigurationManager.AppSettings["labelAreaAtuacaoSingular"];

		public static string labelAreaAtuacaoPlural => ConfigurationManager.AppSettings["labelAreaAtuacaoPlural"];

        public static string textoPaginaRepresentante => ConfigurationManager.AppSettings["textoPaginaRepresentante"];

        public static string instrucoesCadastro => ConfigurationManager.AppSettings["instrucoesCadastro"];

        public static string instrucoesPosCadastro => ConfigurationManager.AppSettings["instrucoesPosCadastro"];

        public static string instrucoesAlteracaoCadastro => ConfigurationManager.AppSettings["instrucoesAlteracaoCadastro"];

        public static string labelNovoCadastroSocio => ConfigurationManager.AppSettings["labelNovoCadastroSocio"];    
            
        public static string labelNovoCadastroNaoAssociado => ConfigurationManager.AppSettings["labelNovoCadastroNaoAssociado"];

        public static string mensagemCadastroRealizado => ConfigurationManager.AppSettings["mensagemCadastroRealizado"];        

        public static string textoBeneficiosCadastroAssociado => ConfigurationManager.AppSettings["textoBeneficiosCadastroAssociado"];        

        
    }
}