using Xunit;
using DbManager;
using DbManager.Security;
using DbManager.Parser;

namespace SecurityParsingTests
{
    public class DeleteUserExecuteTests
    {
        [Fact]
        public void Correct()
        {
            Database db = new Database("Admin", "1234");

            Manager manager = db.SecurityManager;

            Profile adminProfile = new Profile { Name = "Admin" };
            User adminUser = new User("Admin", "1234");
            adminProfile.Users.Add(adminUser);
            manager.Profiles.Add(adminProfile);

            Profile profile = new Profile { Name = "Test" };
            User user = new User("Maria", "1234");
            profile.Users.Add(user);
            manager.Profiles.Add(profile);

            DeleteUser query = new DeleteUser("Maria");
            string result = query.Execute(db);

            Assert.Equal(Constants.DeleteUserSuccess, result);
        }

        /*[Fact]
        public void IncorrectNotAdmin()
        {
            Database db = new Database("notAdmin", "1234");

            Manager manager = db.SecurityManager;

            Profile profile = new Profile { Name = "Test" };
            User user = new User("Maria", "1234");
            profile.Users.Add(user);
            manager.Profiles.Add(profile);

            DeleteUser query = new DeleteUser("Maria");
            string result = query.Execute(db);

            Assert.Equal(Constants.UsersProfileIsNotGrantedRequiredPrivilege, result);
        }*/

        [Fact]
        public void IncorrectUserDoesNotExist()
        {
            Database db = new Database("Admin", "1234");

            Manager manager = db.SecurityManager;

            Profile adminProfile = new Profile { Name = "Admin" };
            User adminUser = new User("Admin", "1234");
            adminProfile.Users.Add(adminUser);
            manager.Profiles.Add(adminProfile);

            Profile profile = new Profile { Name = "Test" };
            manager.Profiles.Add(profile);

            DeleteUser query = new DeleteUser("Maria");
            string result = query.Execute(db);

            Assert.Equal(Constants.UserDoesNotExistError, result);
        }

        [Fact]
        public void IncorrectWithoutProfile()
        {
            Database db = new Database("Admin", "1234");

            Manager manager = db.SecurityManager;

            Profile adminProfile = new Profile { Name = "Admin" };
            User adminUser = new User("Admin", "1234");
            adminProfile.Users.Add(adminUser);
            manager.Profiles.Add(adminProfile);

            DeleteUser query = new DeleteUser("Maria");
            string result = query.Execute(db);

            Assert.Equal(Constants.UserDoesNotExistError, result);
        }
    }
}