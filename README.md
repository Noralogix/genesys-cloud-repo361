## Repo361 Genesys Cloud Raw Data 
1. Make sure that your org have configured Raw Data by Noralogix and you have access rights to control client credentials.
2. Login to [Repo361](https://apps.repo361.com/) with AD credentials.
3. Go to settings page and generate client secret. Store it in a safe place. Next time secret will be hidden. 
4. Run Raw Data files API based on client credentials.

> **For security purposes:** Repo361 is not storing api client secrets, it just gives possibilities to create, update or delete secrets. Raw data files urls accessible for 1 hour. 

Make post request with client credentials to generate auth token 
```http
POST https://api.repo361.com/security/oauth2/token
ContentType: application/x-www-form-urlencoded
Form: {
    "client_id": "put here client_id"
    "client_secret": "put here client_secret"    
    "grant_type": "client_credentials"    
}
```

Make get request with specified header Authorization
```http
GET https://api.repo361.com/rawdata/files/{date}/csv
Headers: {
    "Authorization": "Bearer {put here access_token}"
    "Content-Type": "application/json"
  }
```
Where ***{date}*** in format ***yyyymmdd***, for example 20210613

You can try [PowerShell](https://github.com/Noralogix/repo361-genesyscloud/blob/main/Repo361-RawData-API.ps1 ) example with your own client credentials.
