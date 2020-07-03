using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
namespace MMB.Mangalam.Web.Model.Helpers
{
    public class EditCandidateValidator : AbstractValidator<EditCandidateViewModel>
    {
        public EditCandidateValidator()
        {
            RuleFor(candidate => candidate.candidate_first_name).NotEmpty().MinimumLength(2).WithMessage("Valid candidate first name required");
            RuleFor(candidate => candidate.candidate_last_name).NotEmpty().MinimumLength(2).WithMessage("Valid candidate last name  required");
            RuleFor(candidate => candidate.candidate_phone_number).NotNull().NotEmpty().GreaterThan(0).WithMessage("Valid candidate phone number required");
            RuleFor(candidate => candidate.candidate_address_line_1).NotEmpty().MinimumLength(2).WithMessage("Valid candidate address line 1 required");
            RuleFor(candidate => candidate.candidate_address_line_2).NotEmpty().MinimumLength(2).WithMessage("Valid candidate address line 2 required");
            RuleFor(candidate => candidate.candidate_taluka_id).NotNull().NotEmpty().GreaterThan(0).WithMessage("Valid candidate taluka required");
            RuleFor(candidate => candidate.candidate_district_id).NotNull().NotEmpty().GreaterThan(0).WithMessage("Valid candidate district required");
            RuleFor(candidate => candidate.gender_id).NotNull().NotEmpty().GreaterThan(0).WithMessage("Gender required");
            RuleFor(candidate => candidate.religion_id).NotNull().NotEmpty().GreaterThan(0).WithMessage("Religion required");
            RuleFor(candidate => candidate.language).NotNull().NotEmpty().WithMessage("Language required");
            RuleFor(candidate => candidate.familytype_id).NotNull().NotEmpty().GreaterThan(0).WithMessage("Family type required");
        }
    }
}
