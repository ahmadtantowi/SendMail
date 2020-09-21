# SendMail
Simple console app to send mail that listed on text file. Can run on Windows, Linux or macOS!

![Release](https://github.com/ahmadtantowi/SendMail/workflows/Release/badge.svg)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/ahmadtantowi/SendMail?label=Version)

## Configuration
Canfiguration can be done via `appsetting.json` file. You can configure mail server, mail template and variables that can use to replace string on mail template.

### Configure Mail Server
Object of `SmtpOption`
```json
"SmtpOption": {
    "Host": "",
    "UserName": "",
    "Password": "",
    "Port": 0,
    "EnableSsl": true
}
```

### Configure Mail Template
`Array` of object `MailOption`
```json
"MailOption": [
    {
      "Subject": "Hello from SendMail",
      "Body": "mailtemplate.html",
      "IsBodyHtml": true,
      "Receiver": "mail_receiver_separate_with_newline.txt"
    }
]
```

### Configure Variables
Object of `key: value` pair
```json
"Variables": {
    "FilesHostUrl": "https://your-hosting-files-url.com"
}
```

## Hot to Use
1. Provide value for [mail server](#configure-mail-server)
2. Provide value for [mail template](#configure-mail-template) with following detail
    - `Subject`: email subject
    - `Body`: body of email, you can use plain text or path of html file
    - `IsBody`: set true if you want send body of email with html
    - `Receiver`: path of text file which contain email receiver that separated with new line
3. Provide value for [variables](#configure-variables) (optional) to replace some string on provided mail template
4. Run with following command
    - Linux or macOS:
        ```bash
        $ ./SendMail
        ```
    - Windows:
        ```cmd
        > .\SendMail.exe
        ```
        Or you can just double-click `SendMail.exe`
5. Application running
    
    ![SendMail Run](https://raw.githubusercontent.com/ahmadtantowi/SendMail/master/SendMail/StaticFiles/images/sendmail_run.png)