namespace EquipmentHostingService.Application;

public class OperationResult<T>
{
    public bool Success { get; }

    public string Message { get; }

    public T Data { get; private set; }

    public OperationResult(bool success, string message, T data)
    {
        Success = success;
        Message = message;
        Data = data;
    }
}
