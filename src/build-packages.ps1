param ([Parameter(Mandatory=$True)][System.Version] $Version, [string] $Suffix, [string] $Output, [string] $Config)

[string] $ver = $Version
if (-not [string]::IsNullOrEmpty($Suffix)) {
    $ver += "-" + $Suffix
}

dotnet pack ./XP.SDK/XP.SDK.csproj /p:Version=$ver -o $Output -c $Config
dotnet pack ./XP.Proxy/XP.Proxy.csproj /p:Version=$ver -o $Output -c $Config