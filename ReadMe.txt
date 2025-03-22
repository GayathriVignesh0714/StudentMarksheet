# Student Marksheet Application - QA Instructions

## 1. Start JSON Server
- Double-click `StartJSONServer.bat` to launch JSON Server.
- Ensure it runs at `http://localhost:3001/students`.
- Keep this running while using the application.

## 2. Start the Web API
- Double-click `StartAPIServer.bat` to launch the Web API.
- Ensure the API is running at `http://localhost:5000`.

## 3. Modify Database Connection (If Needed)
- Navigate to StudentMarksheet_Publish\StudentMarkSheetApp\.
- Open `StudentMarksheet.dll.config` in a text editor.
- Locate the `<connectionStrings>` section and update the database details.
- Save the file.

## 4. Launch the Application
- Navigate to StudentMarksheet_Publish\StudentMarkSheetApp\
- Double-click `StudentMarkSheet.exe` to start the application.
- Ensure student data is loading correctly upon clicking the 'Fetch Student MarkSheet' Button.

## 5. Report Issues
- If errors occur, check logs inside the application directory.
