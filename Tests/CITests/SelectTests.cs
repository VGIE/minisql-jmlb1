using DbManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SecurityParsingTests
{
    public class SelectTests
    {
        [Fact]

        public void validSelects()
        {
            //Casos válidos 

            Select query = MiniSQLParser.Parse("SELECT nombre FROM tabla") as Select;
            Assert.NotNull(query);
           
        }

        [Fact]
        
        public void invalidSelects()
        {
            //Casos no válidos

            Select query = MiniSQLParser.Parse("SELECT * FROM tabla") as Select;
            Assert.Null(query);
        }
    }
}
