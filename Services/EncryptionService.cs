using SecurityProject.Storage;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SecurityProject.Services
{
    public class EncryptionService
    {
        static UnicodeEncoding ByteConverter = new UnicodeEncoding();

        public static void RSAEncrypt(string email, bool doOAEPPadding)
        {
            try
            {
                // Ask user for input text
                Console.Write("Enter text to encrypt: ");
                string inputText = Console.ReadLine();
                byte[] dataToEncrypt = ByteConverter.GetBytes(inputText);

                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    // Export public and private key
                    RSAParameters publicKey = rsa.ExportParameters(false);
                    RSAParameters privateKey = rsa.ExportParameters(true);

                    byte[] encryptedData = rsa.Encrypt(dataToEncrypt, doOAEPPadding);
                    string cipherBase64 = Convert.ToBase64String(encryptedData);
                    string privateKeyBase64 = Convert.ToBase64String(rsa.ExportRSAPrivateKey());

                    // Save encrypted data and private key to file
                    FileManager.SaveEncryptedText(email, cipherBase64, privateKeyBase64);

                    Console.WriteLine($"Plain Text: {inputText}");
                    Console.WriteLine($"Cipher Text (Base64): {cipherBase64}");
                    Console.WriteLine("Encrypted data saved.");
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Encryption error: " + e.Message);
            }
        }

        public static List<string> RSADecrypt(string email, bool doOAEPPadding)
        {
            var plainTexts = new List<string>();
            var entries = FileManager.LoadAllEncryptedTexts(email);

            foreach (var (cipherBase64, privateKeyBase64) in entries)
            {
                try
                {
                    byte[] cipherBytes = Convert.FromBase64String(cipherBase64);
                    byte[] privateKeyBytes = Convert.FromBase64String(privateKeyBase64);

                    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                    {
                        rsa.ImportRSAPrivateKey(privateKeyBytes, out _);
                        byte[] decryptedBytes = rsa.Decrypt(cipherBytes, doOAEPPadding);
                        string plainText = ByteConverter.GetString(decryptedBytes);

                        //plainTexts.Add(plainText);
                        Console.WriteLine($"Decrypted Text: {plainText}");
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine($"Decryption failed for one entry: {ex.Message}");
                    // Optionally log the error if needed, but don't add failed ones to the result
                }
            }

            return plainTexts;  // Return only successfully decrypted texts
        }

    }
}
