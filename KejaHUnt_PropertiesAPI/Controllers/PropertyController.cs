using AutoMapper;
using Azure.Core;
using KejaHUnt_PropertiesAPI.Models.Domain;
using KejaHUnt_PropertiesAPI.Models.Dto;
using KejaHUnt_PropertiesAPI.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KejaHUnt_PropertiesAPI.Controllers
{
    [Route("api/property")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public PropertyController(IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }

        // POST: {apibaseurl}/api/property
        [HttpPost]
        public async Task<IActionResult> CreateProperty([FromBody] CreatePropertyRequestDto request)
        {
            // Map DTO to domain model
            var property = _mapper.Map<Property>(request);

            await _propertyRepository.CreatePropertyAsync(property);


            return Ok(_mapper.Map<PropertyDto>(property));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var properties = await _propertyRepository.GetAllAsync();

            return Ok(_mapper.Map<List<PropertyDto>>(properties));

        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetPropertyByIdAsync([FromRoute] int id)
        {
            var property = await _propertyRepository.GetPropertyByIdAsync(id);

            if (property == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PropertyDto>(property));
        }

        // PUT: {apibaseurl}/api/property/{id}
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdatePropertyById([FromRoute] int id, UpdatePropertyRequestDto request)
        {
            

            var updatedProperty = await _propertyRepository.UpdateAsync(id, request);

            if (updatedProperty == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PropertyDto>(updatedProperty));

        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteDiaryEntry([FromRoute] int id)
        {
            var deletedProperty = await _propertyRepository.DeleteAync(id);

            if (deletedProperty == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PropertyDto>(deletedProperty));

        }
    
}
    }
