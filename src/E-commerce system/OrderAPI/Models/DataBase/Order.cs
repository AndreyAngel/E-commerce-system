namespace OrderAPI.Models.DataBase;

public class Order : BaseEntity
{
    public int UserId { get; set; }

    public List<CartProduct> CartProducts { get; set; } = new List<CartProduct>();

    public bool IsReady { get; set; } = false;

    public bool Payment_State { get; set; } = false;

    public static DateTime DateTime { get; set; } = DateTime.Now;
}