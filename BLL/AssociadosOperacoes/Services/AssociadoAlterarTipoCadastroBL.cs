using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Associados;
using BLL.LogsAlteracoes;
using BLL.Pessoas;
using BLL.Services;
using DAL.Associados;
using DAL.Associados.DTO;
using DAL.Entities;
using DAL.LogsAlteracoes;
using DAL.Pessoas;
using DAL.Relacionamentos;
using EntityFramework.Extensions;

namespace BLL.AssociadosOperacoes {

    public class AssociadoAlterarTipoCadastroBL : DefaultBL, IAssociadoAlterarTipoCadastroBL {

        // Atributos
        private IPessoaRelacionamentoBL _IPessoaRelacionamentoBL;
        private ITipoAssociadoBL _ITipoAssociadoBL;

        // Propriedades
        private IPessoaRelacionamentoBL OPessoaRelacionamentoBL => this._IPessoaRelacionamentoBL = this._IPessoaRelacionamentoBL ?? new PessoaRelacionamentoBL();
        private ITipoAssociadoBL OTipoAssociadoBL => this._ITipoAssociadoBL = this._ITipoAssociadoBL ?? new TipoAssociadoBL();

        //
        public UtilRetorno alterarTipoCadastro(List<ItemListaAssociado> listaAssociados, byte? idTipoCadastro, int? idTipoAssociado) {

            if(!listaAssociados.Any()) {
                return UtilRetorno.newInstance(true, "Não foi possível encontrar os membros informados. Tente novamente!");
            }

            this.salvarOcorrencias(listaAssociados, idTipoCadastro, idTipoAssociado);

            var idsAssociados = listaAssociados.Select(x => x.id).ToList();
            
            db.Associado.Where(x => idsAssociados.Contains(x.id)).Update(x => new Associado { idTipoAssociado = idTipoAssociado.toInt(), idTipoCadastro = idTipoCadastro.toByte() });

            db.SaveChanges();
            
            return UtilRetorno.newInstance(false, "Membros alterados com sucesso!");

        }

        private void salvarOcorrencias(List<ItemListaAssociado> listaAssociados, byte? idTipoCadastro, int? idTipoAssociado) {
            
            var OTipoAssociado = OTipoAssociadoBL.carregar(idTipoAssociado.toInt());
            
            foreach (var OAssociado in listaAssociados) {

                if (OAssociado.idTipoCadastro != idTipoCadastro) {
                    var OOcorrenciaTipoCadastro = new PessoaRelacionamento();

                    OOcorrenciaTipoCadastro.idPessoa = OAssociado.idPessoa.toInt();
                    OOcorrenciaTipoCadastro.idOcorrenciaRelacionamento = OcorrenciaRelacionamentoConst.idAlteracaoTipoCadastro;
                    OOcorrenciaTipoCadastro.dtOcorrencia = DateTime.Now;
                    OOcorrenciaTipoCadastro.observacao = $"Alteração Tipo de Cadastro de {TipoCadastroExtensions.descricaoTipoCadastro(OAssociado.idTipoCadastro.toInt())} para {TipoCadastroExtensions.descricaoTipoCadastro(idTipoCadastro.toInt())}";
                
                    OPessoaRelacionamentoBL.salvar(OOcorrenciaTipoCadastro);                    
                }

                if (OAssociado.idTipoAssociado != idTipoAssociado) {
                    var OOcorrenciaTipoAssociado = new PessoaRelacionamento();

                    OOcorrenciaTipoAssociado.idPessoa = OAssociado.idPessoa.toInt();
                    OOcorrenciaTipoAssociado.idOcorrenciaRelacionamento = OcorrenciaRelacionamentoConst.idAlteracaoTipoAssociado;
                    OOcorrenciaTipoAssociado.dtOcorrencia = DateTime.Now;
                    OOcorrenciaTipoAssociado.observacao = $"Alteração Tipo do Membro de {OAssociado.descricaoTipoAssociado} para {OTipoAssociado.descricao}";
                
                    OPessoaRelacionamentoBL.salvar(OOcorrenciaTipoAssociado);
                }

            }
            
        }

    }

}
