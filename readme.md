Introduction of Project 'Skylark JPDict'
===================


本文将介绍如何配置修改、编译、调试 Sklark JPDict 所需的环境

----------


Requirement
-------------
#### MVVM Sidekick
本项目使用了由 Waynebaby 开发的 MVVM Sidekick 框架，若要打开并编译项目，请先根据下面的教程安装并配置 MVVM Sidekick 环境。

 - 单击菜单栏上的 工具-扩展和更新
![enter image description here](https://wt-prj.oss.aliyuncs.com/3646b5bf07b5481b97cf72b8476252f8/4771f5bf-5968-4fab-bd5f-fee0460bc4d2.png)
 - 选择联机，搜索关键词“MVVM Sidekick”。选择第一个搜索结果“MVVM-Sidekick VSIX 2015”下载并安装
 ![enter image description here](https://wt-prj.oss.aliyuncs.com/3646b5bf07b5481b97cf72b8476252f8/87b075b8-a19c-4c1e-84c3-7394679d7a3c.png)
 
 - 使用 MVVM Sidekick 提供的模板，创建一个通用 Windows 的项目。
等待其安装依赖项。如果这时系统报错，点击确定忽略。
![enter image description here](https://wt-prj.oss.aliyuncs.com/3646b5bf07b5481b97cf72b8476252f8/afaac7b0-c2ee-401d-8cdf-94d73e4cf60e.jpg)
 -（如果报错了，就继续看下去，如果没有，那就代表安装成功）
右键项目列表中的“引用”-“管理 NuGet 程序包”- 搜索 MVVM-Sidekick 选择最新版安装
![enter image description here](https://wt-prj.oss.aliyuncs.com/3646b5bf07b5481b97cf72b8476252f8/1b4998ad-9d20-4c43-ae50-f0c8092f5186.png)
安装完成后，右键点击项目，选择“生成”，系统可能会报错。
不要管它，删除刚刚创建的项目，到“扩展和更新”处卸载 MVVM-Sidekick 扩展并重新安装即可。

####  SQLite

点击链接下载并安装 SQLite 库的扩展。
http://sqlite.org/2016/sqlite-uap-3100200.vsix


Using TFS
-------------
请在有主要版本编译通过时将源代码签入到 TFS （https://skylark-wsp.visualstudio.com），在嵌入时，请详细填写迁入注释，格式如下：

> 1/22/16 （日期）, Kevin（修改者名字）, Beijing（在哪里编译成功并签入的（地点）)
> 1）Add some features
> 2）。。。
> （具体的新增内容列表，使用英语）

Work division
-------------
项目的分工如下：

 - Tianyu Huang（fjnhhty777@hotmail.com）：UI和UX优化
 - Kevin Gao（gaojunxuanbox@live.com）：ViewModel 层和联网功能的开发
 - Hao Ling（lingmengc@outlook.com）：离线功能（离线查词）开发
 
 还麻烦各位了解一下各个成员的分工，遇到 Bug 请发在 https://skylark-wsp.visualstudio.com/DefaultCollection/Skylark%20JPDict/_backlogs/board/Features 并使用电子邮件告知负责对应功能的成员（CC 我：gaojunxuanbox@live.com）