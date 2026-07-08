namespace VitaminCTracker.Utilities
{
	internal enum FlaggedLoggingLevel
	{
		Always,
		Debug,
		Verbose,
		Exception
	}

	internal class ModLogger
	{
		public void Log(string message, FlaggedLoggingLevel level = FlaggedLoggingLevel.Always, Exception? exception = null)
		{
			if (level == FlaggedLoggingLevel.Exception || exception != null)
			{
				MelonLogger.Error(message);
				if (exception != null)
				{
					MelonLogger.Error(exception.ToString());
				}
				return;
			}

			if (level == FlaggedLoggingLevel.Debug || level == FlaggedLoggingLevel.Verbose)
			{
				MelonLogger.Msg($"[{level}] {message}");
				return;
			}

			MelonLogger.Msg(message);
		}
	}
}
