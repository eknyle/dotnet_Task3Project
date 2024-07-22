using System.Collections;
using System;
using Task3Project.Classes;

namespace Task3Project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ResetColor();
            Dictionary<string, string> inputParams = new Dictionary<string, string>(3);
            inputParams.Add("a", String.Empty);
            inputParams.Add("b", String.Empty);
            inputParams.Add("c", String.Empty);
            List<ErrorObject> errorList = new List<ErrorObject>(3);

            WelcomeMessage();
            while (true)
            {
                try
                {
                    GetUserParameters(inputParams, errorList);
                    break;
                }
                catch (Exception ex)
                {
                    FormatData(ex.Message, Severity.Error, ex.Data);
                }
                errorList.Clear();
            }

            Console.WriteLine("Проверка входных параметров выполнена.");
            Console.WriteLine("Вычисляем корни квадратного уравнения.");
            try
            {
                SolveEquation(inputParams, errorList);
            }
            catch (NotFoundException ex) 
            {
                
                FormatData(ex.Message, Severity.Error, ex.Data);
            }
            
            Console.ReadKey();

        }

        public static void SolveEquation(Dictionary<string, string> inputParams, List<ErrorObject> errorList)
        {

            
            try
            {
                double a = Convert.ToDouble(inputParams["a"]);
                double b = Convert.ToDouble(inputParams["b"]);
                double c = Convert.ToDouble(inputParams["c"]);
                Console.WriteLine($"Квадратное уравнение {a} * x^2 + ({b}) * x  +({c}) = 0");
                var d = Math.Pow(b, 2) - 4 * a * c;

                switch (d)
                {
                    case < 0:
                        throw new NotFoundException("Дискриминант меньше нуля");
                    case 0:
                        var answer =  -b / (2 * a);
                        double result = 0;
                        if (!Double.TryParse(answer.ToString(), out result) || Double.IsNaN(answer)) {
                            throw new NotFoundException("Ошибка при попытке вычислить корни квадратного уравнения");
                        }
                        Console.WriteLine($"x = {answer}");
                        break;
                    case > 0:
                        Console.WriteLine($"x1 = {(-b + Math.Sqrt(d))/ (2 * a)}");
                        Console.WriteLine($"x2 = {(-b - Math.Sqrt(d)) / (2 * a)}");
                        break;
                }

            }catch (NotFoundException e)
            {
                errorList.Clear();
                errorList.Add(new ErrorObject($"Вещественных значений не найдено", String.Empty));
                e.Data.Add("message", errorList);
                FormatData(e.Message, Severity.Warning, e.Data);

                e=new NotFoundException(e.Message);
                errorList.Clear();
                errorList.Add(new ErrorObject("Ошибка вычисления корней квадратного уравнения", String.Empty));
                e.Data.Add("message", errorList);
                throw e;
            }

        }

        public static void WelcomeMessage()
        {
            Console.WriteLine("Вы запустили программу для решения квадратного уровнения формата a * x^2 + b * x + c = 0.");
            Console.WriteLine("Вам будет предложено ввести целочесленные значения: a, b и c.");
        }
        public static void WriteMessage(string param)
        {
            Console.WriteLine($"Введите значение {param}:");
        }
        public static void GetUserParameters(Dictionary<string, string> inputParams, List<ErrorObject> errorList)
        {

            bool hasErrors = false;
            foreach (string s in inputParams.Keys)
            {
                Console.WriteLine();
                WriteMessage(s);
                ReturnObject returnObject = ReadParameter();
                inputParams[s]=returnObject.Value;
                if (returnObject.HasError())
                {
                    hasErrors = true;
                    errorList.Add(new ErrorObject($"{returnObject.Message} {s}", $"{s} = {inputParams[s]}"));
                }
            }

            if (hasErrors == true)
            {
                Exception e = new Exception("Неправильный формат данных");
                e.Data.Add("message", errorList);
                throw e;
            }
        }

        static ReturnObject ReadParameter()
        {
            ReturnObject returnObj = new ReturnObject();
            int result = 0;
            var param = Console.ReadLine();
            if (Int32.TryParse(param, out result) == false)
            {
                returnObj.SetAllParameters("Неверный формат параметра",param);
            }
            else
            {
                returnObj.SetAllParameters(null, param);
            }

            return returnObj;
        }

        public static void FormatData(string message, Severity severity, IDictionary data)
        {
            Dictionary<string, ConsoleColor> colorSettings=GetSettings(severity);
            Console.ForegroundColor = colorSettings["Color"];
            Console.BackgroundColor = colorSettings["BackgroundColor"];
            
            foreach(var key in (List<ErrorObject>)data["message"])
            {
                for (int i = 0; i < 50; i++)
                {
                    Console.Write("-");
                }
                Console.WriteLine();
                Console.WriteLine(key.Message);
                for (int i = 0; i < 50; i++)
                {
                    Console.Write("-");
                }
                Console.WriteLine();
                if (key.Parameter != String.Empty)
                {
                    Console.WriteLine(key.Parameter);
                    Console.WriteLine();
                }                

            }
            
            Console.ResetColor();
        }

        public static Dictionary<string, ConsoleColor> GetSettings(Severity severity) 
        {
            Dictionary<string, ConsoleColor> data = new Dictionary<string, ConsoleColor>();
            switch (severity)
            {
                case Severity.Warning:
                    data.Add("Color", ConsoleColor.Black);
                    data.Add("BackgroundColor", ConsoleColor.Yellow);
                    break;
                case Severity.Error:
                    data.Add("Color", ConsoleColor.White);
                    data.Add("BackgroundColor", ConsoleColor.Red);
                    break;
            }

            return data;
        }
        public enum Severity
        {
            Warning,
            Error
        }

    }
}
