using Newtonsoft.Json;
using System.Diagnostics;
using System;
using System.IO;
using System.Text;

namespace CalculatorLibrary
{
    public class Calculator
    {
        JsonWriter writer;
        
        public Calculator(string recordAll)
        {
            if (recordAll == "y")  //this will record all the information during executions
            {
                StreamWriter sw = new StreamWriter("calculatorlog.json", true, Encoding.ASCII);
                writer = new JsonTextWriter(sw);
                writer.Formatting = Formatting.Indented;
                writer.WriteStartObject();
                writer.WritePropertyName("Operations");
                writer.WriteStartArray();
            }

            else   //this option will clear the file and record only this execution
            {
                StreamWriter logFile = File.CreateText("calculatorlog.json");
                logFile.AutoFlush = true;
                writer = new JsonTextWriter(logFile);
                writer.Formatting = Formatting.Indented;
                writer.WriteStartObject();
                writer.WritePropertyName("Operations");
                writer.WriteStartArray();
            }
        }
        

        public double DoOperation(double num1, double num2, string op)
        {
            double result = double.NaN; // Default value is "not-a-number" if an operation, such as division, could result in an error.
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(num1);
            if (op != "f" && op != "root")
            {
                writer.WritePropertyName("Operand2");
                writer.WriteValue(num2);
            }
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
                case "d":
                    // Ask the user to enter a non-zero divisor.
                    if (num2 != 0)
                    {
                        result = num1 / num2;
                    }
                    writer.WriteValue("Divide");
                    break;
                case "e":
                    result = calcExponent(num1, num2);
                    writer.WriteValue("Exponent");
                    break;
                case "f":
                    result = calcFactorial(num1);
                    writer.WriteValue("Factorial");
                    break;
                case "max":
                    if (num1 >= num2) result = num1;
                    else result = num2;
                    writer.WriteValue("Maximum of 2 numbers");
                    break;
                case "min":
                    if (num1 <= num2) result = num1;
                    else result = num2;
                    writer.WriteValue("Minimum of 2 numbers");
                    break;
                case "root":
                    result = Math.Sqrt(num1);
                    writer.WriteValue("Square root");
                    break;
                // Return text for an incorrect option entry.
                default:
                    break;
            }
            writer.WritePropertyName("Result");
            writer.WriteValue(result);
            writer.WriteEndObject();

            return result;
        }

        public Double calcExponent(double num1, double num2)
        {
            if (num2 == 0) return 1;
            else return num1 * calcExponent(num1, num2 - 1);   
        }

        public Double calcFactorial(double num1) 
        {
            if(num1 < 0) return double.NaN;
            else if (num1 == 0) return 1;
            else return num1 * calcFactorial(num1 - 1);
        }

        public void Finish()
        {
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();
        }
    }
}
