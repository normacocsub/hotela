using System.Collections.Generic;
using System.Linq;
using Datos;
using Entity;
using Logica;
using ReservaModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

[Route("api/[controller]")]
[ApiController]
public class ReservaController : ControllerBase
{
    private readonly ReservaService _reservaService;

    public IConfiguration Configuration { get; }

    public ReservaController(HotelContext context)
    {
        _reservaService = new ReservaService(context);
    }

    // GET: api/Persona​
    [HttpGet]
    public ActionResult<ReservaViewModel> Gets()
    {
        var response = _reservaService.ConsultarTodos();
        if (response.Error)
        {
            ModelState
            .AddModelError("Error al consultar la Reserva",
            response.Mensaje);
            var detallesproblemas = new ValidationProblemDetails(ModelState);
            detallesproblemas.Status = StatusCodes.Status500InternalServerError;
            return BadRequest(detallesproblemas);
        }
        else
        {
            return Ok(response.Reservas.Select(p => new ReservaViewModel(p)));
        }
    }

    // GET: api/Persona/5​
    [HttpGet("{idreserva}")]
    public ActionResult<ReservaViewModel> Get(string idreserva)
    {
        var reserva = _reservaService.BuscarxIdentificacion(idreserva);
        if (reserva == null) return NotFound();
        var reservaViewModel = new ReservaViewModel(reserva);
        return reservaViewModel;
    }

    // POST: api/Persona​
    [HttpPost]
    public ActionResult<ReservaViewModel> Post(ReservaInputModel reservaInput)
    {
        Reserva reserva = MapearReserva(reservaInput);
        var response = _reservaService.Guardar(reserva);
        if (response.Error)
        {
            ModelState
                .AddModelError("Error al guardar la Reserva", response.Mensaje);
            var detallesproblemas = new ValidationProblemDetails(ModelState);
            detallesproblemas.Status = StatusCodes.Status500InternalServerError;
            return BadRequest(detallesproblemas);
        }
        return Ok(response.Reserva);
    }

    // DELETE: api/Persona/5​
    [HttpDelete("{idreserva}")]
    public ActionResult<string> Delete(string idreserva)
    {
        string mensaje = _reservaService.Eliminar(idreserva);
        return Ok(mensaje);
    }

    private Reserva MapearReserva(ReservaInputModel reservaInput)
    {
        var reserva =
            new Reserva {
                IdReserva = reservaInput.IdReserva,
                IdHabitacion = reservaInput.IdHabitacion,
                FechaReserva = reservaInput.FechaReserva,
                Cedula = reservaInput.Cedula,
                Iva = reservaInput.Iva,
                Total = reservaInput.Total,
                FechaEntrada = reservaInput.FechaEntrada,
                FechaSalida = reservaInput.FechaSalida,
                Dias = reservaInput.Dias,
            };
        return reserva;
    }
}
