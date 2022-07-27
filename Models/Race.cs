namespace Formula1.Models
{
	using Formula1.Models.Contracts;
	using Formula1.Utilities;
	using System;
	using System.Collections.Generic;
	using System.Text;

	public class Race : IRace
	{
		private string raceName;
		private int numberOfLaps;
		private bool tookPlace;
		private ICollection<IPilot> pilots;		

		public Race(string raceName, int numberOfLaps)
		{
			this.RaceName = raceName;
			this.NumberOfLaps = numberOfLaps;
			this.TookPlace = false;
			this.pilots = new List<IPilot>();
		}

		public string RaceName
		{
			get => this.raceName;
			private set
			{
				if (string.IsNullOrWhiteSpace(value) || value.Length < 5)
				{
					throw new ArgumentException(string.Format(ExceptionMessages.InvalidRaceName, value));
				}

				this.raceName = value;
			}
		}

		public int NumberOfLaps
		{
			get => this.numberOfLaps;
			private set
			{
				if (value < 1)
				{
					throw new ArgumentException(string.Format(ExceptionMessages.InvalidLapNumbers, value));
				}

				this.numberOfLaps = value;
			}
		}

		public bool TookPlace
		{
			get => this.tookPlace;
			set => this.tookPlace = value;
		}

		public ICollection<IPilot> Pilots => this.pilots;

		public void AddPilot(IPilot pilot)
		{
			this.pilots.Add(pilot);
		}

		public string RaceInfo()
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine($"The {this.RaceName} race has:").AppendLine($"Participants: {this.Pilots.Count}").AppendLine($"Number of laps: {this.NumberOfLaps}");
			sb.AppendLine($"Took place: {(this.TookPlace ? "Yes" : "No")}");
			return sb.ToString().TrimEnd();
		}
	}
}
