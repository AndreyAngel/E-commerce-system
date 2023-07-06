namespace DeliveryAPI.DataBase;

/// <summary>
/// Delivery status
/// </summary>
public enum DeliveryStatus
{
    /// <summary>
    /// Waiting for the courier
    /// </summary>
    WaitingForTheCourier,

    /// <summary>
    /// The order received by courier
    /// </summary>
    TheOrderReceivedByCourier,

    /// <summary>
    /// The order received by customer
    /// </summary>
    TheOrderReceivedByCustomer,

    /// <summary>
    /// Delivery is canceled
    /// </summary>
    Canceled,

    /// <summary>
    /// The order returned to warehouse
    /// </summary>
    ReturnedToWarehouse
}
