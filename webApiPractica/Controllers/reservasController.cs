using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiPractica.Models;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class reservasController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;


        public reservasController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var listadoEquipo = (from e in _equiposContexto.equipos
                                 join m in _equiposContexto.marcas on e.marcas_id equals m.id_marcas
                                 select new
                                 {
                                     e.id_equipos,
                                     e.nombre,
                                     e.descripcion,
                                     e.tipo_equipo_id,
                                     e.marcas_id,
                                     m.nombre_marca
                                 }).ToList();

            if (listadoEquipo.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoEquipo);
        }

        [HttpGet]
        [Route("Getbyid/{id}")]
        public IActionResult Get(int id)
        {
            var equipo = (from e in _equiposContexto.equipos
                          join m in _equiposContexto.marcas on e.marcas_id equals m.id_marcas
                          join te in _equiposContexto.tipo_equipo on e.tipo_equipo_id equals te.id_tipo_equipo
                          where e.id_equipos == id
                          select new
                          {
                              e.id_equipos,
                              e.nombre,
                              e.descripcion,
                              e.tipo_equipo_id,
                              tipo_descripcion = te.descripcion,
                              e.marcas_id,
                              m.nombre_marca
                          }
                          ).FirstOrDefault();
            if (equipo == null) return NotFound();
            return Ok(equipo);
        }

        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult FindByDescription(string filtro)
        {
            equipos? equipo = (from e in _equiposContexto.equipos where e.descripcion.Contains(filtro) select e).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEquipo([FromBody] equipos equipo)
        {
            try
            {
                _equiposContexto.equipos.Add(equipo);
                _equiposContexto.SaveChanges();
                return Ok(equipo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] equipos equipoModificar)
        {
            equipos? equipoActual = (from e in _equiposContexto.equipos where e.id_equipos == id select e).FirstOrDefault();
            if (equipoActual == null)
            {
                return NotFound();
            }
            equipoActual.nombre = equipoModificar.nombre;
            equipoActual.descripcion = equipoModificar.descripcion;
            equipoActual.tipo_equipo_id = equipoModificar.tipo_equipo_id;
            equipoActual.anio_compra = equipoModificar.anio_compra;
            equipoActual.costo = equipoModificar.costo;
            equipoActual.marcas_id = equipoModificar.marcas_id;

            _equiposContexto.Entry(equipoActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(equipoModificar);


        }
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarEquipo(int id)
        {
            equipos? equipo = (from e in _equiposContexto.equipos where e.id_equipos == id select e).FirstOrDefault();
            if (equipo == null)
                return NotFound();

            _equiposContexto.equipos.Attach(equipo);
            _equiposContexto.equipos.Remove(equipo);
            _equiposContexto.SaveChanges();
            return Ok(equipo);
        }
    }
}
