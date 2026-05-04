using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;
using DbManager.Security;

namespace DbManager
{
 
    public class DropSecurityProfile : MiniSqlQuery
    {
        public string ProfileName { get; set; }

        public DropSecurityProfile(string profileName)
        {
            //TODO DEADLINE 4: Initialize member variables
            ProfileName = profileName;
            
        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, SecurityProfileDoesNotExistError, DropSecurityProfileSuccess

            //comprobamos que sea administrador si no no puede eliminar a nadie
            if (!database.IsUserAdmin())
            {
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;
            }

            //si es adminitrador
            //creamos el perfil
            Profile prof = database.SecurityManager.ProfileByName(ProfileName);

            if(prof == null)
            {
                return Constants.SecurityProfileDoesNotExistError;
            }

            database.SecurityManager.RemoveProfile(ProfileName);

            return Constants.DropSecurityProfileSuccess;

                       
        }

    }
}
