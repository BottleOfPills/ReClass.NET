using System;
using System.Drawing;
using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class DoubleVector2Node : BaseMatrixNode
	{
		public override int ValueTypeSize => sizeof(double);

		public override int MemorySize => 2 * ValueTypeSize;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Double Vector2";
			icon = Properties.Resources.B16x16_Button_Vector_2;
		}

		public override Size Draw(DrawContext context, int x2, int y2)
		{
			return DrawVectorType(context, x2, y2, "Double Vector2", 2);
		}

		protected override int CalculateValuesHeight(DrawContext context)
		{
			return 0;
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			Update(spot, 2);
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
