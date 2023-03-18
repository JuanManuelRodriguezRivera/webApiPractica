using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiPractica.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class equiposController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;


        public equiposController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var listadoEquipos =(from e in _equiposContexto.equipos
                          join m in _equiposContexto.marcas on e.marcas_id equals m.id_marcas select new
                          {
                              e.id_equipos,
                              e.nombre,
                              e.descripcion,
                              e.tipo_equipo_id,
                              e.marcas_id,
                              m.nombre_marca
                          }).ToList();


            if (listadoEquipos.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoEquipos);
        }

        [HttpGet]
        [Route("Getbyid/{id}")]
        public IActionResult Get(int id)
        {
            var listEquipos = (from e in _equiposContexto.equipos
                           join m in _equiposContexto.marcas on e.marcas_id equals m.id_marcas
                           where e.id_equipos == id 
                           select new
                           {
                               e.id_equipos,
                               e.nombre,
                               e.descripcion,
                               e.tipo_equipo_id,
                               e.marcas_id,
                               m.nombre_marca
                           }).ToList();


            if (listEquipos.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listEquipos);
        }

        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult FindByDescription(string filtro) 
        {
            equipos? equipo = (from e in _equiposContexto.equipos where e.descripcion.Contains(filtro) select e).FirstOrDefault();
            if (equipo == null)
            {
                return  NotFound();
            }
            return Ok(equipo);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEquipo([FromBody]equipos equipo) 
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
            equipoActual.nombre=equipoModificar.nombre;
            equipoActual.descripcion = equipoModificar.descripcion;
            equipoActual.tipo_equipo_id = equipoModificar.tipo_equipo_id;
            equipoActual.anio_compra = equipoModificar.anio_compra;
            equipoActual.costo= equipoModificar.costo;
            equipoActual.marcas_id = equipoModificar.marcas_id;

            _equiposContexto.Entry(equipoActual).State= EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(equipoModificar);


        }
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarEquipo(int id)
        {
            equipos? equipo=(from e in _equiposContexto.equipos where e.id_equipos == id select e).FirstOrDefault();
            if(equipo ==null)
                return NotFound();

            _equiposContexto.equipos.Attach(equipo);   
            _equiposContexto.equipos.Remove(equipo);
            _equiposContexto.SaveChanges();
            return Ok(equipo);
        }
    }
}

