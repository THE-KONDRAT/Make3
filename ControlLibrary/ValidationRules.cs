using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace ControlLibrary.ValidationRules
{
    public class ProjectPathRule : ValidationRule
    {
        private StringWrapper projectName;
        public StringWrapper ProjectName
        {
            get { return projectName; }
            set
            {
                projectName = value;
                if (projectName != null)
                {
                    projectName.Value = projectName.Value == null ? null : projectName.Value;
                }

                //MessageBox.Show("PathValue: " + projectName.Value);
                //string path = value.Value;

                //projectPath = FileOperations.FileAccess.GetDirectoryPath(value);
            }
        }

        //public StringWrapper ProjectName { get; set; }

        public ProjectPathRule()
        {
            projectName = new StringWrapper();
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string message = $"Illegal characters";
            try
            {
                //string projectPath = FileOperations.FileAccess.GetDirectoryPathWithSeparator((string)value);
                ValidationResult projectPathSymbols = CheckProjectPathSymbols((string)value);
                if (!projectPathSymbols.IsValid)
                {
                    message = projectPathSymbols.ErrorContent.ToString();
                    return new ValidationResult(false, message);
                }
                else
                {
                    //ValidateProjectName
                    ValidationResult projectNameSymbols = ProjectNameRule.CheckProjectNameSymbols(ProjectName.Value);
                    if (!projectNameSymbols.IsValid)
                    {
                        message = projectNameSymbols.ErrorContent.ToString();
                        return new ValidationResult(false, message);
                    }
                    else
                    {
                        string projectDirectoryFullPath = FileOperations.FileStructure.GetProjectDirectoryFullPath((string)value, ProjectName.Value);
                        //Check if directory already exists
                        if (FileOperations.FileAccess.CheckDirectoryExists(projectDirectoryFullPath))
                        {
                            return new ValidationResult(false, message);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                return new ValidationResult(false, e.Message);
            }

            return ValidationResult.ValidResult;
        }

        internal static ValidationResult CheckProjectPathSymbols(object value)
        {
            string message = $"Illegal characters";
            try
            {
                if (string.IsNullOrWhiteSpace((string)value))
                {
                    message = "empty path";
                    return new ValidationResult(false, message);
                }
                else
                {
                    string projectPath = FileOperations.FileAccess.GetDirectoryPathWithSeparator((string)value);
                    if (!FileOperations.FileAccess.ValidatePathSymbols(projectPath, FileOperations.FileAccess.PathType.Directory))
                    {
                        return new ValidationResult(false, message);
                    }
                }
            }
            catch (Exception e)
            {
                return new ValidationResult(false, e.Message);
            }

            return ValidationResult.ValidResult;
        }
    }

    public class ProjectNameRule : ValidationRule
    {
        private StringWrapper projectPath;
        public StringWrapper ProjectPath
        {
            get { return projectPath; }
            set
            {
                projectPath = value;
                if (projectPath != null)
                {
                    if (projectPath.Value != null)
                    {
                        string projPath = FileOperations.FileAccess.GetDirectoryPathWithSeparator(projectPath.Value);
                        try
                        {
                            projPath = FileOperations.FileAccess.GetDirectoryPath(projPath);
                        }
                        catch
                        {
                            projPath = null;
                        }
                        projectPath.Value = projPath;
                    }
                    //projectPath.Value = projectPath.Value == null ? null : FileOperations.FileAccess.GetDirectoryPath(projectPath.Value);
                }
                
                //string path = value.Value;
                
                //projectPath = FileOperations.FileAccess.GetDirectoryPath(value);
            }
        }

        public ProjectNameRule()
        {
            projectPath = new StringWrapper();
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string message = $"Illegal characters";
            try
            {
                ValidationResult projectNameSymbols = CheckProjectNameSymbols(value);
                if (!projectNameSymbols.IsValid)
                {
                    message = projectNameSymbols.ErrorContent.ToString();
                    return new ValidationResult(false, message);
                }
                else
                {
                    //ValidateProjectPath
                    ValidationResult projectPathSymbols = ProjectPathRule.CheckProjectPathSymbols(ProjectPath.Value);
                    if (!projectPathSymbols.IsValid)
                    {
                        message = projectPathSymbols.ErrorContent.ToString();
                        return new ValidationResult(false, message);
                    }
                    else
                    {
                        string projectDirectoryFullPath = FileOperations.FileStructure.GetProjectDirectoryFullPath(ProjectPath.Value, (string)value);
                        //Check if directory already exists
                        if (FileOperations.FileAccess.CheckDirectoryExists(projectDirectoryFullPath))
                        {
                            return new ValidationResult(false, message);
                        }
                    }
                    
                }

            }
            catch (Exception e)
            {
                return new ValidationResult(false, e.Message);
            }

            return ValidationResult.ValidResult;
        }

        internal static ValidationResult CheckProjectNameSymbols(object value)
        {
            string message = $"Illegal characters";
            try
            {
                if (string.IsNullOrWhiteSpace((string)value))
                {
                    message = "empty path";
                    return new ValidationResult(false, message);
                }
                else
                {
                    if (!FileOperations.FileAccess.ValidatePathSymbols((string)value, FileOperations.FileAccess.PathType.File))
                    {
                        return new ValidationResult(false, message);
                    }
                }
            }
            catch (Exception e)
            {
                return new ValidationResult(false, e.Message);
            }

            return ValidationResult.ValidResult;
        }
    }

    public class StringWrapper : DependencyObject
    {
        public static readonly DependencyProperty ValDP = DependencyProperty.Register("Value", typeof(string), typeof(StringWrapper));
        /*public static readonly DependencyProperty ValDP = DependencyProperty.Register(
           "Value", typeof(string), typeof(StringWrapper), new FrameworkPropertyMetadata(null));*/

        public string Value
        {
            get { return (string)GetValue(ValDP); }
            set
            {
                //MessageBox.Show("WrapperValue: " + value);
                SetValue(ValDP, value);
            }
        }

        public StringWrapper()
        {

        }
    }

    public class BindingProxy : System.Windows.Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new PropertyMetadata(null));
    }

    public class PathRule : ValidationRule
    {
        public FileOperations.FileAccess.PathType PathType { get; set; }

        public PathRule()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string message = $"Illegal characters";
            try
            {
                if (string.IsNullOrWhiteSpace((string)value))
                {
                    message = "empty path";
                    return new ValidationResult(false, message);
                }
                else
                {
                    if (FileOperations.FileAccess.ValidatePathSymbols((string)value, PathType))
                    {
                        return new ValidationResult(false, message);
                    }
                }
                    
            }
            catch (Exception e)
            {
                return new ValidationResult(false, e.Message);
            }

            return ValidationResult.ValidResult;
        }

        
    }

    public class NumberRule : ValidationRule
    {
        //public Type NumberType { get; set; }
        private NumberRuleWrapper validationParams;
        public NumberRuleWrapper ValidationParams
        {
            get { return validationParams; }
            set
            {
                validationParams = value;
                /*if (numberType != null)
                {
                    numberType.Value = numberType.Value == null ? null : numberType.Value;
                }*/
            }
        }

        public NumberRule()
        {
            ValidationParams = new NumberRuleWrapper();
            validationParams.NumberType = DataTypes.NumberType.Integer;
            validationParams.Minimum = 0.0m;
            validationParams.Maximum = 100.0m;
        }

        /*
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string message = $"number is not integer";
            bool success = false;

            try
            {
                var method = NumberType.GetMethods().Single(
                    m => (m.Name.Equals("TryParse")) &&
                    (m.GetParameters().Where(c => c.ParameterType.Equals(typeof(string))).Count() > 0) &&
                    (m.GetParameters().Where(c => c.ParameterType.Equals(typeof(IFormatProvider))).Count() > 0));
                object[] args = { value, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, null };
                if (method != null)
                {
                    DataTypes.NumberType numberType = DataTypes.NumberOperations.DefineNumberType(NumberType);
                    if (numberType == DataTypes.NumberType.Integer)
                    {
                        args[1] = System.Globalization.NumberStyles.Integer;
                        success = (bool)method.Invoke(null, args);
                        //decimal d;
                        //success = decimal.TryParse((string)value, NumberStyles.Integer, System.Globalization.NumberFormatInfo.InvariantInfo, out d);
                        if (!success)
                        {
                            return new ValidationResult(false, message);
                        }
                    }
                    else if (numberType == DataTypes.NumberType.Fractional)
                    {
                        args[1] = System.Globalization.NumberStyles.Float;
                        success = (bool)method.Invoke(null, args);
                        if (!success)
                        {
                            return new ValidationResult(false, message);
                        }
                    }
                    else
                    {
                        return new ValidationResult(false, message);
                    }
                }
                else
                {
                    return new ValidationResult(false, message);
                }

            }
            catch (Exception e)
            {
                return new ValidationResult(false, e.Message);
            }

            return ValidationResult.ValidResult;
        }
        */

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string message = $"number is not integer";
            bool success = false;
            decimal d;
            try
            {
                if (ValidationParams.NumberType == DataTypes.NumberType.Integer)
                {
                    success = decimal.TryParse((string)value, NumberStyles.Integer, System.Globalization.NumberFormatInfo.InvariantInfo, out d);
                    //decimal d;
                    if (!success)
                    {
                        return new ValidationResult(false, message);
                    }
                }
                else if (ValidationParams.NumberType == DataTypes.NumberType.Fractional)
                {
                    success = decimal.TryParse((string)value, NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo, out d);
                    if (!success)
                    {
                        return new ValidationResult(false, message);
                    }
                }
                else
                {
                    return new ValidationResult(false, message);
                }

                //Validate Min/Max

                if(d < ValidationParams.Minimum)
                {
                    message = "value is too low";
                    return new ValidationResult(false, message);
                }
                if (d > ValidationParams.Maximum)
                {
                    message = "value is too big";
                    return new ValidationResult(false, message);
                }

                /*
                var r = value.GetType();
                var method = value.GetType().GetMethods().Single(
                    m => (m.Name.Equals("TryParse")) &&
                    (m.GetParameters().Where(c => c.ParameterType.Equals(typeof(string))).Count() > 0) &&
                    (m.GetParameters().Where(c => c.ParameterType.Equals(typeof(IFormatProvider))).Count() > 0));
                object[] args = { value, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, null };
                if (method != null)
                {
                    if (NumberType.Value == DataTypes.NumberType.Integer)
                    {
                        args[1] = System.Globalization.NumberStyles.Integer;
                        success = (bool)method.Invoke(null, args);
                        //decimal d;
                        //success = decimal.TryParse((string)value, NumberStyles.Integer, System.Globalization.NumberFormatInfo.InvariantInfo, out d);
                        if (!success)
                        {
                            return new ValidationResult(false, message);
                        }
                    }
                    else if (NumberType.Value == DataTypes.NumberType.Fractional)
                    {
                        args[1] = System.Globalization.NumberStyles.Float;
                        success = (bool)method.Invoke(null, args);
                        if (!success)
                        {
                            return new ValidationResult(false, message);
                        }
                    }
                    else
                    {
                        return new ValidationResult(false, message);
                    }
                }
                else
                {
                    return new ValidationResult(false, message);
                }
                */
            }
            catch (Exception e)
            {
                return new ValidationResult(false, e.Message);
            }

            return ValidationResult.ValidResult;
        }
    }

    public class NumberRuleWrapper : DependencyObject
    {
        public static readonly DependencyProperty NumberTypeProperty = DependencyProperty.Register("NumberType", typeof(DataTypes.NumberType), typeof(NumberRuleWrapper));

        public DataTypes.NumberType NumberType
        {
            get { return (DataTypes.NumberType)GetValue(NumberTypeProperty); }
            set { SetValue(NumberTypeProperty, value); }
        }

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(decimal), typeof(NumberRuleWrapper));
        public decimal Minimum
        {
            get { return (decimal)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(decimal), typeof(NumberRuleWrapper));
        public decimal Maximum
        {
            get { return (decimal)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public NumberRuleWrapper()
        {

        }
    }
}
