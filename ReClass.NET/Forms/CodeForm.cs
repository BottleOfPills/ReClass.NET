using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing; // Added for Color
using ColorCode;
using ColorCode.Parsing;
using ReClassNET.CodeGenerator;
using ReClassNET.Extensions;
using ReClassNET.Logger;
using ReClassNET.Nodes;
using ReClassNET.Project;
using ReClassNET.UI;
using ReClassNET.Util.Rtf;

namespace ReClassNET.Forms
{
	public partial class CodeForm : IconForm
	{
		private bool isApplyingTheme = false; // To prevent re-entrancy

		public CodeForm(ICodeGenerator generator, IReadOnlyList<ClassNode> classes, IReadOnlyList<EnumDescription> enums, ILogger logger)
		{
			Contract.Requires(generator != null);
			Contract.Requires(classes != null);
			Contract.Requires(enums != null);

			InitializeComponent();

			codeRichTextBox.SetInnerMargin(5, 5, 5, 5);

			var code = generator.GenerateCode(classes, enums, logger);

			var buffer = new StringBuilder(code.Length * 2);
			using (var writer = new StringWriter(buffer))
			{
				new CodeColorizer().Colorize(
					code,
					generator.Language == Language.Cpp ? Languages.Cpp : Languages.CSharp,
					new RtfFormatter(),
					StyleSheets.Default,
					writer
				);
			}

			codeRichTextBox.Rtf = buffer.ToString();

			this.Activated += CodeForm_Activated;
		}

		private void CodeForm_Activated(object sender, EventArgs e)
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
				// For RichTextBox, the selected color might be too dark if it's used as a general input field background.
				// Sticking to the main BackgroundColor for now, as RTF handles its own foreground colors.
				var rtbBackColor = Program.Settings.BackgroundColor; 

				this.ForeColor = foreColor;
				this.BackColor = backColor;

				UpdateControlTheme(this, foreColor, backColor, rtbBackColor); // Pass rtbBackColor for specific use

				// Specific controls
				codeRichTextBox.BackColor = rtbBackColor; 
				codeRichTextBox.ForeColor = foreColor; // Default text color if no RTF styling applies

				// BannerBox theming
				bannerBox.BackColor = backColor;
				bannerBox.ForeColor = foreColor;
				bannerBox.Invalidate();
			}
			finally
			{
				isApplyingTheme = false;
			}
		}

		private void UpdateControlTheme(Control parentControl, Color foreColor, Color backColor, Color specificBackColor)
		{
			foreach (Control control in parentControl.Controls)
			{
				if (control is BannerBox) continue;

				control.ForeColor = foreColor;
				control.BackColor = backColor;

				if (control is RichTextBox) // Apply specificBackColor to RichTextBox
				{
					control.BackColor = specificBackColor;
				}
				// No other specific input controls like TextBox, ListBox on this form from designer.
				// Buttons will take general ForeColor/BackColor.
				
				if (control.HasChildren)
				{
					UpdateControlTheme(control, foreColor, backColor, specificBackColor);
				}
			}
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);

			GlobalWindowManager.RemoveWindow(this);
		}
	}

	internal class RtfFormatter : IFormatter
	{
		private readonly RtfBuilder builder = new RtfBuilder(RtfFont.Consolas, 20);

		public void Write(string parsedSourceCode, IList<Scope> scopes, IStyleSheet styleSheet, TextWriter textWriter)
		{
			if (scopes.Any())
			{
				builder.SetForeColor(styleSheet.Styles[scopes.First().Name].Foreground).Append(parsedSourceCode);
			}
			else
			{
				builder.Append(parsedSourceCode);
			}
		}

		public void WriteHeader(IStyleSheet styleSheet, ILanguage language, TextWriter textWriter)
		{

		}

		public void WriteFooter(IStyleSheet styleSheet, ILanguage language, TextWriter textWriter)
		{
			textWriter.Write(builder.ToString());
		}
	}
}
