using MMB.Mangalam.Web.Model;
using MMB.Mangalam.Web.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMB.Mangalam.Web.Model
{
    public interface IUserService
    {

        JsonResponse<User> Authenticate(string username, string password);
        IEnumerable<User> GetAll();
    }
}
