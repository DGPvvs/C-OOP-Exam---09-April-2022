namespace Formula1.Repositories
{
	using Formula1.Models.Contracts;
	using System.Linq;

	public class FormulaOneCarRepository : Repository<IFormulaOneCar>
	{
		public override IFormulaOneCar FindByName(string name) => this.Models.FirstOrDefault(x => x.Model == name);
	}
}
