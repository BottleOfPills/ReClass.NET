using System.Drawing;
using System.Text;
using ReClassNET.Util;

namespace ReClassNET
{
	public class Settings
	{
		// Application Settings

		public string LastProcess { get; set; } = string.Empty;

		public bool StayOnTop { get; set; } = false;

		public bool RunAsAdmin { get; set; } = false;

		public bool RandomizeWindowTitle { get; set; } = false;

		public bool EnableDarkMode { get; set; } = false;

		// Node Drawing Settings

		public bool ShowNodeAddress { get; set; } = true;

		public bool ShowNodeOffset { get; set; } = true;

		public bool ShowNodeText { get; set; } = true;

		public bool HighlightChangedValues { get; set; } = true;

		public Encoding RawDataEncoding { get; set; } = Encoding.GetEncoding(1252); /* Windows-1252 */

		// Comment Drawing Settings

		public bool ShowCommentFloat { get; set; } = true;

		public bool ShowCommentInteger { get; set; } = true;

		public bool ShowCommentPointer { get; set; } = true;

		public bool ShowCommentRtti { get; set; } = true;

		public bool ShowCommentSymbol { get; set; } = true;

		public bool ShowCommentString { get; set; } = true;

		public bool ShowCommentPluginInfo { get; set; } = true;

		// Colors

		private Color backgroundColor = Color.FromArgb(255, 255, 255);
		public Color BackgroundColor { get => EnableDarkMode ? DarkBackgroundColor : backgroundColor; set => backgroundColor = value; }
		public Color DarkBackgroundColor { get; set; } = ColorTranslator.FromHtml("#2D2D30");

		private Color selectedColor = Color.FromArgb(240, 240, 240);
		public Color SelectedColor { get => EnableDarkMode ? DarkSelectedColor : selectedColor; set => selectedColor = value; }
		public Color DarkSelectedColor { get; set; } = ColorTranslator.FromHtml("#3F3F46");

		private Color hiddenColor = Color.FromArgb(240, 240, 240);
		public Color HiddenColor { get => EnableDarkMode ? DarkHiddenColor : hiddenColor; set => hiddenColor = value; }
		public Color DarkHiddenColor { get; set; } = ColorTranslator.FromHtml("#434346");

		private Color offsetColor = Color.FromArgb(255, 0, 0);
		public Color OffsetColor { get => EnableDarkMode ? DarkOffsetColor : offsetColor; set => offsetColor = value; }
		public Color DarkOffsetColor { get; set; } = ColorTranslator.FromHtml("#569CD6");

		private Color addressColor = Color.FromArgb(0, 200, 0);
		public Color AddressColor { get => EnableDarkMode ? DarkAddressColor : addressColor; set => addressColor = value; }
		public Color DarkAddressColor { get; set; } = ColorTranslator.FromHtml("#9CDCFE");

		private Color hexColor = Color.FromArgb(0, 0, 0);
		public Color HexColor { get => EnableDarkMode ? DarkHexColor : hexColor; set => hexColor = value; }
		public Color DarkHexColor { get; set; } = ColorTranslator.FromHtml("#D4D4D4");

		private Color typeColor = Color.FromArgb(0, 0, 255);
		public Color TypeColor { get => EnableDarkMode ? DarkTypeColor : typeColor; set => typeColor = value; }
		public Color DarkTypeColor { get; set; } = ColorTranslator.FromHtml("#4EC9B0");

		private Color nameColor = Color.FromArgb(32, 32, 128);
		public Color NameColor { get => EnableDarkMode ? DarkNameColor : nameColor; set => nameColor = value; }
		public Color DarkNameColor { get; set; } = ColorTranslator.FromHtml("#C586C0");

		private Color valueColor = Color.FromArgb(255, 128, 0);
		public Color ValueColor { get => EnableDarkMode ? DarkValueColor : valueColor; set => valueColor = value; }
		public Color DarkValueColor { get; set; } = ColorTranslator.FromHtml("#B5CEA8");

		private Color indexColor = Color.FromArgb(32, 200, 200);
		public Color IndexColor { get => EnableDarkMode ? DarkIndexColor : indexColor; set => indexColor = value; }
		public Color DarkIndexColor { get; set; } = ColorTranslator.FromHtml("#499CD6");

		private Color commentColor = Color.FromArgb(0, 200, 0);
		public Color CommentColor { get => EnableDarkMode ? DarkCommentColor : commentColor; set => commentColor = value; }
		public Color DarkCommentColor { get; set; } = ColorTranslator.FromHtml("#608B4E");

		private Color textColor = Color.FromArgb(0, 0, 255);
		public Color TextColor { get => EnableDarkMode ? DarkTextColor : textColor; set => textColor = value; }
		public Color DarkTextColor { get; set; } = ColorTranslator.FromHtml("#D4D4D4");

		private Color vTableColor = Color.FromArgb(0, 255, 0);
		public Color VTableColor { get => EnableDarkMode ? DarkVTableColor : vTableColor; set => vTableColor = value; }
		public Color DarkVTableColor { get; set; } = ColorTranslator.FromHtml("#C586C0");

		private Color pluginColor = Color.FromArgb(255, 0, 255);
		public Color PluginColor { get => EnableDarkMode ? DarkPluginColor : pluginColor; set => pluginColor = value; }
		public Color DarkPluginColor { get; set; } = ColorTranslator.FromHtml("#DCDCAA");

		public CustomDataMap CustomData { get; } = new CustomDataMap();

		public Settings Clone() => MemberwiseClone() as Settings;
	}
}
