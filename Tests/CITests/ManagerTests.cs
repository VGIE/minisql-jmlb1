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
    }
}
