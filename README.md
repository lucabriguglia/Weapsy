# Weapsy
Weapsy is an ASP.NET Core CMS based on DDD and CQRS.

Each writing operation is represented by a command handle by a command handler.
After the operation is completed successfully one or more events are published by an internal event publisher.
It's extremely easy to subscribe to those events.
The same way each reading operation is fulfilled by a query handle by a query handler.

Weapsy will be extendable with custom apps.
An app can display content through modules added to dynamic pages or through static pages of the app itself.
The first one included is the Text app used to add html modules.

For any queries please visit my LinkedIn profile where you can find all my contact details: https://www.linkedin.com/in/lucabriguglia/

[![Join the chat at https://gitter.im/weapsy](https://badges.gitter.im/weapsy.svg)](https://gitter.im/weapsy/Lobby?utm_source=share-link&utm_medium=link&utm_campaign=share-link)

![weapsywriting 1](https://cloud.githubusercontent.com/assets/8679253/20868301/4724a222-ba50-11e6-9a5f-61e3122f6141.png)

# Demo
Coming Soon

# Roadmap

https://github.com/weapsy/Weapsy/wiki/Roadmap

# Wiki

https://github.com/weapsy/Weapsy/wiki

# Technology

- C#
- ASP.NET Core
- JavaScript, jQuery
- Entity Framework Core
- MSSQL, MySQL, SQLite, PostgreSQL
- DDD
- CQRS

# Prerequisites

- Visual Studio 2015 Update 3
- .NET Core 1.1 for Visual Studio (https://www.microsoft.com/net/download/core)

# How to run on local

- Open the Weapsy.sln solution in Visual Studio
- Build the solution (default apps will be copied to the "Apps" folder)
- Choose the data provider in the appsettings file (default is MSSQL) and modify the default connection string accordingly.
- Run (F5 or Ctrl+F5)
- Login using these credentials:
  - email: admin@default.com
  - password: Ab1234567!

# How to contribute

Please create issues to report bugs, suggest new functionalities, ask questions or just share your thoughts about the project. I will really appreciate your contribution, thanks.

# History

I started working on the very first version in 2012 and release a stable version in the same year and published CodePlex.
For various reasons I didn't work on it for almost 4 years.

At the beginning of 2016 I decided to revive the project.
Since 4 years in technology is an eternity I started everything from scratch.

I hope to update this section with new chapters in the near future :-)
