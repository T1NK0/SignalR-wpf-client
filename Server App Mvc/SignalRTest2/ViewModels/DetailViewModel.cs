using SignalRTest2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRTest2.ViewModels
{
    public class DetailViewModel
    {
        public DetailViewModel()
        {
            Planets = new List<Planet>
            {
                new Planet { Id = 1, Name = "Sun"},
                new Planet { Id = 2, Name = "Earth"}
            };
        }
        public List<Planet> Planets { get; set; }
    }
}
