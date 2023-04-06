﻿using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.Models.DTO;

public class ProductDTORequest
{
    [Required]
    public string? Name { get; set; }

    public string? Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Invalid Price")]
    public double Price { get; set; }

    [Required]
    public Guid CategoryId { get; set; }

    [Required]
    public Guid BrandId { get; set; }
}