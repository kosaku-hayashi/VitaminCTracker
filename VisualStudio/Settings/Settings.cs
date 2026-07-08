namespace VitaminCTracker
{
	public class Settings : JsonModSettings
	{
		internal static Settings Instance { get; } = new();

		[Name("Which Side Should The Values Be On?")]
		[Description("Changing this will make the values either show on the left or the right")]
		[Choice(["Right", "Left"])]
		public bool SwapDirection = false;

		// this is used to set things when user clicks confirm. If you dont need this ability, dont include this method
		/// <inheritdoc/>
		protected override void OnConfirm()
		{
			base.OnConfirm();

			var tracker = InterfaceManager.GetPanel<Panel_FirstAid>()?.GetComponent<VitaminTracking.VitaminTrackerBar>();
			tracker?.ApplyLayout();
		}

		// This is used to load the settings
		internal static void OnLoad()
		{
			Instance.AddToModSettings(BuildInfo.GUIName);

			Instance.RefreshGUI();
		}
	}
}
