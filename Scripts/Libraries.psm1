$UtilitiesPath = Join-Path $PSScriptRoot -ChildPath .\Utilities.psm1
Import-Module $UtilitiesPath -Force

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

function Get-LibrariesPdf {
    [cmdletbinding(DefaultParameterSetName = "default")]
    param(
        [parameter(Mandatory = $true)]
        $Env = "Local",
        $FileName,
        [Switch] $Extended
    )
    $uri = "libraries/pdf"
    if ($Extended -eq $true) {
        $uri = $uri + "?extended=true"
    }
    Invoke-FileDownload -FileName $FileName -Resource $uri -Env $Env
}

function Add-Library {
    [cmdletbinding(DefaultParameterSetName = "default")]
    param(
        [parameter(Mandatory = $true)]
        $Env = "Local",
        [parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        $Title,
        [parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        $Type
    )
    $jsonRequest = [ordered]@{
        Title = $Title
        Type  = $Type
    }
    $response = Invoke-Api -Method POST -Content ($jsonRequest | ConvertTo-Json) -Resource "libraries/add" -Env $Env
    
    if ($response) {
        return $response | ConvertTo-Json
    }
}

function Remove-Library {
    [cmdletbinding(DefaultParameterSetName = "default")]
    param(
        [parameter(Mandatory = $true)]
        $Env = "Local",
        [parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        $Title,
        [parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        $Type
    )
    $jsonRequest = [ordered]@{
        Title = $Title
        Type  = $Type
    }
    $response = Invoke-Api -Method POST -Content ($jsonRequest | ConvertTo-Json) -Resource "libraries/remove" -Env $Env
    
    if ($response) {
        return $response | ConvertTo-Json
    }
}