using MicroservicesExample.NET.ProductsService;
using ServiceCommon;

namespace ProductsService.Validators
{
    public class ProductValidator: EntityValidatorBase<Product>
    {
        public override IOperationResult Validate(Product entity)
        {
            var operationResult = new OperationResult();
            ValidateRequiredStringValue("Id", entity.Id, operationResult);
            ValidateRequiredStringValue("Name", entity.Name, operationResult);
            return operationResult;
        }
    }
}