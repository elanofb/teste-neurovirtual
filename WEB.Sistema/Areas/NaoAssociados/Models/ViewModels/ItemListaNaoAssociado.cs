using System;

namespace WEB.Areas.NaoAssociados.ViewModels {

    public class ItemListaNaoAssociado {

        public int id { get; set; }

        public int? idPessoa { get; set; }

        public int? nroAssociado { get; set; }

        public string descricaoTipoAssociado { get; set; }

        public string flagTipoPessoa { get; set; }

        public string nome { get; set; }

        public string razaoSocial { get; set; }

        public string nroDocumento { get; set; }

        public string nroTelefone { get; set; }

        public string email { get; set; }

        public DateTime dtCadastro { get; set; }

        public string ativo { get; set; }
    }
}