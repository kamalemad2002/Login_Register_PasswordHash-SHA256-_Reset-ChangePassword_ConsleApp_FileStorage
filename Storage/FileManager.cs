using SecurityProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityProject.Storage
{
    public class FileManager
    {
        private static string registerFile = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C:\\Users\\hp\\Documents\\VisualStudio\\SecurityProject\\Storage\\Files\\users.txt"));
        private static string encryptedFile = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C:\\Users\\hp\\Documents\\VisualStudio\\SecurityProject\\Storage\\Files\\encrypted_data.txt"));
        public static void SaveUser(RegisterModel user)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(registerFile, append: true))
                {
                    writer.WriteLine($"{ user.Email},{user.Password}");
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"File write error: {ex.Message}");
            }
        }

        public static List<RegisterModel> LoadUsers()
        {
            List<RegisterModel> users = new List<RegisterModel>();
            if (!File.Exists(registerFile)) return users;

            foreach (var line in File.ReadAllLines(registerFile))
            {
                var parts = line.Split(',');
                if (parts.Length == 2)
                {
                    users.Add(new RegisterModel {  Email = parts[0] ,  Password = parts[1] });
                }
            }
            return users;
        }
        public static void UpdateUserPassword(string email, string newHashedPassword)
        {
            var users = LoadUsers();
            foreach (var user in users)
            {
                if (user.Email == email)
                {
                    user.Password= newHashedPassword;
                    break;
                }
            }
            File.WriteAllLines(registerFile, users.ConvertAll(u => $"{u.Email},{u.Password}"));
        }
        public static void SaveEncryptedText(string email, string encryptedText, string privateKey)
        {
            File.AppendAllText(encryptedFile, $"{email},{encryptedText},{privateKey}\n");
        }

        public static (string, string) LoadEncryptedText(string email)
        {
            if (!File.Exists(encryptedFile)) 
                return (null, null);

            foreach (var line in File.ReadAllLines(encryptedFile))
            {
                var parts = line.Split(',');
                if (parts.Length >= 3 && parts[0] == email)
                {
                    return (parts[1], parts[2]);
                }
            }
            return (null, null);
        }
        public static void InitializeFiles()
        {
            if (!File.Exists(registerFile))
                using (FileStream fs = File.Create(registerFile)) { }

            if (!File.Exists(encryptedFile))
                using (FileStream fs = File.Create(encryptedFile)) { }
        }

    }
}
