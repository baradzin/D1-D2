using System;

namespace FileSystemVisitor
{
    public class Program
    {
        static void Main(string[] args)
        {
            FileSystemVisitor.Filtration filtration = (x) => x.Length == 10;

            FileSystemVisitor fsv = new FileSystemVisitor(@"..\..\..\", filtration);
            fsv.SubscribeEvents(false, false, false, true);
            fsv.StartWalkingOnDirectory();
            Console.ReadKey();
        }
    }
}
