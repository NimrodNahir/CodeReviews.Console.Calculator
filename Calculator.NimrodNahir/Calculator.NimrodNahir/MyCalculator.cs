using CalculatorLibrary;
using System.Text.RegularExpressions;


namespace MyCalculator
{
    internal class MyCalculator
    {
        private int _UsageCount = 0;

        internal void Init()
        {
            bool endApp = false;
            Console.WriteLine("Console Calculator in C#\r");
            Console.WriteLine("------------------------\n");

            Calculator calculator = new Calculator();

            while (!endApp)
            {
                double result = double.NaN;
                double firstOperand = GetNumberFromUser(calculator, $"Welcome to my calculator! (Successfully) used {_UsageCount} times!");
                string operation = GetOperation(firstOperand);
                string opPrint = GetOpPrint(operation);
                if (calculator.IsBinaryOperator(operation))
                {
                    double secondOperand = GetNumberFromUser(calculator, $"Selected first number: {firstOperand}, Selected Operation: {opPrint}");
                    result = calculator.DoOperation(firstOperand, secondOperand, operation);
                }
                else
                {
                    result = calculator.DoOperation(firstOperand, operation);
                }
                endApp = DisplayResult(result);

            }

            calculator.Finish();
        }

        private double GetNumberFromUser(Calculator i_Calculator, string i_Message = "")
        {
            bool isChooseing = true;
            string numRegexPattern = @"^[+-]?(((0|\d\d*)(\.\d*)?)|(\.\d+))$|^h$";
            double selectedNumber = 0;
            while (isChooseing)
            {
                Console.Clear();
                if (!string.IsNullOrEmpty(i_Message))
                {
                    Console.WriteLine(i_Message);
                    Console.WriteLine("-------------------------------------");
                }
                Console.WriteLine("Enter a number (or 'h' to display history) and press Enter: ");
                string? userInput = Console.ReadLine();
                while (string.IsNullOrEmpty(userInput) || !ValidateUserSelection(userInput, numRegexPattern))
                {
                    Console.WriteLine("Invalid input, please either enter a number or 'h'");
                    userInput = Console.ReadLine();
                }
                if (userInput == "h")
                {
                    double? historyNumber = SelectNumberFromHistory(i_Calculator);
                    if (historyNumber is not null)
                    {
                        selectedNumber = historyNumber.Value;
                        isChooseing = false;
                    }
                }
                else
                {
                    selectedNumber = double.Parse(userInput);
                    isChooseing = false;
                }
            }

            return selectedNumber;
        }

        private double? SelectNumberFromHistory(Calculator i_Calculator)
        {
            Console.Clear();
            double? selectedNum = null;

            if (i_Calculator.History.Count == 0)
            {
                Console.WriteLine("History is empty, please press any key to return");
                Console.ReadKey();
            }
            else
            {
                int index = 1;
                Console.WriteLine("Previous Calculations!");
                Console.WriteLine("---------------------------------");
                foreach (double prevCalc in i_Calculator.History)
                {
                    Console.WriteLine($"{index++}. {prevCalc}");
                }
                Console.WriteLine("---------------------------------");
                Console.WriteLine("Please enter the index of the selected number, 'c' for clear history or 'b' to return");
                string? userInput = Console.ReadLine();
                while (string.IsNullOrEmpty(userInput) || !(ValidateUserSelection(userInput, "^[bc]$") || ValidateUserSelection(userInput, 1, index)))
                {
                    Console.WriteLine("Please enter the index of the selected number, 'c' for clear history or 'b' to return");
                    userInput = Console.ReadLine();
                }
                if (userInput == "c")
                {
                    i_Calculator.ClearHistory();
                }
                else if (userInput != "b")
                {
                    selectedNum = i_Calculator.History[Int32.Parse(userInput) - 1];
                }
            }

            return selectedNum;
        }

        private string GetOpPrint(string i_Operation)
        {
            string print = i_Operation switch
            {
                "a" => "+",
                "s" => "-",
                "m" => "*",
                "d" => "/",
                "r" => "Square Root",
                "p" => "Power of",
                "ts" => "Sin",
                "tc" => "Cosine",
                "tt" => "Tangent",
                _ => ""
            };

            return print;
        }

        private  bool DisplayResult(double i_Result)
        {
            bool toEndApp = false;
            Console.Clear();
            if (double.IsNaN(i_Result))
            {
                Console.WriteLine("This operation will result in a mathematical error.\n");
            }
            else
            {
                _UsageCount++;
                Console.WriteLine("Your result: {0:0.##}\n", i_Result);
            }
            Console.WriteLine("------------------------\n");
            Console.Write("Press 'q' and Enter to close the app, or press any other key and Enter to continue: ");
            if (Console.ReadLine() == "q")
            {
                toEndApp = true;
            }

            Console.WriteLine("\n"); // Friendly linespacing.
            return toEndApp;
        }

        private  string GetOperation(double i_SelectedNum)
        {
            Console.Clear();
            Console.WriteLine($"Selected: {i_SelectedNum}");
            Console.WriteLine($"-----------------------------------");
            Console.WriteLine("Please type the desired opertaion:");
            Console.WriteLine("\ta - Add");
            Console.WriteLine("\ts - Subtract");
            Console.WriteLine("\tm - Multiply");
            Console.WriteLine("\td - Divide");
            Console.WriteLine("\tp - Power of");
            Console.WriteLine("\tr - Square Root");
            Console.WriteLine("\tts - Sin");
            Console.WriteLine("\ttc - Cosin");
            Console.WriteLine("\ttt - Tangent");
            string? userInput = Console.ReadLine();
            while (string.IsNullOrEmpty(userInput) || !ValidateUserSelection(userInput, "^(a|s|m|d|p|r|ts|tc|tt)$"))
            {
                Console.WriteLine("Error: Unrecognized input.");
                userInput = Console.ReadLine();
            }

            return userInput;
        }

        private  bool ValidateUserSelection(string i_UserInput, string i_RegexPattern)
        {
            return Regex.IsMatch(i_UserInput, i_RegexPattern);
        }

        private  bool ValidateUserSelection(string i_UserInput ,int i_LowerIndex, int i_HigherIndex)
        {
            int selectedIdx;
            return Int32.TryParse(i_UserInput, out selectedIdx) && (i_LowerIndex <= selectedIdx) && selectedIdx <= i_HigherIndex;

        }
    }
}
