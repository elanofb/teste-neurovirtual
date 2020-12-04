using System;
using System.Linq;

using BLL.Associados.Interface;
using BLL.Services;

using DAL.Configuracoes;
using DAL.Repository.Base;

namespace BLL.Configuracoes.Interface.IConfiguracaoPromocaoBL {

    public class ConfiguracaoPromocaoCadastroBL : DefaultBL, IConfiguracaoPromocaoCadastroBL {
        public UtilRetorno salvar(ConfiguracaoPromocao NewObj) {
        
            if (NewObj.id == 0) {
                return inserir(NewObj);
            }
            return atualizar(NewObj);
        }

        private UtilRetorno inserir(ConfiguracaoPromocao NewObj) {
            var Retorno = new UtilRetorno();
            
            NewObj.setDefaultInsertValues<ConfiguracaoPromocao>();

            db.ConfiguracaoPromocao.Add(NewObj);

            db.SaveChanges();

            if (NewObj.id > 0) {
                Retorno.flagError = false;
                Retorno.listaErros.Add("Configuração Cadastrada com sucesso");
            } else {
                Retorno.flagError = true;
                Retorno.listaErros.Add("Ocorreu um erro ao persistir os dados");
            }

            return Retorno;
        }

        private UtilRetorno atualizar(ConfiguracaoPromocao New) {
            var Retorno = new UtilRetorno();

            var Old = db.ConfiguracaoPromocao.Find(New.id);

            New.setDefaultUpdateValues<ConfiguracaoPromocao>();

            var Entry = db.Entry(Old);

            Entry.CurrentValues.SetValues(New);
            Entry.ignoreFields<ConfiguracaoPromocao>();

            db.SaveChanges();

            if (New.id > 0) {
                Retorno.flagError = false;
                Retorno.listaErros.Add("Configuração Atualizada com sucesso");
            } else {
                Retorno.flagError = true;
                Retorno.listaErros.Add("Ocorreu um erro ao persistir os dados");
            }

            return Retorno;
        }
    }

}