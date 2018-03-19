using System;

namespace FileSystemVisitor
{
    public class Program
    {
        static void Main(string[] args)
        {
            FileSystemVisitor.Filtration filtration = (x) => x.Length == 10;
            bool stopSearch = true;
            bool excludeFiles = true;
            string root = @"..\..\..\";

            FileSystemVisitor fsv = new FileSystemVisitor(root, filtration);
            fsv.SubscribeEvents(stopSearch, excludeFiles);
            fsv.StartWalkingOnDirectory();
            Console.ReadKey();
        }
    }
}
