# 🔐 Secure Login and RSA Encryption Console App

This is a simple C# console app that includes:

- ✅ User registration with SHA-256 password hashing
- ✅ User login with hashed password check
- ✅ RSA encryption of user-entered text
- ✅ Storage of encrypted text and private key in a file
- ✅ Ability to decrypt all previously saved messages for a user

---

## 🔑 How Login & Register Works

- **Register**: User enters name, email, and password  
  → Password is hashed with SHA-256  
  → Data saved in `users.txt`

- **Login**: User enters email and password  
  → Password is hashed and compared to the saved one  
  → If matched, login is successful

---

## 🔐 How Encryption Works

- Text is encrypted using `RSACryptoServiceProvider`
- Encrypted text and private key are saved to `encrypted_data.txt`
- All previous encrypted messages for a user can be decrypted

---

## 📁 File Storage

- `users.txt` → stores: `email,hashedPassword`
- `encrypted_data.txt` → stores: `email,plainText,CipherText`

---

## 🛠 Built With

- C# (.NET Console App)
- SHA-256 (for password hashing)
- RSACryptoServiceProvider (for encryption & decryption)
- System.IO (for file handling)
