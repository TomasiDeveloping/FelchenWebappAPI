﻿using System;
using System.Linq;
using System.Text;
using Api.Helper;

namespace Api.Services
{
    public static class PasswordService
    {
        public static Password CreatePassword(string clearTextPassword)
        {
            var password = new Password();
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            password.PasswordSalt = hmac.Key;
            password.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(clearTextPassword));
            return password;
        }
        
        public static string CreateRandomPassword()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return !computedHash.Where((t, i) => t != passwordHash[i]).Any();
        }
    }
}