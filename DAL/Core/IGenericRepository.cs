using System;

namespace DAL.Repository.Base {

	public interface IGenericRepository<T> : IDisposable where T : class {

		T add(T entity);

		int update(T entity);

		void save(T entity, bool flagMessage);

		void delete(T entity);

		void attach(T entity);

		int saveChanges();
	}
}