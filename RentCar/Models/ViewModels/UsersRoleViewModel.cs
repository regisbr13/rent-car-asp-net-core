using System.Collections.Generic;

namespace RentCar.Models.ViewModels
{
    public class UsersRoleViewModel
    {
        public List<Role> Roles { get; set; }
        public List<User> Users { get; set; }
        public Dictionary<string, string> RoleNames { get; set; }
    }
}
