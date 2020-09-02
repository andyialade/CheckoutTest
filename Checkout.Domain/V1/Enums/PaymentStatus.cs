namespace Checkout.Domain.V1
{
    public enum PaymentStatus
    {
        PaymentSuccessful,     
        InsufficientFunds,
        FailedCardVerification,
        Failed,
        Pending
    }
}
