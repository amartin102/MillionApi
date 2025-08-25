using Application.Dto;
using Application.Interface;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace MillionApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertiesController : Controller
    {

        private readonly IPropertyService _propertyService;

        public PropertiesController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(string id, CancellationToken ct)
        {
            try
            {                
                if (!ObjectId.TryParse(id, out var objectId))
                {                   
                    return NotFound();
                }

                var result = await _propertyService.GetByIdAsync(id, ct);

                if (result != null)
                    return Ok(result);

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error interno del servidor. {(ex.Message == null ? ex.InnerException : ex.Message)}");
            }        
        }

        [HttpGet]
        [Route("GetAllByFilter")]
        public async Task<IActionResult> GetAllByFilter([FromQuery] PropertyFilterDto filter, CancellationToken ct)
        {
            var result = await _propertyService.GetPropertiesAsync(filter, ct);
            return Ok(result);
        }


        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] PropertyItemCreateDto dto, CancellationToken ct)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(400);
                }

                var result = await _propertyService.CreateAsync(dto, ct);

                 return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error interno del servidor. {(ex.Message == null ? ex.InnerException : ex.Message)}");
            }            
        }


    }
}
