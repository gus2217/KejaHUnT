﻿using KejaHUnt_PropertiesAPI.Models.Domain;

namespace KejaHUnt_PropertiesAPI.Repositories.Interface
{
    public interface IPropertyRepository
    {
        Task<Property> CreatePropertyAsync(Property property);

        Task<IEnumerable<Property>> GetAllAsync();

        Task<Property?> GetPropertyByIdAsync(int id);

        Task<Property?> UpdateAsync(Property property);

        Task<Property?> DeleteAync(int id);
    }
}
