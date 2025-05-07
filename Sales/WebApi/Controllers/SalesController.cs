using Microsoft.AspNetCore.Mvc;
using Sales.Core.Application.DTOs;
using Sales.Core.Domain.Entities;
using Sales.Core.Domain.Interfaces;

namespace Sales.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SalesController(ISaleService saleService) => _saleService = saleService;

        [HttpPost]
        public async Task<ActionResult<Sale>> CreateSale( [FromBody] CreateSaleDto createSaleDto )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
 
            var sale = await _saleService.CreateSale(createSaleDto);
            return CreatedAtAction(nameof(GetSaleById), sale);            
            
        }

        [HttpPost]
        public async Task<ActionResult<Sale>> CancelSale(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sale = await _saleService.CancelSale(id);
            return CreatedAtAction(nameof(GetSaleById), sale);

        }

        [HttpPost]
        public async Task<ActionResult<Sale>> CancelSaleItem(int saleId,int saleItemId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sale = await _saleService.CancelSaleItem(saleId,saleItemId);
            return CreatedAtAction(nameof(GetSaleById), sale);

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sale>>> GetAllSales()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sales = await _saleService.GetAllSales();
            return Ok(sales);         
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Sale>> GetSaleById(int saleId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sale = await _saleService.GetSaleById(saleId);

            if (sale == null)
            {
                return NotFound();
            }

            return Ok(sale);
          
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSale(int saleId, [FromBody] UpdateSaleDto updateSaleDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resp = await _saleService.UpdateSale(saleId, updateSaleDto);

            if (!resp)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{saleId}")]
        public async Task<IActionResult> DeleteSale(int saleId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resp = await _saleService.DeleteSale(saleId);

            if (!resp)
            {
                return NotFound();
            }

            return NoContent();


        }
    }

   
}