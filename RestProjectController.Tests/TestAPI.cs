using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestProjectController.Tests
{
    public enum TestResult { Succes = 1, Fail =-1, NoData =0 }
    public class TestAPI
    {
        public string Name { get; set; } = "API Test";
        public string Description { get; set; } = "...";
        public HttpResponseMessage response;
        public TestResult result = TestResult.NoData;

        public TestAPI(string name)
        {
            this.Name = name;
        }
        public TestAPI(){}

        public async void Print()
        {
            var answer = await response.Content.ReadAsStringAsync();
            Console.Write("Тест: ");
            Console.ForegroundColor = ConsoleColor.DarkGreen; Console.Write(this.Name + Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.White; Console.Write("Описание: ");
            Console.ForegroundColor = ConsoleColor.Gray; Console.Write(this.Description + Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.White; Console.Write("Результат запроса: " + Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.Gray; Console.Write(answer + Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.White; Console.Write("Статус HTTP: ");
            Console.ForegroundColor = ConsoleColor.Yellow; Console.Write(response.StatusCode+ Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.White; Console.Write("Результат теста: ");
            switch(result)
            {
                case TestResult.NoData:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("Нет данных");
                    break;
                case TestResult.Succes:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Пройден!");
                    break;
                case TestResult.Fail:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Не пройден!");
                    break;
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Environment.NewLine + Environment.NewLine);
        }
    }
}
