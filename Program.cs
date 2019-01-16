using System;
using System.Collections.Generic;
using System.Linq;
using CoreEscuela.App;
using CoreEscuela.Entidades;
using CoreEscuela.Util;
using static System.Console;

namespace CoreEscuela
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += AccionDelEvento;
            AppDomain.CurrentDomain.ProcessExit += (o, s)=> Printer.Beep(2000, 1000, 1);
            
            var engine = new EscuelaEngine();
            engine.Inicializar();
            Printer.WriteTitle("BIENVENIDOS A LA ESCUELA");  
            
            var reporteador = new Reporteador(engine.GetDiccionarioObjetos());
            var evalList = reporteador.GetListaEvaluaciones();
            var listaAsg = reporteador.GetListaAsignaturas();
            var listaEvalXAsig = reporteador.GetDicEvaluaXAsig();
            var listaPromXAsig = reporteador.GetPromeAlumnPorAsignatura();
            var topFive = reporteador.TopFive("Matemáticas", 5 );

            Printer.WriteTitle("Captura una Evaluación por Consola");
            var newEval = new Evaluación();
            string nombre, notastring;
            float nota;
            
            WriteLine("Ingrese el nombre de la evauación");
            Printer.PresionEnter();
            nombre = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El valor del nombre no puede estar vacio");
                WriteLine("Saliendo del programa");
            }
            else
            {
                newEval.Nombre = nombre.ToLower();
                WriteLine("El nombre de la evaluación ha sido ingresado correctamente");
            }

            WriteLine("Ingrese la nota de la evaluación");
            Printer.PresionEnter();
            notastring = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(notastring))
            {
               Printer.WriteTitle("El valor de la nota no puede estar vacio");
               WriteLine("Saliendo del programa");
            }
            else
            {
                try
                {
                    newEval.Nota = float.Parse(notastring);
                    if (newEval.Nota < 0 || newEval.Nota > 5)
                    {
                        throw new ArgumentOutOfRangeException("La nota debe estar entre 0 y 5");
                    }
                    WriteLine("La nota de la evaluación ha sido ingresado correctamente");
                }
                catch(ArgumentOutOfRangeException arge)
                {
                    WriteLine(arge.Message);
                    WriteLine("Saliendo del programa");
                }
                catch(Exception)
                {
                    Printer.WriteTitle("El valor de la nota no es un numero válido");
                    WriteLine("Saliendo del programa");
                    throw;
                }
                finally
                {
                    Printer.WriteTitle("FINALLY");
                    Printer.Beep(2500, 500, 3);
                }
            }
        }

    private static void AccionDelEvento(object sender, EventArgs e)
    {
        Printer.WriteTitle("SALIENDO");
        Printer.Beep(3000, 1000, 3);
        Printer.WriteTitle("SALIO");
    }

    private static void ImpimirCursosEscuela(Escuela escuela) 
        {
            if (escuela?.Cursos != null)
            {
                foreach (var curso in escuela.Cursos)
                {
                WriteLine($"Nombre {curso.Nombre  }, Id  {curso.UniqueId}");
                }
            }
        }
    }
}