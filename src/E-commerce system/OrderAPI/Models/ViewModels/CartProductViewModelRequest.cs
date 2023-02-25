﻿using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models.ViewModels;

public class CartProductViewModelRequest
{
    public int? Id { get; set; }

    [Range(1, 9999999999999999999, ErrorMessage = "Invalid ProductId")]
    public int ProductId { get; set; }
    //public ProductViewModel? Product { get; set; }


    [Range(1, 999999, ErrorMessage = "Invalid quantity")]
    public int Quantity { get; set; }


    [Range(1, 9999999999999999999, ErrorMessage = "Invalid CartId")]
    public int CartId { get; set; }
}