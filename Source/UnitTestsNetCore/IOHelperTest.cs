﻿namespace ZetaLongPaths.UnitTests
{
    using Native;
    using NUnit.Framework;
    using System;
    using System.IO;
    using FileAccess = Native.FileAccess;
    using FileShare = Native.FileShare;

    [TestFixture]
    public class IOHelperTest
    {
        [Test]
        public void TestFolderVsFile()
        {
            Assert.IsTrue(ZlpIOHelper.FileExists(@"c:\Windows\notepad.exe"));
            Assert.IsFalse(ZlpIOHelper.FileExists(@"c:\dslfsdjklfhsd\kjsaklfjd.exe"));
            Assert.IsFalse(ZlpIOHelper.FileExists(@"c:\Windows"));
            Assert.IsFalse(ZlpIOHelper.FileExists(@"c:\Windows\"));

            Assert.IsFalse(ZlpIOHelper.DirectoryExists(@"c:\Windows\notepad.exe"));
            Assert.IsTrue(ZlpIOHelper.DirectoryExists(@"c:\Windows"));
            Assert.IsTrue(ZlpIOHelper.DirectoryExists(@"c:\Windows\"));
            Assert.IsFalse(ZlpIOHelper.DirectoryExists(@"c:\fkjhskfsdhfjkhsdjkfhsdkjfh"));
            Assert.IsFalse(ZlpIOHelper.DirectoryExists(@"c:\fkjhskfsdhfjkhsdjkfhsdkjfh\"));
        }

        [Test]
        public void TestGeneral()
        {
            var tempFolder = Environment.ExpandEnvironmentVariables("%temp%");
            Assert.True(ZlpIOHelper.DirectoryExists(tempFolder));

            var tempPath = ZlpPathHelper.Combine(tempFolder, "ZlpTest");

            try
            {
                ZlpIOHelper.CreateDirectory(tempPath);
                Assert.IsTrue(ZlpIOHelper.DirectoryExists(tempPath));

                var filePath = ZlpPathHelper.Combine(tempPath, "text.zlp");
                using (var fileHandle = ZlpIOHelper.CreateFileHandle(
                    filePath,
                    CreationDisposition.CreateAlways,
                    FileAccess.GenericWrite | FileAccess.GenericRead,
                    FileShare.None))
                using (var textStream = new StreamWriter(new FileStream(fileHandle, System.IO.FileAccess.Write)))
                {
                    textStream.WriteLine("Zeta Long Paths Extended testing...");
                    textStream.Flush();
                    //textStream.Close();
                    //fileHandle.Close();
                }

                Assert.IsTrue(ZlpIOHelper.FileExists(filePath));


                var m = ZlpIOHelper.GetFileLength(filePath);
                Assert.IsTrue(m > 0);
                Assert.IsTrue(m == new FileInfo(filePath).Length);


                Assert.IsTrue(ZlpIOHelper.FileExists(@"c:\Windows\notepad.exe"));
                Assert.IsFalse(ZlpIOHelper.FileExists(@"c:\dslfsdjklfhsd\kjsaklfjd.exe"));
                Assert.IsFalse(ZlpIOHelper.FileExists(@"c:\ablage"));

                Assert.IsFalse(ZlpIOHelper.DirectoryExists(@"c:\Windows\notepad.exe"));
                Assert.IsTrue(ZlpIOHelper.DirectoryExists(@"c:\Windows"));
                Assert.IsTrue(ZlpIOHelper.DirectoryExists(@"c:\Windows\"));
                Assert.IsFalse(ZlpIOHelper.DirectoryExists(@"c:\fkjhskfsdhfjkhsdjkfhsdkjfh"));
                Assert.IsFalse(ZlpIOHelper.DirectoryExists(@"c:\fkjhskfsdhfjkhsdjkfhsdkjfh\"));

                // --

                Assert.DoesNotThrow(() => ZlpIOHelper.SetFileLastWriteTime(filePath, new DateTime(1986, 1, 1)));
                Assert.DoesNotThrow(() => ZlpIOHelper.SetFileLastAccessTime(filePath, new DateTime(1987, 1, 1)));
                Assert.DoesNotThrow(() => ZlpIOHelper.SetFileCreationTime(filePath, new DateTime(1988, 1, 1)));

                Assert.DoesNotThrow(() => ZlpIOHelper.SetFileLastWriteTime(tempPath, new DateTime(1986, 1, 1)));
                Assert.DoesNotThrow(() => ZlpIOHelper.SetFileLastAccessTime(tempPath, new DateTime(1987, 1, 1)));
                Assert.DoesNotThrow(() => ZlpIOHelper.SetFileCreationTime(tempPath, new DateTime(1988, 1, 1)));

                var anotherFile = ZlpPathHelper.Combine(tempPath, "test2.zpl");
                ZlpIOHelper.WriteAllText(anotherFile, @"äöü.");
                Assert.IsTrue(ZlpIOHelper.FileExists(anotherFile));

                var time = ZlpIOHelper.GetFileLastWriteTime(filePath);
                Assert.Greater(time, DateTime.MinValue);

                var owner = ZlpIOHelper.GetFileOwner(@"c:\Windows\notepad.exe");
                Assert.IsNotEmpty(owner ?? string.Empty);

                var l = ZlpIOHelper.GetFileLength(anotherFile);
                Assert.IsTrue(l > 0);
            }
            finally
            {
                ZlpIOHelper.DeleteDirectory(tempPath, true);
            }
        }

        [Test]
        public void TestAttributes()
        {
            var tempFolder = Environment.ExpandEnvironmentVariables("%temp%");
            Assert.True(ZlpIOHelper.DirectoryExists(tempFolder));

            var tempPath = ZlpPathHelper.Combine(tempFolder, "ZlpTest");

            try
            {
                ZlpIOHelper.CreateDirectory(tempPath);
                Assert.IsTrue(ZlpIOHelper.DirectoryExists(tempPath));

                var filePath = ZlpPathHelper.Combine(tempPath, "text.attributes.tests");
                using (var fileHandle = ZlpIOHelper.CreateFileHandle(
                    filePath,
                    CreationDisposition.CreateAlways,
                    FileAccess.GenericWrite | FileAccess.GenericRead,
                    FileShare.None))
                using (var textStream = new StreamWriter(new FileStream(fileHandle, System.IO.FileAccess.Write)))
                {
                    textStream.WriteLine("Zeta Long Attribute Extended testing...");
                    textStream.Flush();
                    //textStream.Close();
                    //fileHandle.Close();
                }

                Assert.IsTrue(ZlpIOHelper.FileExists(filePath));

                // --

                var now = DateTime.Now;

                Assert.DoesNotThrow(delegate { ZlpIOHelper.SetFileLastAccessTime(filePath, now); });
                Assert.DoesNotThrow(delegate { ZlpIOHelper.SetFileLastWriteTime(filePath, now); });
                Assert.DoesNotThrow(delegate { ZlpIOHelper.SetFileCreationTime(filePath, now); });

                Assert.AreEqual(ZlpIOHelper.GetFileLastAccessTime(filePath), now);
                Assert.AreEqual(ZlpIOHelper.GetFileLastWriteTime(filePath), now);
                Assert.AreEqual(ZlpIOHelper.GetFileCreationTime(filePath), now);
            }
            finally
            {
                ZlpIOHelper.DeleteDirectory(tempPath, true);
            }
        }

        [Test]
        public void TestCodePlex()
        {
            // http://zetalongpaths.codeplex.com/discussions/396147

            const string directoryPath =
                @"c:\1234567890123456789012345678901234567890";
            const string filePath =
                @"c:\1234567890123456789012345678901234567890\1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345.jpg";

            Assert.DoesNotThrow(() => ZlpIOHelper.CreateDirectory(directoryPath));
            Assert.DoesNotThrow(() => ZlpIOHelper.WriteAllText(filePath, @"test"));
            Assert.DoesNotThrow(() => ZlpIOHelper.DeleteFile(filePath));
            Assert.DoesNotThrow(() => ZlpIOHelper.DeleteDirectory(directoryPath, true));
        }

        [Test]
        public void TestGitHub()
        {
            var file = new ZlpFileInfo(@"C:\Ablage\test.txt");
            file.Directory.CheckCreate();
            file.WriteAllText(@"Ein Test.");

            Assert.DoesNotThrow(() => file.MoveTo(@"C:\Ablage\test2.txt", true));

            file.WriteAllText(@"Ein Test.");
            new DirectoryInfo(@"D:\Ablage").Create();
            Assert.DoesNotThrow(() => file.MoveTo(@"D:\Ablage\test3.txt", true));
        }
    }
}