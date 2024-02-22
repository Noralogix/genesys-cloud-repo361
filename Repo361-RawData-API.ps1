$clientId = ""
$clientSecret = ""
$dateTwoDaysAgo = (Get-Date).AddDays(-2)
$date = $dateTwoDaysAgo.ToString("yyyyMMdd")
$downloadsPath = "C:\Repo361RawData"

$rawdataUrl = "https://api.repo361.com/rawdata/files/$date/csv"
$authurl = "https://api.repo361.com/security/oauth2/token"

$authboundary = [System.Guid]::NewGuid().ToString()
$authLF = "`r`n"

$authSuccess = $false
$tokenBearer = ""
$token = ""

$authbodyLines = (
    "--$authboundary",
    "Content-Disposition: form-data; name=`"client_id`"$authLF",
    $clientId,
    "--$authboundary",
    "Content-Disposition: form-data; name=`"client_secret`"$authLF",
    $clientSecret,
    "--$authboundary",
    "Content-Disposition: form-data; name=`"grant_type`"$authLF",
    "client_credentials",
    "--$authboundary--"
) -join $authLF

try {
    $tokenResponse = Invoke-WebRequest -Uri $authurl -Method Post -UseBasicParsing -ContentType "multipart/form-data; boundary=$authboundary" -Body $authbodyLines
    $tokenResponseJSON = ConvertFrom-Json $tokenResponse.Content

    # Extract the token from the response
    $tokenBearer = $tokenResponseJSON.token_type
    $token = $tokenResponseJSON.access_token 
    $authSuccess = $true
    Write-Host "Access token received."
}
catch {
    
    $authSuccess = $false
    $message = $_
    Write-Error "   Unable to get an access token. $message "
}

if ($authSuccess) {
    try {
        $headers = @{
        'Authorization' = $tokenBearer + ' ' + $token
        }
        $rawdataResponse = Invoke-WebRequest -Uri $rawdataUrl -Method Get -UseBasicParsing -ContentType "application/json" -Headers $headers
        $rawdataResponseJSON = ConvertFrom-Json $rawdataResponse.Content

        # Check if the folder exists
        if (-not (Test-Path -Path $downloadsPath)) {
            # Folder does not exist, so create it
            New-Item -ItemType Directory -Path $downloadsPath
            Write-Host "Folder created: $downloadsPath"
        }
        foreach ($rdi in $rawdataResponseJSON)
        {
            $csvFilePath = "$downloadsPath/$($date)"
            # Check if the folder exists
            if (-not (Test-Path -Path $csvFilePath)) {
                # Folder does not exist, so create it
                New-Item -ItemType Directory -Path $csvFilePath
                Write-Host "Folder created: $csvFilePath"
            }        
            Invoke-WebRequest $rdi.url -OutFile  $csvFilePath/$($rdi.name).csv
            Write-Host $rdi.name 
            Write-Host $rdi.url
        }
    }
    catch {
        $message = $_
        Write-Error "   Unable to get raw data. $message "
    }
}
