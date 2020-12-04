using System;
using System.Linq;
using BLL.Services;
using DAL.Financeiro;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Financeiro {

    public class PeriodoRepeticaoBL: DefaultBL, IPeriodoRepeticaoBL {

        //
        public PeriodoRepeticaoBL() {
        }

        //Carregamento de registro pelo ID
        public PeriodoRepeticao carregar(int id) {

            return db.PeriodoRepeticao.Find(id);
        }

        //Listagem de registros de acordo com filtros
        public IQueryable<PeriodoRepeticao> listar(string valorBusca,string ativo) {
            
            var query = from P in db.PeriodoRepeticao
                        where P.flagExcluido == "N"
                        select P;

            if(!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca));
            }

            if(!String.IsNullOrEmpty(ativo)) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

        //Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
        public bool existe(string descricao,int id) {
            
            var query = from P in db.PeriodoRepeticao
                        where P.descricao == descricao && P.id != id && P.flagExcluido == "N"
                        select P;
            var OPeriodoRepeticao = query.Take(1).FirstOrDefault();
            return (OPeriodoRepeticao != null);
        }

        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(PeriodoRepeticao OTipoProduto) {

            if(OTipoProduto.id == 0) {
                return this.inserir(OTipoProduto);
            }

            return this.atualizar(OTipoProduto);
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(PeriodoRepeticao OPeriodoRepeticao) {

            OPeriodoRepeticao.setDefaultInsertValues<PeriodoRepeticao>();
            db.PeriodoRepeticao.Add(OPeriodoRepeticao);
            db.SaveChanges();

            return (OPeriodoRepeticao.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(PeriodoRepeticao OPeriodoRepeticao) {
            
            OPeriodoRepeticao.setDefaultUpdateValues<PeriodoRepeticao>();

            //Localizar existentes no banco
            PeriodoRepeticao dbTipoProduto = this.carregar(OPeriodoRepeticao.id);
            var TipoEntry = db.Entry(dbTipoProduto);
            TipoEntry.CurrentValues.SetValues(OPeriodoRepeticao);
            TipoEntry.ignoreFields<PeriodoRepeticao>();

            db.SaveChanges();
            return (OPeriodoRepeticao.id > 0);
        }

        //Remover um registro (exclusao lógica - nao remove-se fisicamente)
        public bool excluir(int id) {

            var idUsuario = User.id();

            db.PeriodoRepeticao
                .Where(x => x.id == id)
                .Update(x => new PeriodoRepeticao { flagExcluido = "S",dtAlteracao = DateTime.Now,idUsuarioAlteracao = idUsuario });

            return true;
        }

        //Retorna uma data acrescentando dias, meses ou anos de acordo com o periodo de repeticao.
        public DateTime getNewDatePeriodo(DateTime data, int idPeriodoRepeticao) { 
                
            DateTime newData = data;

            switch (idPeriodoRepeticao) { 

                case (int)TipoPeriodoRepeticaoEnum.SEMANALMENTE:
                    newData = newData.AddDays(7);
                    break;

                case (int)TipoPeriodoRepeticaoEnum.MENSALMENTE:
                    newData = newData.AddMonths(1);
                    break;

                case (int)TipoPeriodoRepeticaoEnum.BIMESTRALMENTE:
                    newData = newData.AddMonths(2);
                    break;

                case (int)TipoPeriodoRepeticaoEnum.TRIMESTRALMENTE:
                    newData = newData.AddMonths(3);
                    break;

                case (int)TipoPeriodoRepeticaoEnum.SEMESTRALMENTE:
                    newData = newData.AddMonths(6);
                    break;

                case (int)TipoPeriodoRepeticaoEnum.ANUALMENTE:
                    newData = newData.AddYears(1);
                    break;
            }

            return newData;
        }
    }
}