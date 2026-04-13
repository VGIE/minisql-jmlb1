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
    }
}
