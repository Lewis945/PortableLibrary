$UtilitiesPath = Join-Path $PSScriptRoot -ChildPath .\Utilities.psm1
Import-Module $UtilitiesPath -Force

function Get-BaseUri {
    [cmdletbinding()]
    param(
        [parameter(Mandatory = $true)]
        $Env
    )
    if ( $Env -eq "Local" ) { $result = "http://localhost:49452/api"; }
    elseif ( $Env -eq "CI" ) { $result = "https://portable-library-web-api-ci.azurewebsites.net/api" }
    return $result;
}

function Invoke-Api {
    [cmdletbinding()]
    param(
        [parameter(Mandatory = $true)]
        $Env,
        [parameter(Mandatory = $true)]
        $Resource,
        $Query,
        $Content,
        $Method = "GET",
        $BarearToken
    )
    $uri = Get-BaseUri($Env)
    Write-Host "Base uri: " + $uri
    if (!$uri) {
        Write-Error "Base uri is required! Check environment name and uri it provides."
    }

    # if (!$BarearToken -and ($Resource -ne "membership/register" -or $Resource -ne "membership/login")) {
    #     Write-Error "Authentication is required for this request!"
    # }
        
    $headers = @{
        "Content-Type" = 'application/json';
        "Accept"       = 'application/json';
    }

    if ($BarearToken) {
        $headers.Add("Authorization", "Bearer $BarearToken"); 
    }
    else {
        $BarearToken = Get-Token -Env $Env
        if ($BarearToken) {
            $headers.Add("Authorization", "Bearer $BarearToken"); 
        }
    }
        
    if ($Query) {
        $queryString = "?" + $Query
    }
    
    $uri = $uri + "/" + $Resource + $queryString
        
    if ($Method -eq "POST") {
        $body = $Content
    }
        
    Invoke-RestMethod -Method $Method -Uri $uri -Headers $headers -Body $body
}

function Invoke-Login {
    [cmdletbinding(DefaultParameterSetName = "default")]
    param(
        [parameter(Mandatory = $true)]
        $Env = "Local",
        [parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        $UserName,
        [parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        $Password
    )
    $jsonRequest = [ordered]@{
        UserName = $UserName
        Password = $Password
    }

    $response = Invoke-Api -Method POST -Content ($jsonRequest | ConvertTo-Json) -Resource "membership/login" -Env $Env
    
    if ($response) {
        Set-Token -Env $Env -UserName $UserName -JsonTokenObject ($response | ConvertFrom-Json)
        return $response.auth_token
    }
}

function Get-Libraries {
    [cmdletbinding(DefaultParameterSetName = "default")]
    param(
        [parameter(Mandatory = $true)]
        $Env = "Local"
    )
    $response = Invoke-Api -Method GET -Resource "libraries" -Env $Env
    
    if ($response) {
        return $response | ConvertTo-Json
    }
}