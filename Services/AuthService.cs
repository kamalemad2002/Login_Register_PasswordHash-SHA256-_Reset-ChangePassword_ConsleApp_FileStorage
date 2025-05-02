using SecurityProject.Models;
using SecurityProject.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SecurityProject.Services
{
    public class AuthService
    {
        private RegisterModel _registerModel;
        private LoginModel _loginModel;
        public AuthService(RegisterModel registerModel, LoginModel loginModel) 
        {
            _registerModel = registerModel;
            _loginModel = loginModel;
        }
       
        public static void Register()
        {
            RegisterModel model = new RegisterModel();
            Console.Write("Enter Email:");
            model.Email = Console.ReadLine().ToLower();
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                Console.WriteLine("Email cannot be empty.");
                return;
            }
            var users = FileManager.LoadUsers();
            if(users.Any(user => user.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("User Email is already registered,plz LogIn");
                return;
            }

            Console.Write("Enter Password: ");
            model.Password = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                Console.WriteLine("Password cannot be empty.");
                return ;
            }
            var hashed = Helpers.HashedPasswordSHA256.HashPassword(model.Password);
             FileManager.SaveUser(new RegisterModel{ Email = model.Email, Password= hashed });
            Console.WriteLine("Register Successfully!");
        }
        public static string Login()
        {
            LoginModel model = new LoginModel();
            Console.Write("Enter Email: ");
            model.Email = Console.ReadLine().ToLower();
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                Console.WriteLine("Email cannot be empty.");
                return null;
            }
            Console.Write("Enter Password: ");
            model.Password = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                Console.WriteLine("Password cannot be empty.");
                return null;
            }
            var users = FileManager.LoadUsers();
            var user = users.FirstOrDefault(u => u.Email == model.Email);

            if (user != null && user.Password == Helpers.HashedPasswordSHA256.HashPassword(model.Password))
            {
                Console.WriteLine("Login Successfully!");
                return model.Email;
            }

            Console.WriteLine("Login failed. Incorrect email or password.");
            return null;

        }
    }
}
