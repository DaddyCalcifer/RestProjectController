using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

namespace RestProjectController.Tests
{
    public class API_Tests
    {
        private delegate Task<TestAPI> TestDelegate();
        class TestFunc
        {
            public TestFunc(string name, TestDelegate func)
            {
                this.name = name;
                this.func = func;
            }
            public string name;
            public TestDelegate func;
        }
        static readonly Uri api_adress = new Uri("https://localhost:5001/");
        static int current_test = 0;
        static List<TestFunc> tests = new List<TestFunc>();
        static async Task Main(string[] args)
        {
            tests.Add(new TestFunc("End-to-End тестирование", E2E_Test));
            tests.Add(new TestFunc("Flats.GetAll()",TestGetAll));
            tests.Add(new TestFunc("Flats.GetByName()", TestGetByName));
            tests.Add(new TestFunc("Flats.Add()", AddFlat));
            tests.Add(new TestFunc("Flats.Add(existed)", AddExistedFlat));
            tests.Add(new TestFunc("Reservations.Add()", AddReservation));
            tests.Add(new TestFunc("Reservations.Cancel()", CancelReservation));

            PrintTests();

            while (true)
            {
                switch(Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        if (current_test > 0)
                            current_test--;
                        break;

                    case ConsoleKey.DownArrow:
                        if (current_test < tests.Count -1)
                            current_test++;
                        break;

                    case ConsoleKey.Enter:
                        Console.Clear();
                        tests[current_test].func().Result.Print();
                        Console.ReadKey();
                        break;

                    case ConsoleKey.Escape:
                        return;
                }
                PrintTests();
            }
        }
        static void PrintTests()
        {
            Console.Clear();
            for(int i = 0; i < tests.Count; i++)
            {
                if(i == current_test)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.WriteLine(tests[i].name);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine(Environment.NewLine);
            Console.Write("Навигация: стрелки \nВыбор: Enter\nВыход: Escape");
        }

        public static async Task<TestAPI> TestGetAll()
        {
            TestAPI result = new TestAPI("Flats.GetAll()");
            result.Description = "Функция получения JSON файла со всеми квартирами в бд";
            HttpClient client = new HttpClient();
            client.BaseAddress = api_adress;
            
            
            HttpResponseMessage response = await client.GetAsync("flats/all");
            result.response = response;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                result.result = TestResult.Succes;
            else result.result = TestResult.Fail;


            return result;
        }
        public static async Task<TestAPI> TestGetByName()
        {
            string name = "test_kv";
            TestAPI result = new TestAPI("Flats.GetByName()");
            result.Description = "Функция получения JSON файла с квартирами по имени";
            HttpClient client = new HttpClient();
            client.BaseAddress = api_adress;


            HttpResponseMessage response = await client.GetAsync("flats/byname:" + name);
            result.response = response;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                result.result = TestResult.Succes;
            else result.result = TestResult.Fail;


            return result;
        }
        public static async Task<TestAPI> AddFlat()
        {
            string name = "test kv " + new Random().Next(1000).ToString();
            bool full = true; 
            int sleep = new Random().Next(3); 
            decimal cost = new Random().Next(25) * 100;

            TestAPI result = new TestAPI("Flats.Add()");
            result.Description = "Функция добавления жилья в базу";
            HttpClient client = new HttpClient();
            client.BaseAddress = api_adress;

            HttpResponseMessage reg_response = await client.GetAsync("auth/unitTest:12345678");
            string jwt = await reg_response.Content.ReadAsStringAsync();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwt);

            HttpResponseMessage response = await client.PostAsync("flats/add/" + 
                name + "/" + 
                full.ToString() + "/" + 
                sleep.ToString() + "/" + 
                cost.ToString(), 
                null);

            result.response = response;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                result.result = TestResult.Succes;
            else result.result = TestResult.Fail;


            return result;
        }
        public static async Task<TestAPI> AddExistedFlat()
        {
            string name = "test ex_kv";
            bool full = true;
            int sleep = 2;
            decimal cost = 1111;

            TestAPI result = new TestAPI("Flats.Add(existed)");
            result.Description = "Функция добавления уже существующего жилья в базу";
            HttpClient client = new HttpClient();
            client.BaseAddress = api_adress;


            HttpResponseMessage reg_response = await client.GetAsync("auth/unitTest:12345678");
            string jwt = await reg_response.Content.ReadAsStringAsync();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwt);


            HttpResponseMessage response = await client.PostAsync("flats/add/" +
                name + "/" +
                full.ToString() + "/" +
                sleep.ToString() + "/" +
                cost.ToString(),
                null);

            result.response = response;
            var answer = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK && answer != "error")
                result.result = TestResult.Succes;
            else result.result = TestResult.Fail;


