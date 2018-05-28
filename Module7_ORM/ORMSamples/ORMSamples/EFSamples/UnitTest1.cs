using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EFSamples
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var context = new NorthwindDB())
            {
                foreach (var cat in context.Categories)
                    Console.WriteLine("{0} {1} | {2}",
                        cat.Id, cat.Name, cat.Description);
            }
        }
    }
}
