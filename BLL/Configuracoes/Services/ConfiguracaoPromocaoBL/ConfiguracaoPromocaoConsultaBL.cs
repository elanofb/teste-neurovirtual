using System;
using System.Linq;

using BLL.Services;

using DAL.Configuracoes;

namespace BLL.Configuracoes.Interface.IConfiguracaoPromocaoBL {

    public class ConfiguracaoPromocaoConsultaBL : DefaultBL, IConfiguracaoPromocaoConsultaBL {
        
        public IQueryable<ConfiguracaoPromocao> query(bool history = false) {
            
            var query = from ConfiguracaoPromocao in db.ConfiguracaoPromocao
                        select ConfiguracaoPromocao;
            
            if (history == false) {
                query = query.Where(Config => Config.dtExclusao == null);
            }
            
            return query;
            
        }
        
        public IQueryable<ConfiguracaoPromocao> listar(string valorBusca = "", 
                                                       bool history = false, 
                                                       DateTime? dtInicioPremio = null, 
                                                       DateTime? dtFimPremio = null) {
            var query = this.query(history);
            
            if (string.IsNullOrEmpty(valorBusca)) {
                query = query.Where(Config => Config.descricao.Contains(valorBusca));
            } 
            
            if (dtInicioPremio.HasValue) {
                query = query.Where(Config => Config.dtInicioPremioNovoMembro > dtInicioPremio);
            }
            
            if (dtFimPremio.HasValue) {
                query = query.Where(Config => Config.dtInicioPremioNovoMembro < dtFimPremio);
            }

            return query;
        }
        
        public ConfiguracaoPromocao carregar(int id) {
            var Obj = this.query().FirstOrDefault(Conf => Conf.id == id) 
                   ?? this.query().FirstOrDefault()
                   ?? new ConfiguracaoPromocao();
            
            return Obj;
        }
    }

}