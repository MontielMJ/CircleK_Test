namespace Application.Dtos;

public class PaySaleResult
{
    public string Status { get; set; } = null!;
    public string? AuthCode { get; set; }
    public string Message { get; set; } = null!;
    public decimal? Change { get; set; }
    public bool Success { get; set; }

    public static PaySaleResult Ok(string status, string message, string authCode = null, decimal? change = null)
    {
        return new PaySaleResult
        {
            Success = true,
            Status = status,
            Message = message,
            AuthCode = authCode,
            Change = change
        };
    }

    public static PaySaleResult Fail(string message, string status = "FAILED")
    {
        return new PaySaleResult
        {
            Success = false,
            Status = status,
            Message = message
        };
    }
}
public class SimpleResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string Error { get; set; }
}