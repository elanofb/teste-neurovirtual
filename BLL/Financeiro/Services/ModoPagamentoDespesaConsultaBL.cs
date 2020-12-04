using System;
using System.Linq;
using DAL.Financeiro;
using BLL.Services;

namespace BLL.Financeiro {

    public class ModoPagamentoDespesaConsultaBL: DefaultBL, IModoPagamentoDespesaConsultaBL {

        public const string keyCache = "modo_pagamento_despesa";

        //
        public ModoPagamentoDespesaConsultaBL() {
        }

        // 
        public IQueryable<ModoPagamentoDespesa> query() {

            var query = from Obj in db.ModoPagamentoDespesa
                where Obj.flagExcluido == false
                select Obj;

            return query;

        }

        //Carregamento de registro pelo ID
        public ModoPagamentoDespesa carregar(int id) {

            var query = this.query().condicoesSeguranca();

            return query.FirstOrDefault(x => x.id == id);

        }

        //Listagem de registros de acordo com filtros
        public IQueryable<ModoPagamentoDespesa> listar(bool? ativo) {

            var query = this.query().condicoesSeguranca();

            if (ativo.HasValue) {
                
                query = query.Where(x => x.ativo == ativo);
                
            }

            return query;
        }

    }
}