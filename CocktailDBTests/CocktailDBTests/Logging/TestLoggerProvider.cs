using Microsoft.Extensions.Logging;

namespace CocktailDBTests.Logging
{
    public class TestLoggerProvider : ILoggerProvider
    {
        private readonly TestLogger _testLogger;

        public TestLoggerProvider(TestLogger testLogger)
        {
            _testLogger = testLogger;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _testLogger;
        }

        public void Dispose()
        {
        }
    }
}