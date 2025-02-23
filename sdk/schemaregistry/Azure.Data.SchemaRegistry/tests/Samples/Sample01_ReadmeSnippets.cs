﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.RegularExpressions;
using Azure.Core.TestFramework;
using Azure.Identity;
using NUnit.Framework;

namespace Azure.Data.SchemaRegistry.Tests.Samples
{
    public class Sample01_ReadmeSnippets : SamplesBase<SchemaRegistryClientTestEnvironment>
    {
#pragma warning disable IDE1006 // Naming Styles
        private SchemaRegistryClient client;
#pragma warning restore IDE1006 // Naming Styles
        private SchemaProperties _schemaProperties;
        private string _content;

        [OneTimeSetUp]
        public void CreateSchemaRegistryClient()
        {
            string endpoint = TestEnvironment.SchemaRegistryEndpoint;

            #region Snippet:SchemaRegistryCreateSchemaRegistryClient
            // Create a new SchemaRegistry client using the default credential from Azure.Identity using environment variables previously set,
            // including AZURE_CLIENT_ID, AZURE_CLIENT_SECRET, and AZURE_TENANT_ID.
            // For more information on Azure.Identity usage, see: https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/identity/Azure.Identity/README.md
            var client = new SchemaRegistryClient(endpoint: endpoint, credential: new DefaultAzureCredential());
            #endregion

            this.client = client;
        }

        [Test]
        [Order(1)]
        public void RegisterSchema()
        {
            string groupName = TestEnvironment.SchemaRegistryGroup;

            #region Snippet:SchemaRegistryRegisterSchema
            string name = "employeeSample";
            SerializationType type = SerializationType.Avro;
            // Example schema's content
            string content = @"
            {
               ""type"" : ""record"",
                ""namespace"" : ""TestSchema"",
                ""name"" : ""Employee"",
                ""fields"" : [
                { ""name"" : ""Name"" , ""type"" : ""string"" },
                { ""name"" : ""Age"", ""type"" : ""int"" }
                ]
            }";

            Response<SchemaProperties> schemaProperties = client.RegisterSchema(groupName, name, content, type);
            #endregion

            Assert.NotNull(schemaProperties);
            _schemaProperties = schemaProperties.Value;
            _content = content;
        }

        [Test]
        [Order(2)]
        public void RetrieveSchemaId()
        {
            string groupName = TestEnvironment.SchemaRegistryGroup;

            #region Snippet:SchemaRegistryRetrieveSchemaId
            string name = "employeeSample";
            SerializationType type = SerializationType.Avro;
            // Example schema's content
            string content = @"
            {
               ""type"" : ""record"",
                ""namespace"" : ""TestSchema"",
                ""name"" : ""Employee"",
                ""fields"" : [
                { ""name"" : ""Name"" , ""type"" : ""string"" },
                { ""name"" : ""Age"", ""type"" : ""int"" }
                ]
            }";

            SchemaProperties schemaProperties = client.GetSchemaProperties(groupName, name, content, type);
            string schemaId = schemaProperties.Id;
            #endregion

            Assert.AreEqual(_schemaProperties.Id, schemaId);
        }

        [Test]
        [Order(3)]
        public void RetrieveSchema()
        {
            var schemaId = _schemaProperties.Id;

            #region Snippet:SchemaRegistryRetrieveSchema
            SchemaRegistrySchema schema = client.GetSchema(schemaId);
            string content = schema.Content;
            #endregion

            Assert.AreEqual(Regex.Replace(_content, @"\s+", string.Empty), content);
        }
    }
}
