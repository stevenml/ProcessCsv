# ProcessCsv

Usage: ProcessCsv [command]

> .\ProcessCsv

Commands:
  print-abnormal-values    Print abnormal values n% above or below the median

Options:
  -h, --help    Show help message
  --version     Show version

> .\ProcessCsv print-abnormal-values -h

Usage: ProcessCsv print-abnormal-values [--normal-range-percentage <Int32>] [--directory <String>] [--file <String>] [--parallel] [--help]

Print abnormal values n% above or below the median

Options:
  
  -p, --normal-range-percentage <Int32>    Please specify the normal value range percentage (Required)

  -d, --directory <String>                 Please specify the file directory path, or by default process all the files in current directory
  
  -f, --file <String>                      Please specify the file path, or by default process all the files in current directory
  
  -l, --parallel                           Please specify if you want to process files in parallel
  
  -h, --help                               Show help message

## Examples


> .\ProcessCsv print-abnormal-values -p 20 -d "C:\Sample Files"

> .\ProcessCsv print-abnormal-values -p 20 -d "C:\Sample Files" -l