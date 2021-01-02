using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    // dette er et Class Library .NET core Projekt

    public class Plante
    {
        public int PlanteId { get; set; }
        public string PlanteType { get; set; }
        public string PlanteNavn { get; set; }
        public int Pris { get; set; }
        public int MaksHoejde { get; set; }

        //needed for Intializing all the data fields
        public Plante(int planteId, string planteType, string planteNavn, int pris, int maksHoejde)
        {
            PlanteId = planteId;
            PlanteType = planteType;
            PlanteNavn = planteNavn;
            Pris = pris;
            MaksHoejde = maksHoejde;
        }

        public Plante(string planteType, string planteNavn, int pris, int maksHoejde)
        {
            PlanteType = planteType;
            PlanteNavn = planteNavn;
            Pris = pris;
            MaksHoejde = maksHoejde;
        }

        //needed for JSON transfer. Serializable objects
        public Plante()
        {

        }

        public override string ToString()
        {
            return $"PlanteId: {PlanteId}, PlanteType: {PlanteType}, PlanteNavn: {PlanteNavn}, Pris: {Pris}, MaksHoejde: {MaksHoejde}";
        }
    }
}
