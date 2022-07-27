namespace Formula1.Core
{
	using Formula1.Core.Contracts;
	using Formula1.Models;
	using Formula1.Models.Contracts;
	using Formula1.Repositories;
	using Formula1.Utilities;
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.Text;

	public class Controller : IController
	{
		private PilotRepository pilotRepository;
		private RaceRepository raceRepository;
		private FormulaOneCarRepository carRepository;		

		public Controller()
		{
			this.pilotRepository = new PilotRepository();
			this.raceRepository = new RaceRepository();
			this.carRepository = new FormulaOneCarRepository();
		}

		public string CreatePilot(string fullName)
		{
			IPilot pilot = this.pilotRepository.FindByName(fullName);

			if (pilot != null)
			{
				throw new InvalidOperationException(string.Format(ExceptionMessages.PilotExistErrorMessage, fullName));
			}

			this.pilotRepository.Add(new Pilot(fullName));

			return string.Format(OutputMessages.SuccessfullyCreatePilot, fullName);
		}

		public string CreateCar(string type, string model, int horsepower, double engineDisplacement)
		{
			IFormulaOneCar car = this.carRepository.FindByName(model);

			if (car != null)
			{
				throw new InvalidOperationException(string.Format(ExceptionMessages.CarExistErrorMessage, model));
			}

			if (type == "Ferrari")
			{
				car = new Ferrari(model, horsepower, engineDisplacement);
			}
			else if (type == "Williams")
			{
				car = new Williams(model, horsepower, engineDisplacement);
			}
			else
			{
				throw new InvalidOperationException(string.Format(ExceptionMessages.InvalidTypeCar, type));
			}

			this.carRepository.Add(car);

			return string.Format(OutputMessages.SuccessfullyCreateCar, car.GetType().Name, car.Model);
		}

		public string CreateRace(string raceName, int numberOfLaps)
		{
			IRace race = this.raceRepository.FindByName(raceName);

			if (race != null)
			{
				throw new InvalidOperationException(string.Format(ExceptionMessages.RaceExistErrorMessage, raceName));
			}

			race = new Race(raceName, numberOfLaps);

			this.raceRepository.Add(race);
			
			return string.Format(OutputMessages.SuccessfullyCreateRace, race.RaceName);
		}

		public string AddCarToPilot(string pilotName, string carModel)
		{
			IPilot pilot = this.pilotRepository.FindByName(pilotName);

			if (pilot == null || pilot.CanRace)
			{
				throw new InvalidOperationException(string.Format(ExceptionMessages.PilotDoesNotExistOrHasCarErrorMessage, pilotName));
			}

			IFormulaOneCar car = this.carRepository.FindByName(carModel);

			if (car == null)
			{
				throw new NullReferenceException(string.Format(ExceptionMessages.CarDoesNotExistErrorMessage, carModel));
			}

			pilot.AddCar(car);
			this.carRepository.Remove(car);

			return string.Format(OutputMessages.SuccessfullyPilotToCar, pilot.FullName, car.GetType().Name, car.Model);
			//return string.Format(OutputMessages.SuccessfullyPilotToCar, "pilot.FullName", "car.GetType().Name", "car.Model");
		}

		public string AddPilotToRace(string raceName, string pilotFullName)
		{
			IPilot pilot = this.pilotRepository.FindByName(pilotFullName);
			IRace race = this.raceRepository.FindByName(raceName);

			if (race == null)
			{
				throw new NullReferenceException(string.Format(ExceptionMessages.RaceDoesNotExistErrorMessage, raceName));
			}

			ICollection<IPilot> duplicatePilot = race.Pilots.Where(x => x.FullName == pilotFullName).ToList();

			if (pilot == null || !pilot.CanRace || duplicatePilot.Count > 0)
			{
				throw new InvalidOperationException(string.Format(ExceptionMessages.PilotDoesNotExistErrorMessage, pilotFullName));
			}

			race.AddPilot(pilot);

			return string.Format(OutputMessages.SuccessfullyAddPilotToRace, pilot.FullName, race.RaceName);
			//return string.Format(OutputMessages.SuccessfullyAddPilotToRace, "4", "4");
		}

		public string StartRace(string raceName)
		{
			IRace race = this.raceRepository.FindByName(raceName);

			if (race == null)
			{
				throw new NullReferenceException(string.Format(ExceptionMessages.RaceDoesNotExistErrorMessage, raceName));
			}

			if (race.TookPlace)
			{
				throw new InvalidOperationException(string.Format(ExceptionMessages.RaceTookPlaceErrorMessage, race.RaceName));
			}

			if (race.Pilots.Count < 3)
			{
				throw new InvalidOperationException(string.Format(ExceptionMessages.InvalidRaceParticipants, race.RaceName));
			}

			List<IPilot> sortedPilot = race.Pilots.OrderByDescending(x => x.Car.RaceScoreCalculator(race.NumberOfLaps)).ToList();

			sortedPilot[0].WinRace();
			race.TookPlace = true;

			StringBuilder sb = new StringBuilder();

			sb.AppendLine(string.Format(OutputMessages.PilotFirstPlace, sortedPilot[0].FullName, raceName));
			sb.AppendLine(string.Format(OutputMessages.PilotSecondPlace, sortedPilot[1].FullName, raceName));
			sb.AppendLine(string.Format(OutputMessages.PilotThirdPlace, sortedPilot[2].FullName, raceName));

			return sb.ToString().TrimEnd();
		}

		public string RaceReport()
		{
			List<IRace> list = this.raceRepository.Models.Where(x => x.TookPlace).ToList();

			StringBuilder sb = new StringBuilder();

			foreach (IRace race in list)
			{
				sb.AppendLine(race.RaceInfo());
			}

			return sb.ToString().TrimEnd();
		}

		public string PilotReport()
		{
			List<IPilot> list = this.pilotRepository.Models.OrderByDescending(x => x.NumberOfWins).ToList();

			StringBuilder sb = new StringBuilder();

			foreach (IPilot pilot in list)
			{
				sb.AppendLine($"Pilot {pilot.FullName} has {pilot.NumberOfWins} wins.");
			}

			return sb.ToString().TrimEnd();
		}
	}
}
