#C:\Users\frank\source\Repos\MakeOvdPresentatation\MakeOvdPresentatation\bin\Debug>powershell Scripts\quickAccessAdd.ps1 -directoryName 'C:\Users\frank\Softouch\01-Onze Kerkdiensten\2021-01-10'
param([string]$directoryName='xxx') 
$o = new-object -com shell.application
$o.Namespace($directoryName).Self.InvokeVerb("pintohome")