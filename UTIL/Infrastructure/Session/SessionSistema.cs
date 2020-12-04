using System.Web;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace System{

    public static class SessionSistema {

        private static readonly string prefixSession = "sistema_";

        //
        private static void setSession(string key, object value) {
            string definitiveKey = string.Concat(SessionSistema.prefixSession, key);
            HttpContext.Current.Session[definitiveKey] = value;
        }

        //
        private static object getSession(string key) {
            string definitiveKey = string.Concat(SessionSistema.prefixSession, key);
            return (HttpContext.Current == null || HttpContext.Current.Session == null || HttpContext.Current.Session[definitiveKey] == null) ? null : HttpContext.Current.Session[definitiveKey];
        }
        
        //
        public static void setUser(object User) { SessionSistema.setSession("user_logged", User); }
        public static object getUser(){ return SessionSistema.getSession("user_logged"); }

        public static void setIdUser(int idUsuario){ SessionSistema.setSession("idUser_logged", idUsuario); }
        public static int getIdUser(){ return UtilNumber.toInt32(SessionSistema.getSession("idUser_logged")); }

        public static void setIdPerfilUsuario(int idPerfil){ SessionSistema.setSession("idPerfilUser_logged", idPerfil); }
        public static int getIdPerfilUsuario(){ return UtilNumber.toInt32(SessionSistema.getSession("idPerfilUser_logged")); }

        public static void setListaGrupos<T>(List<T> listaGrupos){ 
			string json = JsonConvert.SerializeObject(listaGrupos);
			SessionSistema.setSession("listaGrupos", json); 
		}

        public static List<T> getListaGrupos<T>(){ 
			string json = (string)SessionSistema.getSession("listaGrupos");
			if(String.IsNullOrEmpty(json)){
				return new List<T>();
			}

			return JsonConvert.DeserializeObject<List<T>>(json);
		}

        public static void setListaRecursos<T>(List<T> listaRecursos){ 
			string json = JsonConvert.SerializeObject(listaRecursos);
			SessionSistema.setSession("listaRecursos", json); 
		}

        public static List<T> getListaRecursos<T>(){ 
			string json = (string)SessionSistema.getSession("listaRecursos");
			if(String.IsNullOrEmpty(json)){
				return new List<T>();
			}

			return JsonConvert.DeserializeObject<List<T>>(json);
		}


        public static void setListaPermissoes<T>(List<T> listaRecursos){ 
			string json = JsonConvert.SerializeObject(listaRecursos);
			SessionSistema.setSession("listaPermissoes", json); 
		}

        public static List<T> getListaPermissoes<T>(){ 
			string json = (string)SessionSistema.getSession("listaPermissoes");
			if(String.IsNullOrEmpty(json)){
				return new List<T>();
			}

			return JsonConvert.DeserializeObject<List<T>>(json);
		}

        //
        public static void setListClientes(List<int> listClientes) { SessionSistema.setSession("listClientes", listClientes); }
        public static List<int> getListClientes(){
            List<int> listClientes = new List<int>();
            if (SessionSistema.getSession("listClientes") != null) {
                listClientes = SessionSistema.getSession("listClientes") as List<int>;
            }
            return listClientes; 
        }
        //
        public static void setPermissoes(object permissoes) { SessionSistema.setSession("listPermissions", permissoes); }
        public static object getPermissoes(){ return SessionSistema.getSession("listPermissions"); }

    }
}
