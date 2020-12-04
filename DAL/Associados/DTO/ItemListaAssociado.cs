using System;

namespace DAL.Associados.DTO {

    public class ItemListaAssociado {

        public int id { get; set; }

        public int? idPessoa { get; set; }

        public int? nroAssociado { get; set; }

        public int? idTipoAssociado { get; set; }
        
        public string descricaoTipoAssociado { get; set; }

        public byte? idTipoCadastro { get; set; }

        public string flagTipoPessoa { get; set; }

        public string nome { get; set; }

        public string razaoSocial { get; set; }

        public string nroDocumento { get; set; }

        public string email { get; set; }

        public string emailSecundario { get; set; }

        public string nroTelefone { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime? dtAdmissao { get; set; }

        public string ativo { get; set; }

        public string flagSituacaoContribuicao { get; set; }

        public bool? flagCarteirinha { get; set; }
    }
}