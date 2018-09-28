$UtilitiesPath = Join-Path $PSScriptRoot -ChildPath .\Utilities.psm1
Import-Module $UtilitiesPath -Force

function Get-Books {
    [cmdletbinding(DefaultParameterSetName = "default")]
    param(
        [parameter(Mandatory = $true)]
        $Env = "Local",
        [parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        $Title
    )
    $response = Invoke-Api -Method GET -Resource "library/$Title/books" -Env $Env
    
    if ($response) {
        return $response | ConvertTo-Json
    }
}

function Add-Book {
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
    $response = Invoke-Api -Method POST -Content ($jsonRequest | ConvertTo-Json) -Resource "library/$Title/add/book" -Env $Env
    
    if ($response) {
        return $response | ConvertTo-Json
    }
}

function Remove-Book {
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
    $response = Invoke-Api -Method POST -Content ($jsonRequest | ConvertTo-Json) -Resource "library/$Title/remove/book" -Env $Env
    
    if ($response) {
        return $response | ConvertTo-Json
    }
}