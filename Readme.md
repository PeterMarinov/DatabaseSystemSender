# Project Info

## Credentials

* Username: admin@sitefinity.com
* Password: password

## Details on log onto the VMs where the replicaiton is set

### Name: SF2
* IP: 192.168.138.27 
* User: win-a32q8i8a6br\administrator
* Pass: Lawrence_A11

### Name: SF1
* IP: 192.168.138.39 
* User: win-95pu1dll9g9\administrator
* Pass: Lawrence_A11


## SQL Code to generate the database table that will hold the Sysmte messages

```sql
/****** 
Use the code below to provide the name of the database you are going to run the code against
USE [Sitefinity]
GO
******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[_systemmessages](
	[target] [nvarchar](50) NULL,
	[data] [varbinary](max) NULL,
	[id] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[_systemmessages] ADD  CONSTRAINT [DF__systemmessages_id]  DEFAULT (newid()) FOR [id]
GO
```
