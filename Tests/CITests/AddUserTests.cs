
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
        

        [Fact]
        public void Execute_NoAdmin()
        {
            Database db = new Database("admin", "password");

            Profile prof = new Profile();
            prof.Name = "admin";

            db.SecurityManager.AddProfile(prof);

            AddUser query = new AddUser("nuevo", "password", "admin");

            string res = query.Execute(db);

            Assert.Equal(Constants.UsersProfileIsNotGrantedRequiredPrivilege, res);
        }


        [Fact]
        public void Execute_SiAdminPerfilNoExiste()
        {
            
            Database db = new Database("admin", "adminPassword");

            // Creamos perfil administrador para que puede añadir
            Profile adminProfile = new Profile();
            adminProfile.Name = "Admin";
                       
            User adminUser = new User("admin", "adminPassword");
            adminProfile.Users.Add(adminUser);

            db.SecurityManager.Profiles.Add(adminProfile);  

            //intento de añadir un usuario a un perifl que no existe
            AddUser query = new AddUser("nuevoUsuario", "password", "PerfilInexistente");

            
            string res = query.Execute(db);

            Assert.Equal(Constants.SecurityProfileDoesNotExistError, res);
        }


        [Fact]
        public void Execute_SiAdminPerfilExiste()
        {
            
            Database db = new Database("admin", "adminPassword");

            Profile adminProfile = new Profile();
            adminProfile.Name = "Admin";

            User adminUser = new User("admin", "adminPassword");
            adminProfile.Users.Add(adminUser);

            db.SecurityManager.Profiles.Add(adminProfile);  

            Profile prof = new Profile();
            prof.Name = "Perfil";
            db.SecurityManager.Profiles.Add(prof);  

            AddUser query = new AddUser("nuevoUsuario", "pass123", "Perfil");

            string result = query.Execute(db);

            Assert.Equal(Constants.AddUserSuccess, result);
            
        }
                     

    }
}

