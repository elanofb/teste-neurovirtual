using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using DAL.Permissao.Security.Extensions;
using LinqKit;

namespace DAL.Repository.Base {

	public class GenericRepository<T> where T : class {

		public Expression<Func<T, bool>> defaultPredicate { get; set; }

		public DataContext _context = null;
		public IDbSet<T> _objectset = null;

		//
		public DataContext getDataContext() {
			this._context = this._context ?? new DataContext();
			return this._context;
		}

		//
		public IDbSet<T> getObjectSet() {
			return this._objectset;
		}

		//
		public T getSingle(int id) {
			try {
				var item = _objectset.Find(id);
				return item;
			} catch (Exception ex) {
				UtilLog.saveError(ex, this._objectset.ToString());
			}
			return null;
		}

		//
		public T getSingle(Expression<Func<T, bool>> whereCondition, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null) {
			try {
				var query = _objectset.AsExpandable().Where(whereCondition);
				if (orderBy != null) {
					return orderBy(query).FirstOrDefault<T>();
				} else {
					return query.FirstOrDefault<T>();
				}
			} catch (Exception ex) {
				UtilLog.saveError(ex, this._objectset.ToString());
			}
			return null;
		}

		//
		public IList<T> getAll() {
			try {
				var list = _objectset.AsExpandable().Where(this.defaultPredicate).ToList<T>();
				return list;
			} catch (Exception ex) {
				UtilLog.saveError(ex, this._objectset.ToString());
			}
			return null;
		}

		//
		public IList<T> getAll(Expression<Func<T, bool>> whereCondition, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null) {
			try {
				whereCondition = whereCondition.And(this.defaultPredicate);
				var query = _objectset.AsExpandable().Where(whereCondition);
				if (orderBy != null) {
					return orderBy(query).ToList<T>();
				}
				return query.ToList<T>();
			} catch (Exception ex) {
				UtilLog.saveError(ex, this._objectset.ToString());
			}
			return null;
		}

		//
		protected T add(T entity) {
			entity = this.setDefaultInsertValues(entity);
			return _objectset.Add(entity);
		}

		//
		protected virtual int update(T entity) {
			entity = this.setDefaultUpdateValues(entity);
			return this.saveChanges();
		}

		//
		protected virtual void     save(T entity, bool flagMessage = true) {
			var classType = entity.GetType();
			var id = classType.GetProperty("id").GetValue(entity, null);

			if (String.IsNullOrEmpty(id.ToString()) || id.ToString() == "0") {
				this.add(entity);
				this.saveChanges();
			} else {
				this.update(entity);
			}
		}

		//
		public virtual void attach(T entity) {
			this._objectset.Attach(entity);
		}

		protected void delete(T entity) {
			_objectset.Remove(entity);
		}

		//
		public virtual int saveChanges() {
			try {
				int result = this._context.SaveChanges();
				return result;
			} catch (DbEntityValidationException dbEx) {
				UtilLog.saveError(dbEx, this._objectset.ToString());
			} catch (Exception ex) {
				UtilLog.saveError(ex, this._objectset.ToString());
			}
			return 0;
		}

		//
		protected T setDefaultInsertValues(T entity) {
			var classType = typeof(T);

		    var User = HttpContextFactory.Current.User;
		    

			var fieldUsuarioCadastro = classType.GetProperty("idUsuarioCadastro");
			if (fieldUsuarioCadastro != null) {
				if (User.id() > 0) {
					int idUser = User.id();
					fieldUsuarioCadastro.SetValue(entity, idUser, null);
				}
			}

			var fieldDtCadastro = classType.GetProperty("dtCadastro");
			if (fieldDtCadastro != null ) {

                var fieldDtCadastroValue = fieldDtCadastro.GetValue(entity);

			    if (fieldDtCadastroValue == null ||  UtilDate.cast(fieldDtCadastroValue.ToString()) == DateTime.MinValue ) {
				    DateTime? today = DateTime.Now;
				    fieldDtCadastro.SetValue(entity, today, null);
			    }
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
				if(tipoProp.Equals("string")){
					fieldAtivo.SetValue(entity, "S", null);
				} else {
					fieldAtivo.SetValue(entity, true, null);
				}
			}

			var fieldExcluido = classType.GetProperty("flagExcluido");
			if (fieldExcluido != null) {
				string tipoProp = fieldExcluido.PropertyType.Name.ToLower();
				if(tipoProp.Equals("string")){
					fieldExcluido.SetValue(entity, "N", null);
				} else {
					fieldExcluido.SetValue(entity, false, null);
				}
			}

			var flagSistema = classType.GetProperty("flagSistema");
			if (flagSistema != null) {
				flagSistema.SetValue(entity, "N", null);
			}

			return entity;
		}

		//
		protected T setDefaultUpdateValues(T entity) {
			var classType = typeof(T);

            var User = HttpContextFactory.Current.User;

			var fieldUsuarioAlteracao = classType.GetProperty("idUsuarioAlteracao");
			if (fieldUsuarioAlteracao != null) {
				if (User.id() > 0) {
					int idUser = User.id();
					fieldUsuarioAlteracao.SetValue(entity, idUser, null);
				}
			}

			var fieldDtAlteracao = classType.GetProperty("dtAlteracao");
			if (fieldDtAlteracao != null) {
				DateTime? today = DateTime.Now;
				fieldDtAlteracao.SetValue(entity, today, null);
			}

			return entity;
		}

		//
		public void Dispose() {
			if (this._context != null) this._context.Dispose();
		}

		//
		public virtual bool exists(Expression<Func<T, bool>> predicate) {
			try {
				predicate = predicate.And(this.defaultPredicate);
				return this._objectset.AsExpandable().Any(predicate);
			} catch (Exception ex) {
				UtilLog.saveError(ex, this._objectset.ToString());
			}
			return true;
		}
	}
}