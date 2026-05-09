using DbManager.Parser;
using DbManager.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbManager
{

    public class DeleteUser : MiniSqlQuery
    {
        public string Username { get; private set; }

        public DeleteUser(string username)
        {
            //TODO DEADLINE 4: Initialize member variables
            Username = username;

        }
        public string Execute(Database database)
        {
            if (database.SecurityManager.IsUserAdmin())
            {
                User user = database.SecurityManager.UserByName(Username);

                if (user != null)
                {
                    Profile profile = database.SecurityManager.ProfileByUser(Username);

                    if (profile != null && profile.Users.Remove(user))
                    {
                        return Constants.DeleteUserSuccess;
                    }
                    else
                    {
                        return Constants.UserDoesNotExistError;
                    }
                }
                else
                {
                    return Constants.UserDoesNotExistError;
                }
            }
            else
            {
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;
            }

        }
    }
}
