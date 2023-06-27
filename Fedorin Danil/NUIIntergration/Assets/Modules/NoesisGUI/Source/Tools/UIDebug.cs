#if UNITY_5_3_OR_NEWER
#define NOESIS
using UnityEngine;
#else

#endif


namespace SmartTwin.NoesisGUI.Tools
{
	/// <summary>
	/// Отладочный класс для Noesis
	/// </summary>
	public static class UIDebug
	{
		public static void Log(object message)
		{
#if NOESIS
			Debug.Log(message);
#endif
		}

		public static void LogWarning(object message)
		{
#if NOESIS
			Debug.LogWarning(message);
#endif
		}
	}
}
