using FluentValidation;
using SecuritiesTransactionSystem.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecuritiesTransactionSystem.Entity.Validator
{
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator()
        {
            RuleFor(x => x.Quantity)
             .GreaterThan(0).WithMessage("數量必須大於 0");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("價格必須大於 0");

            RuleFor(x => x.Symbol)
                .NotEmpty().WithMessage("股票代號不能為空");
        }
    }
}
