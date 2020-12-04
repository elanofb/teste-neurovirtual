namespace DAL.Repository.Base {

	public abstract class TableRepository<T> : GenericRepository<T> where T : class {

		public TableRepository()
			: this(null) {
		}

		public TableRepository(DataContext repositoryContext) {
			this._context = repositoryContext ?? new DataContext();
			this._objectset = this._context.Set<T>();
			//_objectset = _repository.CreateObjectSet<T>();
		}
	}
}