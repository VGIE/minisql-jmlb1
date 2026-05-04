using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;
using DbManager.Security;

namespace DbManager
{
 
    public class AddUser : MiniSqlQuery
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string ProfileName { get; private set; }


        public AddUser(string username, string password, string profileName)
        {
            //TODO DEADLINE 4: Initialize member variables
            Username = username;
            Password = password;
            ProfileName = profileName;
        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, SecurityProfileDoesNotExistError, AddUserSuccess

            //comrpobar que el usuario es administrador, si no no puede añadir
            if (!database.SecurityManager.IsUserAdmin())
            {
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;
            }

            //buscamos perfil de seguridad
            Profile profile = database.SecurityManager.ProfileByName(ProfileName);

            //verificar que el perfil exista
            if(profile == null)
            {
                return Constants.SecurityProfileDoesNotExistError;
            }

            //creamos el usuario y lo añadimos a ese perfil
            User user = new User(Username, Password);
            profile.Users.Add(user);

            return Constants.AddUserSuccess;
            
        }

    }
}
