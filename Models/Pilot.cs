namespace Formula1.Models
{
	using Formula1.Models.Contracts;
	using Formula1.Utilities;
	using System;

	public class Pilot : IPilot
	{
		private string fullName;
		private IFormulaOneCar car;
		private int numberOfWins;

		public Pilot(string fullName)
		{
			this.FullName = fullName;
			this.NumberOfWins = 0;
		}

		public string FullName
		{
			get => this.fullName;
			private set
			{
				if (string.IsNullOrWhiteSpace(value) || value.Length < 5)
				{
					throw new ArgumentException(string.Format(ExceptionMessages.InvalidPilot, value));
				}

				this.fullName = value;
			}
		}

		public IFormulaOneCar Car
		{
			get => this.car;
			private set
			{
				if (value == null)
				{
					throw new NullReferenceException(ExceptionMessages.InvalidCarForPilot);
				}

				this.car = value;
			}
		}

		public int NumberOfWins
		{
			get => this.numberOfWins;
			private set => this.numberOfWins = value;
		}

		public bool CanRace => this.Car != null;

		public void AddCar(IFormulaOneCar car) => this.Car = car;

		public void WinRace() => this.NumberOfWins += 1;

		public override string ToString() => $"Pilot {this.FullName} has {this.NumberOfWins} wins.";		
	}
}
