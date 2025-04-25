using FluentValidation;

namespace CourierCounter.Models.ApiModels.Validator
{
    public class OrderValidator : AbstractValidator<OrdersViewModel>
    {
        public OrderValidator()
        {
            RuleFor(order => order.CustomerName)
                .NotEmpty().WithMessage("Customer Name is required")
                .MaximumLength(50).WithMessage("Customer Name cannot exceed  50 characters")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("Customer Name can only contain letters and spaces");

            RuleFor(order => order.CustomerEmail)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(order => order.CustomerContactNumber)
                .NotEmpty().WithMessage("Customer Contact Number is required")
                .Matches(@"^\d{10}$").WithMessage("Customer Contact Number must be exactly 10 digits long");

            RuleFor(order => order.DeliveryAddress)
                .NotEmpty().WithMessage("Customer Delivery Address is required");

            RuleFor(order => order.DeliveryZone)
            .NotEmpty().WithMessage("Delivery Zone is required");

        }
    }
}
