using System;
using System.Drawing;
using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class DoubleMatrix3x3Node : BaseMatrixNode
	{
		public override int ValueTypeSize => sizeof(double);

		public override int MemorySize => 9 * ValueTypeSize;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Double Matrix 3x3";
			icon = Properties.Resources.B16x16_Button_Matrix_3x3;
		}

		public override Size Draw(DrawContext context, int x2, int y2)
		{
			return DrawMatrixType(context, x2, y2, "Double Matrix (3x3)", 3, 3);
		}

		protected override int CalculateValuesHeight(DrawContext context)
		{
			return 3 * context.Font.Height;
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			Update(spot, 9);
		}

		protected override double ReadValueFromMemory(MemoryBuffer memory, int offset)
		{
			return memory.ReadDouble(offset);
		}

		protected override void WriteValueToMemory(RemoteProcess process, IntPtr address, double value)
		{
			process.WriteRemoteMemory(address, value);
		}
	}
}
