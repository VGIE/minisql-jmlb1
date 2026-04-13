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
    public class ManagerTests
    {
        [Fact]
        public void TestAdminTrue()
        {
            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "Admin" };
            User user = new User("admin", "pass");
            profile.Users.Add(user);
            manager.Profiles.Add(profile);
            bool result = manager.IsUserAdmin();
            Assert.True(result);
        }

        [Fact]
        public void TestAdminFalse()
        {
            Manager manager = new Manager("user1");
            Profile profile = new Profile { Name = "User" };
            User user = new User("user1", "pass");
            profile.Users.Add(user);
            manager.Profiles.Add(profile);
            bool result = manager.IsUserAdmin();
            Assert.False(result);
        }

        [Fact]
        public void TestProfileByUserExists()
        {
            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "Admin" };
            User user = new User("admin", "pass");
            profile.Users.Add(user);
            manager.Profiles.Add(profile);
            Profile result = manager.ProfileByUser("admin");
            Assert.NotNull(result);
            Assert.Equal("Admin", result.Name);
        }

        [Fact]
        public void TestProfileByUserNull()
        {
            Manager manager = new Manager("admin");
            Profile result = manager.ProfileByUser("nonexistent");
            Assert.Null(result);
        }

        [Fact]
        public void TestPasswordCorrect()
        {
            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "Admin" };
            User user = new User("john", "1234");
            profile.Users.Add(user);
            manager.Profiles.Add(profile);
            bool result = manager.IsPasswordCorrect("john", "1234");
            Assert.True(result);
        }

        [Fact]
        public void TestPasswordWrong()
        {
            Manager manager = new Manager("admin");
            Profile profile = new Profile { Name = "Admin" };
            User user = new User("john", "1234");
            profile.Users.Add(user);
            manager.Profiles.Add(profile);
            bool result = manager.IsPasswordCorrect("john", "wrong");
            Assert.False(result);
        }

        [Fact]
        public void TestPasswordUserNoExists()
        {
            Manager manager = new Manager("admin");
            bool result = manager.IsPasswordCorrect("nonexistent", "1234");
            Assert.False(result);
        }

        [Fact]
        public void TestUserByName()
        {
            Manager manager = new Manager("admin");

            Profile profile1 = new Profile { Name = "Admin" };
            User user1 = new User("john", "1234");
            profile1.Users.Add(user1);
            manager.Profiles.Add(profile1);

            // Usuario existe 
            User result1 = manager.UserByName("john");
            Assert.NotNull(result1);
            Assert.Equal("john", result1.Username);

            // Usuario no existe
            User result3 = manager.UserByName("peter");
            Assert.Null(result3);

            // Username null
            User result4 = manager.UserByName(null);
            Assert.Null(result4);
        }

        [Fact]
        public void TestAddProfile()
        {
            Manager manager = new Manager("admin");

            Profile adminProfile = new Profile { Name = "Admin" };

            User adminUser = new User("admin", "password");
            //añadimos el profil
            adminProfile.Users.Add(adminUser);
            manager.Profiles.Add(adminProfile);

            Profile profile1 = new Profile { Name = "Prueba" };
            manager.AddProfile(profile1);

            //ahora deberia haber 2 el admin y el prueba
            Assert.Equal(2, manager.Profiles.Count);
            //comprobamos tambien el nombre del pefrfil añadido, en la posicion 0
            Assert.Equal("Prueba", manager.Profiles[1].Name);

            //probamos para un nuevo perfil
            Profile prof2 = new Profile { Name = "Prueba2" };
            manager.AddProfile(prof2);
            Assert.Equal(3, manager.Profiles.Count);
            Assert.Equal("Prueba2", manager.Profiles[2].Name);

        }
    }
}
