using DigitalBooksWebAPI.Models;

namespace UserValidation.Services
{
    public class UserValidationCheck
    {
        private readonly DigitalBooksContext _context = new DigitalBooksContext();
        public string UserName { get; set; }
        public string Password { get; set; }

        public UserValidationCheck(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public bool IsValidUser()
        {
            return _context.Users.Any(user => user.UserName == UserName && user.Password == Password);
        }
    }
}
