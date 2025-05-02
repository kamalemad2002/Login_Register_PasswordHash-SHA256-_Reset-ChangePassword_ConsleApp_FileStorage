using SecurityProject.Common;
using SecurityProject.Storage;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SecurityProject.Services
{
    public class EncryptionServiceRSA
    {
        
         static RSAParameters pubkey;
         static RSAParameters privkey;
        static EncryptionServiceRSA()
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                pubkey = rsa.ExportParameters(false);
                privkey = rsa.ExportParameters(true);
            }
        }

        public static void RSAEncrypt(string email, bool doPadding)
        {
            try
            {
                // Ask user for input text
                Console.Write("Enter text to encrypt: ");
                string inputText = Console.ReadLine();
                if (string.IsNullOrEmpty(inputText)) 
                {
                    Console.WriteLine("Text cannot be empty.");
                    return;
                }
                byte[] dataToEncrypt = CommonClass.ByteConverter.GetBytes(inputText);

                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportParameters(pubkey); 
                    rsa.ImportParameters(privkey); 
                    byte[] encryptedData = rsa.Encrypt(dataToEncrypt, doPadding);
                    string cipherBase64 = Convert.ToBase64String(encryptedData);
                    //string privateKeyBase64 = Convert.ToBase64String(rsa.ExportRSAPrivateKey());

                    FileManager.SaveEncryptedText(email, inputText, cipherBase64);
                    Console.WriteLine($"Plain Text: {inputText}");
                    Console.WriteLine($"Cipher Text (Base64): {cipherBase64}");
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Encryption error: " + e.Message);
            }
        }

        public static List<string> RSADecrypt(string email, bool doPadding)
        {
            var plainTexts = new List<string>();
            var entries = FileManager.LoadAllEncryptedTexts(email);

            foreach (var (_, cipherBase64) in entries)
            {
                try
                {
                    byte[] cipherBytes = Convert.FromBase64String(cipherBase64);

                    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                    {
                        rsa.ImportParameters(privkey); 
                        byte[] decryptedBytes = rsa.Decrypt(cipherBytes, doPadding);
                        string plainText = CommonClass.ByteConverter.GetString(decryptedBytes);
                        Console.WriteLine($"Decrypted Text: {plainText}");
                        plainTexts.Add(plainText);
                    }
                }
                catch (Exception)
                {
                    
                }
            }

            return plainTexts;
        }


    }
}
