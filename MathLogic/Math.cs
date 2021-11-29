using System;

namespace MathLogic
{
    public static class Math
    {
        #region Convert Units
        public static double? ConvertUnits(double inputValue, DataTypes.LinearUnit inputUnit, DataTypes.LinearUnit outputUnit)
        {
            double? result = null;
            switch (inputUnit)
            {
                case DataTypes.LinearUnit.nm:
                    if (inputUnit == outputUnit)
                    {
                        result = inputValue;
                    }
                    else if (outputUnit == DataTypes.LinearUnit.um)
                    {
                        result = ConvertNMtoUM(inputValue);
                    }
                    else if (outputUnit == DataTypes.LinearUnit.mm)
                    {
                        result = ConvertNMtoMM(inputValue);
                    }
                    else if (outputUnit == DataTypes.LinearUnit.sm)
                    {
                        result = ConvertNMtoSM(inputValue);
                    }
                    else if (outputUnit == DataTypes.LinearUnit.m)
                    {
                        result = ConvertNMtoM(inputValue);
                    }
                    else if (outputUnit == DataTypes.LinearUnit.inch)
                    {
                        result = ConvertNMtoINCH(inputValue);
                    }
                    break;
                case DataTypes.LinearUnit.um:
                    if (inputUnit == outputUnit)
                    {
                        result = inputValue;
                    }
                    else if (outputUnit == DataTypes.LinearUnit.nm)
                    {
                        result = ConvertUMtoNM(inputValue);
                    }
                    else if (outputUnit == DataTypes.LinearUnit.mm)
                    {
                        result = ConvertUMtoMM(inputValue);
                    }
                    else if (outputUnit == DataTypes.LinearUnit.sm)
                    {
                        result = ConvertUMtoSM(inputValue);
                    }
                    else if (outputUnit == DataTypes.LinearUnit.m)
                    {
                        result = ConvertUMtoM(inputValue);
                    }
                    else if (outputUnit == DataTypes.LinearUnit.inch)
                    {
                        result = ConvertUMtoINCH(inputValue);
                    }
                    break;
                case DataTypes.LinearUnit.mm:
                    if (inputUnit == outputUnit)
                    {
                        result = inputValue;
                    }
                    else if (outputUnit == DataTypes.LinearUnit.nm)
                    {
                        result = ConvertMMtoNM(inputValue);
                    }
                    else if (outputUnit == DataTypes.LinearUnit.um)
                    {
                        result = ConvertMMtoUM(inputValue);
                    }
                    else if (outputUnit == DataTypes.LinearUnit.sm)
                    {
                        result = ConvertMMtoSM(inputValue);
                    }
                    else if (outputUnit == DataTypes.LinearUnit.m)
                    {
                        result = ConvertMMtoM(inputValue);
                    }
                    else if (outputUnit == DataTypes.LinearUnit.inch)
                    {
                        result = ConvertMMtoINCH(inputValue);
                    }
                    break;
                case DataTypes.LinearUnit.sm:
                    if (inputUnit == outputUnit)
                    {
                        result = inputValue;
                    }
                    else if (outputUnit == DataTypes.LinearUnit.nm)
                    {
                        result = ConvertSMtoNM(inputValue);
                    }
                    else if (outputUnit == DataTypes.LinearUnit.um)
                    {
                        result = ConvertSMtoUM(inputValue);
                    }
                    else if (outputUnit == DataTypes.LinearUnit.mm)
                    {
                        result = ConvertSMtoMM(inputValue);
                    }
                    else if (outputUnit == DataTypes.LinearUnit.m)
                    {
                        result = ConvertSMtoM(inputValue);
                    }
                    else if (outputUnit == DataTypes.LinearUnit.inch)
                    {
                        result = ConvertSMtoINCH(inputValue);
                    }
                    break;
                case DataTypes.LinearUnit.m:
                    if (inputUnit == outputUnit)
                    {
                        result = inputValue;
                    }
                    else if (outputUnit == DataTypes.LinearUnit.nm)
                    {
                        result = ConvertMtoNM(inputValue);
                    }
                    else if (outputUnit == DataTypes.LinearUnit.um)
                    {
                        result = ConvertMtoUM(inputValue);
                    }
                    else if (outputUnit == DataTypes.LinearUnit.sm)
                    {
                        result = ConvertMtoSM(inputValue);
                    }
                    else if (outputUnit == DataTypes.LinearUnit.mm)
                    {
                        result = ConvertMtoMM(inputValue);
                    }
                    else if (outputUnit == DataTypes.LinearUnit.inch)
                    {
                        result = ConvertMtoINCH(inputValue);
                    }
                    break;
                case DataTypes.LinearUnit.inch:
                    if (inputUnit == outputUnit)
                    {
                        result = inputValue;
                    }
                    else if (outputUnit == DataTypes.LinearUnit.nm)
                    {
                        result = ConvertINCHtoNM(inputValue);
                    }
                    else if (outputUnit == DataTypes.LinearUnit.um)
                    {
                        result = ConvertINCHtoUM(inputValue);
                    }
                    else if (outputUnit == DataTypes.LinearUnit.mm)
                    {
                        result = ConvertINCHtoMM(inputValue);
                    }
                    else if (outputUnit == DataTypes.LinearUnit.sm)
                    {
                        result = ConvertINCHtoSM(inputValue);
                    }
                    else if (outputUnit == DataTypes.LinearUnit.m)
                    {
                        result = ConvertINCHtoM(inputValue);
                    }
                    break;
            }
            return result;
        }

