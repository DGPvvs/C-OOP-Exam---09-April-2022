namespace Formula1.Repositories
{
	using Formula1.Repositories.Contracts;
	using System.Collections.Generic;

	public abstract class Repository<T> : IRepository<T>
	{
		private List<T> models;

		public Repository()
		{
			this.models = new List<T>();
		}

		public IReadOnlyCollection<T> Models => this.models.AsReadOnly();

		public void Add(T model) => this.models.Add(model);

		public abstract T FindByName(string name);

		public bool Remove(T model) => this.models.Remove(model);
	}
}
