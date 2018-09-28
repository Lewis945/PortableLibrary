function Save-File {
    [cmdletbinding()]
    param(
        [parameter(Mandatory = $true)]
        $Path,
        $FileName,
        $FileData
    )

    $FullPath = Join-Path $Path -ChildPath $FileName

    if (!(Test-Path $Path)) {
        New-Item -Path $Path -ItemType directory
    }

    if (!(Test-Path $FullPath)) {
        New-Item -path $Path -name $FileName -type "file" -value $FileData
    }
    else {
        Set-Content -path $FullPath -value $FileData
    }
}

function Get-Token {
    [cmdletbinding()]
    param(
        $Env,
        $UserName
    )
    $CurrentFileName = "current.txt"
    $UserFileName = $UserName + ".txt"
    $Path = Join-Path -Path ([Environment]::GetFolderPath('MyDocuments')) -ChildPath "PortableLibrary" | 
        Join-Path -ChildPath $Env
    if ($UserName) {
        $FullPath = Join-Path $Path -ChildPath $UserFileName
    }
    else {
        $FullPath = Join-Path $Path -ChildPath $CurrentFileName
    }

    if (!(Test-Path $FullPath)) {
        return '';
    }

    $json = Get-Content $FullPath | ConvertFrom-Json

    $token = $json.auth_token

    $Secure = $token | ConvertTo-SecureString -Key (1..16)
    $PlainToken = Get-PlainText $Secure;
    return $PlainToken;
}

function Set-Token {
    [cmdletbinding()]
    param(
        $Env,
        $UserName,
        $JsonTokenObject
    )
    $CurrentFileName = "current.txt"
    $UserFileName = $UserName + ".txt"
    $Path = Join-Path -Path ([Environment]::GetFolderPath('MyDocuments')) -ChildPath "PortableLibrary" | 
        Join-Path -ChildPath $Env
    $FullPath = Join-Path $Path -ChildPath $FileName

    Write-Host "Token file path: " + $Path

    $Secure = ConvertTo-SecureString $JsonTokenObject.auth_token -AsPlainText -Force
    $Encrypted = ConvertFrom-SecureString -SecureString $Secure -Key (1..16)

    $JsonTokenObject.auth_token = $Encrypted
    $FileData = $JsonTokenObject | ConvertTo-Json

    Save-File -Path $Path -FileName $UserFileName -FileData $FileData
    Save-File -Path $Path -FileName $CurrentFileName -FileData $FileData
}

function Get-PlainText() {
    [CmdletBinding()]
    param
    (
        [parameter(Mandatory = $true)]
        [System.Security.SecureString]$SecureString
    )
    BEGIN { }
    PROCESS {
        $bstr = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($SecureString);
 
        try {
            return [Runtime.InteropServices.Marshal]::PtrToStringBSTR($bstr);
        }
        finally {
            [Runtime.InteropServices.Marshal]::FreeBSTR($bstr);
        }
    }
    END { }
}

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

function Invoke-FileDownload {
    [cmdletbinding()]
    param(
        [parameter(Mandatory = $true)]
        $Env,
        [parameter(Mandatory = $true)]
        $Resource,
        $FileName,
        $Method = "GET",
        $BarearToken
    )
    $uri = Get-BaseUri($Env)
    Write-Host "Base uri: " + $uri
    if (!$uri) {
        Write-Error "Base uri is required! Check environment name and uri it provides."
    }
        
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
    
    $uri = $uri + "/" + $Resource
        
    Invoke-WebRequest $uri -Headers $headers -OutFile $FileName -PassThru
}