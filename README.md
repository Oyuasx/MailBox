# Desktop Email Application - C# Mail Client

This project is a desktop email client developed in C# using Windows Forms, inspired by the built-in "Windows Mail" application. It allows users to send, delete, and organize emails into folders like Inbox, Spam, and Deleted Items. The application also provides functionality for sending emails with attachments, images, and custom text formatting.

### Features

- **Send and Receive Emails**: The app supports basic email functions such as sending and receiving messages.
- **Folder Organization**: Users can organize emails into various folders including Inbox, Spam, and Deleted Items.
- **Attachments**: Users can send email attachments, and images can be embedded within the email body.
- **Custom Image Positioning**: Allows positioning and resizing of images within the email content.
- **Custom Fonts and Colors**: Support for custom text fonts and colors for email composition.
- **HTML and Text Email**: Users can compose emails in HTML format or plain text. Incoming emails can be received in either HTML or plain text format using an advanced **substring** algorithm.
- **Filtering**: Advanced substring-based filtering helps ensure proper handling of email content.

### Key Technologies and Libraries Used

- **MimeKit**: A library for handling MIME (Multipurpose Internet Mail Extensions) messages.
- **MailKit**: A cross-platform .NET mail client that supports sending and receiving emails.
- **MarkupConverter**: A library used for converting HTML to text.
- **BouncyCastle**: A cryptographic library used for email encryption.

### Custom Algorithm for HTML-to-Text Conversion

One of the unique features of this application is the custom **HTML-to-Text conversion algorithm**. This algorithm uses advanced **substring matching** to extract plain text from HTML-formatted emails. It removes HTML tags and handles entities like `&nbsp;`, `&copy;`, `&bull;`, and others, transforming the email into a readable text format while preserving line breaks, tabs, and other important formatting.

### Custom Controls

In the design of the application, **custom controls** were created to enhance the user experience. These controls provide tailored functionalities that go beyond the standard Windows Forms controls, adding flexibility and user-friendly features.

---

### Code Sample for HTML to Text Conversion:

Here's a brief code sample that demonstrates the HTML-to-text conversion process:

```csharp
public static string ConvertHtmlToText(string html)
{
    string result;
    result = html.Replace("\r", " ");
    result = result.Replace("\n", " ");
    result = result.Replace("\t", string.Empty);
    result = System.Text.RegularExpressions.Regex.Replace(result, @"( )+", " ");
    result = System.Text.RegularExpressions.Regex.Replace(result, @"<[^>]*>", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
    result = result.Replace("&nbsp;", " ");
    // More regex operations...
    return result;
}
```
### Complete Code in Gist and How to Run the Application

For the full **HTML-to-Text conversion** code, please refer to the [Gist here](https://gist.github.com/bugradaryal/a6a1dbf19e6e75321fbcb22884785b1c).

---

### How to Run the Application

1. **Clone the repository**:
   ```bash
   git clone https://github.com/bugradaryal/DesktopEmailApplication-CSharpMailClient.git
