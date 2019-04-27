/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>


public class TryDebug
{
	public static bool LoggingEnabled = true;
	public static void Log(params string[] messages)
	{
		if (LoggingEnabled)
			Debug.Log(string.Join("", messages));
	}
}
