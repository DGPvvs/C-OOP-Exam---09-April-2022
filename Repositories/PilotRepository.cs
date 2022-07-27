namespace Formula1.Repositories
{
	using Formula1.Models.Contracts;
	using System.Linq;

	public class PilotRepository : Repository<IPilot>
	{
		public override IPilot FindByName(string name) => this.Models.FirstOrDefault(x => x.FullName == name);
	}
}
