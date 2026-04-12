using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Security
{
    public class Profile
    {
        public const string AdminProfileName = "Admin";
        public string Name { get; set; }
        public List<User> Users { get; set; } = new List<User>();

        public Dictionary<string, List<Privilege>> PrivilegesOn { get; private set; } = new Dictionary<string, List<Privilege>>();

        public bool GrantPrivilege(string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Grant this privilege on this table. Return false if there is an error, true otherwise

            //comprobamos si son válidos los datos
            if (table == null || privilege == null)
            {
                return false;
            }

            // si no está la tabla en el dicc la creamos 
            if (!PrivilegesOn.ContainsKey(table))
            {
                PrivilegesOn[table] = new List<Privilege>();
            }

            //comprobamos si la tabla ya tiene ese privilegio
            if (PrivilegesOn[table].Contains(privilege))
            {
                return false;
            }

            //si todo es correcto añadimos ese privilegio a esa tabla
            PrivilegesOn[table].Add(privilege);
            return true;
            
        }

        public bool RevokePrivilege(string table, Privilege privilege)
        {
            //datos no válidos
            if (table == null || privilege == null)
            {
                return false;
            }
            //tabla no existe
            if (!PrivilegesOn.ContainsKey(table))
            {
                return false;
            }

            //tabla no tiene privilegio
            if (!PrivilegesOn[table].Contains(privilege))
            {
                return false;
            }

            //tabla tiene privilegios, quitamos
            PrivilegesOn[table].Remove(privilege);
            return true;
        }
         
        public bool IsGrantedPrivilege(string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Return whether this profile is granted this privilege on this table
            
            return false;
        }
    }
}
