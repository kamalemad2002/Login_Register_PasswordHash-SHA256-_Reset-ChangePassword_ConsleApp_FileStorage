using SecurityProject.Models;
using SecurityProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityProject
{
    public class Menu
    {
        public Menu()
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine("Login/Register\r");
                Console.WriteLine("------------------------\n");
                Console.WriteLine("Choose an option from the following list:");
                Console.WriteLine("\tl - Login");
                Console.WriteLine("\tr - Register");
                Console.WriteLine("\tq - Quit");
                Console.Write("Your option? ");
                switch (Console.ReadLine().ToLower())
                {
                    case "l":
                        Console.WriteLine("Login\r");
                        Console.WriteLine("------------------------\n");

                        string result = Services.AuthService.Login();
                        if (!string.IsNullOrEmpty(result))
                        {
                            ShowAuthenticatedMenu(result);
                        }
                        break;
                    case "r":
                        Console.WriteLine("Register\r");
                        Console.WriteLine("------------------------\n");

                        AuthService.Register();
                        //Console.WriteLine("You have created a new user!");
                        break;
                    case "q":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid input. Try again!");
                        break;
                }
            }
        }

        public void ShowAuthenticatedMenu(string userEmail)
        {
            while (true)
            {
                Console.WriteLine($"\n--- Welcome {userEmail} ---");
                Console.WriteLine("1. Encrypt Text");
                Console.WriteLine("2. Decrypt Text");
                Console.WriteLine("3. Change Password");
                Console.WriteLine("4. Reset Password");
                Console.WriteLine("00. Log Out");
                Console.Write("Choose an option: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        EncryptionServiceRSA.RSAEncrypt(userEmail, false);
                        break;
                    case "2":
                        EncryptionServiceRSA.RSADecrypt(userEmail,false);
                        break;
                    case "3":
                         PassServices.ChangePassword(userEmail);
                        break;
                    case "4":
                        PassServices.ResetPassword(userEmail);
                        break;
                    case "00":
                        Console.WriteLine("Logged out.");
                        return; 
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
    }
}
