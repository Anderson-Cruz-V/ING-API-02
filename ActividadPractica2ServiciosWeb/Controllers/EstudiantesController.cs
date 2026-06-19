using ActividadPractica2ServiciosWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace ActividadPractica2ServiciosWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiantesController : ControllerBase
    {
        private static List<Estudiante> estudiantes = new List<Estudiante>
        {
            new Estudiante
            {
                Id = 1,
                Nombre = "Anderson",
                Apellido = "Cruz Villalona",
                Correo = "anderson.cruz@ufhec.edu.do",
                Carrera = "Ingeniería de Software",
                Edad = 26,
                Promedio = 88.5m,
                Activo = true
            },

            new Estudiante
            {
                Id = 2,
                Nombre = "Ada Victoria",
                Apellido = "Fuentealba Avendaño",
                Correo = "ada.fuentealba@ufhec.edu.do",
                Carrera = "Educación Diferencial",
                Edad = 39,
                Promedio = 94.0m,
                Activo = true
            },

            new Estudiante
            {
                Id = 3,
                Nombre = "Alexa",
                Apellido = "Mercedes",
                Correo = "alexa.mercedes@ufhec.edu.do",
                Carrera = "Ingeniería de Software",
                Edad = 22,
                Promedio = 86.7m,
                Activo = true
            },

            new Estudiante
            {
                Id = 4,
                Nombre = "Jane",
                Apellido = "Rodríguez",
                Correo = "jane.rodriguez@ufhec.edu.do",
                Carrera = "Ingeniería de Software",
                Edad = 21,
                Promedio = 78.5m,
                Activo = false
            },

              new Estudiante
            {
                Id = 5,
                Nombre = "Luis Javier",
                Apellido = "Carl",
                Correo = "luis.carl@ufhec.edu.do",
                Carrera = "Ingeniería de Software",
                Edad = 35,
                Promedio = 96.2m,
                Activo = true
            }
        };

        [HttpGet]
        public IActionResult ObtenerEstudiantes()
        {
            return Ok(estudiantes);


        }
        [HttpGet("{id}")]
        public IActionResult ObtenerEstudiantePorId(int id)
        {
            Estudiante? estudiante = estudiantes.FirstOrDefault(e => e.Id == id);

            if (estudiante == null)
            {
                return NotFound();
            }

            return Ok(estudiante);
        }
        [HttpPost]
        public IActionResult CrearEstudiante([FromBody] Estudiante estudiante)
        {
            estudiante.Id = estudiantes.Max(e => e.Id) + 1;

            estudiantes.Add(estudiante);

            return CreatedAtAction(
                nameof(ObtenerEstudiantePorId),
                new { id = estudiante.Id },
                estudiante
            );
        }
        [HttpPut("{id}")]
        public IActionResult ActualizarEstudiante(int id, [FromBody] Estudiante estudianteActualizado)
        {
            Estudiante? estudiante = estudiantes.FirstOrDefault(e => e.Id == id);

            if (estudiante == null)
            {
                return NotFound();
            }

            estudiante.Nombre = estudianteActualizado.Nombre;
            estudiante.Apellido = estudianteActualizado.Apellido;
            estudiante.Correo = estudianteActualizado.Correo;
            estudiante.Carrera = estudianteActualizado.Carrera;
            estudiante.Edad = estudianteActualizado.Edad;
            estudiante.Promedio = estudianteActualizado.Promedio;
            estudiante.Activo = estudianteActualizado.Activo;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarEstudiante(int id)
        {
            Estudiante? estudiante = estudiantes.FirstOrDefault(e => e.Id == id);

            if (estudiante == null)
            {
                return NotFound();
            }

            estudiantes.Remove(estudiante);

            return NoContent();
        }
        [HttpGet("buscar")]
        public IActionResult BuscarEstudiantes([FromQuery] string texto)
        {
            List<Estudiante> resultados = estudiantes
                .Where(e =>
                    e.Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                    e.Apellido.Contains(texto, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(resultados);
        }
        [HttpGet("carrera")]
        public IActionResult FiltrarPorCarrera([FromQuery] string carrera)
        {
            List<Estudiante> resultados = estudiantes
                .Where(e => e.Carrera.Equals(carrera, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(resultados);
        }
        [HttpGet("aprobados")]
        public IActionResult ObtenerEstudiantesAprobados()
        {
            List<Estudiante> aprobados = estudiantes
                .Where(e => e.Promedio >= 70)
                .ToList();

            return Ok(aprobados);
        }
        [HttpGet("ordenar")]
        public IActionResult OrdenarEstudiantes([FromQuery] string criterio)
        {
            List<Estudiante> resultados;

            if (criterio.ToLower() == "nombre")
            {
                resultados = estudiantes.OrderBy(e => e.Nombre).ToList();
            }
            else if (criterio.ToLower() == "promedio")
            {
                resultados = estudiantes.OrderByDescending(e => e.Promedio).ToList();
            }
            else if (criterio.ToLower() == "edad")
            {
                resultados = estudiantes.OrderBy(e => e.Edad).ToList();
            }
            else
            {
                return BadRequest("El criterio debe ser nombre, promedio o edad.");
            }

            return Ok(resultados);
        }
        [HttpGet("rango")]
        public IActionResult FiltrarPorRango(
    [FromQuery] decimal promedioDesde,
    [FromQuery] decimal promedioHasta)
        {
            List<Estudiante> resultados = estudiantes
                .Where(e => e.Promedio >= promedioDesde &&
                            e.Promedio <= promedioHasta)
                .ToList();

            return Ok(resultados);
        }
        [HttpGet("estadisticas")]
        public IActionResult ObtenerEstadisticas()
        {
            int cantidadTotal = estudiantes.Count;
            int cantidadAprobados = estudiantes.Count(e => e.Promedio >= 70);
            int cantidadReprobados = estudiantes.Count(e => e.Promedio < 70);
            decimal promedioGeneral = estudiantes.Average(e => e.Promedio);
            decimal mejorPromedio = estudiantes.Max(e => e.Promedio);
            decimal peorPromedio = estudiantes.Min(e => e.Promedio);

            var estadisticas = new
            {
                cantidadTotal,
                cantidadAprobados,
                cantidadReprobados,
                promedioGeneral,
                mejorPromedio,
                peorPromedio
            };

            return Ok(estadisticas);
        }
        [HttpPut("{id}/estado")]
        public IActionResult CambiarEstadoEstudiante(
    int id,
    [FromQuery] bool activo)
        {
            Estudiante? estudiante = estudiantes.FirstOrDefault(e => e.Id == id);

            if (estudiante == null)
            {
                return NotFound();
            }

            estudiante.Activo = activo;

            return NoContent();

        }

        [HttpGet("activos")]
        public IActionResult ObtenerEstudiantesActivos()
        {
            List<Estudiante> activos = estudiantes
                .Where(e => e.Activo == true)
                .ToList();

            return Ok(activos);
        }

    }
}