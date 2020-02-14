using MMB.Mangalam.Web.Model;
using MMB.Mangalam.Web.Service.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMB.Mangalam.Web.Test.Helpers
{
    public static class NewRegistrationHelper
    {
        public static NewRegistrationViewModel Get()
        {
            User expectedUser = UserHelper.Get(null, "First_name", false, "Last_name", "1234567890", UserRoles.Candidate, "", "1234567890");
            

            Candidate expectedCandidate
                = CandidateHelper.Get("cfn", "cln", 1234567891, Castes.Jain, HighestEducations.BE_BTECH,
                            FamilyTypes.Joint, Genders.Female, Religions.HINDU);

            Address expectedUserAddress = AddressHelper.Get("ual1", "ual2", Districts.NASHIK, States.GUJRAT, Talukas.SHIROL, 123);
            Address expectedCandidateAddress = AddressHelper.Get("cal1", "cal2", Districts.KOLHAPUR, States.GOA, Talukas.DAUND, 345);


            NewRegistrationViewModel newRegistration = new NewRegistrationViewModel();
            newRegistration.address_line_1 = expectedUserAddress.address_line_1;
            newRegistration.address_line_2 = expectedUserAddress.address_line_2;

            newRegistration.candidate_address_line_1 = expectedCandidateAddress.address_line_1;
            newRegistration.candidate_address_line_2 = expectedCandidateAddress.address_line_2;
            newRegistration.candidate_district_id = expectedCandidateAddress.district_id;
            newRegistration.candidate_first_name = expectedCandidate.first_name;
            newRegistration.candidate_last_name = expectedCandidate.last_name;
            newRegistration.candidate_phone_number = expectedCandidate.phone_number;
            newRegistration.candidate_state_id = expectedCandidateAddress.state_id;
            newRegistration.candidate_taluka_id = expectedCandidateAddress.taluka_id;

            newRegistration.caste_id = expectedCandidate.caste_id;
            newRegistration.district_id = expectedUserAddress.district_id;
            newRegistration.education_id = expectedCandidate.education_id;
            newRegistration.familytype_id = expectedCandidate.family_type_id;
            newRegistration.first_name = expectedUser.first_name;
            newRegistration.gender_id = expectedCandidate.gender_id;

            newRegistration.language = new int[] { Languages.Marathi, Languages.Hindi };
            newRegistration.last_name = expectedUser.last_name;
            newRegistration.phone_number = Convert.ToInt64(expectedUser.phone_number);
            newRegistration.religion_id = expectedCandidate.religion_id;
            newRegistration.state_id = expectedUserAddress.state_id;
            newRegistration.taluka_id = expectedUserAddress.taluka_id;


            newRegistration.zip_code = expectedUserAddress.zip_code;
            newRegistration.candidate_zip_code = expectedCandidateAddress.zip_code;

            return newRegistration;
        }

    }
}
