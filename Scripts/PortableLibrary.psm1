function Get-BaseUri {
    [cmdletbinding()]
    param(
        [parameter(Mandatory = $true)]
        $Env
    )
    if ( $Env -eq "Local" ) { $result = "http://localhost:5000/api"; }
    elseif ( $Env -eq "CI" ) { $result = '' }
    return $result;
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

function Get-Token {
    [cmdletbinding()]
    param(
        $UserName
    )
    $CurrentFileName = "current.txt"
    $UserFileName = $UserName + ".txt"
    $Path = Join-Path -Path ([Environment]::GetFolderPath('MyDocuments')) -ChildPath "PortableLibrary" | 
    Join-Path -ChildPath $Env
    if($UserName){
        $FullPath = Join-Path $Path -ChildPath $UserFileName
    }else{
        $FullPath = Join-Path $Path -ChildPath $CurrentFileName
    }

    if (!(Test-Path $FullPath)){
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
        [parameter(Mandatory = $true)]
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

function Save-File{
    [cmdletbinding()]
    param(
        [parameter(Mandatory = $true)]
        $Path,
        $FileName,
        $FileData
    )

    $FullPath = Join-Path $Path -ChildPath $FileName

    if (!(Test-Path $Path))
    {
        New-Item -Path $Path -ItemType directory
    }

    if (!(Test-Path $FullPath))
    {
        New-Item -path $Path -name $FileName -type "file" -value $FileData
    }
    else
    {
        Set-Content -path $FullPath -value $FileData
    }
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
    } else{
        $BarearToken = Get-Token
        if($BarearToken){
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
        Set-Token -UserName $UserName -JsonTokenObject ($response | ConvertFrom-Json)
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