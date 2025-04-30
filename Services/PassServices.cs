using SecurityProject.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityProject.Services
{
    public class PassServices
    {
        public static void ResetPassword(string email)
        {
            Console.Write("Enter new password: ");
            string newPass = Console.ReadLine();
            FileManager.UpdateUserPassword(email, Helpers.HashedPasswordSHA256.HashPassword(newPass));
            Console.WriteLine("Password reset successfully.");
        }
        public static void ChangePassword(string email)
        {
            Console.Write("Enter current password: ");
            string current = Console.ReadLine();

            var users = FileManager.LoadUsers();
            var user = users.FirstOrDefault(u => u.Email == email);

            if (user != null && user.Password== Helpers.HashedPasswordSHA256.HashPassword(current))
            {
                Console.Write("Enter new password: ");
                string newPass = Console.ReadLine();
                FileManager.UpdateUserPassword(email, Helpers.HashedPasswordSHA256.HashPassword(newPass));
                Console.WriteLine("Password changed successfully.");
            }
            else
            {
                Console.WriteLine("Incorrect current password.");
            }
        }
    }
}
