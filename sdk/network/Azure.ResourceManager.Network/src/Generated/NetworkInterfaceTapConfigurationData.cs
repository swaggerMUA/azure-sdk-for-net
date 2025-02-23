// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using Azure.ResourceManager.Network.Models;
using Azure.ResourceManager.Resources.Models;

namespace Azure.ResourceManager.Network
{
    /// <summary> A class representing the NetworkInterfaceTapConfiguration data model. </summary>
    public partial class NetworkInterfaceTapConfigurationData : WritableSubResource
    {
        /// <summary> Initializes a new instance of NetworkInterfaceTapConfigurationData. </summary>
        public NetworkInterfaceTapConfigurationData()
        {
        }

        /// <summary> Initializes a new instance of NetworkInterfaceTapConfigurationData. </summary>
        /// <param name="id"> The id. </param>
        /// <param name="name"> The name of the resource that is unique within a resource group. This name can be used to access the resource. </param>
        /// <param name="etag"> A unique read-only string that changes whenever the resource is updated. </param>
        /// <param name="type"> Sub Resource type. </param>
        /// <param name="virtualNetworkTap"> The reference to the Virtual Network Tap resource. </param>
        /// <param name="provisioningState"> The provisioning state of the network interface tap configuration resource. </param>
        internal NetworkInterfaceTapConfigurationData(string id, string name, string etag, string type, VirtualNetworkTapData virtualNetworkTap, ProvisioningState? provisioningState) : base(id)
        {
            Name = name;
            Etag = etag;
            Type = type;
            VirtualNetworkTap = virtualNetworkTap;
            ProvisioningState = provisioningState;
        }

        /// <summary> The name of the resource that is unique within a resource group. This name can be used to access the resource. </summary>
        public string Name { get; set; }
        /// <summary> A unique read-only string that changes whenever the resource is updated. </summary>
        public string Etag { get; }
        /// <summary> Sub Resource type. </summary>
        public string Type { get; }
        /// <summary> The reference to the Virtual Network Tap resource. </summary>
        public VirtualNetworkTapData VirtualNetworkTap { get; set; }
        /// <summary> The provisioning state of the network interface tap configuration resource. </summary>
        public ProvisioningState? ProvisioningState { get; }
    }
}
