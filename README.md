# Json compare
This is a simple tool to compare any two json files
and produce a report indicating matches and differences.
The comparison is based on string representations of the keys and the values.

# Parameters

| Parameter  | Required | Description | Example|
| --------- | ---- | ---------------------- |-----------|
|File 1 | Yes| The path to the first file. It must have ".json" ext. and exist| file1.json |
|File 2 | Yes| The path to the second file. It must have ".json" ext. and exist| file2.json|
|Output folder path  | No| The output folder must exist. The default is: '.\'| .\out |
|Report suffix  | No| Suffix used in the report file name. The default is ''| -my-test|


# Example
`JsonCompare.exe first.json second.json .\out -first-second`

The above call will run the comparison and create a text report in the .\out folder

# Report
A text report is being generated. It has 4 sections:
1. Missed key-value pairs in file 2
2. Missed key-value pairs in file 1
3. Matched key-value pairs by key and value 
4. Unmatched key-value pairs by value 