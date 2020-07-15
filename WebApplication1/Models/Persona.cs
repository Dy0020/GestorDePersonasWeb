using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Persona
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [Required(ErrorMessage = "La identificación es requerida")]
        [JsonProperty(PropertyName = "Identificación")]
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [JsonProperty(PropertyName = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es requerido")]
        [JsonProperty(PropertyName = "Primer Apellido")]
        public string PrimerApellido { get; set; }

        [Required(ErrorMessage = "El segundo apellido es requerido")]
        [JsonProperty(PropertyName = "Segundo Apellido")]
        public string SegundoApellido { get; set; }

        [JsonProperty(PropertyName = "Foto")]
        public string Foto { get; set; }



    }
}
