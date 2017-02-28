This project Judge Sharp made by Ahmad Bashar Eter to provide an     
trustworthy, easy to use, offline tool to test problem-solving contestants.    

You can create a new problem set or open some a previously created problem set and solve it.
The power of this project is shown by the fact that you can lock your problem and the the contestant won’t know the real output
unless he solves the problem. This is because the problem output is encrypted by a password that has been set when the problem was created.
When the contestant solves the problem, the password will be shown as well as the true output.
We can describe the way of encryption as follows:
1) The output is hashed using PBKDF2 algorithm and encrypted using Rijndael algorithm with a user specified password (See EncryptionHelper class) then the hashes and 
   the cyphers are stored in the problem set file.
2) The password is also hashed and encrypted then Stord but this time it is encrypted using the true output of the problem.
3) Now when you open a problem you can unlock it either by solving it through providing a code that will produce the true output, or by entering the password.

Any answer asks me on my email below.
Feel free to fork this project and improve it!
GitHub: https://github.com/BasharKernel/JudgeSharp

NOTE: This program uses the GNU G++ Compiler in order to use it correctly you have to install the G++ Compiler and configure its path.
      If you are working on windows machine install the MinGW form http://www.mingw.org/ which contains the GNU G++ Compiler then 
      add its bin directory to the PATH environment variable.

This program is free software: you can redistribute it and/or modify          
it under the terms of the GNU General Public License version 3.               
This program is distributed in the hope that it will be useful, but WITHOUT   
ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details 
GNU General Public: http://www.gnu.org/licenses.                              
For usage not under GPL please request my approval for commercial license.    
Copyright(C) 2017 Ahmad Bashar Eter.                                          
KernelGD@Hotmail.com                                                          