            return result;
        }
        public static async Task<TestAPI> AddReservation()
        {
            string flat_id = "64a2b29b6d333eeabd9c0f9c";
            string date = "2023-07-11";
            int days = 2;

            TestAPI result = new TestAPI("Reservations.Add()");
            result.Description = "Функция добавления брони на квартиру";
            HttpClient client = new HttpClient();
            client.BaseAddress = api_adress;

            HttpResponseMessage reg_response = await client.GetAsync("auth/unitTest:12345678");
            string jwt = await reg_response.Content.ReadAsStringAsync();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwt);

            HttpResponseMessage response = await client.PostAsync("booking/add/" +
                flat_id + "/" +
                date + ":" +
                days,
                null);

            result.response = response;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                result.result = TestResult.Succes;
            else result.result = TestResult.Fail;


            return result;
        }
        public static async Task<TestAPI> CancelReservation()
        {
            string reservation_id = "64ac46772045341e948646a5";
            TestAPI result = new TestAPI("Reservations.Cancel()");
            result.Description = "Функция отмены брони на квартиру";
            HttpClient client = new HttpClient();
            client.BaseAddress = api_adress;

            HttpResponseMessage reg_response = await client.GetAsync("auth/unitTest:12345678");
            string jwt = await reg_response.Content.ReadAsStringAsync();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwt);

            HttpResponseMessage response = await client.PatchAsync("booking/cancel:" + reservation_id, null);
            result.response = response;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                result.result = TestResult.Succes;
            else result.result = TestResult.Fail;


            return result;
        }

        public static async Task<TestAPI> E2E_Test()
        {
            string login = "e2e_user", name = "Самый обычный пользователь", password = "qazxdrews";
            string flat_id = "64a2b29b6d333eeabd9c0f9c", user_id = "64a040c6601e4d5f061adcd6", reservation_id = "64abe2c4103c656e5f6b34ea";
            string Date = "2023-09-13"; int days = 5;
            string jwt = "";

            TestAPI result = new TestAPI("E2E [Клиент]");
            result.Description = "Проверка полного функционала сервиса от лица пользователя";
            result.response = null;

            HttpClient client = new HttpClient();
            client.BaseAddress = api_adress;

            //Создание аккаунта
            HttpResponseMessage reg_response = await client.PostAsync("auth/register/"
                + name +"/"
                + login + ":"
                + password,null);

            var Register = new TestAPI("E2E [Регистрация]");
            Register.Description = "Создание аккаунта пользователем";
            Register.response = reg_response;

            if (Register.response.IsSuccessStatusCode) Register.result = TestResult.Succes;
            else Register.result = TestResult.Fail;
            Register.Print();

            //Авторизация
            HttpResponseMessage log_response = await client.GetAsync("auth/"
                + login + ":" + password);

            var login_ = new TestAPI("E2E [Авторизация]");
            login_.Description = "Авторизация пользователя в созданный аккаунт";
            login_.response = log_response;
            jwt = await log_response.Content.ReadAsStringAsync();
            if (login_.response.IsSuccessStatusCode) login_.result = TestResult.Succes;
            else login_.result = TestResult.Fail;
            login_.Print();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwt);

            //Бронь квартиры
            HttpResponseMessage booking_response = await client.PostAsync("booking/add/"
               + flat_id + "/"
               + Date + ":" 
               + days.ToString(), null);

            var booking = new TestAPI("E2E [Бронь квартиры]");
            booking.Description = "Создание пользователем брони на квартиру";
            booking.response = booking_response;
            if (booking.response.IsSuccessStatusCode) booking.result = TestResult.Succes;
            else booking.result = TestResult.Fail;
            booking.Print();

            //Отмена брони квартиры
            HttpResponseMessage cancel_response = await client.PatchAsync("booking/cancel:" + reservation_id, null);

            var cancel = new TestAPI("E2E [Отмена брони квартиры]");
            cancel.Description = "Отмена пользователем брони на квартиру";
            cancel.response = cancel_response;
            if (cancel.response.IsSuccessStatusCode) cancel.result = TestResult.Succes;
            else cancel.result = TestResult.Fail;
            cancel.Print();

            //Удаление аккаунта
            HttpResponseMessage delete_response = await client.PatchAsync("auth/delete", null);

            var delete = new TestAPI("E2E [Удаление аккаунта]");
            delete.Description = "Удаление пользователем созданного аккаунта";
            delete.response = delete_response;
            if (delete.response.IsSuccessStatusCode) delete.result = TestResult.Succes;
            else delete.result = TestResult.Fail;
            delete.Print();


            //Финал
            if (Register.response.IsSuccessStatusCode &&
                login_.response.IsSuccessStatusCode &&
                booking.response.IsSuccessStatusCode &&
                cancel.response.IsSuccessStatusCode &&
                delete.response.IsSuccessStatusCode)
                result.result = TestResult.Succes;
            else result.result = TestResult.Fail;

            return result;
        }
    }
}