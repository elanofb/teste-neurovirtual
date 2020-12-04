using System;
using System.Data.Entity;
using System.Json;
using System.Linq;
using BLL.Permissao.Emails;
using BLL.Services;
using DAL.Documentos;
using DAL.Permissao;
using DAL.Pessoas;
using DAL.Enderecos;
using DAL.Permissao.Security.Extensions;
using UTIL.Resources;

namespace BLL.Permissao {

	public class UsuarioSistemaBL : DefaultBL, IUsuarioSistemaBL {
		
		//Atributos

		//Propriedades

        //Carregar registro pelo ID
        public UsuarioSistema carregar(int id) {

			var query = from Usuario in db.UsuarioSistema
                                                         .Include(x => x.Pessoa)
                                                         .Include(x => x.Pessoa.listaEnderecos)
                                                         .Include(x => x.PerfilAcesso)

						where  Usuario.id == id && 
                               Usuario.dtExclusao == null
						select Usuario;

			return query.FirstOrDefault();

		}

		//Listar os usuários do sistema
		public IQueryable<UsuarioSistema> listar(int idPerfilAcesso, string valorBusca, string ativo) {

			var query = from Usuario in db.UsuarioSistema.condicoesSeguranca()
                                                         .Include(x => x.PerfilAcesso)
						where Usuario.dtExclusao == null
						select Usuario;

			User.idUnidade();
            
			if (idPerfilAcesso > 0) {
				query = query.Where(x => x.idPerfilAcesso == idPerfilAcesso);
			}

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.Pessoa.nome.Contains(valorBusca) || x.login.Contains(valorBusca) || x.Pessoa.nroDocumento.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		//Salvar um novo registro ou atualizar um existente
		public virtual bool salvar(UsuarioSistema OUsuarioSistema) {

		    OUsuarioSistema.Pessoa.nome = OUsuarioSistema.Pessoa.nome.TrimEnd().TrimStart();

		    OUsuarioSistema.nome = OUsuarioSistema.Pessoa.nome;

		    OUsuarioSistema.login = OUsuarioSistema.login.TrimEnd().TrimStart();

		    OUsuarioSistema.Pessoa.emailPrincipal = UtilString.notNull(OUsuarioSistema.Pessoa.emailPrincipal).ToLower();

		    OUsuarioSistema.email = OUsuarioSistema.Pessoa.emailPrincipal;

		    OUsuarioSistema.Pessoa.emailSecundario = UtilString.notNull(OUsuarioSistema.Pessoa.emailSecundario).ToLower();

			OUsuarioSistema.Pessoa.idTipoDocumento = TipoDocumentoConst.CPF;

            OUsuarioSistema.Pessoa.nroDocumento = UtilString.onlyAlphaNumber(OUsuarioSistema.Pessoa.nroDocumento);

            OUsuarioSistema.Pessoa.nroTelPrincipal = UtilString.onlyNumber(UtilString.notNull(OUsuarioSistema.Pessoa.nroTelPrincipal));

            OUsuarioSistema.Pessoa.nroTelSecundario = UtilString.onlyNumber(UtilString.notNull(OUsuarioSistema.Pessoa.nroTelSecundario));

            OUsuarioSistema.Pessoa.nroTelTerciario = UtilString.onlyNumber(UtilString.notNull(OUsuarioSistema.Pessoa.nroTelTerciario));

            OUsuarioSistema.Pessoa.idTipoDocumento = TipoDocumentoConst.CPF;

			if(OUsuarioSistema.id == 0){	
				return this.inserir(OUsuarioSistema);
			}

			return this.atualizar(OUsuarioSistema);
		}

		//Persistir e inserir um novo registro 
		//Inserir Empresa, Pessoa e lista de Endereços vinculados
		private bool inserir(UsuarioSistema OUsuarioSistema) { 

            string senha = UtilString.randomString(8);

			OUsuarioSistema.senha = UtilCrypt.SHA512(senha);

		    OUsuarioSistema.flagAlterarSenha = "S";

			OUsuarioSistema.setDefaultInsertValues();

			OUsuarioSistema.Pessoa.setDefaultInsertValues();

            OUsuarioSistema.Pessoa.listaEnderecos.ForEach(x => {

                x.setDefaultInsertValues();

                x.idPais = "BRA";

                x.idTipoEndereco = TipoEnderecoConst.PRINCIPAL;

            });

            if(OUsuarioSistema.Pessoa.listaEnderecos != null && OUsuarioSistema.Pessoa.listaEnderecos.Count > 0) {

                OUsuarioSistema.Pessoa.listaEnderecos.ForEach((x) => x.setDefaultInsertValues());

                OUsuarioSistema.Pessoa.listaEnderecos.ForEach((x) => x.idTipoEndereco = TipoEnderecoConst.PRINCIPAL);

            }
            
			db.UsuarioSistema.Add(OUsuarioSistema);

            db.SaveChanges();

		    bool flagCastrado = OUsuarioSistema.id > 0;

		    if (flagCastrado) {

                IEnvioNovoUsuario OEmail = EnvioNovoUsuario.factory(OUsuarioSistema.idOrganizacao.toInt(), OUsuarioSistema.Pessoa.ToEmailsPessoa(), null);

                OEmail.enviar(OUsuarioSistema, senha);
            }

			return OUsuarioSistema.id > 0;
		}

		//Persistir e atualizar um registro existente 
		//Atualizar dados da Empresa, Pessoa e lista de endereços
		private bool atualizar(UsuarioSistema OUsuarioSistema) { 

			//Localizar existentes no banco
			UsuarioSistema dbRegistro = this.carregar(OUsuarioSistema.id);

            if (dbRegistro == null) {
                return false;
            }

			Pessoa dbPessoa = db.Pessoa.FirstOrDefault(x => x.id == dbRegistro.idPessoa);

			//Configurar valores padrão
			OUsuarioSistema.setDefaultUpdateValues();

			OUsuarioSistema.Pessoa.setDefaultUpdateValues();

            OUsuarioSistema.Pessoa.listaEnderecos.ForEach(x => {

                x.setDefaultInsertValues();

                x.idPais = "BRA";

                x.idTipoEndereco = TipoEnderecoConst.PRINCIPAL;

            });

            OUsuarioSistema.idPessoa= dbPessoa.id;

            OUsuarioSistema.Pessoa.id = dbPessoa.id;

            if (OUsuarioSistema.Pessoa.listaEnderecos != null && OUsuarioSistema.Pessoa.listaEnderecos.Count > 0) {

                OUsuarioSistema.Pessoa.listaEnderecos.ForEach((x) => x.setDefaultUpdateValues());
            }
            
            //Atualizacao da Empresa
            var RegistroEntry = db.Entry(dbRegistro);
			RegistroEntry.CurrentValues.SetValues(OUsuarioSistema);
			RegistroEntry.ignoreFields(new [] { "senha", "flagAlterarSenha" });

			//Atualizacao Dados Pessoa
			var PessoaEntry = db.Entry(dbPessoa);
			PessoaEntry.CurrentValues.SetValues(OUsuarioSistema.Pessoa);
			PessoaEntry.ignoreFields();
			
			db.SaveChanges();

			return OUsuarioSistema.id > 0;
		}

        //Alteracao de senha 
        public UtilRetorno alterarSenha(int idUsuario, string senha) {

            var User = this.carregar(idUsuario);

            string cryptSenha = UtilCrypt.SHA512(senha);

            User.senha = cryptSenha;

            User.flagAlterarSenha = "N";

            this.db.SaveChanges();

            return UtilRetorno.newInstance(false, "Senha alterada com sucesso.");
        }

        /// <summary>
        /// Ativar ou desativar um registro
        /// </summary>
        public JsonMessageStatus alterarStatus(int id) {

            var retorno = new JsonMessageStatus();

            var item = this.carregar(id);

            if (item == null) {
                retorno.error = true;
                retorno.message = NotificationMessages.invalid_register_id;
            } else {
                item.ativo = (item.ativo == "S" ? "N" : "S");
                db.SaveChanges();

                retorno.active = item.ativo == "S" ? "S" : "N";
                retorno.message = NotificationMessages.updateSuccess;
            }
            return retorno;
        }

        //Excluir registros
        public UtilRetorno excluir(int idUsuario, int idUsuarioExclusao) {

		    var OUsuario = this.carregar(idUsuario);

		    if (OUsuario == null) {
		        return UtilRetorno.newInstance(true, "O usuário informado não foi localizado.");
		    }

            OUsuario.dtExclusao = DateTime.Now;

		    OUsuario.idUsuarioExclusao = idUsuarioExclusao;

		    db.SaveChanges();

            return UtilRetorno.newInstance(false, "Registro removido com sucesso.");
		}

	}
}