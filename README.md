# THIS REPOSITORY IS NO LONGER MAINTAINED

# This project is replaced by [Atles](https://github.com/lucabriguglia/Atles).

# Weapsy

[![Build status](https://ci.appveyor.com/api/projects/status/ptwkjgk7gwledwh3/branch/master?svg=true)](https://ci.appveyor.com/project/lucabriguglia/weapsy/branch/master)

Weapsy is an ASP.NET Core CMS.

# Technology

- C#
- ASP.NET Core
- Entity Framework Core
- MSSQL, MySQL, SQLite, PostgreSQL

# Prerequisites

- Visual Studio 2017
- .NET Core 2.0 (https://www.microsoft.com/net/core)

# How to run on local

- Open the Weapsy.sln solution in Visual Studio
- Build the solution with release configuration (default apps will be copied over to the "Apps" folder)
- Set the data provider of your choice in the appsettings.json file and modify the default connection string accordingly if needed.
- Run (F5 or Ctrl+F5)
- Login using these credentials:
  - email: admin@default.com
  - password: Ab1234567!
- Database and seed data will be created automatically the first time you run the application.
