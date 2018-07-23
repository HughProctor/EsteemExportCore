using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace EntityModel.Test.FileExort
{
    public static class JSON_FileExport
    {
        public const string _fileLocation = @"C:\Users\proctorh\source\repos\ExtractData\ExtractData_";

        public static void WriteFile(string fileExt, object data, int count)
        {
            if (string.IsNullOrEmpty(fileExt) || data == null)
                throw new Exception("JSON_FileExport FileExt and Data object cannot be null or empty");

            using (StreamWriter file = File.CreateText(string.Concat(_fileLocation, fileExt, ".json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                //serialize object directly into file stream

                var jsonExport = new JsonExport()
                {
                    RecordCount = count,
                    Data = data
                };
                serializer.Serialize(file, jsonExport);
            }
        }

    }

    public class JsonExport
    {
        public int RecordCount { get; set; }
        public object Data { get; set; }
    }
}
