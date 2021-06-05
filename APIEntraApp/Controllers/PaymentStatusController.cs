using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIEntraApp.Data;
using APIEntraApp.Services.PaymentStatuses.Core;
using APIEntraApp.Services.PaymentStatuses.Models.Request;

namespace APIEntraApp.Controllers
{
    [Authorize(Roles = "SuperUser,Admin,Shop")]
    [ApiController]
    [Route("/PaymentStatus")]
    public class PaymentStatusController : ControllerBase
    {
        private readonly IPaymentStatusService _paymentStatusService;
        private readonly ApiDbContext _apiDbContext;

        public PaymentStatusController(
            ApiDbContext apiDbContext,
            IPaymentStatusService paymentStatusService)
        {
            _apiDbContext = apiDbContext;
            _paymentStatusService = paymentStatusService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición inválida");
                }

                return Ok(await _paymentStatusService.GetAllAsync(_apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición inválida");
                }

                return Ok(await _paymentStatusService.GetByIdAsync(id, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaymentStatusPostRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de alta inválida");
                }

                return Ok(await _paymentStatusService.CreateAsync(model, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(PaymentStatusPutRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de actualización inválida");
                }

                return Ok(await _paymentStatusService.UpdateAsync(model, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Petición de eliminar inválida");
                }

                return Ok(await _paymentStatusService.DeleteAsync(id, _apiDbContext));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
