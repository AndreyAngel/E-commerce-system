namespace DeliveryAPI.DataBase;

public enum DeliveryStatus
{
    WaitingForTheCourier,

    TheOrderReceivedByCourier,

    TheOrderReceivedByCustomer,

    Canceled,

    ReturnedToWarehouse
}
