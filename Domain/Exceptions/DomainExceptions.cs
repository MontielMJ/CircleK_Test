namespace Domain.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
    protected DomainException(string message, Exception innerException) : base(message, innerException) { }
}

public class NotFoundException : DomainException
{
    public NotFoundException(string entityName, object id) 
        : base($"{entityName} with id {id} was not found.") { }
    
    public NotFoundException(string message) : base(message) { }
    public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
}

public class BusinessException : DomainException
{
    public BusinessException(string message) : base(message) { }
    public BusinessException(string message, Exception innerException) : base(message, innerException) { }
}

public class ValidationException : DomainException
{
    public ValidationException(string message) : base(message) { }
    public ValidationException(string message, Exception innerException) : base(message, innerException) { }
}

public class InsufficientStockException : BusinessException
{
    public InsufficientStockException(string productName, int requested, int available)
        : base($"Insufficient stock for product '{productName}'. Requested: {requested}, Available: {available}") { }
}

public class InvalidPaymentException : BusinessException
{
    public InvalidPaymentException(string message) : base(message) { }
}

public class DuplicateSkuException : BusinessException
{
    public DuplicateSkuException(string sku)
        : base($"Product with SKU '{sku}' already exists.") { }
}