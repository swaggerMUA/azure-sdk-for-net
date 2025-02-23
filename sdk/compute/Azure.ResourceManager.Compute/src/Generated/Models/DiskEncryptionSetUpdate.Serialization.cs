// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace Azure.ResourceManager.Compute.Models
{
    public partial class DiskEncryptionSetUpdate : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsCollectionDefined(Tags))
            {
                writer.WritePropertyName("tags");
                writer.WriteStartObject();
                foreach (var item in Tags)
                {
                    writer.WritePropertyName(item.Key);
                    writer.WriteStringValue(item.Value);
                }
                writer.WriteEndObject();
            }
            if (Optional.IsDefined(Identity))
            {
                writer.WritePropertyName("identity");
                writer.WriteObjectValue(Identity);
            }
            writer.WritePropertyName("properties");
            writer.WriteStartObject();
            if (Optional.IsDefined(EncryptionType))
            {
                writer.WritePropertyName("encryptionType");
                writer.WriteStringValue(EncryptionType.Value.ToString());
            }
            if (Optional.IsDefined(ActiveKey))
            {
                writer.WritePropertyName("activeKey");
                writer.WriteObjectValue(ActiveKey);
            }
            if (Optional.IsDefined(RotationToLatestKeyVersionEnabled))
            {
                writer.WritePropertyName("rotationToLatestKeyVersionEnabled");
                writer.WriteBooleanValue(RotationToLatestKeyVersionEnabled.Value);
            }
            writer.WriteEndObject();
            writer.WriteEndObject();
        }
    }
}
