namespace Zen {

	public class GenerationStatus {

		public int MinPathCount;
		public int CurrentPathCount;
		public int NewPathAttempts;
		public int PathAttempLimit;

		public GenerationStatus() {

			reset();
		}

		public void reset() {

			MinPathCount = 5;
			CurrentPathCount = 1;
			NewPathAttempts = 1;
			PathAttempLimit = 10000;
		}
	}
}