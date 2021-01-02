using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlanteShopRESTService.Controllers
{


    //HUSK til SWAGGER hent nugetpacket : Swashbuckle.AspNetCore og tilføj i startup.cs
    //HUSK til CORS hent nugetpacket : Microsoft.AspNetCore.Cors og tilføj i startup.cs  Se PDF i mappe for test i Postman

    //HUSK til AZURE(RESTservice) se mappe for upload og publish    denne RESTservice ligger på: http://studymock1restservice.azurewebsites.net/planter

    [Route("Planter")]
    [ApiController]
    public class PlanteController : ControllerBase
    {
        //mock data man kan teste på
        private static readonly List<Plante> PlanteListe = new List<Plante>()
        {
            new Plante(1, "Rose", "Albertine", 400, 199),
            new Plante(2, "Busk", "Aronia", 200, 169),
            new Plante(3, "FrugtOgBær", "AromaÆble", 350, 399),
            new Plante(4, "Rhododendron", "Astrid", 40, 269),
            new Plante(5, "Rose", "The dark lady", 100, 199)
        };

        private static int nextId = 6;

        // GET: Planter
        [HttpGet]
        public IEnumerable<Plante> GetAllPlanter()
        {
            return PlanteListe;
        }

        // GET Planter/5
        [HttpGet]
        [Route("{id}")]
        public Plante GetPlanteById(int Id)
        {
            return PlanteListe.Find(i => i.PlanteId == Id);

        }

        //HUSK Så længe der ikke er http + route vil den ikke du i swagger

        // GET Planter/planteType/rose
        [HttpGet]
        [Route("PlanteType/{type}")]
        public IEnumerable<Plante> GetPlanterByType(string type)
        {
            return PlanteListe.FindAll(i => i.PlanteType.ToLower().Contains(type));
        }


        // POST Planter
        [HttpPost]
        public int AddPlante(Plante plante)
        {
            Plante p = new Plante(nextId++, plante.PlanteType, plante.PlanteNavn, plante.Pris, plante.MaksHoejde);
            PlanteListe.Add(p);
            return p.PlanteId;
        }


        //----------------------IKKE En del af opgaven------------------


        // PUT Planter/5
        [HttpPut]
        [Route("{id}")]
        public void Put(int id, [FromBody] Plante value)
        {
            Plante plante = GetPlanteById(id);
            if (plante != null)
            {
                plante.PlanteId = value.PlanteId;
                plante.PlanteType = value.PlanteType;
                plante.PlanteNavn = value.PlanteNavn;
                plante.MaksHoejde = value.MaksHoejde;
                plante.Pris = value.Pris;
            }
        }

        // DELETE Planter/5
        [HttpDelete]
        [Route("{id}")]
        public void Delete(int id)
        {
            Plante plante = GetPlanteById(id);
            PlanteListe.Remove(plante);
        }


        //her søges der på alle planter der har {f.eks rose} i sit navn
        [HttpGet]
        [Route("PlanteNavn/{substring}")]
        public IEnumerable<Plante> GetFromSubstring(String substring)
        {
            return PlanteListe.FindAll(i => i.PlanteNavn.ToLower().Contains(substring));
        }
        //http://localhost:53852/planter/plantenavn/albertine her søges der på planter der hedder albertine


        //her er søge kriterier der skal være et interval
        //– Note the annotation [FromQuery] meaning it takes the information from the URI /URL as shown above.
        [HttpGet]
        [Route("Pris/")]
        public IEnumerable<Plante> GetWithFilter([FromQuery] PlantePriser prisfilter)
        {
            return PlanteListe.FindAll(i => (i.Pris < prisfilter.HoejPris) && (i.Pris > prisfilter.LavPris));
        }

        //http://localhost:53852/planter/pris/?LavPris=30&&HoejPris=250 her søges der på planter i listen der ligger indefor  kriterierne 30 og 250 

    }
}

