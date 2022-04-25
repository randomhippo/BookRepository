namespace BookRepository.Tests.EndToEnd
{
    public class ReadWriteFixture : WebServerFixture
    {
        public override string DatabaseName => "TestReadWrite.db";
    }
}
