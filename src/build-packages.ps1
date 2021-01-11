param ([Parameter(Mandatory=$True)][System.Version] $Version, [string] $Suffix, [string] $Output, [string] $Config)

[string] $ver = $Version
if (-not [string]::IsNullOrEmpty($Suffix)) {
    $ver += "-" + $Suffix
}

dotnet pack ./XP.SDK/XP.SDK.csproj /p:Version=$ver -o $Output -c $Config
dotnet pack ./XP.Proxy/XP.Proxy.csproj /p:Version=$ver -o $Output -c $Config
dotnet pack ./XP.SDK.OpenTK.Graphics/XP.SDK.OpenTK.Graphics.csproj /p:Version=$ver -o $Output -c $Config
dotnet pack ./XP.SDK.Silk.NET/XP.SDK.Silk.NET.csproj /p:Version=$ver -o $Output -c $Config
dotnet pack ./XP.SDK.Analyzers/XP.SDK.Analyzers.Package/XP.SDK.Analyzers.Package.csproj /p:Version=$ver -o $Output -c $Config