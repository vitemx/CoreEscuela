using System;

namespace CoreEscuela.Entidades
{
    public class Evaluación
    {
        public string UniqueId { get; set; }
        public Alumno Alumno { get; set; }
        public Asignatura Asignatura { get; set; }
        public float Nota { get; set; }
        public string Nombre { get; set; }
        public Evaluación() => UniqueId = Guid.NewGuid().ToString();

  }
}