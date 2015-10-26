using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Vmd2.Processing.Helper;

namespace Vmd2.Processing.TransferFunctions
{
    public class TransferFunction1D
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
            
            if (value <= root.Value)
            {
                return root.Color;
            }

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
                return ColorHelper.Blend(item.Color, last.Color, amount);
            }
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
