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
            List<equipos> listadoEquipo = (from e in _equiposContexto.equipos select e).ToList();

            if (listadoEquipo.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoEquipo);
    } 



    }
}

