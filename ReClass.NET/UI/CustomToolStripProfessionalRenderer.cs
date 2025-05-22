using System.Drawing;
using System.Windows.Forms;
using ReClassNET; // Added for Program.Settings

namespace ReClassNET.UI
{
	internal class CustomToolStripProfessionalRenderer : ToolStripProfessionalRenderer
	{
		private readonly bool renderGrip;
		private readonly bool renderBorder;

		public CustomToolStripProfessionalRenderer(bool renderGrip, bool renderBorder)
			: base(new CustomProfessionalColorTable())
		{
			this.renderGrip = renderGrip;
			this.renderBorder = renderBorder;
		}

		protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
		{
			if (renderGrip)
			{
				base.OnRenderGrip(e);
			}
		}

		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			if (renderBorder)
			{
				base.OnRenderToolStripBorder(e);
			}
		}

		protected override void OnRenderToolStripPanelBackground(ToolStripPanelRenderEventArgs e)
		{
			//base.OnRenderToolStripPanelBackground(e);
		}
	}

	internal class CustomProfessionalColorTable : ProfessionalColorTable
	{
		private static readonly Color DarkModeBackgroundColor = Color.FromArgb(45, 45, 48);

		public override Color MenuStripGradientBegin => Program.Settings.EnableDarkMode ? DarkModeBackgroundColor : SystemColors.Control;

		public override Color MenuStripGradientEnd => Program.Settings.EnableDarkMode ? DarkModeBackgroundColor : SystemColors.Control;

		public override Color ToolStripGradientBegin => Program.Settings.EnableDarkMode ? DarkModeBackgroundColor : SystemColors.Control;

		public override Color ToolStripGradientMiddle => Program.Settings.EnableDarkMode ? DarkModeBackgroundColor : SystemColors.Control;

		public override Color ToolStripGradientEnd => Program.Settings.EnableDarkMode ? DarkModeBackgroundColor : SystemColors.Control;

		// It's important to override other colors for a complete dark mode experience.
		// For example, item selection, borders, text, etc.
		// Adding a few more overrides as examples:

		public override Color MenuItemSelected => Program.Settings.EnableDarkMode ? Color.FromArgb(70, 70, 70) : SystemColors.Highlight;
		public override Color MenuItemBorder => Program.Settings.EnableDarkMode ? Color.FromArgb(80, 80, 80) : SystemColors.MenuBar; // Or another appropriate color
		public override Color ToolStripDropDownBackground => Program.Settings.EnableDarkMode ? DarkModeBackgroundColor : SystemColors.Control;
		public override Color ImageMarginGradientBegin => Program.Settings.EnableDarkMode ? DarkModeBackgroundColor : SystemColors.ControlLight;
		public override Color ImageMarginGradientMiddle => Program.Settings.EnableDarkMode ? DarkModeBackgroundColor : SystemColors.ControlLight;
		public override Color ImageMarginGradientEnd => Program.Settings.EnableDarkMode ? DarkModeBackgroundColor : SystemColors.ControlLight;
		public override Color SeparatorDark => Program.Settings.EnableDarkMode ? Color.FromArgb(80, 80, 80) : SystemColors.ControlDark;
		public override Color SeparatorLight => Program.Settings.EnableDarkMode ? Color.FromArgb(100, 100, 100) : SystemColors.ControlLightLight;
		public override Color StatusStripGradientBegin => Program.Settings.EnableDarkMode ? DarkModeBackgroundColor : SystemColors.Control;
		public override Color StatusStripGradientEnd => Program.Settings.EnableDarkMode ? DarkModeBackgroundColor : SystemColors.Control;

		// For text, you would typically rely on the control's ForeColor,
		// but if the renderer specifically uses a color table item for text, it should be overridden.
		// Example (if there was a TextColor property):
		// public override Color TextColor => Program.Settings.EnableDarkMode ? Color.White : SystemColors.ControlText;
	}
}
