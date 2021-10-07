# Find Images and Create PowerPoint Slides

This Windows application is used to find images related to a user-input title and text. Images can then be selected and a PowerPoint slide will be generated with the Title and Text overlaid.

---

## Requirements

- Visual Studio ([download here](https://visualstudio.microsoft.com/downloads/))
- Windows
- Microsoft Office Suite (PowerPoint specifically)
- C#/Dotnet ([download here](https://dotnet.microsoft.com/download))

---

## Installation instructions

1. Clone this repository `git clone https://github.com/Usarneme/WPF_Image_To_PPT` or download the files using GitHub's download link (green, above and to the right)
2. Open Visual Studio
3. Open the project in Visual Studio
4. Run the project in debug/testing with F5, or Build the project with Ctrl+B
5. Open the HelloWpf\HelloWpf\bin\Debug\netcoreapp3.1 directory
6. Double click to open the HelloWpf.exe executable

---

## Usage instructions

- Enter Title text in the input area
- Enter Text in the second input area - NOTE: This is a Rich Text input so you can enter bold (Ctrl+B), Italic (Ctrl+I), Underline (Ctrl+U) and other styles. See [https://www.richtexteditor.com/demos/shortcuts.aspx](https://www.richtexteditor.com/demos/shortcuts.aspx) for more Rich Text info.
- Once you have input Title and Text click on the Find Images button to query the Pexels API for images related to your text
- Click on any of the images to select them; you will know they are selected when they have a green border
- Click on any selected images to un-select them (which will remove the green border)
- Once you have chosen 0 or more images and have input your title and text, click on the Create Slide button to generate a PowerPoint slide

---

## Known Issues

- You must have a valid copy of PowerPoint (MS Office) installed for the slides to be generated
- Other issues please feel free to open a pull request

---

## License

GPLv3 - Feel free to use this code as you like, attribution is appreciated but not required.
AS-IS - This code has not been thoroughly tested. It is unlikely to break anything but use at your own risk.

---

Thanks!