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
    public class DropSecurityProfileTests
    {
        [Fact]
        public void Correct()
        {
            DropSecurityProfile query = MiniSQLParser.Parse("DROP SECURITY PROFILE profile") as DropSecurityProfile;
            Assert.Equal("profile", query.ProfileName);

            query = MiniSQLParser.Parse("DROP SECURITY PROFILE OtherProfile") as DropSecurityProfile;
            Assert.Equal("OtherProfile", query.ProfileName);
        }

        [Fact]
        public void CorrectWithSpaces()
        {
            DropSecurityProfile query = MiniSQLParser.Parse("DROP     SECURITY PROFILE      profile") as DropSecurityProfile;
            Assert.Equal("profile", query.ProfileName);

            query = MiniSQLParser.Parse("DROP SECURITY     PROFILE OtherProfile") as DropSecurityProfile;
            Assert.Equal("OtherProfile", query.ProfileName);
        }

        [Fact]
        public void IncorrectCapitalization()
        {
            DropSecurityProfile query = MiniSQLParser.Parse("Drop SECURITY PROFILE profile") as DropSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("drop security profile OtherProfile") as DropSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DROP SECURITY PROFILE profile") as DropSecurityProfile;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectProfileWithForbiddenChars()
        {
            DropSecurityProfile query = MiniSQLParser.Parse("DROP SECURITY PROFILE pro-file") as DropSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DROP SECURITY PROFILE Pro file") as DropSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DROP SECURITY PROFILE profile") as DropSecurityProfile;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectWithoutProfile()
        {
            DropSecurityProfile query = MiniSQLParser.Parse("DROP SECURITY PROFILE ") as DropSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DROP SECURITY PROFILE") as DropSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DROP SECURITY PROFILE profile") as DropSecurityProfile;
            Assert.NotNull(query);
        }

        [Fact]
        public void Execute_NoAdmin()
        {
            Database db = new Database("usuario", "password");

            //creamos un perfil normal
            Profile prof = new Profile();
            prof.Name = "prueba";

            //añadimos el usuario al perfil
            User normalUser = new User("usuario", "password");
            prof.Users.Add(normalUser);

            db.SecurityManager.Profiles.Add(prof);


            //probar a eliminar el perfil
            DropSecurityProfile query = new DropSecurityProfile("perfilPrueba");

            string res = query.Execute(db);

            //no dejará eliminar porque no es administrador
            Assert.Equal(Constants.UsersProfileIsNotGrantedRequiredPrivilege, res);
        }


        [Fact]
        public void Execute_AdminPerfilNoExiste()
        {
            Database db = new Database("admin", "password");

            //perfil de administrador
            Profile adminProf = new Profile();
            adminProf.Name = "Admin";

            User adminUser = new User("admin", "password");
            adminProf.Users.Add(adminUser);

            db.SecurityManager.Profiles.Add(adminProf);

            DropSecurityProfile query = new DropSecurityProfile("PerfilNoExiste");

            string res = query.Execute(db);

            Assert.Equal(Constants.SecurityProfileDoesNotExistError, res);
        }

        [Fact]
        public void Execute_AdminPerfilExiste()
        {
            Database db = new Database("admin", "password");

            //administrador
            Profile adminProf = new Profile();
            adminProf.Name = "Admin";
            User adminUser = new User("admin", "password");
            adminProf.Users.Add(adminUser);

            db.SecurityManager.Profiles.Add(adminProf);

            //perfil que vamos a elimianr
            Profile prueba = new Profile();
            prueba.Name = "prueba";
            db.SecurityManager.Profiles.Add(prueba);

            DropSecurityProfile query = new DropSecurityProfile("prueba");

            string res = query.Execute(db);

            //como es administrador y existe el perfil lo deberia elimianr
            Assert.Equal(Constants.DropSecurityProfileSuccess, res);
        }
    }
}
