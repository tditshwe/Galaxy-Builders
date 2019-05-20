# Galaxy-Builders

## Overview

This is a system that helps managers measure productivity of their teams. Employees only have access to their team overview and the tasks assigned to them. They cannot see others teams information and the overall company productivity. Only managers can assign tasks to employees in their teams and can see only the detailed information of their teams. Managers also have acceess to an overview of the whole company’s
productivity.

## Prerequisites

Visual Studio 2015 or later
Microsoft SQL Server 2014 or later or Visual Studio LocalDb

## Setting up the database

Firstly, set the connection string in `web.config file` for DefaultConnection with your database connection. The database `GalaxyBuilders` and all related tables will automatically be created and tables will automatically be populated with initial data when the application is launched.

### Initial data

#### Employee table
- employee1@domain.com
- employee2@domain.com
- employee3@domain.com
- employee4@domain.com