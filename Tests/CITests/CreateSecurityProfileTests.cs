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
        public void CreateSecurityProfileExecute_Admin_CreatesProfile()
        {
            //1.creo una bd con un usuario admin
            Database db = new Database("admin", "adminPassword");

            //2.añado el perfil y meto al usuario "admin" dentro
            Profile adminProfile = new Profile();
            adminProfile.Name = "Admin";
            adminProfile.Users.Add(new User("admin", "adminPassword"));
            db.SecurityManager.Profiles.Add(adminProfile);

            //3. creo el comando CreateSecurityProfile para un nuevo perfil
            CreateSecurityProfile cmd = new CreateSecurityProfile("perfil");

            //4.lo ejecuto
            string resultado = cmd.Execute(db);

            Assert.Equal(Constants.CreateSecurityProfileSuccess, resultado);

            Assert.NotNull(db.SecurityManager.ProfileByName("perfil"));
        }

        [Fact]
        public void CreateSecurityProfileExecute_NoAdmin_ReturnsError()
        {
            //1.dreo una base de datos con un usuario normal sin ser admin
            Database db = new Database("maria", "pass");

            //2.no añado perfil admin, así que IsUserAdmin será false

            Profile normal = new Profile();
            normal.Name = "Normal";
            normal.Users.Add(new User("maria", "pass"));
            db.SecurityManager.Profiles.Add(normal);

            //3.intento crear un perfil
            CreateSecurityProfile cmd = new CreateSecurityProfile("perfil");
            string resultado = cmd.Execute(db);

            //4.debe devolver el error de falta de privilegios
            Assert.Equal(Constants.UsersProfileIsNotGrantedRequiredPrivilege, resultado);

            //5. y el perfil "Invitado" NO se ha creado
            Assert.Null(db.SecurityManager.ProfileByName("perfil"));
        }
    }
}
