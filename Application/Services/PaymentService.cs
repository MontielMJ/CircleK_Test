using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class PaymentService
{
    public Payment ProcessPayment(Sale sale, Payment payment)
    {
        if (payment.Method == PaymentMethod.Cash)
        {
            payment.Status = PaymentStatus.Approved;
            payment.Message = "Pago en efectivo aprobado";
            return payment;
        }


        //if (payment.Method == PaymentMethod.Card && payment.Amount != sale.Total)
        //{
        //    payment.Status = PaymentStatus.Declined;
        //    payment.Message = "Monto insuficiente";
        //    return payment;
        //}


        if (payment.Amount < sale.Total)
        {
            payment.Status = PaymentStatus.Declined;
            payment.Message = "Monto insuficiente";
            return payment;
        }


        if (string.IsNullOrWhiteSpace(payment.Reference))
        {
            payment.Status = PaymentStatus.Declined;
            payment.Message = "Referencia requerida para pago con tarjeta";
            return payment;
        }

        if (payment.Reference.Contains("FAIL"))
        {
            payment.Status = PaymentStatus.Declined;
            payment.Message = "Pago rechazado por el emisor";
            return payment;
        }

        payment.Status = PaymentStatus.Approved;
        payment.AuthCode = GenerateAuthCode();
        payment.Message = "Pago aprobado";

        return payment;
    }

    private string GenerateAuthCode()
        => Guid.NewGuid().ToString("N")[..8].ToUpper();
}
