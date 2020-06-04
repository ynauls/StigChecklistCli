![.NET Core](https://github.com/ynauls/StigChecklistCli/workflows/.NET%20Core/badge.svg)

# StigChecklistCLI
CLI to copy STIG Viewer Checklist from a checklist (source) to another (target). 

Run the following command for CLI help

```
stigchecklist-cli.exe --help
```

# Schemas Supported
* STIGViewer 2.9
* STIGViewer 2.10

# Usage

```
stigchecklist-cli.exe copy -s "C:\[PATH]\STIGViewer_Checklist.ckl" -t "C:\[PATH]\New_STIGViewer_Checklist.ckl"
```
