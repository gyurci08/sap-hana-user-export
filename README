# SAP HANA User Export Tool

## Overview

The SAP HANA User Export Tool is a Windows Forms application designed to simplify the process of exporting and recreating user authorizations in SAP HANA databases. This tool allows you to generate SQL scripts for creating users and granting their permissions based on existing user data.

## Features

- Copy authorization data query to clipboard
- Load authorization data from a file
- Generate SQL scripts for user creation and permission granting
- Support for both normal and activated role grants
- Automatic password generation for new users
- Export generated SQL scripts to a file and clipboard

## How to Use

1. **Copy Query**: Click the "Copy Query" button to copy the authorization data query to your clipboard. You can then run this query in your SAP HANA database to extract the necessary data.

2. **Load Data**: Click the "Load Data" button to select and load the authorization data file (output from the query in step 1).

3. **Enter User Information**:
   - In the "Source User" field, enter the name of the existing user whose permissions you want to replicate.
   - (Optional) In the "Target User" field, enter the name of the new user. If left blank, it will default to the source user name.

4. **Generate SQL**: Click the "Generate SQL" button to create the SQL script for user creation and permission granting.

5. The generated SQL script will be:
   - Saved as a text file in the "Documents/sap-hana-user-export/createSQL" folder
   - Copied to your clipboard for immediate use

## Technical Details

- The application is written in C# using Windows Forms
- It uses asynchronous file I/O operations for better performance
- The tool generates a random password for new user creation
- It handles various types of grants, including schema-specific and activated roles

## Notes

- The tool automatically forces a password change for newly created users
- Some specific grants (e.g., PUBLIC roles) are ignored in the output
- The application creates a folder structure in your Documents folder to store the generated SQL files

## Version

Current version: v2.0.1

## Error Handling

The application includes error handling for various scenarios, such as:
- Missing source user input
- No data loaded before generating SQL
- File I/O errors
- Unexpected exceptions during processing

Error messages are displayed to the user via message boxes for easy troubleshooting.