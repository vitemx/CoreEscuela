using System;
using System.Linq;
using System.Collections.Generic;
using CoreEscuela.Entidades;

namespace CoreEscuela.App
{
    public class Reporteador
    {
        Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> _dicccionario;
        public Reporteador(Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> dicObsEsc)
        {
            if (dicObsEsc == null)
                throw new ArgumentNullException(nameof(dicObsEsc));
            _dicccionario = dicObsEsc;
        }

        public IEnumerable<Evaluación> GetListaEvaluaciones()
        {
           if (_dicccionario.TryGetValue(LlaveDiccionario.Evaluación, out IEnumerable<ObjetoEscuelaBase> lista))
           {
                return lista.Cast<Evaluación>();
           }
           {
               return new List<Evaluación>();
           }
        }

        public IEnumerable<string> GetListaAsignaturas()
        {
            return GetListaAsignaturas(out var dummy );
        }
        public IEnumerable<string> GetListaAsignaturas(out IEnumerable<Evaluación>listaEvaluaciones)
        {
            listaEvaluaciones = GetListaEvaluaciones();
            
            return (from Evaluación ev in listaEvaluaciones
                select ev.Asignatura.Nombre).Distinct();
        }

        public Dictionary<string, IEnumerable<Evaluación>> GetDicEvaluaXAsig()
        {
            var dictaRta = new Dictionary<string, IEnumerable<Evaluación>>();

            var listaAsig = GetListaAsignaturas(out var listaEval);

            foreach (var asig in listaAsig)
            {
                var evalsAsig = from eval in listaEval
                                where eval.Asignatura.Nombre == asig
                                select eval;

                dictaRta.Add(asig, evalsAsig);
            }
            return dictaRta;
        }

        public Dictionary<string, IEnumerable<object>> GetPromeAlumnPorAsignatura()
        {
            var rta = new Dictionary<string, IEnumerable<object>>();
            var dicEvaluaXAsig = GetDicEvaluaXAsig();

            foreach (var asigConEval in dicEvaluaXAsig)
            {
                var promsAlumn = from eval in asigConEval.Value
                    group eval by new
                    {
                        eval.Alumno.UniqueId,
                        eval.Alumno.Nombre
                    } 
                    into grupoEvalAlumno
                    select new AlumnoPromedio
                    { 
                        alumnoId = grupoEvalAlumno.Key.UniqueId,
                        alumnoNombre = grupoEvalAlumno.Key.Nombre,
                        promedio = grupoEvalAlumno.Average( evaluacion => evaluacion.Nota)
                    }; 
                    rta.Add(asigConEval.Key, promsAlumn);
            }
            return rta;
        }

        public List<object> TopFive(string materia, int rango)
        {
            var promedios = GetPromeAlumnPorAsignatura();
            var top = promedios.GetValueOrDefault(materia).OrderByDescending(prom => ((AlumnoPromedio)prom).promedio
            ).Take(rango);

            return top.ToList();
        }
    }
}