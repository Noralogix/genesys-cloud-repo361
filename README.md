## Noralogix Genesys Cloud Data Extraction using Repo361 API
This repository provides scripts and tools to extract data from Genesys Cloud by leveraging the Repo361 API. The project simplifies the process of obtaining raw data for analytics, reporting, and monitoring purposes. 
It includes configuration examples, usage guidelines, and setup instructions for exporting data in formats like CSV and Avro.

## Repo361 Genesys Cloud Raw Data
Genesys Cloud historical data to CSV.
- conversation details, attributes, summary, session summary, first agent summary
- users, users primary presence, users routing status
- queues membership
- qualty evaluations, surveys
- wfm management units, wfm management units adherence
- wrapups
- routing languages, skills
- campaigns, campaigns contactslists contacts
   
---

## How to Create API Credentials

To use the Repo361 Raw Data API, follow these steps to create and configure your API credentials:

1. **Ensure Raw Data is Configured**  
   Verify that your organization has configured Raw Data services by Noralogix and that you have the necessary access rights to manage client credentials.

2. **Login to Repo361**  
   Log in to [Repo361](https://apps.repo361.com/) using your Active Directory (AD) credentials.

3. **Generate Client Secret**  
   Navigate to the settings page and generate a client secret. Store this secret securely, as it will only be shown once. If you lose it, you will need to regenerate it.

4. **Run Raw Data Files API**  
   You can now use the generated client credentials to run the Raw Data Files API and request data.

### Important Security Note:
- Repo361 does not store API client secrets. You can only create, update, or delete them.
- Raw data file URLs are accessible for only **1 hour** from the time of generation.

---

### Generate an Authentication Token

To interact with the API, you need to authenticate by generating an OAuth2 token using your client credentials. Send a POST request to the token endpoint:

```http
POST https://api.repo361.com/security/oauth2/token
Content-Type: application/x-www-form-urlencoded
Form Data:
{
    "client_id": "your_client_id",
    "client_secret": "your_client_secret",
    "grant_type": "client_credentials"
}
```

---

### Access Raw Data CSV Files

Once you have the token, you can use it to access raw data files. Make a GET request to retrieve the data for a specific date:

```http
GET https://api.repo361.com/rawdata/files/{date}/csv
Headers:
{
    "Authorization": "Bearer {access_token}",
    "Content-Type": "application/json"
}
```

- **{date}** should be in the format `yyyymmdd`, for example, `20210613` for June 13, 2021.

### Access Raw Data AVRO Files

---

### Example Implementations

- You can try this [PowerShell example script](https://github.com/Noralogix/repo361-genesyscloud/blob/main/Repo361-RawData-API.ps1) using your own client credentials.

- Check out this [Microsoft PowerApps to SharePoint integration](https://www.noralogix.com/genesys/sharepoint-connector) for further details.

- A sample [C# .NET6 application](https://github.com/Noralogix/repo361-genesyscloud/tree/main/csharp-samples) demonstrates how to fetch data and push it to MySQL. Before running the sample app, make sure to review the [MySQL scripts](https://github.com/Noralogix/repo361-genesyscloud/tree/main/mysql) to create the necessary database tables.

- Genesys Cloud [export in avro](https://github.com/Noralogix/repo361-genesyscloud/tree/main/export) 
---