using DbManager;
using DbManager.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SecurityParsingTests
{
    public class Deadline3_Update
    {
        [Fact]
        public void UpdateExecute()
        {
            Database db = new Database("admin", "password");
            Table tabla = Table.CreateTestTable();
            db.AddTable(tabla);

            List<SetValue> updates = new List<SetValue> { new SetValue("Age", "100") };
            Condition condition = new Condition("Age", ">", "30");

            Update x = new Update(tabla.Name, updates, condition);

            string result = x.Execute(db);

            Assert.Equal(Constants.UpdateSuccess, result);
        }

        [Fact]
        public void UpdateExecute_NotTable()
        {

            Database db = new Database("admin", "password");

            List<SetValue> updates = new List<SetValue> { new SetValue("Age", "100") };
            Condition condition = new Condition("Age", ">", "30");

            Update x = new Update("NonExistentTable", updates, condition);

            string result = x.Execute(db);

            Assert.Equal(Constants.TableDoesNotExistError, result);
        }

        [Fact]
        public void UpdateExecute_NotColumn()
        {
            Database db = new Database("admin", "password");
            Table tabla = Table.CreateTestTable();
            db.AddTable(tabla);

            List<SetValue> updates = new List<SetValue> { new SetValue("NoExistentColumn", "100") };
            Condition condition = new Condition("Age", ">", "30");

            Update x = new Update(tabla.Name, updates, condition);

            string result = x.Execute(db);

            Assert.Equal(Constants.ColumnDoesNotExistError, result);
        }
    }
}
