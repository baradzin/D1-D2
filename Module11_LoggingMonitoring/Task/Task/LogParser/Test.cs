using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace LogParser
{
    [TestFixture]
    class Test
    {
        [Test]
        public void TestMethod()
        {
            var logGenerator = new LogParser(@"E:\Mentoring\D1-D2\Module11_LoggingMonitoring\Task\Task\MvcMusicStore\logs\2018-06-15.log");

            var errors = logGenerator.Error;
            var count = logGenerator.LevelsCount;

            while (!count.atEnd())
            {
                var row = count.getRecord();
                Console.WriteLine($"{row.getValue("Level")} , {row.getValue("LevelsCount")}");
                count.moveNext();
            }

            while (!errors.atEnd())
            {
                var row = errors.getRecord();
                Console.WriteLine($"{row.getValue("Date")} , {row.getValue("Level")}, {row.getValue("Message")}");
                errors.moveNext();
            }
        }
    }
}
