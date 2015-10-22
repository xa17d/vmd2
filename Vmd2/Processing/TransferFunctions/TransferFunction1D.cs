using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vmd2.Processing.TransferFunctions
{
    class TransferFunction1D
    {
        public TransferFunction1D()
        { }

        public void Add(double value, Color color)
        {
            TFItem newItem = new TFItem(value, color);

            if (root == null)
            {
                root = newItem;
            }
            else
            {
                if (value <= last.Value)
                {
                    throw new Exception("Value must be larger than last value");
                }

                last.Next = newItem;
            }

            last = newItem;
        }

        private TFItem root = null;
        private TFItem last = null;

        public Color GetColor(double value)
        {
            var item = root;
            var last = item;
            while (item != null)
            {
                if (item.Value >= value)
                {
                    break;
                }

                last = item;
                item = item.Next;
            }

            if (item == null)
            {
                return last.Color;
            }
            else if (value < last.Value)
            {
                return last.Color;
            }
            else
            {
                double amount = (value - last.Value) / (item.Value - last.Value);
                return Blend(item.Color, last.Color, amount);
            }
        }

        /// <summary>Blends the specified colors together.</summary>
        /// <param name="color">Color to blend onto the background color.</param>
        /// <param name="backColor">Color to blend the other color onto.</param>
        /// <param name="amount">How much of <paramref name="color"/> to keep,
        /// “on top of” <paramref name="backColor"/>.</param>
        /// <returns>The blended colors.</returns>
        public static Color Blend(Color color, Color backColor, double amount)
        {
            byte r = (byte)((color.R * amount) + backColor.R * (1 - amount));
            byte g = (byte)((color.G * amount) + backColor.G * (1 - amount));
            byte b = (byte)((color.B * amount) + backColor.B * (1 - amount));
            return Color.FromArgb(r, g, b);
        }

        private class TFItem
        {
            public TFItem(double value, Color color)
            {
                this.Value = value;
                this.Color = color;
            }

            public double Value;
            public Color Color;
            public TFItem Next;
        }
    }
}
