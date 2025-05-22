using System;
using System.Diagnostics.Contracts;
using System.Windows.Forms;
using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Native;
using ReClassNET.Project;
using ReClassNET.UI;
using ReClassNET.Util;
using System.Drawing; // Required for Color

namespace ReClassNET.Forms
{
	public partial class SettingsForm : IconForm
	{
		private readonly Settings settings;
		private readonly CppTypeMapping typeMapping;

		public TabControl SettingsTabControl => settingsTabControl;

		public SettingsForm(Settings settings, CppTypeMapping typeMapping)
		{
			Contract.Requires(settings != null);
			Contract.Requires(typeMapping != null);

			this.settings = settings;
			this.typeMapping = typeMapping;

			InitializeComponent();

			var imageList = new ImageList();
			imageList.Images.Add(Properties.Resources.B16x16_Gear);
			imageList.Images.Add(Properties.Resources.B16x16_Color_Wheel);
			imageList.Images.Add(Properties.Resources.B16x16_Settings_Edit);

			settingsTabControl.ImageList = imageList;
			generalSettingsTabPage.ImageIndex = 0;
			colorsSettingTabPage.ImageIndex = 1;
			typeDefinitionsSettingsTabPage.ImageIndex = 2;

			SetGeneralBindings();
			SetColorBindings();
			SetTypeDefinitionBindings();

			// Apply theme after bindings are set.
			ApplyTheme();

			if (NativeMethods.IsUnix())
			{
				fileAssociationGroupBox.Enabled = false;
				runAsAdminCheckBox.Enabled = false;
			}
			else
			{
				NativeMethodsWindows.SetButtonShield(createAssociationButton, true);
				NativeMethodsWindows.SetButtonShield(removeAssociationButton, true);
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			GlobalWindowManager.AddWindow(this);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);

			GlobalWindowManager.RemoveWindow(this);
		}

		private void createAssociationButton_Click(object sender, EventArgs e)
		{
			WinUtil.RunElevated(PathUtil.LauncherExecutablePath, $"-{Constants.CommandLineOptions.FileExtRegister}");
		}

		private void removeAssociationButton_Click(object sender, EventArgs e)
		{
			WinUtil.RunElevated(PathUtil.LauncherExecutablePath, $"-{Constants.CommandLineOptions.FileExtUnregister}");
		}

		private static void SetBinding(IBindableComponent control, string propertyName, object dataSource, string dataMember)
		{
			Contract.Requires(control != null);
			Contract.Requires(propertyName != null);
			Contract.Requires(dataSource != null);
			Contract.Requires(dataMember != null);

			control.DataBindings.Add(propertyName, dataSource, dataMember, true, DataSourceUpdateMode.OnPropertyChanged);
		}

		private void SetGeneralBindings()
		{
			SetBinding(stayOnTopCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.StayOnTop));
			stayOnTopCheckBox.CheckedChanged += (_, _2) => GlobalWindowManager.Windows.ForEach(w => w.TopMost = stayOnTopCheckBox.Checked);

