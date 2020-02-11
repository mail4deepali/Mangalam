using MMB.Mangalam.Web.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMB.Mangalam.Web.Model
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
    }
}
