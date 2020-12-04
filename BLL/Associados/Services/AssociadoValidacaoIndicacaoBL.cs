using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Associados;

namespace BLL.Associados {

    public class AssociadoValidacaoIndicacaoBL : DefaultBL, IAssociadoValidacaoIndicacaoBL {
        
        //Validar rota do membro responsável pela indicação através da url  
        public UtilRetorno carregarMembroIndicacao(string rotaConta, int? idOrganizacaoParam = null) {
            
            UtilRetorno ORetorno = new UtilRetorno{ flagError =  false };
            
            if (rotaConta.isEmpty()){
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Não foi possível localizar o membro responsável pela indicação!");

                return ORetorno;
            }
            
            var query = from Ass in db.Associado
                        where                                                         
                             !Ass.dtExclusao.HasValue && Ass.ativo == "S"
                        select Ass;
            
            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }
            
            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }
            
            query = query.Where(x => x.rotaConta == rotaConta);
            
            var listaResultados = query.Select(x => new{
                x.id,
                x.idOrganizacao,
                x.idPessoa,
                Pessoa = new{
                    x.Pessoa.nome,
                    x.Pessoa.nroDocumento
                }
            }).ToListJsonObject<Associado>() ?? new List<Associado>();
            
            if (!listaResultados.Any()){
                
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Não foi possível localizar o membro responsável pela indicação!");

                return ORetorno;
            }
            
            if (listaResultados.Count > 1){
                
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Não foi possível localizar o membro responsável pela indicação!");

                return ORetorno;
            }
            
            ORetorno.info = listaResultados.FirstOrDefault();
                        
            return ORetorno;

        }
        
        //Validar membro responsável pela indicação através do id  
        public UtilRetorno carregarMembroIndicacao(int id, int? idOrganizacaoParam = null) {
            
            UtilRetorno ORetorno = new UtilRetorno{ flagError =  false };
            
            if (id.toInt() == 0){
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Não foi possível localizar o membro responsável pela indicação!");

                return ORetorno;
            }
            
            var query = from Ass in db.Associado
                        where                                                         
                             !Ass.dtExclusao.HasValue && Ass.ativo == "S"
                        select Ass;
            
            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }
            
            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }
            
            query = query.Where(x => x.id == id);
            
            var listaResultados = query.Select(x => new{
                x.id,
                x.idOrganizacao,
                x.idPessoa,
                x.rotaConta,
                Pessoa = new{
                    x.Pessoa.nome,
                    x.Pessoa.nroDocumento
                }
            }).ToListJsonObject<Associado>() ?? new List<Associado>();
            
            if (!listaResultados.Any()){
                
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Não foi possível localizar o membro responsável pela indicação!");

                return ORetorno;
            }
            
            if (listaResultados.Count > 1){
                
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Não foi possível localizar o membro responsável pela indicação!");

                return ORetorno;
            }
            
            ORetorno.info = listaResultados.FirstOrDefault();
                        
            return ORetorno;

        }

    }
}