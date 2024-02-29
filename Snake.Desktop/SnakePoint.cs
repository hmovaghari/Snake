using Snake.Desktop.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake.Desktop
{
    public class SnakePoint : Label
    {
        private static int index = 0;

        public static int Score => index - 5;

        private static Color DarckGreen = Color.FromArgb(34, 177, 76);

        private static Color LightGreen = Color.FromArgb(132, 232, 162);

        public SnakePoint()
        {
            ++index;
            TabIndex = index;
            ForeColor = TabIndex == 1 ? DarckGreen : TabIndex % 2 == 0 ? DarckGreen : LightGreen;
            BackColor = TabIndex == 1 ? DarckGreen : TabIndex % 2 == 0 ? DarckGreen : LightGreen;
            Size = new Size(10,10);
        }

        internal static void Clear()
        {
            index = 0;
        }
    }
}
