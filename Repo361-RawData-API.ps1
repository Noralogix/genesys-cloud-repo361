$clientId = ""
$clientSecret = ""
$date = "20210613"

$rawdataUrl = "https://api.repo361.com/rawdata/files/$date/csv"
$resource = "https://api.repo361.com"
$authurl = "https://api.repo361.com/security/oauth2/token"

$authboundary = [System.Guid]::NewGuid().ToString()
$authLF = "`r`n"

$authSuccess = $false
$tokenBearer = ""
$token = ""

$authbodyLines = (
    "--$authboundary",
    "Content-Disposition: form-data; name=`"resource`"$authLF",
    $resource,
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
    $tokenResponse = Invoke-WebRequest -Uri $authurl -Method Post -ContentType "multipart/form-data; boundary=$authboundary" -Body $authbodyLines
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
        $rawdataResponse = Invoke-WebRequest -Uri $rawdataUrl -Method Get -ContentType "application/json" -Headers $headers
        $rawdataResponseJSON = ConvertFrom-Json $rawdataResponse.Content

        foreach ($rdi in $rawdataResponseJSON)
        {
            Write-Host $rdi.name 
            Write-Host $rdi.url
        }
    }
    catch {
        $message = $_
        Write-Error "   Unable to get raw data. $message "
    }
}