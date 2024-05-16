This application uses SQL sever to connect to the database

link for API documentation https://documenter.getpostman.com/view/10206655/2sA3JRXdrz

Instruction to run the application
  - In the sql create a new database with name "teknotest".
  - Replace your server name in appsettings.js for the connection string
  - Run the following commands for database migration to create tables
    -   dotnet ef migrations add CreateTables
    -   dotnet ef database update

before create a job make sure you have data into department and location tables. You can run the following scripts to insert data into the tables

#################### DEPARTMENTS ###############################

USE [teknotest]
GO

INSERT INTO [dbo].[Departments]
           ([Title])
     VALUES
           ("Deeloper")
GO

USE [teknotest]
GO

INSERT INTO [dbo].[Departments]
           ([Title])
     VALUES
           ("Accountent")
GO

#################### END ###############################

#################### LOCATION ##################

USE [teknotest]
GO

INSERT INTO [dbo].[Locations]
           ([Title]
           ,[City]
           ,[State]
           ,[Country]
           ,[Zip])
     VALUES
           ('US Head Office',
           'Baltimore',
           'MD',
		   'United States'
           ,'21202')
GO


USE [teknotest]
GO

INSERT INTO [dbo].[Locations]
           ([Title]
           ,[City]
           ,[State]
           ,[Country]
           ,[Zip])
     VALUES
           ('Indian Office',
           'Margao',
           'Goa',
		   'India'
           ,'403601')
GO

########################## END #######################
