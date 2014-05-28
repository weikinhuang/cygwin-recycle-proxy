using System;
using System.IO;
using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;
using Microsoft.VisualBasic.Devices;

namespace recycle
{
    static class Recycle
    {

        enum ExitStatus
        {
            Success = 0,
            UnknownError = 1,
            FileNotFound = 2,
        }

        static Computer computer = new Computer();

        static int Main(string[] args)
        {
            if (args.Length == 0 || args[0] == "/?" || args[0] == "-?")
            {
                Usage();
                return (int)ExitStatus.Success;
            }

            return (int)Trash(args[0]);
        }

        static void Usage()
        {
            string exe = Process.GetCurrentProcess().ProcessName;
            Console.WriteLine(String.Format("Usage: {0} file", exe));
            Console.WriteLine("Author: Wei Kin Huang <wei@closedinterval.com>");
            Console.WriteLine("Report bugs to: https://github.com/weikinhuang/cygwin-recycle-proxy/issues");
        }

        static ExitStatus Trash(string path)
        {
            string filename = Path.GetFullPath(path);

            try
            {
                // We need to use the right method call here
                if (Directory.Exists(filename))
                {
                    computer.FileSystem.DeleteDirectory(filename, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                }
                else
                {
                    computer.FileSystem.DeleteFile(filename, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                }
            }
            catch (FileNotFoundException e)
            {
                Console.Error.WriteLine(String.Format("Error: {0} - {1}", e.GetType(), filename));
                return ExitStatus.FileNotFound;
            }
            catch (Exception e)
            {
                // Worst case, let's fail quietly without throwing tantrum
                Console.Error.WriteLine(String.Format("Error: {0} - {1}", e.GetType(), filename));
                return ExitStatus.UnknownError;
            }

            return ExitStatus.Success;
        }
    }
}
