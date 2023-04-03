﻿namespace CatalogAPI.Models.DataBase;

public class Product : BaseEntity
{
    public double Price { get; set; }

    public Guid CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public Guid BrandId { get; set; }

    public virtual Brand? Brand { get; set; }

    public bool IsSale { get; set; } = false;
}
