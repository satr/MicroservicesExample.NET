namespace ServiceCommon
{
    public abstract class EntityValidatorBase<T>: IValidator<T>
    {
        public abstract IOperationResult Validate(T entity);

        protected static void ValidateRequiredStringValue(string name, string value, IOperationResult operationResult)
        {
            if (string.IsNullOrWhiteSpace(value))
                operationResult.AddError($"Required property \"{0}\" is missed.", name);
        }
    }
}
