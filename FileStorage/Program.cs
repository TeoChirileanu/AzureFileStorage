using System;
using System.IO;
using Azure.Storage.Files.Shares;

var connectionString = Environment.GetEnvironmentVariable("AZURE_CONNECTION_STRING");
var shareName = Environment.GetEnvironmentVariable("AZURE_SHARE_NAME");
var share = new ShareClient(connectionString, shareName);

const string localFilePath = @"C:\Users\teodo\foo.txt";
var fileInfo = new FileInfo(localFilePath);

var directoryName = fileInfo.Directory?.Name;
var directoryClient = share.GetDirectoryClient(directoryName);
await directoryClient.CreateIfNotExistsAsync();

var fileName = fileInfo.Name;
var fileClient = directoryClient.GetFileClient(fileName);
fileClient.Create(fileInfo!.Length);
await using var fileStream = fileInfo.OpenRead();
await fileClient.UploadAsync(fileStream);

Console.WriteLine($"Uploaded {localFilePath}");