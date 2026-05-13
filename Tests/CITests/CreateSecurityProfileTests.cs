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
    public class CreateSecurityProfileTests
    {
        [Fact]
        public void Correct()
        {
            CreateSecurityProfile query = MiniSQLParser.Parse("CREATE SECURITY PROFILE profile") as CreateSecurityProfile;
            Assert.Equal("profile", query.ProfileName);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE OtherProfile") as CreateSecurityProfile;
            Assert.Equal("OtherProfile", query.ProfileName);
        }

        [Fact]
        public void CorrectWithSpaces()
        {
            CreateSecurityProfile query = MiniSQLParser.Parse("CREATE     SECURITY PROFILE      profile") as CreateSecurityProfile;
            Assert.Equal("profile", query.ProfileName);

            query = MiniSQLParser.Parse("CREATE SECURITY     PROFILE OtherProfile") as CreateSecurityProfile;
            Assert.Equal("OtherProfile", query.ProfileName);
        }

        [Fact]
        public void IncorrectWithSpaces()
        {
            CreateSecurityProfile query = MiniSQLParser.Parse("CREATE SECURITY PROFILE new profile") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE prof ile") as CreateSecurityProfile;
            Assert.Null(query);
        }

        [Fact]
        public void IncorrectCapitalization()
        {
            CreateSecurityProfile query = MiniSQLParser.Parse("Create SECURITY PROFILE profile") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("create security profile OtherProfile") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE profile") as CreateSecurityProfile;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectProfileWithForbiddenChars()
        {
            CreateSecurityProfile query = MiniSQLParser.Parse("CREATE SECURITY PROFILE pro-file") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE Pro file") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE profile") as CreateSecurityProfile;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectWithoutProfile()
        {
            CreateSecurityProfile query = MiniSQLParser.Parse("CREATE SECURITY PROFILE ") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE profile") as CreateSecurityProfile;
            Assert.NotNull(query);
        }


        [Fact]
        public void IncorrectProfileWithNumbers()
        {
            CreateSecurityProfile query = MiniSQLParser.Parse("CREATE SECURITY PROFILE prof123") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE 123prof") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE p1r2o3f") as CreateSecurityProfile;
            Assert.Null(query);
        }

        [Fact]
        public void IncorrectProfileWithUnserscore()
        {
            CreateSecurityProfile query = MiniSQLParser.Parse("CREATE SECURITY PROFILE profile_one") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE _profile") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE profile_") as CreateSecurityProfile;
            Assert.Null(query);
        }

        /*[Fact]
        public void CheckPermissions()
        {
            //admin si puede crear
            Database dbAdmin = new Database("admin", "adminPassword");

            Profile adminProfile = dbAdmin.SecurityManager.ProfileByName("Admin");
            adminProfile.Users.Add(new User("admin", "adminPassword"));
            
            CreateSecurityProfile query = new CreateSecurityProfile("test");
            string result = query.Execute(dbAdmin);

            Assert.Equal(Constants.CreateSecurityProfileSuccess, result);
            Assert.NotNull(dbAdmin.SecurityManager.ProfileByName("test"));

            //usuario normal no puede crear perfil
            Database dbUser = new Database("jorge", "pass");

            Profile normal = new Profile();
            normal.Name = "Normal";
            normal.Users.Add(new User("jorge", "pass"));
            dbUser.SecurityManager.Profiles.Add(normal);

            result = query.Execute(dbUser);

            Assert.Equal(Constants.UsersProfileIsNotGrantedRequiredPrivilege, result);
            Assert.Null(dbUser.SecurityManager.ProfileByName("test"));
        }*/
    }
}
