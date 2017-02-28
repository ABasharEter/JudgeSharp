//╔═════════════════════════════════════════════════════════════════════════════════╗
//║                                                                                 ║
//║   ╔╗      ╔╗   ╔╗╔╗╔╗╔╗╔╗   ╔╗╔╗╔╗╔╗     ╔╗      ╔╗   ╔╗╔╗╔╗╔╗╔╗   ╔╗           ║
//║   ╚╝    ╔╗╚╝   ╚╝╚╝╚╝╚╝╚╝   ╚╝╚╝╚╝╚╝╔╗   ╚╝╔╗    ╚╝   ╚╝╚╝╚╝╚╝╚╝   ╚╝           ║
//║   ╔╗  ╔╗╚╝     ╔╗           ╔╗      ╚╝   ╔╗╚╝    ╔╗   ╔╗           ╔╗           ║
//║   ╚╝╔╗╚╝       ╚╝╔╗╔╗╔╗╔╗   ╚╝╔╗╔╗╔╗     ╚╝  ╔╗  ╚╝   ╚╝╔╗╔╗╔╗╔╗   ╚╝           ║
//║   ╔╗╚╝╔╗       ╔╗╚╝╚╝╚╝╚╝   ╔╗╚╝╚╝╚╝╔╗   ╔╗  ╚╝  ╔╗   ╔╗╚╝╚╝╚╝╚╝   ╔╗           ║
//║   ╚╝  ╚╝╔╗     ╚╝           ╚╝      ╚╝   ╚╝    ╔╗╚╝   ╚╝           ╚╝           ║
//║   ╔╗    ╚╝╔╗   ╔╗╔╗╔╗╔╗╔╗   ╔╗      ╔╗   ╔╗    ╚╝╔╗   ╔╗╔╗╔╗╔╗╔╗   ╔╗╔╗╔╗╔╗╔╗   ║
//║   ╚╝      ╚╝   ╚╝╚╝╚╝╚╝╚╝   ╚╝      ╚╝   ╚╝      ╚╝   ╚╝╚╝╚╝╚╝╚╝   ╚╝╚╝╚╝╚╝╚╝   ║
//║                                                                                 ║
//║   This file is a part of the project Judge Sharp done by Ahmad Bashar Eter.     ║
//║   This program is free software: you can redistribute it and/or modify          ║
//║   it under the terms of the GNU General Public License version 3.               ║
//║   This program is distributed in the hope that it will be useful, but WITHOUT   ║
//║   ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS ║
//║   FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details ║
//║   GNU General Public: http://www.gnu.org/licenses.                              ║
//║   For usage not under GPL please request my approval for commercial license.    ║
//║   Copyright(C) 2017 Ahmad Bashar Eter.                                          ║
//║   KernelGD@Hotmail.com                                                          ║
//║                                                                                 ║
//╚═════════════════════════════════════════════════════════════════════════════════╝

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JudgeSharp.Core
{
    public static class EncryptionHelper
    {
        private const int KeySize = 256;

        private const int DerivationIterations = 12800;

        public static string Encrypt(string plainText, string passPhrase)
        {
            var saltStringBytes = GenerateBitsOfRandomEntropy();
            var ivStringBytes = GenerateBitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var hashBytes = Convert.FromBase64String(Hash(passPhrase));
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(KeySize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = KeySize;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(hashBytes, 0, hashBytes.Length);
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            try
            {
                var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
                var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(KeySize / 8).ToArray();
                var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(KeySize / 8).Take(KeySize / 8).ToArray();
                var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((KeySize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - (KeySize / 8) * 2).ToArray();

                using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
                {
                    var keyBytes = password.GetBytes(KeySize / 8);
                    using (var symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.BlockSize = KeySize;
                        symmetricKey.Mode = CipherMode.CBC;
                        symmetricKey.Padding = PaddingMode.PKCS7;
                        using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                        {
                            using (var memoryStream = new MemoryStream(cipherTextBytes))
                            {
                                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                                {
                                    var plainTextBytes = new byte[cipherTextBytes.Length - KeySize / 8];
                                    var hashBytes = new byte[KeySize / 8*2];
                                    var hashBytesCount = cryptoStream.Read(hashBytes, 0, hashBytes.Length);
                                    var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                    memoryStream.Close();
                                    cryptoStream.Close();
                                    if (hashBytesCount != KeySize / 8*2)
                                        return null;
                                    string hashString = Convert.ToBase64String(hashBytes);
                                    if (!ValidateHash(passPhrase, hashString))
                                        return null;
                                    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                                }
                            }
                        }
                    }
                }
            }catch
            {
                return null;
            }
        }
        public static string FastHash(Byte[] data)
        {
            using(MD5 md5 = MD5.Create())
            {
                var salat = GenerateBitsOfRandomEntropy().Take(md5.HashSize / 8);
                byte[] fulldata = salat.Concat(data).ToArray();
                byte[] hashBytes = md5.ComputeHash(fulldata);
                return Convert.ToBase64String(salat.Concat(hashBytes).ToArray());
            }
        }
        public static bool FastHashValidation(Byte[] data,string hashString)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashwithsalat = Convert.FromBase64String(hashString);
                var salat = hashwithsalat.Take(md5.HashSize / 8);
                var hash = hashwithsalat.Skip(md5.HashSize / 8);
                byte[] fulldata = salat.Concat(data).ToArray();
                byte[] newhash = md5.ComputeHash(fulldata);
                return Convert.ToBase64String(salat.Concat(newhash).ToArray()) == hashString;
            }
        }
        public static string Hash(string password,string salat)
        {
            byte[] salt = Convert.FromBase64String(salat);
            byte[] hash = PBKDF2(password, salt, DerivationIterations);
            return Convert.ToBase64String(salt.Concat(hash).ToArray());
        }

        public static string Hash(string password)
        {
            return Hash(password, Convert.ToBase64String(GenerateBitsOfRandomEntropy()));
        }

        public static string ExtractSalat(string hash)
        {
            byte[] hashBytes = Convert.FromBase64String(hash);
            return Convert.ToBase64String(hashBytes.Take(KeySize / 8).ToArray());
        }
        
        public static bool ValidateHash(string password, string correctHash)
        {
            byte[] correctHashBytes = Convert.FromBase64String(correctHash);
            byte[] salt = correctHashBytes.Take(KeySize / 8).ToArray();
            byte[] hash = correctHashBytes.Skip(KeySize / 8).ToArray();

            byte[] testHash = PBKDF2(password, salt,DerivationIterations);
            return SlowEquals(hash, testHash);
        }
        
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }
        
        private static byte[] PBKDF2(string password, byte[] salt,int IterationCount)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, IterationCount);
            return pbkdf2.GetBytes(KeySize/8);
        }

        private static byte[] GenerateBitsOfRandomEntropy()
        {
            var randomBytes = new byte[KeySize / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
}
