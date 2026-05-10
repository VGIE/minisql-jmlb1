using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using DbManager.Security;
using DbManager;

namespace SecurityParsingTests
{
    public class ProfileTest
    {
        [Fact]
        public void IsGrantedPrivilegeTrueTest()
        {
            Profile profile = new Profile();

            profile.GrantPrivilege("empleado", Privilege.Select);

            bool result = profile.IsGrantedPrivilege("empleado", Privilege.Select);

            Assert.True(result);
        }

        [Fact]
        public void IsGrantedPrivilegeFalseTest()
        {
            Profile profile = new Profile();

            profile.GrantPrivilege("empleado", Privilege.Insert);

            bool result = profile.IsGrantedPrivilege("empleado", Privilege.Select);

            Assert.False(result);
        }

        [Fact]
        public void IsGrantedPrivilegeTableNotExistTest()
        {
            Profile profile = new Profile();

            bool result = profile.IsGrantedPrivilege("empleado", Privilege.Select);

            Assert.False(result);
        }

        [Fact]
        public void IsGrantedPrivilegeNonExistentTableTest()
        {
            Profile profile = new Profile();

            bool result = profile.IsGrantedPrivilege("empleado", Privilege.Select);

            Assert.False(result);
        }


        [Fact]
        public void testGrantPrivilege()
        {
            Profile profile = new Profile();
            profile.Name = "Editor";

            

            //dando el privilegio de seleccionar debería dar true el resultado
            bool resultado = profile.GrantPrivilege("Users", Privilege.Select);
            //comprobamos que da true
            Assert.True(resultado);

            

            //comprobamos que esta esa tabla con privilegios
            Assert.Contains("Users", profile.PrivilegesOn.Keys);
            //comprobamos que la tabla users contiene ese privilegio 
            Assert.Contains(Privilege.Select, profile.PrivilegesOn["Users"]);

            //casos incorrectos
            
            bool resultado2 = profile.GrantPrivilege("", Privilege.Select);
            Assert.False(resultado2);

           

            

        }
    }
}
