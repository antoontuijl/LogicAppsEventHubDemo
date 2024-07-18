param workflows_la_lip_aanvragen_subsriber_name string = 'la-lip-aanvragen-subsriber'
param connections_eventhubs_externalid string
param connections_outlook_externalid string

resource workflows_la_lip_aanvragen_subsriber_name_resource 'Microsoft.Logic/workflows@2017-07-01' = {
  name: workflows_la_lip_aanvragen_subsriber_name
  location: 'westeurope'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    state: 'Disabled'
    definition: {
      '$schema': 'https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#'
      contentVersion: '1.0.0.0'
      parameters: {
        '$connections': {
          defaultValue: {}
          type: 'Object'
        }
      }
      triggers: {
        When_events_are_available_in_Event_Hub: {
          recurrence: {
            interval: 3
            frequency: 'Minute'
          }
          evaluatedRecurrence: {
            interval: 3
            frequency: 'Minute'
          }
          splitOn: '@triggerBody()'
          type: 'ApiConnection'
          inputs: {
            host: {
              connection: {
                name: '@parameters(\'$connections\')[\'eventhubs\'][\'connectionId\']'
              }
            }
            method: 'get'
            path: '/@{encodeURIComponent(\'aanvragen\')}/events/batch/head'
            queries: {
              contentType: 'application/octet-stream'
              consumerGroupName: '$Default'
              maximumEventsCount: 50
            }
          }
        }
      }
      actions: {
        'Send_an_email_(V2)': {
          runAfter: {}
          type: 'ApiConnection'
          inputs: {
            host: {
              connection: {
                name: '@parameters(\'$connections\')[\'outlook\'][\'connectionId\']'
              }
            }
            method: 'post'
            body: {
              To: 'test@test.com'
              Subject: 'Event'
              Body: '<p>@{base64ToString(triggerBody()?[\'ContentData\'])}</p>'
              Importance: 'Normal'
            }
            path: '/v2/Mail'
          }
        }
      }
      outputs: {}
    }
    parameters: {
      '$connections': {
        value: {
          eventhubs: {
            id: '/subscriptions/xxxxxxxxxxx/providers/Microsoft.Web/locations/westeurope/managedApis/eventhubs'
            connectionId: connections_eventhubs_externalid
            connectionName: 'eventhubs'
            connectionProperties: {
              authentication: {
                type: 'ManagedServiceIdentity'
              }
            }
          }
          outlook: {
            id: '/subscriptions/xxxxxx/providers/Microsoft.Web/locations/westeurope/managedApis/outlook'
            connectionId: connections_outlook_externalid
            connectionName: 'outlook'
          }
        }
      }
    }
  }
}
