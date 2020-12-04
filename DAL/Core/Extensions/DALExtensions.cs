using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Principal;
using DAL.Permissao.Security.Extensions;
using DAL.Repository.Base;

namespace System {

    public static class DALExtensions {

        //
        private static IPrincipal User = HttpContextFactory.Current.User;

        //
        public static T setDefaultInsertValues<T>(this T entity) {
            var User = HttpContextFactory.Current.User;

            var classType = typeof(T);

            var fieldId = classType.GetProperty("id");
            if (fieldId != null && fieldId.GetValue(entity) == null) {
                fieldId.SetValue(entity, null, null);
            }

            var fieldidOrganizacao = classType.GetProperty("idOrganizacao");
            if (fieldidOrganizacao != null) {
                fieldidOrganizacao.SetValue(entity, 1, null);
            }

            var fieldIdUnidade = classType.GetProperty("idUnidade");

            if (fieldIdUnidade != null) {

                if (User.idUnidade() > 0) {

                    fieldIdUnidade.SetValue(entity, User.idUnidade(), null);

                }
            }

            var fieldUsuarioCadastro = classType.GetProperty("idUsuarioCadastro");
            if (fieldUsuarioCadastro != null) {
                if (User.id() > 0) {
                    fieldUsuarioCadastro.SetValue(entity, User.id(), null);
                }
            }

            var fieldUsuarioAlteracao = classType.GetProperty("idUsuarioAlteracao");
            if (fieldUsuarioAlteracao != null) {
                if (User.id() > 0) {
                    fieldUsuarioAlteracao.SetValue(entity, User.id(), null);
                }
            }

            var fieldDtCadastro = classType.GetProperty("dtCadastro");
            if (fieldDtCadastro != null) {
                
                fieldDtCadastro.SetValue(entity, DateTime.Now, null);
            }

            var fieldDtAlteracao = classType.GetProperty("dtAlteracao");
            if (fieldDtAlteracao != null) {

                var fieldDtAlteracaoValue = fieldDtAlteracao.GetValue(entity);

                if (fieldDtAlteracaoValue == null || UtilDate.cast(fieldDtAlteracaoValue.ToString()) == DateTime.MinValue) {
                    DateTime? today = DateTime.Now;
                    fieldDtAlteracao.SetValue(entity, today, null);
                }
            }

            var fieldAtivo = classType.GetProperty("ativo");

            if (fieldAtivo != null && fieldAtivo.GetValue(entity) == null) {
                string tipoProp = fieldAtivo.PropertyType.Name.ToLower();
                if (tipoProp.Equals("string")) {
                    fieldAtivo.SetValue(entity, "S", null);
                } else {
                    fieldAtivo.SetValue(entity, true, null);
                }
            }

            var fieldExcluido = classType.GetProperty("flagExcluido");
            if (fieldExcluido != null) {

                string tipoProp = fieldExcluido.PropertyType.Name.ToLower();
                bool isEmpty = fieldExcluido.GetValue(entity).isEmpty();

                if (isEmpty) {

                    if (tipoProp.Equals("string") && isEmpty) {
                        fieldExcluido.SetValue(entity, "N", null);
                    } else {
                        fieldExcluido.SetValue(entity, false, null);
                    }

                }

            }

            var flagSistema = classType.GetProperty("flagSistema");
            if (flagSistema != null && flagSistema.GetValue(entity) == null) {
                string tipoProp = flagSistema.PropertyType.Name.ToLower();
                if (tipoProp.Equals("string")) {
                    flagSistema.SetValue(entity, "N", null);
                } else {
                    flagSistema.SetValue(entity, true, null);
                }
            }

            return entity;
        }

        public static T setDataUser<T>(this T entity, int idUsuario) {
            var classType = typeof(T);

            var fieldUsuarioCadastro = classType.GetProperty("idUsuarioCadastro");
            if (fieldUsuarioCadastro != null) {
                if (idUsuario > 0) {
                    fieldUsuarioCadastro.SetValue(entity, idUsuario, null);
                }
            }

            var fieldUsuarioAlteracao = classType.GetProperty("idUsuarioAlteracao");
            if (fieldUsuarioAlteracao != null) {
                if (idUsuario > 0) {
                    fieldUsuarioAlteracao.SetValue(entity, idUsuario, null);
                }
            }


            return entity;
        }

        //Inserir os valores padrão para uma entidade ser atualizada
        public static T setDefaultUpdateValues<T>(this T entity) {
            var classType = typeof(T);

            var User = HttpContextFactory.Current.User;

            var fieldUsuarioAlteracao = classType.GetProperty("idUsuarioAlteracao");
            if (fieldUsuarioAlteracao != null) {
                if (User.id() > 0) {
                    fieldUsuarioAlteracao.SetValue(entity, User.id(), null);
                }
            }

            var fieldDtAlteracao = classType.GetProperty("dtAlteracao");
            if (fieldDtAlteracao != null) {
                DateTime? today = DateTime.Now;
                fieldDtAlteracao.SetValue(entity, today, null);
            }

            var fieldExcluido = classType.GetProperty("flagExcluido");
            if (fieldExcluido != null) {
                string tipoProp = fieldExcluido.PropertyType.Name.ToLower();
                if (tipoProp.Equals("string")) {
                    fieldExcluido.SetValue(entity, "N", null);
                } else {
                    fieldExcluido.SetValue(entity, false, null);
                }
            }

            return entity;
        }

