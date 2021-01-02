using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary;
using Microsoft.EntityFrameworkCore;

namespace PlanteShopRESTService
{
    //til inMemory skal der hentes nugetpacket: Microsoft.EntityFrameworkCore.InMemory og Microsoft.EntityFrameworkCore.SqlServer
    //den skal tilføjes i startup som en dependency injection

    public class PlanteContext : DbContext
    {
        // her laver vi en context til vores inMemory "database" så man kan få fat på dataen
        public PlanteContext(DbContextOptions<PlanteContext> options) : base(options)
        {

        }
        //her laver vi et entityframework set - vi får adgang til vores liste af planter (ListOfPlants fra startup.cs)
        public DbSet<Plante> Planter { get; set; }
    }
}
