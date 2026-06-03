# Enhancement 3 - Excel Export

## Manual NuGet Installation Required

Due to the .NET Framework 4.8 project structure, ClosedXML must be installed via Visual Studio Package Manager Console:

```powershell
Install-Package ClosedXML
```

Or add manually to `packages.config`:

```xml
<package id="ClosedXML" version="0.102.2" targetFramework="net48" />
<package id="DocumentFormat.OpenXml" version="2.16.0" targetFramework="net48" />
<package id="ExcelNumberFormat" version="1.1.0" targetFramework="net48" />
```

## Implementation Below

The Export action and button have been added assuming ClosedXML will be installed.

ExcelNumberFormat, DocumentFormat.OpenXml, DocumentFormat Added to solution 
Continue the  ENHANCEMENT where allow display page to action / execute the workflow
fix the workflow assign Role from list of roles.
