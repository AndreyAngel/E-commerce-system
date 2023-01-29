﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace OrderAPI.Models;

public class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public List<CartProduct> cartProducts { get; set; } = new List<CartProduct>();

    public bool IsReady { get; set; } = false;

    public bool Payment_State { get; set; } = false;

    public static DateTime DateTime { get; set; } = DateTime.Now;
}