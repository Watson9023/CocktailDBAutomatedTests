using NUnit.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using CocktailDBTests.Apis;
using CocktailDBTests.Logging;

namespace CocktailDBTests.Tests
{
    [TestFixture]
    public class CocktailApiSecurityServiceTest
    {
        private TestLogger _testLogger;
        private ILogger<CocktailApiSecurityService> _logger;

        [SetUp]
        public void Setup()
        {
            _testLogger = new TestLogger(new List<LogEntry>());
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new TestLoggerProvider(_testLogger));
            _logger = loggerFactory.CreateLogger<CocktailApiSecurityService>();
        }

        [Test]
        public void RunSecurityScan_ShouldPass()
        {
            // Arrange
            var cocktailApiSecurityService = new CocktailApiSecurityService(_logger);

            // Act
            try
            {
                cocktailApiSecurityService.RunSecurityScan();

                // Assert
                Console.WriteLine("Security testing completed successfully.");
            }
            catch (Exception ex)
            {
                // If an exception occurs during the security scan, fail the test
                Assert.Fail($"An error occurred: {ex.Message}");
            }
        }

    }
}