        #region input - nm
        public static double ConvertNMtoINCH(double inputValue)
        {
            double result = inputValue / (System.Math.Pow(10, 7) * 2.54);
            return result;
        }

        public static double ConvertNMtoUM(double inputValue)
        {
            double result = inputValue / System.Math.Pow(10, 3);
            return result;
        }

        public static double ConvertNMtoMM(double inputValue)
        {
            double result = inputValue / System.Math.Pow(10, 6);
            return result;
        }

        public static double ConvertNMtoSM(double inputValue)
        {
            double result = inputValue / System.Math.Pow(10, 7);
            return result;
        }

        public static double ConvertNMtoM(double inputValue)
        {
            double result = inputValue / System.Math.Pow(10, 9);
            return result;
        }

        #endregion

        #region input - um
        public static double ConvertUMtoINCH(double inputValue)
        {
            double result = inputValue / (System.Math.Pow(10, 4) * 2.54);
            return result;
        }

        public static double ConvertUMtoNM(double inputValue)
        {
            double result = inputValue * System.Math.Pow(10, 3);
            return result;
        }

        public static double ConvertUMtoMM(double inputValue)
        {
            double result = inputValue / System.Math.Pow(10, 3);
            return result;
        }

        public static double ConvertUMtoSM(double inputValue)
        {
            double result = inputValue / System.Math.Pow(10, 4);
            return result;
        }

        public static double ConvertUMtoM(double inputValue)
        {
            double result = inputValue / System.Math.Pow(10, 6);
            return result;
        }

        #endregion

        #region input - mm
        public static double ConvertMMtoNM(double inputValue)
        {
            double result = inputValue * System.Math.Pow(10, 6);
            return result;
        }

        public static double ConvertMMtoUM(double inputValue)
        {
            double result = inputValue * System.Math.Pow(10, 3);
            return result;
        }

        public static double ConvertMMtoSM(double inputValue)
        {
            double result = inputValue * 10;
            return result;
        }

        public static double ConvertMMtoM(double inputValue)
        {
            double result = inputValue * System.Math.Pow(10, 3);
            return result;
        }

        public static double ConvertMMtoINCH(double inputValue)
        {
            double result = inputValue / 25.4;
            return result;
        }

        #endregion

        #region input - sm
        public static double ConvertSMtoNM(double inputValue)
        {
            double result = inputValue * System.Math.Pow(10, 7);
            return result;
        }

        public static double ConvertSMtoUM(double inputValue)
        {
            double result = inputValue / System.Math.Pow(10, 4);
            return result;
        }

        public static double ConvertSMtoMM(double inputValue)
        {
            double result = inputValue / 10;
            return result;
        }

        public static double ConvertSMtoM(double inputValue)
        {
            double result = inputValue / System.Math.Pow(10, 2);
            return result;
        }

        public static double ConvertSMtoINCH(double inputValue)
        {
            double result = inputValue / 2.54;
            return result;
        }
        #endregion

        #region input - m
        public static double ConvertMtoNM(double inputValue)
        {
            double result = inputValue * System.Math.Pow(10, 9);
            return result;
        }
        public static double ConvertMtoUM(double inputValue)
        {
            double result = inputValue * System.Math.Pow(10, 6);
            return result;
        }

        public static double ConvertMtoMM(double inputValue)
        {
            double result = inputValue * System.Math.Pow(10, 3);
            return result;
        }

        public static double ConvertMtoSM(double inputValue)
        {
            double result = inputValue * System.Math.Pow(10, 2);
            return result;
        }

        public static double ConvertMtoINCH(double inputValue)
        {
            double result = inputValue / 0.0254;
            return result;
        }
        #endregion

        #region input - inch
        public static double ConvertINCHtoNM(double inputValue)
        {
            double result = inputValue * (System.Math.Pow(10, 6) * 25.4);
            return result;
        }

        public static double ConvertINCHtoUM(double inputValue)
        {
            double result = inputValue * (System.Math.Pow(10, 3) * 25.4);
            return result;
        }

        public static double ConvertINCHtoMM(double inputValue)
        {
            double result = inputValue * 25.4;
            return result;
        }

        public static double ConvertINCHtoSM(double inputValue)
        {
            double result = inputValue * 2.54;
            return result;
        }

        public static double ConvertINCHtoM(double inputValue)
        {
            double result = inputValue * 0.0254;
            return result;
        }
        #endregion
        #endregion

        public static double polinomBershtain(int i, int n, double t)// Polynom Bershtain
        {
            return (Factorial(n) / (Factorial(i) * Factorial(n - i))) * System.Math.Pow(t, i) * System.Math.Pow(1 - t, n - i);
        }

        public static int Factorial(int n) // Calk Factorial
        {
            int res = 1;
            for (int i = 1; i <= n; i++)
                res *= i;
            return res;
        }
    }
}
