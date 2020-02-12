using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
namespace MMB.Mangalam.Web.Model.Helpers
{
    public class NewRegistrationValidator : AbstractValidator<NewRegistrationViewModel>
    {
        public NewRegistrationValidator()
        {
            RuleFor(candidate => candidate.first_name).NotNull().NotEmpty().MinimumLength(2).WithMessage("vallid user first name required");
            RuleFor(candidate => candidate.last_name).NotNull().NotEmpty().MinimumLength(2).WithMessage("vallid user last name required");
            RuleFor(candidate => candidate.phone_number).NotNull().NotEmpty().GreaterThan(999999999).LessThan(9999999999).WithMessage("phone number must be 10 digit");
            RuleFor(candidate => candidate.address_line_1).NotNull().NotEmpty().MinimumLength(2).WithMessage("vallid user address line 1 required");
            RuleFor(candidate => candidate.address_line_2).NotNull().NotEmpty().MinimumLength(2).WithMessage("vallid user address line 2 required");
            RuleFor(candidate => candidate.taluka_id).NotNull().NotEmpty().GreaterThan(0).WithMessage("vallid user taluka required");
            RuleFor(candidate => candidate.district_id).NotNull().NotEmpty().GreaterThan(0).WithMessage("vallid user district required");
            RuleFor(candidate => candidate.state_id).NotNull().NotEmpty().GreaterThan(0).WithMessage("vallid user state required");
            RuleFor(candidate => candidate.candidate_first_name).NotNull().NotEmpty().MinimumLength(2).WithMessage("vallid candidate first name required");
            RuleFor(candidate => candidate.candidate_last_name).NotNull().NotEmpty().MinimumLength(2).WithMessage("vallid candidate last name  required");
            RuleFor(candidate => candidate.candidate_phone_number).NotNull().NotEmpty().GreaterThan(0).WithMessage("vallid candidate phone number required");
            RuleFor(candidate => candidate.candidate_address_line_1).NotNull().NotEmpty().MinimumLength(2).WithMessage("vallid candidate address line 1 required");
            RuleFor(candidate => candidate.candidate_address_line_2).NotNull().NotEmpty().MinimumLength(2).WithMessage("vallid candidate address line 2 required");
            RuleFor(candidate => candidate.candidate_taluka_id).NotNull().NotEmpty().GreaterThan(0).WithMessage("vallid candidate taluka required");
            RuleFor(candidate => candidate.candidate_district_id).NotNull().NotEmpty().GreaterThan(0).WithMessage("vallid candidate district required");
            RuleFor(candidate => candidate.candidate_state_id).NotNull().NotEmpty().GreaterThan(0).WithMessage("vallid candidate state required");
            RuleFor(candidate => candidate.gender_id).NotNull().NotEmpty().GreaterThan(0).WithMessage("gender required");
            RuleFor(candidate => candidate.religion_id).NotNull().NotEmpty().GreaterThan(0).WithMessage("religion required");
            RuleFor(candidate => candidate.caste_id).NotNull().NotEmpty().GreaterThan(0).WithMessage("caste required");
            RuleFor(candidate => candidate.education_id).NotNull().NotEmpty().GreaterThan(0).WithMessage("education required");
            RuleFor(candidate => candidate.language).NotNull().NotEmpty().WithMessage("language required");
            RuleFor(candidate => candidate.familytype_id).NotNull().NotEmpty().GreaterThan(0).WithMessage("family type required");
        }
    }
}
