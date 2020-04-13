﻿using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage
{
    class UserController
    {
        private Dictionary<string, User> Users;

        public UserController() {
            Users = new Dictionary<string, User>();
            DalController dalC = new DalController();
            List<DataAccessLayer.User> DALusers = dalC.LoadAllUsers();
            foreach (DataAccessLayer.User DALuser in DALusers) {
                User savedUser = new User(DALuser.Email, DALuser.Password, DALuser.Nickname);
                Users.Add(savedUser.email, savedUser);
            }
        }

        public void Register(string email, string password, string nickname) {
            if (!Users.ContainsKey(email)) {
               ValidatePassword(password);
               ValidateEmail(email);
               User newUser = new User(email, password, nickname);
               Users.Add(email, newUser);
               newUser.Save("Users\\");
            }
            else
               throw new ArgumentException("A user with " + email + " E-mail address already exists, please re-evaluate your information and try again.");
        }

        public User Login (string email, string password) {
            if (!Users.ContainsKey(email))
                throw new ArgumentException(email + " does not exist in the database, please register and try again.");
            else if (!Users[email].password.Equals(password))
                throw new ArgumentException("Incorrect password. Please try again.");
            else
                Users[email].Login();
            return Users[email];
        }

        public void Logout (string email) {
            Users[email].Logout();
        }

        public void ChangePassword (string email, string oldPassword, string newPassword) {
            if (Users[email].password.Equals(oldPassword)) {
                ValidatePassword(newPassword);
                Users[email].ChangePassword(newPassword);
                Users[email].Save("Users\\");
            }
            else
                throw new ArgumentException("Old password does not match the current password. Please try again.");
        }

        private void ValidatePassword (string password) {
            if (password.Length < 4 | password.Length > 20 || //Checks that the password length is within the appropriate bounds
            !Regex.IsMatch(password, "^[a-zA-Z0-9]*$") | //Checks that the password consists of alphanumerical characters only
            !password.Any(char.IsDigit) | !password.Any(char.IsLower) | !password.Any(char.IsUpper)) //Checks if the password contains at least 1 digit, lowercase letter and uppercase letter
                throw new ArgumentException("A user password must be in length of 4 to 20 characters and must include at least one uppercase letter, one lowercase letter and a number.");
        }

        private void ValidateEmail (string email) {
            if (!Regex.IsMatch(email, "^[a-zA-Z0-9]*$"))
                throw new ArgumentException(email + " is invalid, please use only alphanumerical characters and consult the following form *example@gmail.com*");
        }
    }
}
