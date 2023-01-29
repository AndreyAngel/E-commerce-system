﻿using OrderAPI.Models;

namespace OrderAPI.Services.Interfaces;

interface ICartService
{
    Task<Cart> GetById(int id);
    Task<Cart> Create(Cart cart);
    Task<Cart> ComputeTotalValue(int id);
    Task<Cart> Clear(int id);
}
