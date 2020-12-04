using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

using DAL.Arquivos;
using DAL.MeiosDivulgacao;
using DAL.Organizacoes;
using DAL.Permissao;
using DAL.Pessoas;
using DAL.Planos;
using DAL.Unidades;

namespace DAL.Associados {

    [Serializable]
    public class Associado {
        public int       id                            { get; set; }
        public int?      idOrigem                      { get; set; }
        public int       idOrganizacao                 { get; set; }
        public int?      idUnidade                     { get; set; }
        public int       idPessoa                      { get; set; }
        public int       idTipoAssociado               { get; set; }
        public byte      idTipoCadastro                { get; set; }
        public int?      nroAssociado                  { get; set; }
        public int?      idIndicador                   { get; set; }
        public int?      idIndicadorSegundoNivel       { get; set; }
        public int?      idIndicadorTerceiroNivel      { get; set; }
        public int?      idPlanoCarreira               { get; set; }
        public decimal?  percentualDesconto            { get; set; }
        public DateTime? dtAdmissao                    { get; set; }
        public int?      idUsuarioAdmissao             { get; set; }
        public DateTime? dtUltimoPagamentoContribuicao { get; set; }
        public DateTime? dtProximoVencimento           { get; set; }
        public DateTime? dtImportacao                  { get; set; }
        public int?      idMeioDivulgacao              { get; set; }
        public DateTime  dtCadastro                    { get; set; }
        public int?      idUsuarioCadastro             { get; set; }
        public DateTime  dtAlteracao                   { get; set; }
        public int?      idUsuarioAlteracao            { get; set; }
        public DateTime? dtDesativacao                 { get; set; }
        public int?      idUsuarioDesativacao          { get; set; }
        public int?      idMotivoDesativacao           { get; set; }
        public DateTime? dtReativacao                  { get; set; }
        public int?      idUsuarioReativacao           { get; set; }
        public DateTime? dtExclusao                    { get; set; }
        public int?      idUsuarioExclusao             { get; set; }
        public int?      idMotivoDesligamento          { get; set; }
        public DateTime? dtAprovacaoDocumento          { get; set; }
        public int?      idUsuarioAprovacaoDocumento   { get; set; }
        public string    observacaoDesligamento        { get; set; }
        public string    senhaTransacao                { get; set; }
        public string    ativo                         { get; set; }
        public string    flagInformativosOnline        { get; set; }
        public string    codigoIndicador               { get; set; }
        public string    nroDocumentoIndicador         { get; set; }
        public string    rotaConta                     { get; set; }
        public string    observacoes                   { get; set; }
        public string    outrasInformacoes             { get; set; }
        public string    dadoCustomizado01             { get; set; }
        public string    dadoCustomizado02             { get; set; }
        public string    dadoCustomizado03             { get; set; }
        public string    observacaoDesativacao         { get; set; }

        public Organizacao        Organizacao            { get; set; }
        public Unidade            Unidade                { get; set; }
        public Associado          Indicador              { get; set; }
        public Associado          IndicadorSegundoNivel  { get; set; }
        public Associado          IndicadorTerceiroNivel { get; set; }
        public PlanoCarreira      PlanoCarreira          { get; set; }
        public MeioDivulgacao     MeioDivulgacao         { get; set; }
        public MotivoDesativacao  MotivoDesativacao      { get; set; }
        public MotivoDesligamento MotivoDesligamento     { get; set; }
        public ArquivoUpload      Foto                   { get; set; }

        public virtual Pessoa         Pessoa                    { get; set; }
        public virtual TipoAssociado  TipoAssociado             { get; set; }
        public virtual UsuarioSistema UsuarioCadastro           { get; set; }
        public virtual UsuarioSistema UsuarioAdmissao           { get; set; }
        public virtual UsuarioSistema UsuarioAprovacaoDocumento { get; set; }
    }

    internal sealed class AssociadoMapper : EntityTypeConfiguration<Associado> {
        public AssociadoMapper() {
            this.ToTable("tb_associado");

            this.HasKey(o => o.id);

            this.Ignore(x => x.Foto);

            this.HasRequired(o => o.Organizacao).WithMany().HasForeignKey(o => o.idOrganizacao);
            this.HasOptional(o => o.Unidade).WithMany().HasForeignKey(o => o.idUnidade);
            this.HasOptional(o => o.Indicador).WithMany().HasForeignKey(o => o.idIndicador);
            this.HasOptional(o => o.IndicadorSegundoNivel).WithMany().HasForeignKey(o => o.idIndicadorSegundoNivel);
            this.HasOptional(o => o.IndicadorTerceiroNivel).WithMany().HasForeignKey(o => o.idIndicadorTerceiroNivel);
            this.HasOptional(o => o.PlanoCarreira).WithMany().HasForeignKey(o => o.idPlanoCarreira);
            this.HasRequired(o => o.Pessoa).WithMany().HasForeignKey(o => o.idPessoa);
            this.HasRequired(o => o.TipoAssociado).WithMany().HasForeignKey(o => o.idTipoAssociado);
            this.HasOptional(o => o.UsuarioCadastro).WithMany().HasForeignKey(o => o.idUsuarioCadastro);
            this.HasOptional(o => o.UsuarioAdmissao).WithMany().HasForeignKey(o => o.idUsuarioAdmissao);
            this.HasOptional(o => o.UsuarioAprovacaoDocumento).WithMany().HasForeignKey(o => o.idUsuarioAdmissao);
            this.HasOptional(o => o.MeioDivulgacao).WithMany().HasForeignKey(o => o.idMeioDivulgacao);
            this.HasOptional(o => o.MotivoDesativacao).WithMany().HasForeignKey(o => o.idMotivoDesativacao);
            this.HasOptional(o => o.MotivoDesligamento).WithMany().HasForeignKey(o => o.idMotivoDesligamento);

            this.Property(o => o.ativo).HasMaxLength(1);
            this.Property(o => o.observacaoDesligamento).HasMaxLength(255);
            this.Property(o => o.senhaTransacao).HasMaxLength(128);
            this.Property(o => o.observacaoDesativacao).HasMaxLength(255);
            this.Property(o => o.flagInformativosOnline).HasMaxLength(1);
            this.Property(o => o.codigoIndicador).HasMaxLength(50);
            this.Property(o => o.nroDocumentoIndicador).HasMaxLength(30);
            this.Property(o => o.rotaConta).HasMaxLength(20);
            this.Property(o => o.observacoes).HasMaxLength(1000);
            this.Property(o => o.outrasInformacoes).HasMaxLength(1000);
            this.Property(o => o.dadoCustomizado01).HasMaxLength(100);
            this.Property(o => o.dadoCustomizado02).HasMaxLength(100);
            this.Property(o => o.dadoCustomizado03).HasMaxLength(100);
        }
    }

}