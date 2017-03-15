# Judge Sharp
This project Judge Sharp made by Ahmad Bashar Eter to provide an     
trustworthy, easy to use, offline tool to test problem-solving contestants.    

## Introduction and Intuition

You can create a new problem set or open some a previously created problem set and solve it.
The power of this project is shown by the fact that you can lock your problem and the contestant won’t know the real output
unless he solves the problem. This is because the problem output is encrypted by a password that has been set when the problem was created.
When the contestant solves the problem, the password will be shown as well as the true output.
We can describe the way of encryption as follows:
1) The output is hashed using PBKDF2 algorithm and encrypted using Rijndael algorithm with a user specified password (See EncryptionHelper class) then the hashes and 
   the cyphers are stored in the problem set file.
2) The password is also hashed and encrypted then Stord but this time it is encrypted using the true output of the problem.
3) Now when you open a problem you can unlock it either by solving it through providing a code that will produce the true output, or by entering the password.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites
This program uses the GNU G++ Compiler in order to use it correctly you have to install the G++ Compiler and configure its path.
If you are working on windows machine install the MinGW form http://www.mingw.org/ which contains the GNU G++ Compiler, then 
add its bin directory to the PATH environment variable.
Or you can just install mingw to a folder named "MinGW64" and put it at the same folder of the JudgeSharp.exe file which is the working directory of this application
, then the JudgeSharp.exe find it automatically.

If you don't have the g++ compiler on your machine, you can start the application and open a problem set but you can't test your solution.

### Compiling

This project is built on Visual Studio 2015 with .net version 4 for backwards compatibility and test on windows 7 x64 bit with x64 bit mingw compiler.
The fastest way to compile and run this project is to have Visual Studio 2015 then open the solution and hit build.

### Running

After you build the project and install MinGW to MinGW64 folder (or as specified in Prerequisites section) you run the project and test it.

To create new problem set from File menu click on new.

To add a problem to this problem set click on the + button on above the problem set list view or click on the Edit menu and select Add Problem.
To remove a problem from this problem set click on the - button on above the problem set list view or click on the Edit menu and select Delete Problem.
After adding a problem to a problem set you can it this problem by specifying its time limit, memory limit, password, and its name,
as well as its test cases by adding some test cases and its document.
Each test case is a set of input associated with their output.
The document is the document that contain the problem statement. For now, Judge Sharp only support xps document file format (see Limitations for more information).
After finish editing the problem click on the Edit button (the pen icon button at the top of the problem view). This may take few second to complete encrypting the problem data.
After that you can lock the problem which will erase all the unencrypted data from the memory and prepare the problem to be solved.
After that to solve the problem prepare you solution source file then select your compiler (which is for now the g++ compiler) then select your source files (type their name or select them from open file dialog).
Then hit the Solve button, the problem solution will be compiled and tested then you will see a verdict on the color bar of your problem.
The verdict will be one of the following:
1) Accepted:
 which will be green and will tell you that your solution is correct.
2) Wrong Answer:
 which will be red and will tell you that your solution runs correctly but output false answer.
3) Time Limit Excided:
 which will be red and will tell you that your solution runs correctly but Took too much time while running, much more than the problem time limit.
4) Memory: Limit Excided:
 which will be red and will tell you that your solution runs correctly but Took too much memory while running, much more than the problem memory limit.
5) Run Time Error:
 which will be red and will tell you that your solution got an error while its running or throws an unhandled exception like dividing by zero or accessing violated memory.
6) Compile Time Error:
 which will be red and will tell you that your solution can't be compiled correctly. This type of error will also show the compiler output which will contains the error discerption.

After you got Accepted the problem will be unlocked and the password will be shown as well as the test cases also you can edit the problem like if it is a new problem.
If you want to try other solution for a solved problem, you have to lock it in order to solve it again.

## Limitations

### Supported Compilers and Programming Languages

The current last version of Judge Sharp only support g++ compiler which is not distributed with the Judge Sharp project instead you have to install it on your own.
IF you want to support other compiler or programming language you have to create a class for this compiler which inherits the Core.Compiler class which is the base class for every compiler, 
then you have to add it to the compilers list to be shown on the compilers combo box.

### Supported Document File Format

The current last version of Judge Sharp only support xps documents if you want to support other type of document you have to implement it yourself.
The easiest way to do this is to use the Microsoft Xps Printer to print your document to xps file then to load it as xps file.

### Supported Problem Type

The current last version of Judge Sharp only support string equality compeer problem which is the problem that have a single formatted output to the single formatted input,
In other words, if the output of a test case must not differ from the accepted solution output even in the white spaces or in the trebling zeros or anything.
This is because that the test case output is hashed and compered with the solution output by the hash validating algorithm.
If you want to support other type of problems, you can add the functionality to format the solution output before test it by overriding the Core.ProblemSpecification.FormatOutput method.
This will allow you to accept differed output that differ by its format only.
If you want to support multi accepted output problem, you may have to implement the Core.TesterAndGeneratorProblem class which provide an intuition for this problem or implement your one problem type.

## Version and Updates

### Version 1.1.0.0

* Downgrade to .net 4.0 for backward compatibility.  
* Adding Add Batch Test Cases Files feature.  
* Adding Lock feature for to problem set when it has at least one locked problem.  
* Allowing the application to launch without the g++ compiler if the g++ compiler failed to initialize.  
* Allowing the application to automatically detect the g++ compiler if it is in the working directory inside the MinGW64/bin folder.  
* Recalibration the time and memory constants of the dummy program which is used to calculate the relative resource usage.  
* Allowing any type of files to be opened as a test case file.  
* Fixing bug when testing a file multiple times on low performance machine.  
* Fixing bug when opening a problem set two times.  
* Adding and improving icons and images.  

### Version 1.0.0.0

First Lunch of the project.

## Authors

[Ahmad Bashar Eter](https://github.com/BasharKernel)  
Any answer asks me on my email below.  
Feel free to fork this project and improve it!  
[Judge Sharp on GitHub](https://github.com/BasharKernel/JudgeSharp)  
Thanks to M.Besher Massri for testing the project.  

## License

This program is free software: you can redistribute it and/or modify          
it under the terms of the GNU General Public License version 3.               
This program is distributed in the hope that it will be useful, but WITHOUT   
ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details 
GNU General Public: http://www.gnu.org/licenses.                              
For usage not under GPL please request my approval for commercial license.    
Copyright(C) 2017 Ahmad Bashar Eter.                                          
KernelGD@Hotmail.com                                                          