﻿using OrderAPI.Models;

namespace OrderAPI.Services.Interfaces;

public interface ICartService
{
    Task<CartDomainModel> GetById(Guid id);

    Task Create(Guid id);

    Task<CartDomainModel> ComputeTotalValue(Guid id);

    Task<CartDomainModel> Clear(Guid id);
}