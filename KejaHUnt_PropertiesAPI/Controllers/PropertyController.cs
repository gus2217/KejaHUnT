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

        public PropertyController(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        // POST: {apibaseurl}/api/property
        [HttpPost]
        public async Task<IActionResult> CreateProperty([FromBody] CreatePropertyRequestDto request)
        {
            // Map DTO to domain model
            var property = new Property
            {
                Name = request.Name,
                Location = request.Location,
                Type = request.Type,
                Units = request.Units.Select(unitDto => new Unit
                {
                    Price = unitDto.Price,
                    Type = unitDto.Type,
                    Bathrooms = unitDto.Bathrooms,
                    Size = unitDto.Size,
                    NoOfUnits = unitDto.NoOfUnits
                }).ToList()
            };

            await _propertyRepository.CreatePropertyAsync(property);

            var response = new PropertyDto
            {
                Id = property.Id,
                Name = property.Name,
                Location = property.Location,
                Type = property.Type,
                Units = request.Units.Select(unit => new UnitDto
                {
                    Price = unit.Price,
                    Type = unit.Type,
                    Bathrooms = unit.Bathrooms,
                    Size = unit.Size,
                    NoOfUnits = unit.NoOfUnits
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var properties = await _propertyRepository.GetAllAsync();

            var response = new List<PropertyDto>();
            foreach (var propety in properties)
            {
                response.Add(new PropertyDto
                {
                    Id = propety.Id,
                    Name = propety.Name,
                    Location = propety.Location,
                    Type = propety.Type,
                    Units = propety.Units.Select(c => new UnitDto
                    {
                        Price = c.Price,
                        Type = c.Type,
                        Bathrooms = c.Bathrooms,
                        Size = c.Size,
                        NoOfUnits = c.NoOfUnits

                    }).ToList()
                });
            }

            return Ok(response);

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

            var response = new PropertyDto
            {
                Id = property.Id,
                Name = property.Name,
                Location = property.Location,
                Type = property.Type,
                Units = property.Units.Select(unit => new UnitDto
                {
                    Price = unit.Price,
                    Type = unit.Type,
                    Bathrooms = unit.Bathrooms,
                    Size = unit.Size,
                    NoOfUnits = unit.NoOfUnits
                }).ToList()
            };

            return Ok(response);
        }

        // PUT: {apibaseurl}/api/property/{id}
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdatePropertyById([FromRoute] int id, UpdatePropertyRequestDto request)
        {
            var property = new Property()
            {
                Id = id,
                Name = request.Name,
                Type = request.Type,
                Location = request.Location,
                Units = new List<Unit>()
            };

            foreach (var unit in request.Units.ToList()) 
            {

                var updatedUnit = new Unit
                {
                    Price = unit.Price,
                    Type = unit.Type,
                    Bathrooms = unit.Bathrooms,
                    Size = unit.Size,
                    NoOfUnits = unit.NoOfUnits,
                    PropertyId = property.Id // optional if your Unit has a foreign key
                };

                property.Units.Add(updatedUnit);
            }


            var updatedProperty = await _propertyRepository.UpdateAsync(property);

            if (updatedProperty == null)
            {
                return NotFound();
            }

            var response = new PropertyDto
            {
                Id = updatedProperty.Id,
                Name = updatedProperty.Name,
                Location = updatedProperty.Location,
                Type = updatedProperty.Type,
                Units = updatedProperty.Units.Select(unit => new UnitDto
                {
                    Price = unit.Price,
                    Type = unit.Type,
                    Bathrooms = unit.Bathrooms,
                    Size = unit.Size,
                    NoOfUnits = unit.NoOfUnits
                }).ToList()
            };

            return Ok(response);

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

            var response = new PropertyDto
            {
                Id = deletedProperty.Id,
                Name = deletedProperty.Name,
                Location = deletedProperty.Location,
                Type = deletedProperty.Type,
                Units = deletedProperty.Units.Select(unit => new UnitDto
                {
                    Price = unit.Price,
                    Type = unit.Type,
                    Bathrooms = unit.Bathrooms,
                    Size = unit.Size,
                    NoOfUnits = unit.NoOfUnits
                }).ToList()
            };

            return Ok(response);

        }

    

}
    }
