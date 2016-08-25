namespace ServiceCommon
{
    public interface IValidator<T>
    {
        IOperationResult Validate(T entity);
    }
}