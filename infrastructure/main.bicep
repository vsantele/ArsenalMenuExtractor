
param projectName string = 'arsenalExtractor'
param location string = resourceGroup().location

var formRecognizerName = '${projectName}-2208-fr'
var storageAccountName = '${projectName}2208st'
var functionAppName = '${projectName}-2208-func'
var serverFarmName = '${projectName}-2208-plan'
var appInsightsName = '${projectName}-2208-appi'
var cosmosDbName = '${projectName}-2208-cdb'
var cosmosDbDatabaseName = 'arsenal'
var cosmosDbCollectionName = 'menus'

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-05-01' = {
  kind: 'StorageV2'
  name: storageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
}

resource cosmosDbAccount 'Microsoft.DocumentDB/databaseAccounts@2021-03-15' = {
  name: cosmosDbName
  location: location
  kind: 'GlobalDocumentDB'
  properties: {
    consistencyPolicy: {
      defaultConsistencyLevel: 'Eventual'
      maxStalenessPrefix: 1
      maxIntervalInSeconds: 5
    }
    locations: [
      {
        locationName: location
        failoverPriority: 0
      }
    ]
    databaseAccountOfferType: 'Standard'
    capabilities: [
      {
        name: 'EnableServerless'
      }
    ]
  }
}

resource sqlDb 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2022-05-15' = {
  parent: cosmosDbAccount
  name: cosmosDbDatabaseName
  properties: {
    resource: {
      id: cosmosDbDatabaseName
    }
  }
}

resource sqlContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2022-05-15' = {
  parent: sqlDb
  name: cosmosDbCollectionName
  location: location
  properties: {
    resource: {
      id: cosmosDbCollectionName
      indexingPolicy: {
        automatic: true
      }
      partitionKey: {
        kind: 'hash'
        paths: [
          '/id'
        ]
      }
    }
  }
}

resource formRecognizer 'Microsoft.CognitiveServices/accounts@2022-03-01' = {
  name: formRecognizerName
  location: location
  sku: {
    name: 'F0'
  }
  properties: {
    customSubDomainName: toLower(formRecognizerName)
  }
  kind: 'FormRecognizer'
}


resource appInsightsComponents 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
  }
}


resource serverFarms 'Microsoft.Web/serverfarms@2020-12-01' = {
  kind: 'functionapp'
  name: serverFarmName
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
}


resource azureFunction 'Microsoft.Web/sites@2022-03-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp'
  properties: {
    serverFarmId: serverFarms.id
    siteConfig: {
      alwaysOn: false
      http20Enabled: true
      appSettings: [
        {
          name: 'AzureWebJobsDashboard'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};AccountKey=${listKeys(storageAccountName, '2019-06-01').keys[0].value}'
        }
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};AccountKey=${listKeys(storageAccountName, '2019-06-01').keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};AccountKey=${listKeys(storageAccountName, '2019-06-01').keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTSHARE'
          value: toLower(functionAppName)
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: reference(appInsightsComponents.id, '2015-05-01').InstrumentationKey
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet'
        }
        {
          name: 'AzureFormRecognizerEndpoint'
          value: reference(formRecognizer.id, '2021-04-30').endpoint
        }
        {
          name: 'AzureFormRecognizerApiKey'
          value: listKeys(formRecognizer.id, '2021-04-30').key1
        }
      ]
      connectionStrings: [
        {
          name: 'CosmosDBConnectionString'
          type: 'Custom'
          connectionString  : listConnectionStrings(resourceId('Microsoft.DocumentDB/databaseAccounts', cosmosDbName), '2020-04-01').connectionStrings[0].connectionString
        }
      ]
    }
  }
}
