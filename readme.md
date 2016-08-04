Introduction of Project "Skylark JPDict"
===================


This document is about how to configure the enviornment in order to compile this project.

----------


Requirement
-------------
#### MVVM Sidekick
This project uses MVVM Sidekick as its MVVM framework. To install MVVM Sidekick, please follow the instructions below.

 - Tools - Extensions & Updates
 - 
![enter image description here](https://wt-prj.oss.aliyuncs.com/3646b5bf07b5481b97cf72b8476252f8/4771f5bf-5968-4fab-bd5f-fee0460bc4d2.png)
 - Select "Online". Search for keyword "MVVM Sidekick". Select the first result named "MVVM-Sidekick VSIX 2015" and install it.
 - 
 ![enter image description here](https://wt-prj.oss.aliyuncs.com/3646b5bf07b5481b97cf72b8476252f8/87b075b8-a19c-4c1e-84c3-7394679d7a3c.png)
 
 - Create a blank Universal Windows project using the template given by MVVM-Sidekick
Wait for the framework to install its dependencies. If there is an error, just simply ignore it.

![enter image description here](https://wt-prj.oss.aliyuncs.com/3646b5bf07b5481b97cf72b8476252f8/afaac7b0-c2ee-401d-8cdf-94d73e4cf60e.jpg)

 - The following content is only for those who get errors when creating the first MVVM-Sidekick project.
 
Right click "Reference"-"Manage NuGet packages"- search for "MVVM-Sidekick" and install the latest version.

![enter image description here](https://wt-prj.oss.aliyuncs.com/3646b5bf07b5481b97cf72b8476252f8/1b4998ad-9d20-4c43-ae50-f0c8092f5186.png)

Right click the project and select "Build" after finishing the installation of MVVM-Sidekick"s package. Ignore the error message. Just delete the project you created just now and reinstall the MVVM-Sidekick extension from the Extensions page.

####  SQLite

Click the link below to download and install SQLite extension.
http://sqlite.org/2016/sqlite-uap-3100200.vsix


Using Git
-------------
Please commit your changes to git repo. Attention, you should only commit your changes if you affirm that your version can pass the compiling process.
Notes are required. You should clarify the changes you have made to the codes in the commit notes. If there're several changes, please seperate them using semi-colons or use numbers to label them.


Work division
-------------
If there is a bug. Be free to email me or post an issue here.
