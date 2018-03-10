using LiteDB;
using PresentSir.Web.Models;
using System.Linq;

namespace PresentSir.Web.Utils
{
    public class UserManager
    {
        private readonly LiteCollection<ApplicationUser> users;
        private static UserManager singleton;
        private static object lockObject = new object();

        public static UserManager Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (singleton == null)
                        singleton = new UserManager();

                    return singleton;
                }
            }
        }

        private UserManager()
        {
            users = ApplicationDbContext.Instance.Users;
            users.EnsureIndex(x => x.PasswordHash);
            users.EnsureIndex(x => x.Username);
        }

        public AddUserResponse AddUser(ApplicationUser user)
        {
            var existingUser = users.FindOne(x => x.Username == user.Username || x.IndexNumber == user.IndexNumber);
            if (existingUser == null)
            {
                user.PasswordHash = GeneratePasswordHash(user.Password);
                user.Password = string.Empty;

                var userId = users.Insert(user);
                user.Id = userId;

                return new AddUserResponse
                {
                    User = user,
                    ErrorMessage = string.Empty
                };
            }
            else
            {
                return new AddUserResponse
                {
                    User = null,
                    ErrorMessage = "A user with that username or index number already exists."
                };
            }
        }

        public ApplicationUser FindUser(string username)
        {
            return users.Find(x => x.Username == username).FirstOrDefault();
        }

        public ApplicationUser FindUser(string username, string password)
        {
            var passwordHash = GeneratePasswordHash(password);
            return users.Find(x => x.Username == username && x.PasswordHash == passwordHash).FirstOrDefault();
        }

        public bool ChangeUserPassword(string username, string oldPassword, string newPassword)
        {
            var user = FindUser(username, oldPassword);
            if (user != null)
            {
                user.PasswordHash = GeneratePasswordHash(newPassword);
                users.Update(user);
                return true;
            }

            return false;
        }

        private string GeneratePasswordHash(string password)
        {
            return Cryptor.Encrypt(password);
        }
    }

    public class AddUserResponse
    {
        public ApplicationUser User { get; set; }
        public string ErrorMessage { get; set; }
    }
}