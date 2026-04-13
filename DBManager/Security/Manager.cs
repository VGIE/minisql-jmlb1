using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Security
{
    public class Manager
    {
        public List<Profile> Profiles { get; private set; } = new List<Profile>();

        private string m_username;
        public Manager(string username)
        {
            m_username = username;
        }

        public bool IsUserAdmin()
        {
            //TODO DEADLINE 5: Return true if the user logged-in (m_username) is the admin, false otherwise
            Profile profile = ProfileByUser(m_username);
            return profile != null && profile.Name == "Admin" ;

        }

        public bool IsPasswordCorrect(string username, string password)
        {
            //TODO DEADLINE 5: Return true if the user's password is correct. The given password should be encrypted before comparing with the saved one
            //encriptamos para poder comparar
            string encrypted = Encryption.Encrypt(password);

            //recorremos perfiles
            for (int i = 0; i < Profiles.Count; i++)
            {
                if (Profiles[i] != null)
                {
                    //recorremos todos los usuarios de ese perfil
                    for (int j = 0; j < Profiles[i].Users.Count; j++)
                    {
                        //comparamos
                        if (Profiles[i].Users[j].Username == username &&
                            Profiles[i].Users[j].EncryptedPassword == encrypted)
                        {
                            return true;  
                        }
                    }
                }
            }
            return false;
            
        }

        public void GrantPrivilege(string profileName, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Add this privilege on this table to the profile with this name
            //If the profile or the table don't exist, do nothing
            
            //comprobamos que el usuario es admin ********
            if(!IsUserAdmin())
            {
                //si no lo es devuelve vacio
                return;
            }
            //comprobar que la tabla y el usuario existen
            if(profileName == null || table == null)
            {
                return;
            }

            //buscamos el perfil
            Profile profile = ProfileByName(profileName);
            if(profile != null) 
            {
            
              profile.GrantPrivilege(table, privilege);
                
            }
        }

        public void RevokePrivilege(string profileName, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Remove this privilege on this table to the profile with this name
            //If the profile or the table don't exist, do nothing

            if (profileName == null || table == null)
            {
                return;
            }

            if (IsUserAdmin()){

                Profile profile = ProfileByName(profileName);
                
                if(profile != null)
                {
                    profile.RevokePrivilege(table, privilege);
                }
            }
        }

        public bool IsGrantedPrivilege(string username, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Return true if the username has this privilege on this table. False otherwise (also in case of error)
            
            return false;
            
        }

        public void AddProfile(Profile profile)
        {
            //TODO DEADLINE 5: Add this profile

            //si es administrados puede añadir perfiles
            if(IsUserAdmin())
            {
                //comprobar que el perfil no es nulo y que no esta ya añadido
                if(profile != null && !Profiles.Contains(profile))
                {
                    Profiles.Add(profile);
                }
            }
            //si no es administrados no hace nada
                        
        }

        public User UserByName(string username)
        {
            if (username == null)
            {
                return null;
            }

            foreach (Profile p in Profiles)
            {
                List<User> users = p.Users;

                foreach (User u in users)
                {
                    if (u.Username == username)
                    {
                        return u;
                    }
                }
            }

            return null;
        }


        public Profile ProfileByName(string profileName)
        {
            //TODO DEADLINE 5: Return the profile by name. If it doesn't exist, return null

            //comprobar que el nombre no sea nulo
            if(profileName == null)
            {
                return null;
            }
            foreach (Profile p in Profiles)
            {
                if(p.Name == profileName)
                {
                    return p;
                }
            }

                return null;
            
        }

        public Profile ProfileByUser(string username)
        {
            //TODO DEADLINE 5: Return the profile by user. If the user doesn't exist, return null

            if (string.IsNullOrWhiteSpace(username))
            {
                return null;
            }
            foreach(var profile in Profiles)
            {
                foreach(var user in profile.Users)
                {
                    if (user.Username == username)
                    {
                        return profile;
                    }
                }
            }
            return null;

        }

        public bool RemoveProfile(string profileName)
        {
            //TODO DEADLINE 5: Remove this profile

            //si no es administrados no puede eliminar
            if (!IsUserAdmin())
            {
                       
                return false;
            }

            //no se puede eliminar a si mismo 
            if(profileName == Profile.AdminProfileName)
            {
                return false;
            }

            //buscamos el perfil
            var perfil = ProfileByName(profileName);
            
            //comprobar que no sea nula
            if(perfil != null && Profiles.Contains(perfil))
            {
                return Profiles.Remove(perfil);

            }

            return false;
                
        }

        public static Manager Load(string databaseName, string username)
        {
            //TODO DEADLINE 5: Load all the profiles and users saved for this database. The Manager instance should be created with the given username
            
            return null;
            
        }

        public void Save(string databaseName)
        {
            //TODO DEADLINE 5: Save all the profiles and users/passwords created for this database.
            
        }
    }
}
