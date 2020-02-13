using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using Xunit;
using MMB.Mangalam.Web.Model;


namespace MMB.Mangalam.Web.Test.Helpers
{
    public class CandidateHelper
    {
        public static void Assrt(Candidate expected, Candidate actual)
        {
            Assert.Equal(expected.first_name, actual.first_name);
            Assert.Equal(expected.last_name, actual.last_name);
            Assert.Equal(expected.phone_number, actual.phone_number);
            Assert.Equal(expected.caste_id, actual.caste_id);
            Assert.Equal(expected.education_id, actual.education_id);
            Assert.Equal(expected.family_type_id, actual.family_type_id);
            Assert.Equal(expected.gender_id, actual.gender_id);
            Assert.Equal(expected.religion_id, actual.religion_id);
            


        }

        public static Candidate Get(string first_name, string last_name, 
            long phone_number, Int16 caste_id, Int16 education_id, Int16 family_type_id, Int16 gender_id, Int16 religion_id)
        {
            Candidate candidate = new Candidate();
            candidate.first_name = first_name;
            candidate.phone_number = phone_number;
            candidate.caste_id = caste_id;
            candidate.education_id = education_id;
            candidate.family_type_id = family_type_id;
            candidate.gender_id = gender_id;
            candidate.religion_id = religion_id;

            return candidate;
        }
    }
}
