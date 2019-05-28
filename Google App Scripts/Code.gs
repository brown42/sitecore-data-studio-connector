function getConfig(request) {
  var config = {
    configParams: [
      {
        name: 'endpoint',
        displayName: 'Endpoint',
        helpText: 'The URL of the Data Studio Connector API',
        placeholder: 'https://<your site>/-/api/datastudio'
      },
      
      {
        name: 'apiKey',
        displayName: 'API Key'
      },
      
      {
        name: 'dataSetId',
        displayName: 'Data Set ID',
        helpText: 'Data Set Item ID',
        placeholder: '{3EF1903E-F5F5-4361-A526-48CE0987AABD}'
      }
    ],
    
    dateRangeRequired: true
  };
  
  return config;
}

function throwConnectorError(message, userSafe) {
  userSafe = (typeof userSafe !== 'undefined' &&
              typeof userSafe === 'boolean') ?  userSafe : false;
  if (userSafe) {
    message = 'DS_USER:' + message;
  }
  
  throw new Error(message);
}

function getSchema(request) {
  validateConfig(request.configParams);
  
  try {
    var url = _getRequestUrl(request, 'schema');
    var response = UrlFetchApp.fetch(url)
    var json = JSON.parse(response.getContentText());
    return json;
  } catch (e) {
    throwConnectorError('Error loading schema from Sitecore Connector. See server logs for details.', true);
  }
}

function getData(request) {
  validateConfig(request.configParams);
  
  try {
    var url = _getRequestUrl(request, 'data');
    var response = UrlFetchApp.fetch(url);
    var json = JSON.parse(response.getContentText());  
    return json;
  } catch (e) {
    throwConnectorError('Error loading data from Sitecore Connector. See server logs for details.', true);
  }
}

function getAuthType() {
  return { "type": "NONE" };
}
      
function isValueSet(value) {
  return value !== undefined && value !== null && typeof(value) === 'string' && value.length > 0;    
}

function validateProperty(config, propertyName, errors, message) {
  if (!isValueSet(config[propertyName])) {
    errors.push(message);
  }
}

function validateConfig(config) {
  var errors = [];
  
  try {
  validateProperty(config, 'endpoint', errors, 'Endpoint URL missing.');
  validateProperty(config, 'apiKey', errors, 'API Key is missing.');
  validateProperty(config, 'dataSetId', errors, 'Data Set ID is missing.');
  } catch (e) {
    throwConnectorError('Error validating config:\n' + e, true);
  }

  if (errors.length > 0) {
    throwConnectorError('Connector is not configured correctly. Please correct the following:\n' + errors.join('\n'), true);
  }
}

function _getRequestUrl(request, type) {
  var endpoint = request.configParams['endpoint'];
  var encodedType = encodeURIComponent(type);
  var encodedApikey = encodeURIComponent(request.configParams['apiKey']);
  var encodedDataSetId = encodeURIComponent(request.configParams['dataSetId']);
  var urlFormatString = '%s/%s?key=%s&dataSetId=%s';

  var url = Utilities.formatString(urlFormatString, endpoint, encodedType, encodedApikey, encodedDataSetId);
  
  if (request.dateRange) {
    var dateRange = request.dateRange;
    url += Utilities.formatString('&startDate=%s&endDate=%s', dateRange.startDate, dateRange.endDate);
  }
  
  if (request.fields) {
    var fields = '';
    
    for (var i = 0; i < request.fields.length; i++) {
     fields += encodeURIComponent(request.fields[i].name);
      
      if (i < request.fields.length - 1) {
       fields += ','; 
      }
    }
    
    url += '&fields=' + fields;
  }
  
  return url;
}