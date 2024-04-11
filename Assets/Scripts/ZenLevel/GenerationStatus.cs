namespace IceGame
{
	public class GenerationStatus
	{
		public int MinPathCount;
		public int CurrentPathCount;
		public int NewPathAttempts;
		public int PathAttempLimit;

		public GenerationStatus()
		{
			Reset();
		}

		public void Reset()
		{
			MinPathCount = 6;
			CurrentPathCount = 1;
			NewPathAttempts = 1;
			PathAttempLimit = 100;
		}
	}
}