$UtilitiesPath = Join-Path $PSScriptRoot -ChildPath .\Utilities.psm1
Import-Module $UtilitiesPath -Force

function Get-TvShows {
    [cmdletbinding(DefaultParameterSetName = "default")]
    param(
        [parameter(Mandatory = $true)]
        $Env = "Local",
        [parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        $Title
    )
    $response = Invoke-Api -Method GET -Resource"library/$Title/tvshows" -Env $Env
    
    if ($response) {
        return $response | ConvertTo-Json
    }
}

function Add-TvShow {
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
    $response = Invoke-Api -Method POST -Content ($jsonRequest | ConvertTo-Json) -Resource "library/$Title/add/tvshow" -Env $Env
    
    if ($response) {
        return $response | ConvertTo-Json
    }
}

function Remove-TvShow {
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
    $response = Invoke-Api -Method POST -Content ($jsonRequest | ConvertTo-Json) -Resource "library/$Title/remove/tvshow" -Env $Env
    
    if ($response) {
        return $response | ConvertTo-Json
    }
}