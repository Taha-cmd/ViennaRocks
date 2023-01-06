
[CmdletBinding()]
param(
    [Parameter(Mandatory)][string] $Environment
)

$environmentPath = ".\Deployment\Environments\$Environment"
if (-not (Test-Path $environmentPath)) {
    throw "could not find $environmentPath"
}

kubectl apply -f $environmentPath
kubectl apply -f ".\Deployment"