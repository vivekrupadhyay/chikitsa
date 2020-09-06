using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chikitsa.Entities
{

    public class User
    {
        public User()
        {
        }
        public User(Int64 UserId)
        {
            this.UserId = UserId;
            FirstName = LastName = Mobile = Email = Password = string.Empty;
            WorkingSince = WorkingSinceWithUs = DateTime.Now;
            CompanyId = Status = UserType = 0;
            IsActiveOnSite = false;
        }
        public long UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [MaxLength(10)]
        public string Mobile { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? WorkingSince { get; set; }
        public DateTime? WorkingSinceWithUs { get; set; }
        public int Status { get; set; }
        public string strStatus { get; set; }
        public bool IsActiveOnSite { get; set; }
        public Int16 UserType { get; set; }
        public string strUserType { get; set; }
        public long CompanyId { get; set; }
        public long CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
    }

    public class UserDetailsVM : Layout
    {
        public UserDetailsVM() { User = new User(); }
        public User User { get; set; }
        public List<CodeDetail> lstUserTypes { get; set; }
    }

    public class UserListViewModel : Layout
    {
        public List<User> lstUsers { get; set; }
        public UserFilter objFilter { get; set; }
        public List<CodeDetail> lstUserTypes { get; set; }
    }

    public class UserFilter : QueryBO
    {
        public Int64 UserId { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public string UserType { get; set; }
    }
}
