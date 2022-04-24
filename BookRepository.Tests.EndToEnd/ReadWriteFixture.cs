using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRepository.Tests.EndToEnd
{
    public class ReadWriteFixture : WebServerFixture
    {
        public override string DatabaseName => "TestReadWrite.db";
    }
}
