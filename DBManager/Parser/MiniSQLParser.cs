using DbManager.Parser;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DbManager
{
    public class MiniSQLParser
    {
        public static MiniSqlQuery Parse(string miniSQLQuery)
        {
            //TODO DEADLINE 2
            const string selectPattern = @"/^SELECT\s+(\*|[a-zA-Z0-9]+(?:,[a-zA-Z0-9]+)*)\sFROM\s([a-zA-Z0-9]+)\sWHERE\s([a-zA-Z0-9]+\s(>|<|=)\s'([-]*[a-zA-Z0-9]+([.]*[a-zA-Z0-9]+)*)')$";
            
            
            const string dropTablePattern = @"^DROP\s+TABLE\s+([a-zA-Z0-9]+)$";
          

            //LEIRE --> #16
            const string insertPattern = @"INSERT\s+INTO\s+(\w+)\s+VALUES\s*\(([^)]+)\)";

            
            //Note: The parsing of CREATE TABLE should accept empty columns "()"
            //And then, an execution error should be given if a CreateTable without columns is executed
            const string createTablePattern = @"CREATE\s+TABLE\s+([a-zA-Z][a-zA-Z0-9]*)\s*\(\s*(.*?)\s*\)";
            
            const string updateTablePattern = null;
            
            const string deletePattern = @"^DELETE\s+FROM\s+([a-zA-Z0-9]+)\s+WHERE\s+([a-zA-Z0-9]+)(<|=|>)'([^']*)'$"; 
            

            //TODO DEADLINE 4
            const string createSecurityProfilePattern = null;
            
            const string dropSecurityProfilePattern = null;
            
            const string grantPattern = null;
            
            const string revokePattern = null;
            
            const string addUserPattern = null;
            
            const string deleteUserPattern = null;


            //TODO DEADLINE 2
            //Parse query using the regular expressions above one by one. If there is a match, create an instance of the query with the parsed parameters
            //For example, if the query is a "SELECT ...", there should be a match with selectPattern. We would create and return an instance of Select
            //initialized with the table name, the columns, and (possibly) an instance of Condition.
            //If there is no match, it means there is a syntax error. We will return null.

            //Select
            Match matchSelect = Regex.Match(miniSQLQuery, selectPattern);
            if (matchSelect.Success)
            {
                //Colmunas del select
                string columnas = matchSelect.Groups[1].Value;
                string table = matchSelect.Groups[2].Value;
                //Condicion del select
                string condicion = matchSelect.Groups[3].Value;
                string columna = condicion.Split(' ')[0];
                // Simbolo y comparando por ejemplo:
                // simbolo = "<"
                //comparando = "28"
                string simbolo = matchSelect.Groups[4].Value;
                string comparando = matchSelect.Groups[5].Value;
                comparando = comparando.Trim('\'');

                List<string> columns = new List<string>();
                string[] cols = columnas.Split(',');
                //Nunca debería entrar por aquí ya que la regex no lo permite.
                if (cols.Length == 0)
                {
                    return null;
                }

                foreach (string col in cols)
                {
                    columns.Add(col.Trim());
                }

                Condition condition = new Condition(columna, simbolo, comparando);
                Select select = new Select(table, columns, condition);

                return select;
            }

            //Insert
            Match matchInsert = Regex.Match(miniSQLQuery, insertPattern);
            if (matchInsert.Success)
            {
                string table = matchInsert.Groups[1].Value;
                string valores = matchInsert.Groups[2].Value;

                string newPattern = @"'[^']+'";
                MatchCollection matchCollectionComillas = Regex.Matches(valores, newPattern);

                //Contar cuantas comas hay
                int commaCount = valores.Split(',').Length - 1;

                //Ver si hay los mismos valores que comas +1
                if (matchCollectionComillas.Count == 0 || matchCollectionComillas.Count != commaCount + 1)
                {
                    return null;
                }

                List<string> values = new List<string>();

                foreach (Match match in matchCollectionComillas)
                {
                    string valor = match.Value;
                    valor = valor.Trim('\'');
                    values.Add(valor);
                }

                return new Insert(table, values);
            }

           
            
            //create table
            Match createMatch = Regex.Match(miniSQLQuery,createTablePattern);

            //CREATE TABLE válido
            if (createMatch.Success)
            {
                string nombreTabla = createMatch.Groups[1].Value;
                string columnas = createMatch.Groups[2].Value;

                //lista para guardar las columnas
                List<ColumnDefinition> crearColumnas = new List<ColumnDefinition>();

                if (!string.IsNullOrWhiteSpace(columnas))
                {
                    //separamos las distintas columnas
                    string[] columnaSep = columnas.Split(',');

                    //para cada columnas
                    foreach (string parte in columnaSep)
                    {
                        //separar nombre y tipo
                        string[] columna = Regex.Split(parte, @"\s+");
                        if (columna.Length == 2)
                        {
                            string name = columna[0];
                            string tipo = columna[1];

                            ColumnDefinition.DataType columnType;
                            if (tipo.Equals("INT"))
                            {
                                columnType = ColumnDefinition.DataType.Int;
                            }
                            else if (tipo.Equals("TEXT"))
                            {
                                columnType = ColumnDefinition.DataType.String;
                            }
                            else if (tipo.Equals("DOUBLE"))
                            {
                                columnType = ColumnDefinition.DataType.Double;
                            }
                            else
                            {
                                return null;
                            }

                            crearColumnas.Add(new ColumnDefinition(columnType, name));
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                    return new CreateTable(nombreTabla, crearColumnas);
            }
            //TODO DEADLINE 4
            //Do the same for the security queries (CREATE SECURITY PROFILE, ...)

            return null;
           
        }

        static List<string> CommaSeparatedNames(string text)
        {
            string[] textParts = text.Split(",", System.StringSplitOptions.RemoveEmptyEntries);
            List<string> commaSeparator = new List<string>();
            for(int i=0; i < textParts.Length; i++)
            {
                commaSeparator.Add(textParts[i]);
            }
            return commaSeparator;
        }
        
    }
}
