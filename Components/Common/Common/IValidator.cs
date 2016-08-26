namespace Common
{
    public interface IValidator<T>
    {
        IOperationResult Validate(T entity);
    }
}