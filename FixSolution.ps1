# Fix Solution Script
# This script removes the old project and updates the solution file

Write-Host "Fixing LiveOn Ecommerce Solution..." -ForegroundColor Green

# Step 1: Close Visual Studio first!
Write-Host "`n??  IMPORTANT: Please close Visual Studio before running this script!" -ForegroundColor Yellow
$response = Read-Host "Have you closed Visual Studio? (y/n)"

if ($response -ne 'y') {
    Write-Host "Please close Visual Studio and run this script again." -ForegroundColor Red
    exit
}

# Step 2: Backup old project file
Write-Host "`nBacking up old project file..." -ForegroundColor Cyan
if (Test-Path "LiveOn Ecommerce.csproj") {
    Copy-Item "LiveOn Ecommerce.csproj" "LiveOn Ecommerce.csproj.backup"
    Write-Host "? Backup created: LiveOn Ecommerce.csproj.backup" -ForegroundColor Green
}

# Step 3: Delete old project file
Write-Host "`nRemoving old project file..." -ForegroundColor Cyan
if (Test-Path "LiveOn Ecommerce.csproj") {
    Remove-Item "LiveOn Ecommerce.csproj" -Force
    Write-Host "? Old project file removed" -ForegroundColor Green
}

# Step 4: Backup old solution
Write-Host "`nBacking up old solution file..." -ForegroundColor Cyan
if (Test-Path "LiveOn Ecommerce.sln") {
    Copy-Item "LiveOn Ecommerce.sln" "LiveOn Ecommerce.sln.backup"
    Write-Host "? Backup created: LiveOn Ecommerce.sln.backup" -ForegroundColor Green
}

# Step 5: Create new solution file
Write-Host "`nCreating new solution file with Clean Architecture projects..." -ForegroundColor Cyan

$solutionContent = @"

Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
VisualStudioVersion = 17.14.36518.9
MinimumVisualStudioVersion = 10.0.40219.1
Project("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "src", "src", "{11111111-1111-1111-1111-111111111111}"
EndProject
Project("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "tests", "tests", "{22222222-2222-2222-2222-222222222222}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "LiveOn.Ecommerce.Domain", "src\LiveOn.Ecommerce.Domain\LiveOn.Ecommerce.Domain.csproj", "{A1B2C3D4-E5F6-4A5B-8C9D-1E2F3A4B5C6D}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "LiveOn.Ecommerce.Application", "src\LiveOn.Ecommerce.Application\LiveOn.Ecommerce.Application.csproj", "{B2C3D4E5-F6A7-4B5C-9D1E-2F3A4B5C6D7E}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "LiveOn.Ecommerce.Infrastructure", "src\LiveOn.Ecommerce.Infrastructure\LiveOn.Ecommerce.Infrastructure.csproj", "{C3D4E5F6-A7B8-4C5D-9E1F-3A4B5C6D7E8F}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "LiveOn.Ecommerce.API", "src\LiveOn.Ecommerce.API\LiveOn.Ecommerce.API.csproj", "{D4E5F6A7-B8C9-4D5E-AF1F-4A5B6C7D8E9F}"
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{A1B2C3D4-E5F6-4A5B-8C9D-1E2F3A4B5C6D}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{A1B2C3D4-E5F6-4A5B-8C9D-1E2F3A4B5C6D}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{A1B2C3D4-E5F6-4A5B-8C9D-1E2F3A4B5C6D}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{A1B2C3D4-E5F6-4A5B-8C9D-1E2F3A4B5C6D}.Release|Any CPU.Build.0 = Release|Any CPU
		{B2C3D4E5-F6A7-4B5C-9D1E-2F3A4B5C6D7E}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{B2C3D4E5-F6A7-4B5C-9D1E-2F3A4B5C6D7E}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{B2C3D4E5-F6A7-4B5C-9D1E-2F3A4B5C6D7E}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{B2C3D4E5-F6A7-4B5C-9D1E-2F3A4B5C6D7E}.Release|Any CPU.Build.0 = Release|Any CPU
		{C3D4E5F6-A7B8-4C5D-9E1F-3A4B5C6D7E8F}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{C3D4E5F6-A7B8-4C5D-9E1F-3A4B5C6D7E8F}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{C3D4E5F6-A7B8-4C5D-9E1F-3A4B5C6D7E8F}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{C3D4E5F6-A7B8-4C5D-9E1F-3A4B5C6D7E8F}.Release|Any CPU.Build.0 = Release|Any CPU
		{D4E5F6A7-B8C9-4D5E-AF1F-4A5B6C7D8E9F}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{D4E5F6A7-B8C9-4D5E-AF1F-4A5B6C7D8E9F}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{D4E5F6A7-B8C9-4D5E-AF1F-4A5B6C7D8E9F}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{D4E5F6A7-B8C9-4D5E-AF1F-4A5B6C7D8E9F}.Release|Any CPU.Build.0 = Release|Any CPU
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
	GlobalSection(NestedProjects) = preSolution
		{A1B2C3D4-E5F6-4A5B-8C9D-1E2F3A4B5C6D} = {11111111-1111-1111-1111-111111111111}
		{B2C3D4E5-F6A7-4B5C-9D1E-2F3A4B5C6D7E} = {11111111-1111-1111-1111-111111111111}
		{C3D4E5F6-A7B8-4C5D-9E1F-3A4B5C6D7E8F} = {11111111-1111-1111-1111-111111111111}
		{D4E5F6A7-B8C9-4D5E-AF1F-4A5B6C7D8E9F} = {11111111-1111-1111-1111-111111111111}
	EndGlobalSection
	GlobalSection(ExtensibilityGlobals) = postSolution
		SolutionGuid = {B71397A9-5B3F-42D3-99B3-BE7064DBFD6D}
	EndGlobalSection
EndGlobal
"@

Set-Content -Path "LiveOn Ecommerce.sln" -Value $solutionContent
Write-Host "? New solution file created" -ForegroundColor Green

# Step 6: Delete old App.config if exists
if (Test-Path "App.config") {
    Remove-Item "App.config" -Force
    Write-Host "? Old App.config removed" -ForegroundColor Green
}

Write-Host "`n? Solution fixed successfully!" -ForegroundColor Green
Write-Host "`nNext steps:" -ForegroundColor Yellow
Write-Host "1. Open 'LiveOn Ecommerce.sln' in Visual Studio"
Write-Host "2. Right-click on solution ? Restore NuGet Packages"
Write-Host "3. Build ? Rebuild Solution"
Write-Host "`nYour new Clean Architecture structure is ready! ??" -ForegroundColor Green
