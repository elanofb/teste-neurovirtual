using System;
using System.Data.Entity;
using MySql.Data.Entity;


namespace DAL.Repository.Base {
    
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public partial class DataContextMySQL : DbContext {
          
        //Iniciar conexão com banco de dados
        public DataContextMySQL() : base("STMySQLConnection") {
        }
        
                
        //Aqui faz o mapeamento das tabelas e a conexão com o banco de dados
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            
            this.mapperModuloMigracao(modelBuilder);
                                        
            base.OnModelCreating(modelBuilder);

        }
        
    }
}
