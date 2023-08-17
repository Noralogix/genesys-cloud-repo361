## .NET 6 app
Before run please specify appsettings.json
- MySqlConnection - your own connection
- ClientId - from repo 361
- ClientSecret - from repo 361

Craete table with [MySQL scripts](https://github.com/Noralogix/repo361-genesyscloud/tree/main/mysql ).  

1. Schedule the Data Fetching. Later than 5am by UTC.
2. Fetch Data.
3. Process Data and Push to MySQL.


## CSV import to MySQL based on 'LOAD DATA LOCAL INFILE'

Please check if MySQL variable "local_infile" is ON

Run command:
```
show global variables like 'local_infile';
```

In the MySqlConnection add  
```
AllowLoadLocalInfile=true
```