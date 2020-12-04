using System;

namespace DAL.Associados.DTO {

    public class ItemListaAssociadoDesligado {

        public int id { get; set; }

        public int? idPessoa { get; set; }

        public int? nroAssociado { get; set; }

        public string descricaoTipoAssociado { get; set; }

        public string flagTipoPessoa { get; set; }

        public string nome { get; set; }

        public string razaoSocial { get; set; }

        public string nroDocumento { get; set; }

        public string email { get; set; }

        public string nroTelefone { get; set; }

        public DateTime dtCadastro { get; set; }

        public string ativo { get; set; }

        public string flagSituacaoContribuicao { get; set; }

        public DateTime? dtDesligamento { get; set; }

        public string motivoDesligamento { get; set; }

    }
}