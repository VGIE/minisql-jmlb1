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
            return m_username == "Admin";

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
            if (!IsUserAdmin())
            {
                //si no lo es devuelve vacio
                return;
            }
            //comprobar que la tabla y el usuario existen
            if (profileName == null || table == null)
            {
                return;
            }

            //buscamos el perfil
            Profile profile = ProfileByName(profileName);
            if (profile != null)
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

            if (IsUserAdmin())
            {

                Profile profile = ProfileByName(profileName);

                if (profile != null)
                {
                    profile.RevokePrivilege(table, privilege);
                }
            }
        }

        public bool IsGrantedPrivilege(string username, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Return true if the username has this privilege on this table. False otherwise (also in case of error)
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(table) || privilege == null)
            {
                return false;
            }

            Profile profile = ProfileByUser(username);

            if (profile == null)
            {
                return false;
            }

            if (profile.Name == Profile.AdminProfileName)
            {
                return true;
            }

            return profile.IsGrantedPrivilege(table, privilege);
        }

        public void AddProfile(Profile profile)
        {
            //TODO DEADLINE 5: Add this profile

            //si es administrados puede añadir perfiles
            if (IsUserAdmin())
            {
                //comprobar que el perfil no es nulo y que no esta ya añadido
                if (profile != null && ProfileByName(profile.Name) == null)
                {
                    Profiles.Add(profile);
                }
            }
            if (!IsUserAdmin()) return;

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
            if (profileName == null)
            {
                return null;
            }
            foreach (Profile p in Profiles)
            {
                if (p.Name == profileName)
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
            foreach (var profile in Profiles)
            {
                foreach (var user in profile.Users)
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
            if (profileName == Profile.AdminProfileName)
            {
                return false;
            }

            //buscamos el perfil
            var perfil = ProfileByName(profileName);

            //comprobar que no sea nula
            if (perfil != null && Profiles.Contains(perfil))
            {
                return Profiles.Remove(perfil);

            }

            return false;

        }

        public static Manager Load(string databaseName, string username)
        {
            string managerDir = Path.Combine(databaseName, "managerData");

            Manager manager = new Manager(username);
            // Si no existe la carpeta, manager vacío
            if (Directory.Exists(managerDir))
            {
                string[] folders = Directory.GetDirectories(managerDir);

                foreach (string folder in folders)
                {
                    string[] files = Directory.GetFiles(folder, "*.txt");

                    //Seguir solo si hay archivos en la carpeta
                    if (files.Length > 0)
                    {
                        string[] lines = File.ReadAllLines(files[0]);

                        //Seguir solo si el archivo no está vacío
                        if (lines.Length > 0)
                        {
                            Profile profile = new Profile();

                            int index = 0;

                            foreach (string line in lines)
                            {
                                //La primera linea es el nombre de perfil, luego se actualiza el index
                                if (index == 0)
                                {
                                    profile.Name = line;
                                }
                                else
                                {
                                    //Para usuarios 
                                    if (line.StartsWith("USER:"))
                                    {
                                        string data = line.Substring(5);
                                        //Separar user y password
                                        string[] parts = data.Split(',');

                                        //Para que el formato sea correcto tiene que tener exactamente las 2 partes
                                        if (parts.Length == 2)
                                        {
                                            User user = new User();
                                            user.Username = parts[0];
                                            user.EncryptedPassword = parts[1];

                                            profile.Users.Add(user);
                                        }
                                    }
                                    //Para privilegios
                                    else if (line.StartsWith("PRIV:"))
                                    {
                                        string data = line.Substring(5);
                                        string[] parts = data.Split(',');

                                        if (parts.Length == 2)
                                        {
                                            string text = parts[1];
                                            //Privilege es un enumnerado, busca el privilegio
                                            Privilege privilege = Privilege.Select;
                                            bool found = false;

                                            foreach (Privilege p in Enum.GetValues(typeof(Privilege)))
                                            {
                                                if (p.ToString() == text)
                                                {
                                                    privilege = p;
                                                    found = true;
                                                    break;
                                                }
                                            }

                                            if (found)
                                            {
                                                profile.GrantPrivilege(parts[0], privilege);
                                            }
                                        }
                                    }
                                }

                                index++;
                            }
                            //Añadir perfil a manager
                            manager.Profiles.Add(profile);
                        }
                    }
                }
            }

            return manager;
        }

        public void Save(string databaseName)
        {
            //TODO DEADLINE 5: Save all the profiles and users/passwords created for this database.
            string managerDir = Path.Combine(databaseName, "managerData");

            if (Directory.Exists(managerDir))
            {
                Directory.Delete(managerDir, true);
            }

            Directory.CreateDirectory(managerDir);

            int index = 1;
            foreach (Profile profile in Profiles)
            {
                string profileFolder = Path.Combine(managerDir, index.ToString());
                Directory.CreateDirectory(profileFolder);

                string profileFile = Path.Combine(profileFolder, profile.Name + ".txt");

                using (StreamWriter writer = new StreamWriter(profileFile))
                {
                    writer.WriteLine(profile.Name);

                    foreach (User user in profile.Users)
                    {
                        writer.WriteLine($"USER:{user.Username},{user.EncryptedPassword}");
                    }

                    foreach (var tablePrivileges in profile.PrivilegesOn)
                    {
                        string table = tablePrivileges.Key;
                        foreach (Privilege privilege in tablePrivileges.Value)
                        {
                            writer.WriteLine($"PRIV:{table},{privilege}");
                        }
                    }
                }
                index++;
            }
        }
    }
}
