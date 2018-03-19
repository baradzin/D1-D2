using System;

namespace FileSystemVisitor
{
    public class Program
    {
        static void Main(string[] args)
        {
            FileSystemVisitor.Filtration filtration = (x) => x.Length == 10;

            FileSystemVisitor fsv = new FileSystemVisitor(@"..\..\..\", filtration);
            SubscribeEvents(fsv);
            fsv.StartWalkingOnDirectory();
            Console.ReadKey();
        }

        public static void SubscribeEvents(FileSystemVisitor fsv)
        {
            FileSystemVisitorHandler handler = new FileSystemVisitorHandler();
            fsv.ProgressStart += handler.Start_Progress;
            fsv.ProgressFinished += handler.Finished_Progress;
            fsv.FileFinded_ActionRequired += handler.FileFinded_ActionRequired;
            fsv.DirectoryFinded_ActionRequired += handler.DirectoryFinded_ActionRequired;
        }
    }
}
