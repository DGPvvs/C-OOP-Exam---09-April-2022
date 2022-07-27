namespace Formula1.Repositories
{
	using Formula1.Models.Contracts;
	using System.Linq;

	public class RaceRepository : Repository<IRace>
	{
		public override IRace FindByName(string name) => this.Models.FirstOrDefault(x => x.RaceName == name);
	}
}
