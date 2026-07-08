namespace VitaminCTracker.VitaminTracking
{
	[RegisterTypeInIl2Cpp(false)]
	public class VitaminTrackerBar : MonoBehaviour
	{
		static readonly Vector3 RightPosition = new(110f, -298.916f, 0f);
		static readonly Vector3 LeftPosition = new(-110f, -298.916f, 0f);

		GameObject? labelRoot;
		UILabel? valueLabel;
		bool loggedMissingFont;
		bool loggedMissingNutrition;
		bool loggedLabelCreation;

		public VitaminTrackerBar() { }
		public VitaminTrackerBar(IntPtr pointer) : base(pointer) { }

		public void Awake()
		{
			EnsureLabel();
			ApplyLayout();
		}

		public void Update()
		{
			var panel = GetComponent<Panel_FirstAid>();
			if (!SceneUtilities.IsScenePlayable() || panel == null || !panel.enabled)
			{
				SetVisible(false);
				return;
			}

			if (!EnsureLabel())
			{
				SetVisible(false);
				return;
			}

			if (!TryGetVitaminC(out var amount, out var lossPerDay))
			{
				SetVisible(false);
				return;
			}

			SetVisible(true);
			Refresh(amount, lossPerDay);
		}

		public void ApplyLayout()
		{
			if (labelRoot == null)
			{
				return;
			}

			labelRoot.transform.localPosition = Settings.Instance.SwapDirection ? LeftPosition : RightPosition;
		}

		bool EnsureLabel()
		{
			if (valueLabel != null && labelRoot != null)
			{
				return true;
			}

			labelRoot ??= new GameObject("VitaminCTrackerLabel") { layer = vp_Layer.UI };
			labelRoot.transform.SetParent(transform, false);
			labelRoot.transform.localScale = Vector3.one;

			valueLabel = labelRoot.GetComponent<UILabel>() ?? labelRoot.AddComponent<UILabel>();
			var uiFont = GameManager.GetFontManager()?.GetUIFontForCharacterSet(FontManager.m_CurrentCharacterSet);
			if (uiFont == null)
			{
				if (!loggedMissingFont)
				{
					Main.Logger.Log("VitaminCTracker: UIFont could not be resolved on Panel_FirstAid.");
					loggedMissingFont = true;
				}
				return false;
			}

			valueLabel.ambigiousFont = uiFont;
			valueLabel.bitmapFont = uiFont;
			valueLabel.font = uiFont;
			valueLabel.alignment = NGUIText.Alignment.Center;
			valueLabel.fontSize = 24;
			valueLabel.width = 340;
			valueLabel.height = 40;
			valueLabel.enabled = true;

			ApplyLayout();
			if (!loggedLabelCreation)
			{
				Main.Logger.Log("VitaminCTracker: fallback UILabel attached to Panel_FirstAid.");
				loggedLabelCreation = true;
			}
			return true;
		}

		bool TryGetVitaminC(out float amount, out float lossPerDay)
		{
			amount = 0f;
			lossPerDay = 0f;

			var nutrition = Il2CppTLD.Player.Nutrition.Instance;
			if (nutrition == null || nutrition.m_Amounts == null || nutrition.m_Amounts.Count == 0)
			{
				if (!loggedMissingNutrition)
				{
					Main.Logger.Log("VitaminCTracker: Nutrition data is not available on Panel_FirstAid.");
					loggedMissingNutrition = true;
				}
				return false;
			}

			loggedMissingNutrition = false;
			amount = nutrition.m_Amounts[0];
			if (nutrition.m_LossPerDay != null && nutrition.m_LossPerDay.Count > 0)
			{
				lossPerDay = nutrition.m_LossPerDay[0];
			}
			return true;
		}

		void Refresh(float amount, float lossPerDay)
		{
			if (valueLabel == null)
			{
				return;
			}

			valueLabel.text = $"Vitamin C  {amount:N2}   {lossPerDay:N2}/Day";
		}

		void SetVisible(bool visible)
		{
			if (labelRoot != null && labelRoot.activeSelf != visible)
			{
				labelRoot.SetActive(visible);
			}
		}
	}
}
