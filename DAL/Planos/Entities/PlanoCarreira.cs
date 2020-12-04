using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Web.WebSockets;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.Planos {
    public class PlanoCarreira {
        
        public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public string descricao { get; set; }

        public string observacoes { get; set; }

        public int? pontuacao { get; set; }

        public DateTime dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public UsuarioSistema UsuarioSistema { get; set; }

        public bool? ativo { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

        public UsuarioSistema UsuarioExclusao { get; set; }                
        
    }

    //
    internal sealed class PlanoCarreiraMapper : EntityTypeConfiguration<PlanoCarreira> {
        public PlanoCarreiraMapper() {
            this.ToTable("tb_plano_carreira");

            this.HasKey(o => o.id);
            this.HasOptional(x => x.UsuarioSistema).WithMany().HasForeignKey(x => x.idUsuarioCadastro);
            this.HasOptional(x => x.UsuarioExclusao).WithMany().HasForeignKey(x => x.idUsuarioExclusao);
        }
    }
    
    
    
}