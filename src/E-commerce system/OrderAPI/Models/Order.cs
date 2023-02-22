using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models;

public class Order
{
    public int Id { get; set; }


    [Range(1, 9999999999999999999, ErrorMessage = "Invalid UserId")]
    public int UserId { get; set; }

    public List<CartProduct> CartProducts { get; set; } = new List<CartProduct>();

    public bool IsReady { get; set; } = false;

    public bool Payment_State { get; set; } = false;

    public static DateTime DateTime { get; set; } = DateTime.Now;
}