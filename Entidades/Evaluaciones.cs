using System;

namespace CoreEscuela.Entidades
{
    public class Evaluaciones
    {
        public Alumno Alumno { get; set; }
        public Asignatura Asignatura { get; set; }
        public float Nota { get; set; }

        public string Nombre { get; set; }

    public override string ToString() => $"{Nota}, {Alumno.Nombre}, {Asignatura.Nombre}";
  }
}