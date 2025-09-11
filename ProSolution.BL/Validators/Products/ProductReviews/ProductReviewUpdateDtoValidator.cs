//using FluentValidation;
//using ProSolution.BL.DTOs;

//namespace ProSolution.BL.Validators.Products.ProductReviews
//{
//    public class ProductReviewUpdateDtoValidator : AbstractValidator<ProductReviewUpdateDto>
//    {
//        public ProductReviewUpdateDtoValidator()
//        {
//            RuleFor(x => x.Text)
//                .NotEmpty().WithMessage("Text is required.")
//                .MaximumLength(1000).WithMessage("Text must be at most 1000 characters.")
//                .Matches(@"^[\s\S]{1,1000}$").WithMessage("Text contains invalid characters.");

//            RuleFor(x => x.Name)
//                .NotEmpty().WithMessage("Name is required.")
//                .MaximumLength(100).WithMessage("Name must be at most 100 characters.")
//                .Matches(@"^[\p{L}\s\-']+$").WithMessage("Name can only contain letters, spaces, hyphens, and apostrophes.");

//            RuleFor(x => x.Email)
//                .NotEmpty().WithMessage("Email is required.")
//                .EmailAddress().WithMessage("Email format is invalid.")
//                .MaximumLength(150).WithMessage("Email must be at most 150 characters.");

//            RuleFor(x => x.Rating)
//                .NotEmpty().WithMessage("Rating is required.")
//                .Must(r => new[] { "1", "2", "3", "4", "5" }.Contains(r))
//                .WithMessage("Rating must be a value from 1 to 5.");

//            RuleFor(x => x.ProductId)
//                .NotEmpty().WithMessage("ProductId is required.");
//        }
//    }
//}
