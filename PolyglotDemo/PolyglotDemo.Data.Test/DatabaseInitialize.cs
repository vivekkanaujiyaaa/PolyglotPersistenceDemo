﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolyglotDemo.Data;
using PolyglotDemo.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using ServiceStack.Redis;
using System.Security.Cryptography;

namespace PolyglotDemo.Data.Test
{
    public class DatabaseInitialize
    {
        public static void DatabaseInitializeFactory(string database)
        {
            if (database.Equals("Redis"))
            {
                ExecuteRedis();
            }
            else if(database.Equals("Mongo"))
            {
                ExecuteMongo();
            }
        }

        public static void ExecuteRedis()
        {
            RedisClient dataContext = new RedisClient("localhost");
            //dataContext.RemoveAll(new List<string>() { "./test.txt", "./temp.txt" });
            dataContext.Add<string>("harageth./file.txt", "This is a test file to make sure that things are working correctly.");
            dataContext.Add<string>("harageth./temp.txt", "And now we will store a second file to just see if we can get a couple of files at once.");
        }

        public static void ExecuteMongo()
        {
            const string connectionString = "mongodb://localhost";
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase("Polyglot");
            var directory = database.GetCollection<RootDirectory>("rootdirectory");
            directory.Remove(new QueryDocument());

            byte[] inputBytes = System.Text.Encoding.Unicode.GetBytes("password");//will need to change to being the user input
            SHA256Managed hashstring = new SHA256Managed();
            byte[] dbHash = hashstring.ComputeHash(inputBytes);
            

            directory.Insert(new RootDirectory()
            {
                _id = ObjectId.GenerateNewId().ToString(),
                un = "harageth",
                pw = System.Text.Encoding.UTF8.GetString(dbHash),//need to encrypt this password
                folders = new List<Folder>() { new Folder() { folderName = "firstFolder", files = new List<string>() { "temp1.txt", "file1.txt" } }, new Folder() { folderName = "secondFolder", files = new List<string>() { "temp2.txt", "file2.txt" } }, new Folder() { folderName = "thirdFolder", files = new List<string>() { "temp3.txt", "file3.txt" } } },
                files = new List<string>( ) { "temp.txt", "file.txt" }
                
            });


            directory.Insert(new RootDirectory()
            {
                _id = ObjectId.GenerateNewId().ToString(),
                un = "Guest",
                //folders = new List<Folder>() { new Folder() { folderName = "firstFolder", files = new List<string>() { "temp1.txt", "file1.txt" } }, new Folder() { folderName = "secondFolder", files = new List<string>() { "temp2.txt", "file2.txt" } }, new Folder() { folderName = "thirdFolder", files = new List<string>() { "temp3.txt", "file3.txt" } } },
                //files = new List<string>() { "temp.txt", "file.txt" }

            });
        }
    }
}
