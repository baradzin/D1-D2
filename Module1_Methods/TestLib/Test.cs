using NUnit.Framework;

namespace TestLib
{
    [TestFixture]
    public class Test
    {
        private string _testFolderPath = System.IO.Path.GetTempPath() + "NTest";
        private string[] _testDirs;
        private string[] _testfiles;

        [OneTimeSetUp]
        public void Setup()
        {
            string[] testDirs = {MakePath(_testFolderPath), MakePath(_testFolderPath,"dir1"),
                                 MakePath(_testFolderPath,"dir1", "dir2" ), MakePath(_testFolderPath,"dir1", "dir2", "dir3") };
            string[] testFiles = {MakePath(_testFolderPath, "dir1", "fileLen13.txt"),
                                  MakePath(_testFolderPath, "dir1", "file2.txt"),
                                  MakePath(_testFolderPath, "dir1", "dir2", "file3.txt"),
                                  MakePath(_testFolderPath, "dir1", "dir2", "file4.txt") };
            _testDirs = testDirs;
            _testfiles = testFiles;

            foreach (string dir in testDirs)
            {
                System.IO.Directory.CreateDirectory(dir);
            }

            foreach (string file in testFiles)
            {
                System.IO.FileStream str = System.IO.File.Create(file);
                str.Close();
            }
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            System.IO.Directory.Delete(_testFolderPath, true);
        }
        
        [Test]
        public void TestStartWalkingOnDirectory()
        {              
            FileSystemVisitor.FileSystemVisitor vsf = new FileSystemVisitor.FileSystemVisitor(_testFolderPath);
            vsf.SubscribeEvents(false, false);
            var listFiles = vsf.StartWalkingOnDirectory();

            Assert.IsTrue(listFiles.Count + 1 == _testDirs.Length + _testfiles.Length);
        }

        [Test]
        public void TestFilteredStopAction()
        {
            FileSystemVisitor.FileSystemVisitor.Filtration filtration = (x) => x.Length == 13;
            bool stopSearch = true;
            bool excludeFiles = false;

            FileSystemVisitor.FileSystemVisitor vsf = new FileSystemVisitor.FileSystemVisitor(_testFolderPath, filtration);
            vsf.SubscribeEvents(stopSearch, excludeFiles);
            var listFiles = vsf.StartWalkingOnDirectory();
            Assert.IsTrue(listFiles.Count == 3);
        }

        [Test]
        public void TestFilteredExcludeFilesAction()
        {
            FileSystemVisitor.FileSystemVisitor.Filtration filtration = (x) => x.Length == 4;
            bool stopSearch = false;
            bool excludeFiles = true;

            FileSystemVisitor.FileSystemVisitor vsf = new FileSystemVisitor.FileSystemVisitor(_testFolderPath, filtration);
            vsf.SubscribeEvents(stopSearch, excludeFiles);
            var listFiles = vsf.StartWalkingOnDirectory();
            Assert.IsTrue(listFiles.Count == 4);
        }

        private string MakePath(params string[] tokens)
        {
            string fullpath = "";
            foreach (string token in tokens)
            {
                fullpath = System.IO.Path.Combine(fullpath, token);
            }
            return fullpath;
        }
    }
}

/*var company = new Mock<TClient>();
            company.Setup(x => x.FiscalYearStartMonth)
                .Returns(month);
            company.Setup(x => x.FiscalYearCalculation)
                .Returns(fiscalYearCalculation);

            Mock<IObjectBroker> mockedObjectBroker = new Mock<IObjectBroker>(); ;
            mockedObjectBroker
                .Setup(x => x.GetObjectByPK<TClient>(It.IsAny<object[]>()))
                .Returns(company.Object);

        public IEnumerable<File> fileCollection()
        {
            yield return new File(@"pisos/pisos.cs", false);
            yield return new File(@"pisos/pisos", true);
            yield return new File(@"pisos/chlen.cs", false);
            yield return new File(@"pisos/vagina.cs", false);
        }*/
