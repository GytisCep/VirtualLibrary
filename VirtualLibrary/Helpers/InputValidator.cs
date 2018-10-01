﻿using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Linq;
using VirtualLibrary.DataSources;
using VirtualLibrary.Model;
using VirtualLibrary.Repositories;
using VirtualLibrary.View;

namespace VirtualLibrary.Helpers
{
    public class InputValidator
    {
        public IUser ValidateUserInput(IUser userView)
        {
            IUser newUser = new User
            {
                Password = userView.Password,
                DateOfBirth = userView.DateOfBirth,
                Nickname = userView.Nickname,
                Name = userView.Name,
                Surname = userView.Surname,
                Email = userView.Email
            };
            return newUser;
            

        }

        public bool ValidUsername (string username, string defaultUsername = "default") 
        {
            if (username == null) username = defaultUsername;
            var userRepository = new UserRepository(DataSources.Data.StaticDataSource._dataSource);
            var users = userRepository.GetList();

            return users.Select(user => user.Nickname).Contains(username);
        }
    }
}
