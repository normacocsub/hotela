using Microsoft.AspNetCore.Mvc;
using Logica;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Entity;
using Datos;
using HabitacionModel;

[Route("api/[controller]")]
[ApiController]
public class HabitacionController : ControllerBase
{
    private readonly HabitacionService _habitacionService;
    public IConfiguration Configuration { get; }
    public HabitacionController(HotelContext context)
    {
        _habitacionService = new HabitacionService(context);
    }
    // GET: api/Persona​
    [HttpGet]
    public ActionResult<HabitacionViewModel> Gets()
    {
        var response = _habitacionService.ConsultarTodos();
        if (response.Error)
        {
            return BadRequest(response.Mensaje);
        }
        else
        {
            return Ok(response.Habitaciones.Select(p => new HabitacionViewModel(p)));
        }
    }
    // GET: api/Persona/5​
    [HttpGet("{idhabitacion}")]
    public ActionResult<HabitacionViewModel> Get(string idhabitacion)
    {
        var habitacion = _habitacionService.BuscarxIdentificacion(idhabitacion);
        if (habitacion == null) return NotFound();
        var habitacionViewModel = new HabitacionViewModel(habitacion);
        return habitacionViewModel;
    }

    // POST: api/Persona​

    [HttpPost]
    public ActionResult<HabitacionViewModel> Post(HabitacionInputModel habitacionInput)
    {
        Habitacion habitacion = MapearHabitacion(habitacionInput);
        var response = _habitacionService.Guardar(habitacion);
        if (response.Error)
        {
            return BadRequest(response.Mensaje);
        }
        return Ok(response.Habitacion);
    }

    // DELETE: api/Persona/5​

    [HttpDelete("{idhabitacion}")]
    public ActionResult<string> Delete(string idhabitacion)
    {
        string mensaje = _habitacionService.Eliminar(idhabitacion);
        return Ok(mensaje);
    }

    private Habitacion MapearHabitacion(HabitacionInputModel habitacionInput)
    {
        var habitacion = new Habitacion
        {
            IdHabitacion = habitacionInput.IdHabitacion,
            Tipo = habitacionInput.Tipo,
            nPersonas = habitacionInput.nPersonas,
            Estado = habitacionInput.Estado,
            Precio = habitacionInput.Precio,
        };
        return habitacion;
    }
}