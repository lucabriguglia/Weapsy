# Weapsy
Weapsy is an ASP.NET Core CMS based on DDD and CQRS.

For each writing operation, there is a command and a command handler. 
After the operation is completed successfully, one or more events are published by an internal event publisher. 
It's extremely easy to subscribe to those events.

Same way, for each reading operation there is a query and a query handler.

Weapsy will be extendable with custom apps. An app can display content through modules added to dynamic pages or through static pages of the app itself. 
The first one included is the Text app used to add html modules to dynamic pages.

For any queries please visit my LinkedIn profile where you can find all my contact details: https://www.linkedin.com/in/lucabriguglia/

[![Join the chat at https://gitter.im/weapsy](https://badges.gitter.im/weapsy.svg)](https://gitter.im/weapsy/Lobby?utm_source=share-link&utm_medium=link&utm_campaign=share-link)

# Build Status

[![Build status](https://ci.appveyor.com/api/projects/status/ptwkjgk7gwledwh3/branch/master?svg=true)](https://ci.appveyor.com/project/lucabriguglia/weapsy/branch/master)

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

- Visual Studio 2017
- .NET Core 2.0 Preview 1 (https://www.microsoft.com/net/core/preview)

# How to run on local

- Open the Weapsy.sln solution in Visual Studio
- Build the solution (default apps will be copied over to the "Apps" folder)
- Set the data provider of your choice in the appsettings.json file and modify the default connection string accordingly if needed.
- Run (F5 or Ctrl+F5)
- Login using these credentials:
  - email: admin@default.com
  - password: Ab1234567!
- Database and seed data will be created automatically the first time you run the application.

# How to contribute

Please create issues to report bugs, suggest new functionalities, ask questions or just share your thoughts about the project. I will really appreciate your contribution, thanks.
