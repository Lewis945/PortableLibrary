$UtilitiesPath = Join-Path $PSScriptRoot -ChildPath .\Utilities.psm1
Import-Module $UtilitiesPath -Force

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