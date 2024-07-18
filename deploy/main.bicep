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
