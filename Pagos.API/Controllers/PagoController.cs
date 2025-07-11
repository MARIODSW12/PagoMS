using System.Text.Json;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MPago.Application.DTOs;
using Pagos.Application.Commands;
using Pagos.Application.DTOs;
using Pagos.Domain.Aggregate;
using Pagos.Domain.ValueObjects;
using Pagos.Infrastructure.Interfaces;
using Pagos.Infrastructure.Queries;
using Pagos.Infrastructure.Queries.QueryHandler;
using Pagos.Infrastructure.Services;
using RestSharp;

namespace Pagos.API.Controllers
{
    [ApiController]
    [Route("api/pagos")]
    public class PagoController: ControllerBase
    {
        private readonly IMediator Mediator;
        private readonly IPublishEndpoint PublishEndpoint;
        private readonly IRestClient RestClient;

        public PagoController(IMediator mediator, IPublishEndpoint publishEndpoint, IRestClient restClient)
        {
            Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            PublishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            RestClient = restClient ?? throw new ArgumentNullException(nameof(restClient));
        }

        #region GetPagoPorId
        [HttpGet("GetPagoPorId")]
        public async Task<IActionResult> getPagoPorId([FromQuery] string idPago)
        {
            try
            {
                var Pago = await Mediator.Send(new GetPagoPorIdQuery(idPago));

                if (Pago == null)
                {
                    return NotFound($"No se encontró un pago con el id {idPago}");
                }

                return Ok(Pago);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno en el servidor.");
            }
        }
        #endregion

        #region GetPagoPorIdSubasta
        [HttpGet("GetPagoPorIdSubasta")]
        public async Task<IActionResult> GetPagoPorIdSubasta([FromQuery] string idSubasta)
        {
            try
            {
                var Pagos = await Mediator.Send(new GetPagoPorIdSubastaQuery(idSubasta));

                if (Pagos == null)
                {
                    return NotFound($"No se encontró un pago con el id {idSubasta}");
                }

                return Ok(Pagos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno en el servidor.");
            }
        }
        #endregion
        
        #region AgregarPago
        [HttpPost("AgregarPago")]
        public async Task<IActionResult> AgregarPago([FromBody] AgregarPagoDto pago)
        {

            try
            {
                var request = new RestRequest(Environment.GetEnvironmentVariable("MPAGO_MS_URL") + $"/GetMPagoPorId", Method.Get);
                request.AddParameter("idMPago", pago.IdMPago);
                var response = await RestClient.ExecuteAsync(request);
                if (!response.IsSuccessful)
                {
                    return StatusCode(500, "Error al obtener el metodo de pago");
                }
                var mPago = JsonDocument.Parse(response.Content).Deserialize<MPagoDto>();
                var IdPago = await Mediator.Send(new AgregarPagoCommand(pago, mPago.idClienteStripe, mPago.idMPagoStripe));

                if (IdPago == null)
                {
                    return BadRequest("No se pudo crear el pago.");
                }

                var requestAuction = new RestRequest(Environment.GetEnvironmentVariable("AUCTION_MS_URL") + $"/completeAuction/{pago.IdAuction}", Method.Post);
                var responseAuction = await RestClient.ExecuteAsync(requestAuction);
                if (!responseAuction.IsSuccessful)
                {
                    return StatusCode(500, "Error al completar la subasta");
                }


                return CreatedAtAction(nameof(AgregarPago), new { id = IdPago }, new
                {
                    id = IdPago
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Error interno en el servidor. AGREGAR PAGO");
            }
        }
        #endregion

        #region GetPagosUsuario
        [HttpGet("GetPagosUsuario")]
        public async Task<IActionResult> GetPagosUsuario([FromQuery] string idUsuario)
        {
            try
            {
                var pagos = await Mediator.Send(new GetPagosUsuarioQuery(idUsuario));

                return Ok(pagos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno en el servidor.");
            }
        }
        #endregion

        #region GetPagosSubastador
        [HttpGet("GetPagosSubastador/{idSubastador}")]
        public async Task<IActionResult> GetPagosSubastador([FromRoute] string idSubastador, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            try
            {
                Console.WriteLine($"Obteniendo pagos del subastador: {idSubastador} desde {from} hasta {to}");
                var request = new RestRequest(Environment.GetEnvironmentVariable("AUCTION_MS_URL") + $"/getUserAuctions/" + idSubastador, Method.Get);
                var response = await RestClient.ExecuteAsync(request);
                if (!response.IsSuccessful)
                {
                    return StatusCode(500, "Error al obtener las subastas");
                }
                Console.WriteLine(response.Content);
                var subastas = JsonDocument.Parse(response.Content).Deserialize<List<GetAuctionDto>>();
                subastas = subastas.Where(s => s.startDate >= from && s.startDate <= to).ToList();
                var pagos = new List<ObtenerPagoSubastadorDto>();
                foreach (var subasta in subastas)
                {
                    var pago = await Mediator.Send(new GetPagoPorIdSubastaQuery(subasta.id));
                    if (pago != null)
                    {
                        var requestMPago = new RestRequest(Environment.GetEnvironmentVariable("MPAGO_MS_URL") + $"/GetMPagoPorId", Method.Get);
                        requestMPago.AddParameter("idMPago", pago.IdMPago);
                        var responseMPago = await RestClient.ExecuteAsync(requestMPago);
                        if (!responseMPago.IsSuccessful)
                        {
                            return StatusCode(500, "Error al obtener el metodo de pago");
                        }
                        var mPago = JsonDocument.Parse(responseMPago.Content).Deserialize<MPagoDto>();
                        var APIRequest = new RestRequest(Environment.GetEnvironmentVariable("USER_MS_URL") + "/getuserbyid/" + subasta.userId, Method.Get);
                        var APIResponse = await RestClient.ExecuteAsync(APIRequest);
                        if (!APIResponse.IsSuccessful)
                        {
                            throw new Exception("Error al obtener la información del usuario.");
                        }
                        var user = JsonDocument.Parse(APIResponse.Content);

                        var compradorRequest = new RestRequest(Environment.GetEnvironmentVariable("USER_MS_URL") + "/getuserbyid/" + pago.IdUsuario, Method.Get);
                        var compradorResponse = await RestClient.ExecuteAsync(compradorRequest);
                        if (!compradorResponse.IsSuccessful)
                        {
                            throw new Exception("Error al obtener la información del comprador.");
                        }
                        var comprador = JsonDocument.Parse(compradorResponse.Content);
                        pagos.Add(new ObtenerPagoSubastadorDto
                        {
                            IdPago = pago.IdPago,
                            MPago = mPago.marca + " - " + mPago.ultimos4,
                            Usuario = user.RootElement.GetProperty("name").GetString() + " " + user.RootElement.GetProperty("lastName"),
                            Comprador = comprador.RootElement.GetProperty("name").GetString() + " " + comprador.RootElement.GetProperty("lastName"),
                            Subasta = subasta.name,
                            FechaPago = pago.FechaPago,
                            Monto = pago.Monto
                        });
                    }
                }

                return Ok(pagos);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Error interno en el servidor.");
            }
        }
        #endregion
    }
}
