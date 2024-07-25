param location string = resourceGroup().location

param environment string 

var eventHubNamespaceName = 'lip-aanvragen-eh-${environment}'
var eventHubName = 'aanvragen'

module eventHubNamespace 'event-hub-namespace.bicep' = {
    name: 'event-hub-namespace'
    params: {
        location: location
        eventHubNamespaceName: eventHubNamespaceName
    }
}

module eventHub 'event-hub.bicep' = {
    name: 'event-hub'
    params: {
        eventHubNamespaceName: eventHubNamespaceName
        eventHubName: eventHubName
    }
    dependsOn: [
        eventHubNamespace
    ]
}

var storageAccountName = 'lipeventhubstorage'
resource storageAccount 'Microsoft.Storage/storageAccounts@2023-05-01' = {
    name: storageAccountName
    location: location
    sku: {
        name:'Standard_LRS'
    }
   kind: 'StorageV2'
}


resource blobServices 'Microsoft.Storage/storageAccounts/blobServices@2023-05-01' = {
    name: 'default'
    parent: storageAccount
  }
  
 
  var containerName = 'checkpoints'
  resource containers 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-05-01' = {
    name: containerName
    parent: blobServices
    properties: {
      publicAccess: 'None'
      metadata: {}
    }
  }

/*
manually deploy this file using the Azure CLI:

az login --use-device-code
az account set --subscription "FreeSubscription"
az deployment group create `
    --resource-group tryout_rg `
    --template-file main.bicep `
    --parameters `
      environment=dev
*/