			SetBinding(showNodeAddressCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.ShowNodeAddress));
			SetBinding(showNodeOffsetCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.ShowNodeOffset));
			SetBinding(showTextCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.ShowNodeText));
			SetBinding(highlightChangedValuesCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.HighlightChangedValues));

			SetBinding(showFloatCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.ShowCommentFloat));
			SetBinding(showIntegerCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.ShowCommentInteger));
			SetBinding(showPointerCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.ShowCommentPointer));
			SetBinding(showRttiCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.ShowCommentRtti));
			SetBinding(showSymbolsCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.ShowCommentSymbol));
			SetBinding(showStringCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.ShowCommentString));
			SetBinding(showPluginInfoCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.ShowCommentPluginInfo));
			SetBinding(runAsAdminCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.RunAsAdmin));
			SetBinding(randomizeWindowTitleCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.RandomizeWindowTitle));

			// Dark Mode CheckBox Binding
			SetBinding(darkModeCheckBox, nameof(CheckBox.Checked), settings, nameof(Settings.EnableDarkMode));
			darkModeCheckBox.CheckedChanged += (_, _2) => ApplyTheme();
		}

		private void ApplyTheme()
		{
			// Determine colors based on dark mode setting
			var foreColor = settings.EnableDarkMode ? settings.DarkTextColor : SystemColors.ControlText;
			var backColor = settings.EnableDarkMode ? settings.DarkBackgroundColor : SystemColors.Control;

			this.ForeColor = foreColor;
			this.BackColor = backColor;

			// Recursively update all child controls
			UpdateControlTheme(this);

			// Special handling for specific controls if needed
			settingsTabControl.ForeColor = foreColor;
			settingsTabControl.BackColor = backColor; // TabControl itself might not change much

			foreach (TabPage tabPage in settingsTabControl.TabPages)
			{
				tabPage.ForeColor = foreColor;
				tabPage.BackColor = backColor;
				UpdateControlTheme(tabPage); // Apply to controls within each tab page
			}

			// BannerBox - Assuming it has public ForeColor/BackColor properties to set.
			// If BannerBox internally uses specific colors, it might need its own dark mode logic.
			bannerBox.ForeColor = foreColor;
			bannerBox.BackColor = settings.EnableDarkMode ? settings.DarkSelectedColor : SystemColors.Control; // Example: using selected color for banner bg
		}

		private void UpdateControlTheme(Control parentControl)
		{
			var foreColor = settings.EnableDarkMode ? settings.DarkTextColor : SystemColors.ControlText;
			var backColor = settings.EnableDarkMode ? settings.DarkBackgroundColor : SystemColors.Control;
			var contrastingBackColor = settings.EnableDarkMode ? settings.DarkSelectedColor : SystemColors.Window; // For TextBox, etc.

			foreach (Control control in parentControl.Controls)
			{
				control.ForeColor = foreColor;
				control.BackColor = backColor;

				if (control is TextBox || control is ComboBox || control is ListBox)
				{
					control.BackColor = contrastingBackColor; // Use a contrasting background for input fields
				}
				else if (control is ButtonBase) // Buttons often use system styling
				{
					// Buttons might not fully support custom BackColor/ForeColor on all OS versions or themes.
					// We can try, but it might not have the desired effect.
					control.ForeColor = foreColor;
					// control.BackColor = backColor; // Often ignored for buttons
				}
				else if (control is CheckBox)
				{
					// CheckBox text color is handled by ForeColor, background is usually transparent or uses parent's.
				}
				else if (control is GroupBox groupBox)
				{
					groupBox.ForeColor = foreColor; // GroupBox title color
					UpdateControlTheme(groupBox); // Recursively update controls within the GroupBox
				}
				else if (control is Panel panel)
				{
					UpdateControlTheme(panel); // Recursively update controls within the Panel
				}
				else if (control is TabControl tabControl)
				{
					UpdateControlTheme(tabControl); // Apply to the TabControl itself
					foreach (TabPage tabPage in tabControl.TabPages)
					{
						tabPage.ForeColor = foreColor;
						tabPage.BackColor = backColor;
						UpdateControlTheme(tabPage); // Apply to controls within each tab page
					}
				}
				else if (control is ColorBox colorBox) // Assuming ColorBox is a custom control
				{
					// ColorBox might need specific properties if Fore/Back color don't cover its display
					// For now, apply standard colors. It might have its own drawing logic.
					colorBox.BackColor = backColor;
				}
				else if (control.HasChildren && !(control is TabPage || control is TextBoxBase)) // Avoid re-recursing on TabPage children here as it's handled above / TextBox
				{
					UpdateControlTheme(control);
				}
			}
		}

		private void SetColorBindings()
		{
			SetBinding(backgroundColorBox, nameof(ColorBox.Color), settings, nameof(Settings.BackgroundColor));

			SetBinding(nodeSelectedColorBox, nameof(ColorBox.Color), settings, nameof(Settings.SelectedColor));
			SetBinding(nodeHiddenColorBox, nameof(ColorBox.Color), settings, nameof(Settings.HiddenColor));
			SetBinding(nodeAddressColorBox, nameof(ColorBox.Color), settings, nameof(Settings.AddressColor));
			SetBinding(nodeOffsetColorBox, nameof(ColorBox.Color), settings, nameof(Settings.OffsetColor));
			SetBinding(nodeHexValueColorBox, nameof(ColorBox.Color), settings, nameof(Settings.HexColor));
			SetBinding(nodeTypeColorBox, nameof(ColorBox.Color), settings, nameof(Settings.TypeColor));
			SetBinding(nodeNameColorBox, nameof(ColorBox.Color), settings, nameof(Settings.NameColor));
			SetBinding(nodeValueColorBox, nameof(ColorBox.Color), settings, nameof(Settings.ValueColor));
			SetBinding(nodeIndexColorBox, nameof(ColorBox.Color), settings, nameof(Settings.IndexColor));
			SetBinding(nodeVTableColorBox, nameof(ColorBox.Color), settings, nameof(Settings.VTableColor));
			SetBinding(nodeCommentColorBox, nameof(ColorBox.Color), settings, nameof(Settings.CommentColor));
			SetBinding(nodeTextColorBox, nameof(ColorBox.Color), settings, nameof(Settings.TextColor));
			SetBinding(nodePluginColorBox, nameof(ColorBox.Color), settings, nameof(Settings.PluginColor));
		}

		private void SetTypeDefinitionBindings()
		{
			SetBinding(boolTypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeBool));
			SetBinding(int8TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeInt8));
			SetBinding(int16TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeInt16));
			SetBinding(int32TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeInt32));
			SetBinding(int64TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeInt64));
			SetBinding(nintTypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeNInt));
			SetBinding(uint8TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeUInt8));
			SetBinding(uint16TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeUInt16));
			SetBinding(uint32TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeUInt32));
			SetBinding(uint64TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeUInt64));
			SetBinding(nuintTypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeNUInt));
			SetBinding(floatTypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeFloat));
			SetBinding(doubleTypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeDouble));
			SetBinding(vector2TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeVector2));
			SetBinding(vector3TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeVector3));
			SetBinding(vector4TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeVector4));
			SetBinding(matrix3x3TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeMatrix3x3));
			SetBinding(matrix3x4TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeMatrix3x4));
			SetBinding(matrix4x4TypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeMatrix4x4));
			SetBinding(utf8TextTypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeUtf8Text));
			SetBinding(utf16TextTypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeUtf16Text));
			SetBinding(utf32TextTypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeUtf32Text));
			SetBinding(functionPtrTypeTextBox, nameof(TextBox.Text), typeMapping, nameof(CppTypeMapping.TypeFunctionPtr));
		}
	}
}
