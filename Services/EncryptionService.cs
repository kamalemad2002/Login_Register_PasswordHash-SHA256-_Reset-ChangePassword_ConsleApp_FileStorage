using SecurityProject.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SecurityProject.Services
{
    public class EncryptionService
    {
        public static void EncryptText(string email)
        {
            Console.Write("Enter text to encrypt: ");
            string plainText = Console.ReadLine();

            using (RSA rsa = RSA.Create())
            {
                string publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
                string privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());

                byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] encryptedBytes = rsa.Encrypt(inputBytes, RSAEncryptionPadding.OaepSHA256);
                string encryptedText = Convert.ToBase64String(encryptedBytes);

                FileManager.SaveEncryptedText(email, encryptedText, privateKey);
                Console.WriteLine("Text encrypted and saved using RSA.");
            }
        }
        public static void DecryptText(string email)
        {
            var (encryptedText, privateKeyBase64) = FileManager.LoadEncryptedText(email);
            if (encryptedText == null)
            {
                Console.WriteLine("No encrypted text found.");
                return;
            }

            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] privateKey = Convert.FromBase64String(privateKeyBase64);

            using (RSA rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey(privateKey, out _);
                byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.OaepSHA256);
                string plainText = Encoding.UTF8.GetString(decryptedBytes);
                Console.WriteLine("Decrypted Text: " + plainText);
            }
        }


    }
}
