using System.Drawing;

namespace ReClassNET.UI
{
	public static class ThemeService
	{
		public static Settings Light { get; }
		public static Settings Dark { get; }

		public static Settings Current { get; private set; }

		static ThemeService()
		{
			Light = new Settings();

			Dark = new Settings
			{
				BackgroundColor = Color.FromArgb(255, 18, 18, 18),
				SelectedColor = Color.FromArgb(255, 50, 50, 50),
				HiddenColor = Color.FromArgb(255, 80, 80, 80),
				OffsetColor = Color.FromArgb(255, 255, 255, 0),
				AddressColor = Color.FromArgb(255, 0, 255, 0),
				HexColor = Color.FromArgb(255, 255, 255, 255),
				TypeColor = Color.FromArgb(255, 0, 255, 255),
				NameColor = Color.FromArgb(255, 160, 160, 255),
				ValueColor = Color.FromArgb(255, 255, 128, 0),
				IndexColor = Color.FromArgb(255, 32, 200, 200),
				CommentColor = Color.FromArgb(255, 0, 128, 0),
				TextColor = Color.FromArgb(255, 0, 255, 255),
				VTableColor = Color.FromArgb(255, 0, 255, 0),
				PluginColor = Color.FromArgb(255, 255, 0, 255)
			};

			Current = Light;
		}

		public static void SetTheme(DisplayMode displayMode)
		{
			Current = displayMode == DisplayMode.Dark ? Dark : Light;
		}
	}
}
