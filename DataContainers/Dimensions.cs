using System;
using System.Collections.Generic;

namespace DataContainers
{
    public class LinearDimension
    {
        public uint Width;
        public uint Height;

        public LinearDimension(uint width, uint height)
        {
            Width = width;
            Height = height;
        }
    }

    public class LinearDimension<T>
    {
        public T Width;
        public T Height;

        public LinearDimension(T width, T height)
        {
            Width = width;
            Height = height;
        }
    }

    public class Resolution// : INotifyPropertyChanged
    {
        // property changed event
        //public event PropertyChangedEventHandler PropertyChanged;

        public uint X
        {
            get;
            set;
        }
        public uint Y
        {
            get;
            set;
        }

        public Resolution(uint x, uint y)
        {
            X = x;
            Y= y;
        }

        /*internal void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }*/
    }

    public class Resolution<T>
    {
        public T X
        {
            get;
            set;
        }
        public T Y
        {
            get;
            set;
        }

        public Resolution(T x, T y)
        {
            X = x;
            Y = y;
        }
    }
}
namespace Ranges
{
    public class GreyRange
    {
        public uint Low;
        public uint High;

        public GreyRange(uint low, uint high)
        {
            Low = low;
            High = high;
        }
    }

    public class GreyRange<T>
    {
        public T Low;
        public T High;

        public GreyRange(T low, T high)
        {
            Low = low;
            High = high;
        }
    }
}

namespace DataTypes
{
    public enum NumberType
    {
        Integer,
        Fractional,
        NotNumber=-1
    }

    public enum LinearUnit
    {
        nm,
        um,
        mm,
        sm,
        m
    }

    public enum ImageDimnsionUnit
    {
        pix,
        si //Si sysyem
    }

    public static class NumberOperations
    {
        public static DataTypes.NumberType DefineNumberType(object value)
        {
            if (value == null)
            {
                throw new Exception("value is null");
            }

            Type valueType = value.GetType();
            List<Type> intTypes = new List<Type>();
            #region целые
            intTypes.Add(typeof(int));
            intTypes.Add(typeof(uint));
            intTypes.Add(typeof(long));
            intTypes.Add(typeof(ulong));
            #endregion
            List<Type> fractionalTypes = new List<Type>();
            #region дробные
            fractionalTypes.Add(typeof(decimal));
            fractionalTypes.Add(typeof(float));
            fractionalTypes.Add(typeof(double));
            #endregion

            if (intTypes.Contains(valueType))
            {
                return NumberType.Integer;
            }
            else if (fractionalTypes.Contains(valueType))
            {
                return NumberType.Fractional;
            }
            else
            {
                return NumberType.NotNumber;
            }

        }
    }
}