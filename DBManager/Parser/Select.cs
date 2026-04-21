using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager
{
    public class Select : MiniSqlQuery
    {
        public string Table { get; private set; }
        public List<string> Columns { get; private set; }
        public Condition Where { get; private set; }

        public Select(string table, List<string> columns, Condition condition = null)
        {
            //TODO DEADLINE 2: Initialize member variables
            Table = table;
            Columns = columns;
            Where = condition;
        }

        public string Execute(Database database)
        {
            //TODO DEADLINE 3: Run the query and return the table as a string (or the last error in the database)

            //No existe database
            if (database == null)
            {
                return Constants.Error;
            }
            //El nombre de la tabla no existe
            Table t = database.TableByName(Table);

            if (t == null)
            {
                return Constants.TableDoesNotExistError;
            }

            //No existe el nombre de la columna
            foreach (string col in Columns)
            {
                if (t.ColumnByName(col) == null)
                {
                    return Constants.ColumnDoesNotExistError;
                }

            }

            //Columnas en condición incorrectas
            if (Where != null)
            {
                string c = Where.ColumnName;
                if (c == null || t.ColumnByName(c) == null)
                {
                    return Constants.ColumnDoesNotExistError;
                }
            }

            Table table = database.Select(Table, Columns, Where);

            if (table == null)
            {
                return Constants.Error;
            }

            return table.ToString();
        }
    }
}
