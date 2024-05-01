using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace dominio
{
    public class Disco
    {
        public int Id { get; set; }
        //Annotations --> [DisplayName("ejemplo")]
        [DisplayName("Título del Album")]
        public string Titulo { get; set; }
        [DisplayName("Cantidad de canciones")]
        public int CantidadCanciones { get; set; }
        [DisplayName("Fecha de lanzamiento")]
        public DateTime FechaLanzamiento { get; set; }
        public string UrlImagenTapa { get; set; }
        [DisplayName("Género")]
        public Estilo Estilo { get; set; }
        [DisplayName("Formato")]
        public Edicion Edicion { get; set; }
    }
}
