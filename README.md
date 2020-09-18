# SendMail
Simple console app to send mail that listed on text file. Can run on Windows, Linux or macOS!

![Release](https://github.com/ahmadtantowi/SendMail/workflows/Release/badge.svg)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/ahmadtantowi/SendMail?label=Version)

## [Configuration](#configuration)
Canfiguration can be done via `appsetting.json` file. You can configure mail server and mail template.

### [Configure Mail Server](#configure-mail-server)
```json
"SmtpOption": {
    "Host": "",
    "UserName": "",
    "Password": "",
    "Port": 0,
    "EnableSsl": true
},
```

### [Configure Mail Template](#configure-mail_template)
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

## [Hot to Use](#how-to-use)
1. Provide value for [mail server](#configure-mail-server)
2. Provide value for [mail template](#configure-mail-template) with following detail
    - `Subject`: email subject
    - `Body`: body of email, you can use plain text or path of html file
    - `IsBody`: set true if you want send body of email with html
    -  `Receiver`: path of text file which contain email receiver that separated with new line
3. Run with following command
    - Linux or macOS:
        ```bash
        $ ./SendMail`
        ```
    - Windows:
        ```cmd
        > .\SendMail.exe
        ```
        Or you can just double-click `SendMail.exe`
4. Application running
    
    ![SendMail Run](https://raw.githubusercontent.com/ahmadtantowi/SendMail/master/SendMail/StaticFiles/images/sendmail_run.png)