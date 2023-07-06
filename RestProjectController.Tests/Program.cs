using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

namespace RestProjectController.Tests
{
    public class API_Tests
    {
        static readonly Uri api_adress = new Uri("https://localhost:5001/");
        static async Task Main(string[] args)
        {
            var GetAll = await TestGetAll(); GetAll.Print();
            var GetByName = await TestGetByName(); GetByName.Print();
            //var addFlat = await AddFlat(); addFlat.Print();
            var addExFlat = await AddExistedFlat(); addExFlat.Print();
            var addReservation = await AddReservation(); addReservation.Print();
            var cancelReservation = await CancelReservation(); cancelReservation.Print();
        }
        public static async Task<TestAPI> TestGetAll()
        {
            TestAPI result = new TestAPI("Flats.GetAll()");
            result.Description = "Функция получения JSON файла со всеми квартирами в бд";
            HttpClient client = new HttpClient();
            client.BaseAddress = api_adress;
            
            
            HttpResponseMessage response = await client.GetAsync("flats/allы");
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
            string flat_id = "64a2b29b6d333eeabd9c0f9cы";
            string client_id = "64a040c6601e4d5f061adcd6";
            string date = "2023-07-11";
            int days = 2;

            TestAPI result = new TestAPI("Reservations.Add()");
            result.Description = "Функция добавления брони на квартиру";
            HttpClient client = new HttpClient();
            client.BaseAddress = api_adress;


            HttpResponseMessage response = await client.PostAsync("booking/add/" +
                flat_id + ":" +
                client_id + "/" +
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
            string reservation_id = "64a703a40b0b602d78971897";
            TestAPI result = new TestAPI("Reservations.Cancel()");
            result.Description = "Функция отмены брони на квартиру";
            HttpClient client = new HttpClient();
            client.BaseAddress = api_adress;


            HttpResponseMessage response = await client.PatchAsync("booking/cancel:" + reservation_id, null);
            result.response = response;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                result.result = TestResult.Succes;
            else result.result = TestResult.Fail;


            return result;
        }
    }
}