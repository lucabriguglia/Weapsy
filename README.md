# Weapsy
Weapsy is an ASP.NET Core CMS based on DDD and CQRS.

Each writing operation is represented by a command. After the operation is completed successfully one or more events are published by an internal dispatcher.
It's extremely easy to subscribe to the domain events.

Weapsy will be extendable with custom apps.
An app can display content through modules added to dynamic pages or through static pages of the app itself.
The first one included is the Text app used to add html modules.
Right after the first RTM my plan is to develop a Blog and a Forum app.

# Roadmap

https://github.com/weapsy/Weapsy/wiki/Roadmap

# Wiki

https://github.com/weapsy/Weapsy/wiki

# Technology

- C#
- ASP.NET Core MVC and Web Api
- JavaScript, jQuery, KnockoutJS
- Entity Framework Core
- SQL Server

# Prerequisites

- Visual Studio 2015 Update 3
- .NET Core 1.0 for Visual Studio (https://www.microsoft.com/net/core#windows)
- SQL Server

# How to run on local

- Create a SQL database (weapsy.dev)
- Run install-full.sql script (in scripts folder)
- Open the Weapsy.sln solution in Visual Studio
- Run (F5 or Ctrl+F5). _Note: there could be an internal server error the first time the app runs. In case that happens just refresh the page, I'm working on fixing the issue._
- Login using these credentials:
  - email: admin@default.com
  - password: Ab1234567!

# How to contribute

Please create issues to report bugs, suggest new functionalities, ask questions or just share your thoughts about the project. I will really appreciate your contribution, thanks.

# History

I started working on the very first version in 2012 and release a stable version in the same year and published CodePlex.
For various reasons I didn't work on it for almost 4 years.
At the beginning of 2016 I decided to revive the project.
Since 4 years in technology is an eternity I started everything from scratch again.
I hope to update this history with other chapters in the near future :-)
