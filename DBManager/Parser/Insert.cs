using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager
{
    public class Insert: MiniSqlQuery
    {
        public string Table { get; private set; }
        public List<string> Values { get; private set; }
        public Insert(string table, List<string> values)
        {
            //TODO DEADLINE 2: Initialize member variables
            Table = table;
            Values = values;
        }

        public string Execute(Database database)
        {
            // Verificar que database no es null
            if (database == null)
            {
                return Constants.Error;
            }

            // Verificar que la tabla existe
            Table t = database.TableByName(Table);
            if (t == null)
            {
                return Constants.TableDoesNotExistError;
            }

            // Insertar (deja que database.Insert() valide el resto)
            if (database.Insert(Table, Values))
            {
                return Constants.InsertSuccess;
            }

            return database.LastErrorMessage;
        }
    }
}
