using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary;
using Newtonsoft.Json;

namespace Consumer
{

    //HUSK at Hente nugetpacket newtonsoft.json og WebApi.client
    //HUSK dependencies til ClassLibrary

    class Program
    {
        public static string URI = "http://localhost:53852/planter";

        static void Main(string[] args)
        {
            Console.WriteLine("Hent alle planter");
            var planteListe = GetAllPlanterAsync().Result;
            foreach (var p in planteListe)
            {
                Console.WriteLine(p);
            }

            Console.WriteLine();

            Console.WriteLine("Hent plante med Id: 3");
            Plante plante = GetOnePlanteAsync(3).Result;
            Console.WriteLine(plante);

            Console.WriteLine();

            //---------IKKE en del af opgaven

            Console.WriteLine("Tilføj en plante");
            string message = Console.ReadLine();
            PostPlantAsync(message);

            Console.WriteLine();
            Thread.Sleep(2000);

            Console.WriteLine("Vis planten er tilføjet listen");
            planteListe = GetAllPlanterAsync().Result;
            foreach (var p in planteListe)
            {
                Console.WriteLine(p);
            }

            Console.WriteLine();

            Console.WriteLine("Slet en plante");
            int id = Int32.Parse(Console.ReadLine());
            DeletePlanteAsync(id);

            Console.WriteLine();
            Thread.Sleep(2000);

            Console.WriteLine("vis planten er slettet fra listen");
            planteListe = GetAllPlanterAsync().Result;
            foreach (var p in planteListe)
            {
                Console.WriteLine(p);
            }
        }



        //her laver jeg et asyncron metode der indeholder en liste af planter
        public static async Task<IList<Plante>> GetAllPlanterAsync()
        {
            //her siger den hver gang jeg benytter metoden laver den en ny client (Task)
            using (HttpClient client = new HttpClient())
            {
                //her afventer jeg at clienten har hentet uri'en og initeliaizere variablen content med en json sting 
                string content = await client.GetStringAsync(URI);
                //her deserilizere jeg stringen af hver plante og sætter dem ind i en IList der hedder cList
                IList<Plante> cList = JsonConvert.DeserializeObject<IList<Plante>>(content);

                return cList;

            }

        }

        public static async Task<Plante> GetOnePlanteAsync(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                //henter string objekt på Uri/id og gemmer som string variable
                string content = await client.GetStringAsync($"{URI}/{id}");
                //conventere string variable til et Plante objekt 
                Plante plante = JsonConvert.DeserializeObject<Plante>(content);
                //retunere objekt 
                return plante;
            }
        }

        //----------------------------------IKKE en del af opgaven

        public static async void PostPlantAsync(string message)
        {
            using (HttpClient client = new HttpClient())
            {
                //vi skal have en Json string derfor laver vi stringen (vores message) til et objekt og vores post serializere objektet til en Json string
                Plante p = JsonConvert.DeserializeObject<Plante>(message);
                await client.PostAsJsonAsync(URI, p);

                //Det ville være dette man skulle skrive en consumer vinduet:

                //{"planteId":4,"planteType":"Rose","planteNavn":"Maren","pris":120,"maksHoejde":150}

            }

        }

        public static async void DeletePlanteAsync(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                await client.DeleteAsync($"{URI}/{id}");
            }
        }
    }
}
