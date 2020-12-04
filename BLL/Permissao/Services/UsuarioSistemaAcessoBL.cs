using System;
using System.Data.Entity;
using System.Json;
using System.Linq;
using BLL.Permissao.Emails;
using BLL.Services;
using DAL.Pessoas;
using BLL.UsuariosUnidades;
using DAL.Organizacoes;
using DAL.Permissao;

namespace BLL.Permissao {

    public class UsuarioSistemaAcessoBL : DefaultBL, IUsuarioSistemaAcessoBL {

        //Atributos
        private IUsuarioUnidadeBL _UsuarioUnidadeBL;

        //Propriedades
        private IUsuarioUnidadeBL OUsuarioUnidadeBL => this._UsuarioUnidadeBL = this._UsuarioUnidadeBL ?? new UsuarioUnidadeBL();


        // 1 - Receber os dados informados pelo usuário
        // 2 - Criptografar a senha
        // 3 - Realizar a busca no banco de dados com base na senha e no login
        // 4 - Verificar se o usuário está ativo
        public UtilRetorno login(string login, string senha) {

            string encryptSenha = UtilCrypt.SHA512(senha);

            var OUsuario = (from Usu in db.UsuarioSistema
                            where
                              Usu.login == login &&
                              Usu.senha == encryptSenha &&
                              Usu.dtExclusao == null
                            select Usu).Include(x => x.Organizacao).FirstOrDefault();

            if (OUsuario == null) {
                return UtilRetorno.newInstance(true, "Os dados de acesso são inválidos.");
            }

            if (OUsuario.ativo == "N") {
                return UtilRetorno.newInstance(true, "O usuário informado não está ativo no momento.", OUsuario);
            }

            if (OUsuario.idPerfilAcesso != PerfilAcessoConst.DESENVOLVEDOR) {
                if (OUsuario.Organizacao.ativo != true) {
                    return UtilRetorno.newInstance(true, "A associação do usuário não esta ativa no momento.", OUsuario);
                }

                if (OUsuario.Organizacao.idStatusOrganizacao == StatusOrganizacaoConst.BLOQUEADO){
                    return UtilRetorno.newInstance(true, "A sua associação não está ativa no momento, por favor, entre em contato com o gerente de sua assinatura.", OUsuario);
                }

                if (OUsuario.Organizacao.idStatusOrganizacao == StatusOrganizacaoConst.DESATIVADO){
                    return UtilRetorno.newInstance(true, "A sua associação não está ativa no momento, por favor, entre em contato com o gerente de sua assinatura.", OUsuario);
                }

                if (OUsuario.Organizacao.idStatusOrganizacao == StatusOrganizacaoConst.SUSPENSAO_FINANCEIRA){
                    return UtilRetorno.newInstance(true, "Não foi possível acessar o sistema, por favor, entre em contato com o departamento financeiro.", OUsuario);
                }
            }
            
            if (OUsuario.PerfilAcesso.flagTodasUnidades != true) {
                var flagUnidadeConfigurada = this.OUsuarioUnidadeBL.listar(OUsuario.id, null).Any();

                if (!flagUnidadeConfigurada) {
                    return UtilRetorno.newInstance(true, "O usuário informado não possui unidade configurada.", OUsuario);
                }
            }

            if (OUsuario.isDegustacao() && !OUsuario.periodoDegustacaoAtivo()) {

                if (OUsuario.dtFimDegustacao < DateTime.Today) {
                    return UtilRetorno.newInstance(true, "O seu período de degustação expirou.", OUsuario);
                }

                if (OUsuario.dtInicioDegustacao > DateTime.Today) {
                    return UtilRetorno.newInstance(true, "O seu acesso temporário ainda não está disponível.", OUsuario);
                }

                return UtilRetorno.newInstance(true, "Houve algum problema com o seu acesso temporário. Tente novamente mais tarde.", OUsuario);
            }

            return UtilRetorno.newInstance(false, "Dados de acesso validados com sucesso.", OUsuario);
        }

      
        //Rotina para recuperacao de senha do usuario
        public UtilRetorno recuperarSenha(string login) {

            var OUsuario = (from Usu in this.db.UsuarioSistema
                            where
                                Usu.login == login && 
                                Usu.dtExclusao == null
                            select Usu).FirstOrDefault();

            if (OUsuario == null) {
                return UtilRetorno.newInstance(true, "Nenhum usuário encontrado com os dados informados.");
            }

            if (OUsuario.ativo == "N") {
                return UtilRetorno.newInstance(true, "O usuário vinculado à essa conta está desativado no momento, entre em contato com a administração.");
            }

            string novaSenha = UtilString.randomString(8);

            OUsuario.senha = UtilCrypt.SHA512(novaSenha);

            OUsuario.flagAlterarSenha = "S";

            db.SaveChanges();

            IEnvioNovoUsuario OEmail = RecuperacaoSenhaUsuario.factory(OUsuario.idOrganizacao.toInt(), OUsuario.Pessoa.ToEmailsPessoa(), null);

            OEmail.enviar(OUsuario, novaSenha);

            return UtilRetorno.newInstance(false, "Uma nova senha foi reenviada para os e-mails vinculados à essa conta.");
        }

        //Geracao de uma nova senha para o usuario
        public UtilRetorno criarNovaSenha(int idUsuario) {

            var OUsuario = (from Usu in this.db.UsuarioSistema
                            where
                                Usu.id == idUsuario && Usu.dtExclusao == null
                            select Usu).FirstOrDefault();

            if (OUsuario == null) {
                return UtilRetorno.newInstance(true, "Desculpe, esse usuário não pôde ser localizado.");
            }


            string novaSenha = UtilString.randomString(8);

            OUsuario.senha = UtilCrypt.SHA512(novaSenha);

            OUsuario.flagAlterarSenha = "S";

            db.SaveChanges();

            IEnvioNovoUsuario OEmail = ReenvioSenhaUsuario.factory(OUsuario.idOrganizacao.toInt(), OUsuario.Pessoa.ToEmailsPessoa(), null);

            OEmail.enviar(OUsuario, novaSenha);

            return UtilRetorno.newInstance(false, "Uma nova senha foi enviada para o e-mail vinculado à conta.");
        }
    }
}