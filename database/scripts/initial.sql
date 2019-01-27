﻿IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190126231033_Initial')
BEGIN
    CREATE TABLE [Proxies] (
        [Id] uniqueidentifier NOT NULL,
        [Ip] nvarchar(max) NULL,
        [Port] int NOT NULL,
        [Protocol] varchar(100) NOT NULL,
        [Country] nvarchar(max) NULL,
        CONSTRAINT [PK_Proxies] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190126231033_Initial')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190126231033_Initial', N'2.2.1-servicing-10028');
END;

GO

