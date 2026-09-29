using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AKRS.Galaxy2.Dispense
{
    public class Point2DTypeConverter : TypeConverter
    {
        public Point2DTypeConverter()
        {
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(Point2D);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string str = ((string)value).Trim();

            if (str.StartsWith("(") && str.EndsWith(")"))
            {
                string substr = str.Substring(1, str.Length - 2);
                int iSeparatorIndex = substr.IndexOf(',');
                if (iSeparatorIndex > 0 && iSeparatorIndex < substr.Length - 1)
                {
                    string strX = substr.Substring(0, iSeparatorIndex).Trim();
                    float x;
                    if (float.TryParse(strX, out x))
                    {
                        string strY = substr.Substring(iSeparatorIndex + 1).Trim();
                        float y;
                        if (float.TryParse(strY, out y))
                        {
                            return new Point2D(x, y);
                        }
                    }
                }
            }

            return null;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return ((Point2D)value).ToString();
        }
    }
}
