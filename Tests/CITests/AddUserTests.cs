
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
    public class AddUserTests
    {
        [Fact]
        public void Correct()
        {
            AddUser query = MiniSQLParser.Parse("ADD USER (user,password,profile)") as AddUser;
            Assert.Equal("user", query.Username);

            query = MiniSQLParser.Parse("ADD USER (User,Password,Profile)") as AddUser;
            Assert.Equal("User", query.Username);
        }

        [Fact]
        public void CorrectWithSpaces()
        {
            AddUser query = MiniSQLParser.Parse("ADD     USER      (user,password,profile)") as AddUser;
            Assert.Equal("user", query.Username);

            query = MiniSQLParser.Parse("ADD USER     (OtherUser,password,profile)") as AddUser;
            Assert.Equal("OtherUser", query.Username);
        }

        [Fact]
        public void IncorrectCapitalization()
        {
            AddUser query = MiniSQLParser.Parse("ADD USER (user,password,profile)") as AddUser;
            Assert.NotNull(query);
            
            query = MiniSQLParser.Parse("Add User (user,password,profile)") as AddUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("add user (user,password,profile)") as AddUser;
            Assert.Null(query);
        }

        [Fact]
        public void IncorrectUserWithForbiddenChars()
        {
            AddUser query = MiniSQLParser.Parse("ADD USER (user,password,profile)") as AddUser;
            Assert.NotNull(query);

            query = MiniSQLParser.Parse("ADD USER (user_1,password,profile)") as AddUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("ADD USER (user 1,password,profile)") as AddUser;
            Assert.Null(query);
        }

        [Fact]
        public void IncorrectWithoutProfile()
        {
            AddUser query = MiniSQLParser.Parse("ADD USER (user,password,profile)") as AddUser;
            Assert.NotNull(query);

            query = MiniSQLParser.Parse("ADD USER ()") as AddUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("ADD USER (,,)") as AddUser;
            Assert.Null(query);
        }
        

        /*[Fact]
        public void Execute_NoAdmin()
        {
            Database db = new Database("Admin", "password");

            Profile prof = new Profile();
            prof.Name = "Admin";

            db.SecurityManager.AddProfile(prof);

            AddUser query = new AddUser("nuevo", "password", "Admin");

            string res = query.Execute(db);

            Assert.Equal(Constants.UsersProfileIsNotGrantedRequiredPrivilege, res);
        }*/


        [Fact]
        public void Execute_SiAdminPerfilNoExiste()
        {
            Database db = new Database("Admin", "adminPassword");

            // usuario Admin al perfil Admin
            Profile adminProfile = db.SecurityManager.ProfileByName("Admin");
            adminProfile.Users.Add(new User("Admin", "adminPassword"));

            // Intentar añadir a perfil que no existe
            AddUser query = new AddUser("nuevoUsuario", "password", "PerfilInexistente");
            string result = query.Execute(db);

            Assert.Equal(Constants.SecurityProfileDoesNotExistError, result);
        }


        [Fact]
        public void Execute_SiAdminPerfilExiste()
        {
            Database db = new Database("Admin", "adminPassword");

            // usuario Admin al perfil Admin
            Profile adminProfile = db.SecurityManager.ProfileByName("Admin");
            adminProfile.Users.Add(new User("Admin", "adminPassword"));

            // Crear el perfil 
            Profile profile = new Profile { Name = "Perfil" };
            db.SecurityManager.Profiles.Add(profile);

            AddUser query = new AddUser("nuevoUsuario", "pass123", "Perfil");
            string result = query.Execute(db);

            Assert.Equal(Constants.AddUserSuccess, result);
        }


    }
}

