using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace CalculatorLibrary
{
    public class Calculator
    {

        private JsonWriter writer;
        private readonly List<double> _PreviousCalcsList; 

        public ReadOnlyCollection<double> History
        {
            get => new ReadOnlyCollection<double>(_PreviousCalcsList);
        }
        public Calculator()
        {
            _PreviousCalcsList = [];
            StreamWriter logFile = File.CreateText("calculatorlog.json");
            logFile.AutoFlush = true;
            writer = new JsonTextWriter(logFile);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("Operations");
            writer.WriteStartArray();
        }

        public double DoOperation(double num,string op)
        {
            double result = double.NaN;
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(num);
            writer.WritePropertyName("Operation");
            switch (op)
            {
                case "ts":
                    result = Math.Sin(num);
                    writer.WriteValue("Sin");
                    break;                
                case "tc":
                    result = Math.Cos(num);
                    writer.WriteValue("Cosine");
                    break;                
                case "tt":
                    result = Math.Tan(num);
                    writer.WriteValue("Tan");
                    break;                
                case "r":
                    double temp = Math.Sqrt(num);
                    writer.WriteValue("Sqrt");
                    result = temp == double.PositiveInfinity ? double.NaN : temp;
                    break;
                default:
                    break;
            };

            _PreviousCalcsList.Add(result);
            writer.WritePropertyName("Result");
            writer.WriteValue(result);
            writer.WriteEndObject();
            return result;

        }
        public double DoOperation(double num1, double num2, string op)
        {
            double result = double.NaN; // Default value is "not-a-number" if an operation, such as division, could result in an error.
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(num1);
            writer.WritePropertyName("Operand2");
            writer.WriteValue(num2);
            writer.WritePropertyName("Operation");
            // Use a switch statement to do the math.
            switch (op)
            {
                case "a":
                    result = num1 + num2;
                    writer.WriteValue("Add");
                    break;
                case "s":
                    result = num1 - num2;
                    writer.WriteValue("Subtract");
                    break;
                case "m":
                    result = num1 * num2;
                    writer.WriteValue("Multiply");
                    break;
                case "p":
                    result = Math.Pow(num1,num2);
                    writer.WriteValue("Power of");
                    break;
                case "d":
                    // Ask the user to enter a non-zero divisor.
                    if (num2 != 0)
                    {
                        result = num1 / num2;
                    }
                    writer.WriteValue("Divide");
                    break;
                // Return text for an incorrect option entry.
                default:
                    break;
            }
            _PreviousCalcsList.Add(result);
            writer.WritePropertyName("Result");
            writer.WriteValue(result);
            writer.WriteEndObject();

            return result;
        }

        public void ClearHistory()
        {
            _PreviousCalcsList.Clear();
        }

        public bool IsBinaryOperator(string i_Operation)
        {
            return Regex.IsMatch(i_Operation, "^(a|s|m|d|p)$");
        }
        public void Finish()
        {
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();
        }
    }
}
