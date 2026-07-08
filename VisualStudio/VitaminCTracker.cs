#region System Directives
global using System;
global using System.Text.RegularExpressions;
#endregion
#region Il2Cpp Directives
#endregion
#region Unity Directives
#endregion
#region Mod Directives
global using VitaminCTracker.Utilities;
global using VitaminCTracker.Utilities.Exceptions;
#endregion

namespace VitaminCTracker
{

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
	public class Main : MelonMod
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
	{
		internal static ModLogger Logger { get; } = new();

        /// <inheritdoc/>
        public override void OnInitializeMelon()
        {
			Settings.OnLoad();
		}
	}
}
