using System;

namespace DAL.Entities {

    //
    [Serializable]
    public class Geral {
        public int id { get; set; }

        public string descricao { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime dtAlteracao { get; set; }

        public int idUsuarioCadastro { get; set; }

        public int idUsuarioAlteracao { get; set; }

        public string ativo { get; set; }

        public string flagExcluido { get; set; }
    }

}