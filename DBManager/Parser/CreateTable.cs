using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;

namespace DbManager
{
 
    public class CreateTable : MiniSqlQuery
    {
        public string Table { get; private set; }
        public List<ColumnDefinition> ColumnsParameters { get; private set; } = new List<ColumnDefinition>();

        public CreateTable(string table, List<ColumnDefinition> columns)
        {
            //TODO DEADLINE 2: Initialize member variables
            Table = table;
            ColumnsParameters = columns;
        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 3: Run the query and return the appropriate message
            //CreateTableSuccess or the last error in the database

            //No existe database
            if (database == null)
            {
                return Constants.Error;
            }
            //El nombre de la tabla ya existe en la base de datos
            Table t = database.TableByName(Table);

            if(t != null)
            {
                return Constants.TableAlreadyExistsError;
            }
            //No hay columnas en la tabla
            int ncol = t.NumColumns();
            
            if(ncol == 0)
            {
                return Constants.DatabaseCreatedWithoutColumnsError;
            }
            //Tabla creada con éxito
            if(database.CreateTable(Table,ColumnsParameters))
            {
                return Constants.CreateTableSuccess;
            }
         
            return Constants.Error;   
        }
        
    }
}
