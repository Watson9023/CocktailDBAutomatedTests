
using NUnit.Framework;
using System.Diagnostics;
using System.IO;

namespace CocktailDBTests.Setup

{
    [SetUpFixture]
    public class TestLogSetup
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            string logFilePath = "path-to-your-log-file.log"; // Specify the desired log file path
            TraceListener logFileListener = new TextWriterTraceListener(File.CreateText(logFilePath));
            Trace.Listeners.Add(logFileListener);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Trace.Flush();
            Trace.Close();
        }
    }
}
