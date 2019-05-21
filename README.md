# Galaxy-Builders

## Overview

This is a system that helps managers measure productivity of their teams. Employees only have access to their team overview and the tasks assigned to them. They cannot see others teams information and the overall company productivity. Only managers can assign tasks to employees in their teams and can see only the detailed information of their teams. Managers also have acceess to an overview of the whole company’s productivity.

Users are assigned to teams of their choice and pick their role on registration. If the user picks a `Manager` role, the current manager for the team they are assigned to will be replaced and be demoted to `Employee` role.

## Prerequisites

- Visual Studio 2015 or later
- Microsoft SQL Server 2014 or later or Visual Studio LocalDb

## Setting up the database

Firstly, set the connection string in `web.config file` for `DefaultConnection` with your database connection. The database `GalaxyBuilders` will automatically be created and populated with test data when the application runs. The database is created only if it doesn't exist.

## Test data

### Teams

- Development Team
- Marketing Team
- Graphic Design Team
- Hosting and Support Team

You are able to login and test the app with the following user accounts and also welcome to create yours.

### Employees

- employee1@domain.com
- employee2@domain.com

Both employee accounts belong to the same team (Development Team)

### Managers

The list of manager accounts and the teams they are managing
- manager1@domain.com - Development Team
- manager2@domain.com - Marketing Team
- manager3@domain.com - Graphic Design Team
- manager4@domain.com - Hosting and Support Team

All users share the same password: `P@ssw0rd`