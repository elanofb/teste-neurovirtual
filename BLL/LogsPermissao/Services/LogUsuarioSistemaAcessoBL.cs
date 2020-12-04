using System;
using System.Linq;
using BLL.Services;
using DAL.LogsPermissao;
using System.Web;
using DAL.Permissao.Config;
using System.Data.Entity;
using System.Collections.Generic;
using DAL.Permissao.Security.Extensions;

namespace BLL.LogsPermissao {

    public class LogUsuarioSistemaAcessoBL : DefaultBL {
        
        public LogUsuarioSistemaAcessoBL(){
        }

        //Carregar registro a partir do ID
        public LogUsuarioSistemaAcesso carregar(int id) {

            var query = from Log in db.LogUsuarioSistemaAcesso
                            .Include(x => x.UsuarioSistema)
                        where Log.id == id
                        select Log;

            return query.FirstOrDefault();
        }

        //Listagem de registro a partir de parametros
        public IQueryable<LogUsuarioSistemaAcesso> listar(int idUsuario) {

            var query = from Log in db.LogUsuarioSistemaAcesso
                            .Include(x => x.UsuarioSistema)
                        select Log;

            if (idUsuario > 0) {
                query = query.Where(x => x.idUsuario == idUsuario);
            }


            return query;
        }

        //Salvar dados e imagem (se for enviado)
        public bool salvar(HttpContextBase OHttpContext, LogUsuarioSistemaAcesso OLogUsuarioSistemaAcesso) {

            OLogUsuarioSistemaAcesso.idSessao = OHttpContext.Session.SessionID;
            OLogUsuarioSistemaAcesso.dtAcesso = DateTime.Now;
            OLogUsuarioSistemaAcesso.ipAcesso = OHttpContext.Request.UserHostAddress;
            OLogUsuarioSistemaAcesso.browser = OHttpContext.Request.Browser.Browser;
            OLogUsuarioSistemaAcesso.sistemaOperacional = getUserAgentSo(OHttpContext.Request.UserAgent);

            return this.inserir(OLogUsuarioSistemaAcesso);
        }

        private bool inserir(LogUsuarioSistemaAcesso OLogUsuarioSistemaAcesso) {

            db.LogUsuarioSistemaAcesso.Add(OLogUsuarioSistemaAcesso);
            db.SaveChanges();

            return (OLogUsuarioSistemaAcesso.id > 0);
        }

        private string getUserAgentSo(string userAgentText) {

            Dictionary<string, string> osList = new Dictionary<string, string>
            {
                {"Windows NT 10.0", "Windows 10"},
                {"Windows NT 6.3", "Windows 8.1"},
                {"Windows NT 6.2", "Windows 8"},
                {"Windows NT 6.1", "Windows 7"},
                {"Windows NT 6.0", "Windows Vista"},
                {"Windows NT 5.2", "Windows Server 2003"},
                {"Windows NT 5.1", "Windows XP"},
                {"Windows NT 5.0", "Windows 2000"}
            };

            if (userAgentText != null) {

                int startPoint = userAgentText.IndexOf('(') + 1;

                int endPoint = userAgentText.IndexOf(';');

                int length = (endPoint - startPoint);

                if (length < 0) {

                    length = 0;
                }

                string osVersion = userAgentText.Substring(startPoint, length);

                if (osList.ContainsKey(osVersion)) {
                    return osList[osVersion];
                }

                return osVersion;
            }

            return "";
        }
    }
}