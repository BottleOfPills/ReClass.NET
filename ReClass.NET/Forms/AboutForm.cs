using System;
using System.Diagnostics;
using System.Drawing; // Added for Color
using System.Windows.Forms;
using ReClassNET.UI;

namespace ReClassNET.Forms
{
	public partial class AboutForm : IconForm
	{
		private bool isApplyingTheme = false; // To prevent re-entrancy

		public AboutForm()
		{
			InitializeComponent();

			bannerBox.Icon = Properties.Resources.ReClassNet.ToBitmap();
			bannerBox.Title = Constants.ApplicationName;
			bannerBox.Text = $"Version: {Constants.ApplicationVersion}";

			platformValueLabel.Text = Constants.Platform;
			buildTimeValueLabel.Text = Properties.Resources.BuildDate;
			authorValueLabel.Text = Constants.Author;
			homepageValueLabel.Text = Constants.HomepageUrl;

			this.Activated += AboutForm_Activated;
		}

		private void AboutForm_Activated(object sender, EventArgs e)
		{
			ApplyTheme();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			GlobalWindowManager.AddWindow(this);

			ApplyTheme();
		}

		private void ApplyTheme()
		{
			if (isApplyingTheme) return;
			isApplyingTheme = true;

			try
			{
				var foreColor = Program.Settings.TextColor;
				var backColor = Program.Settings.BackgroundColor;
				var backColorSelected = Program.Settings.SelectedColor; // For input-like backgrounds
				var linkColor = Program.Settings.OffsetColor; // Using OffsetColor for links as an example

				this.ForeColor = foreColor;
				this.BackColor = backColor;

				// Apply to child controls recursively
				UpdateControlTheme(this, foreColor, backColor, backColorSelected);

				// Specific controls
				licenseTextBox.BackColor = backColorSelected;
				licenseTextBox.ForeColor = foreColor;

				homepageValueLabel.LinkColor = linkColor;
				// Make ActiveLinkColor slightly different, e.g., a bit lighter or darker than LinkColor
				homepageValueLabel.ActiveLinkColor = ControlPaint.Light(linkColor); 
				homepageValueLabel.VisitedLinkColor = ControlPaint.Dark(linkColor);


				// BannerBox theming will be handled more deeply in BannerBox.cs later.
				// For now, set basic colors.
				bannerBox.BackColor = backColor; // Or a specific banner background from settings if available
				bannerBox.ForeColor = foreColor; // For title and text, if not custom painted
				bannerBox.Invalidate(); // Ensure it redraws
			}
			finally
			{
				isApplyingTheme = false;
			}
		}

		private void UpdateControlTheme(Control parentControl, Color foreColor, Color backColor, Color backColorSelected)
		{
			foreach (Control control in parentControl.Controls)
			{
				// Skip BannerBox as it will have its own detailed theming.
				if (control is BannerBox) continue;

				control.ForeColor = foreColor;
				control.BackColor = backColor;

				if (control is TextBoxBase || control is ListBox || control is ComboBox)
				{
					control.BackColor = backColorSelected;
				}
				else if (control is GroupBox groupBox)
				{
					// GroupBox ForeColor sets the title color.
					// Children are handled by recursion.
				}
				else if (control is LinkLabel linkLabel)
				{
					// Already handled homepageValueLabel specifically, but a general case:
					linkLabel.LinkColor = Program.Settings.OffsetColor; 
					linkLabel.ActiveLinkColor = ControlPaint.Light(Program.Settings.OffsetColor);
					linkLabel.VisitedLinkColor = ControlPaint.Dark(Program.Settings.OffsetColor);
				}


				if (control.HasChildren)
				{
					UpdateControlTheme(control, foreColor, backColor, backColorSelected);
				}
			}
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);

			GlobalWindowManager.RemoveWindow(this);
		}

		private void homepageValueLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(Constants.HomepageUrl);
		}
	}
}