        //Inserir os valores padrão para uma entidade ser atualizada
        public static T setDefaultUpdateValues<T>(this T entity, ref DataContext db) {
            var classType = typeof(T);

            var User = HttpContextFactory.Current.User;

            var fieldUsuarioAlteracao = classType.GetProperty("idUsuarioAlteracao");
            if (fieldUsuarioAlteracao != null) {
                if (User.id() > 0) {
                    fieldUsuarioAlteracao.SetValue(entity, User.id(), null);
                }
            }

            var fieldDtAlteracao = classType.GetProperty("dtAlteracao");
            if (fieldDtAlteracao != null) {
                DateTime? today = DateTime.Now;
                fieldDtAlteracao.SetValue(entity, today, null);
            }

            var fieldAtivo = classType.GetProperty("ativo");
            if (fieldAtivo != null) {
                string tipoProp = fieldAtivo.PropertyType.Name.ToLower();
                if (tipoProp.Equals("string")) {
                    fieldAtivo.SetValue(entity, "S", null);
                } else {
                    fieldAtivo.SetValue(entity, true, null);
                }
            }

            var fieldExcluido = classType.GetProperty("flagExcluido");
            if (fieldExcluido != null) {
                string tipoProp = fieldExcluido.PropertyType.Name.ToLower();
                if (tipoProp.Equals("string")) {
                    fieldExcluido.SetValue(entity, "N", null);
                } else {
                    fieldExcluido.SetValue(entity, false, null);
                }
            }
            return entity;
        }

        //Igonrar campos que nao podem ser alterados
        public static void ignoreFields<T>(this DbEntityEntry<T> Entry, string[] fields = null) where T : class {
            
            PropertyInfo[] OPropriedades = Entry.Entity.GetType().GetProperties().ToArray();

            if (OPropriedades.Any(x => x.Name == "dtCadastro")) {
                Entry.Property("dtCadastro").IsModified = false;
            }

            if (OPropriedades.Any(x => x.Name == "idOrganizacao")) {
                Entry.Property("idOrganizacao").IsModified = false;
            }

            if (OPropriedades.Any(x => x.Name == "idUsuarioCadastro")) {
                Entry.Property("idUsuarioCadastro").IsModified = false;
            }

            if (OPropriedades.Any(x => x.Name == "flagExcluido")) {
                Entry.Property("flagExcluido").IsModified = false;
            }

            if (OPropriedades.Any(x => x.Name == "dtExclusao")) {
                Entry.Property("dtExclusao").IsModified = false;
            }

            if (OPropriedades.Any(x => x.Name == "idUsuarioExclusao")) {
                Entry.Property("idUsuarioExclusao").IsModified = false;
            }

            if (fields != null) {
                foreach (string fieldName in fields) {
                    Entry.Property(fieldName).IsModified = false;
                }
            }
        }

        public static IQueryable<T> condicoesSeguranca<T>(this IQueryable<T> entity) {

            var classType = typeof(T);
            var parameter = Expression.Parameter(classType);

            var idOrganizacao = User.idOrganizacao() == 0 ? null : (int?)User.idOrganizacao();

            if (classType.GetProperty("idOrganizacao") != null && idOrganizacao > 0) {

                var property = Expression.Convert(Expression.Property(parameter, "idOrganizacao"), typeof(int?));

                var propertyComparasion = Expression.Convert(Expression.Constant(idOrganizacao), typeof(int?));

                var comparison = Expression.Equal(property, propertyComparasion);

                entity = entity.Where(Expression.Lambda<Func<T, bool>>(comparison, parameter));
            }

            var idUnidade = User.idUnidade() == 0 ? null : (int?)User.idUnidade();

            if (classType.GetProperty("idUnidade") != null && idUnidade > 0) {

                var property = Expression.Convert(Expression.Property(parameter, "idUnidade"), typeof(int?));

                var propertyComparasion = Expression.Convert(Expression.Constant(idUnidade), typeof(int?));

                var comparison = Expression.Equal(property, propertyComparasion);

                entity = entity.Where(Expression.Lambda<Func<T, bool>>(comparison, parameter));
            }

            var flagSomenteCadastroProprio = User.flagSomenteCadastroProprio();

            if (classType.GetProperty("idRepresentante") != null && flagSomenteCadastroProprio) {

                var idUsuario = User.id();

                var property = Expression.Convert(Expression.Property(parameter, "idRepresentante"), typeof(int));

                var propertyComparasion = Expression.Convert(Expression.Constant(idUsuario), typeof(int));

                var comparison = Expression.Equal(property, propertyComparasion);

                entity = entity.Where(Expression.Lambda<Func<T, bool>>(comparison, parameter));
            }
            return entity;
        }
    }
}