
using System;

namespace NextGenSoftware.OASIS.API.WebAPI
{
    public class User : BaseEntity
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return string.Concat(Title, " ", FirstName, " ", LastName);
            }
        }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string Landline { get; set; }
        public string Mobile { get; set; }
        public DateTime DOB { get; set; }
        public int UserType { get; set; }
    }

    //TODO: More types will be added later.
    public enum UserType
    {
        Admin, //0
        Standard //1
    }
}