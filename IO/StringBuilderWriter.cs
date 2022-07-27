namespace Formula1.IO
{
	using Formula1.IO.Contracts;
	using System.Text;

	public class StringBuilderWriter : IWriter
	{
		private StringBuilder sb;

		public StringBuilderWriter()
		{
			this.sb = new StringBuilder();
		}

		public void Write(string message)
		{
			this.sb.Append(message);
		}

		public void WriteLine(string message)
		{
			this.sb.AppendLine(message);
		}

		public override string ToString() => this.sb.ToString().TrimEnd();

	}
}
