using VitaminCTracker.VitaminTracking;

namespace VitaminCTracker.Patches
{
	[HarmonyPatch(typeof(Panel_FirstAid), nameof(Panel_FirstAid.Enable), new Type[] { typeof(bool) })]
	public class Panel_FirstAid_Enable
	{
		public static void Postfix(Panel_FirstAid __instance, bool enable)
		{
			if (!enable) return;
			Main.Logger.Log("VitaminCTracker: Panel_FirstAid.Enable(true) detected.");
			var tracker = __instance.gameObject.GetComponent<VitaminTrackerBar>() ?? __instance.gameObject.AddComponent<VitaminTrackerBar>();
			tracker.ApplyLayout();
		}
	}
}
