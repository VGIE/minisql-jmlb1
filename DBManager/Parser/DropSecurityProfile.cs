using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;

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
            if (!database.SecurityManager.IsUserAdmin())
            {
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;
            }

            //si es adminitrados

            bool seHaEliminado = database.SecurityManager.RemoveProfile(ProfileName);

            //si se ha podido eliminar
            if (seHaEliminado)
            {
                return Constants.DropSecurityProfileSuccess;
            }

            return Constants.SecurityProfileDoesNotExistError;

        }

    }
}
