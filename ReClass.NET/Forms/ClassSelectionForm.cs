using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing; // Added for Color
using System.Linq;
using System.Windows.Forms;
using ReClassNET.Nodes;
using ReClassNET.UI;

namespace ReClassNET.Forms
{
	public partial class ClassSelectionForm : IconForm
	{
		private readonly List<ClassNode> allClasses;
		private bool isApplyingTheme = false; // To prevent re-entrancy

		public ClassNode SelectedClass => classesListBox.SelectedItem as ClassNode;

		public ClassSelectionForm(IEnumerable<ClassNode> classes)
		{
			Contract.Requires(classes != null);

			allClasses = classes.ToList();

			InitializeComponent();

			ShowFilteredClasses();

			this.Activated += ClassSelectionForm_Activated;
		}

		private void ClassSelectionForm_Activated(object sender, EventArgs e)
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
				var backColorSelected = Program.Settings.SelectedColor;

				this.ForeColor = foreColor;
				this.BackColor = backColor;

				UpdateControlTheme(this, foreColor, backColor, backColorSelected);

				// Specific controls
				classesListBox.BackColor = backColorSelected;
				classesListBox.ForeColor = foreColor;

				filterNameTextBox.BackColor = backColorSelected;
				filterNameTextBox.ForeColor = foreColor;
				
				// BannerBox theming will be handled more deeply in BannerBox.cs later.
				bannerBox.BackColor = backColor; 
				bannerBox.ForeColor = foreColor;
				bannerBox.Invalidate();
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
				if (control is BannerBox) continue; // Skip BannerBox

				control.ForeColor = foreColor;
				control.BackColor = backColor;

				if (control is TextBoxBase || control is ListBox || control is ComboBox)
				{
					control.BackColor = backColorSelected;
				}
				else if (control is ButtonBase)
				{
					// Standard buttons often don't style well with BackColor. ForeColor is usually fine.
					// If using FlatStyle.Flat or FlatStyle.Popup, BackColor can be set.
					// For now, we'll assume default button styling is mostly acceptable or will be tweaked if issues arise.
				}
				else if (control is GroupBox)
				{
					// GroupBox ForeColor sets the title color.
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

		private void filterNameTextBox_TextChanged(object sender, EventArgs e)
		{
			ShowFilteredClasses();
		}

		private void classesListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			selectButton.Enabled = SelectedClass != null;
		}

		private void classesListBox_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (SelectedClass != null)
			{
				selectButton.PerformClick();
			}
		}

		private void ShowFilteredClasses()
		{
			IEnumerable<ClassNode> classes = allClasses;

			if (!string.IsNullOrEmpty(filterNameTextBox.Text))
			{
				classes = classes.Where(c => c.Name.IndexOf(filterNameTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
			}

			classesListBox.DataSource = classes.ToList();
		}
	}
}
