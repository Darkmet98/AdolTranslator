![AdolTranslator](https://github.com/Darkmet98/AdolTranslator/blob/main/Images/AdolBanner.jpg?raw=true)
# AdolTranslator
A simple program for Ys fan-translations.

# Version

## 1.0
* Initial Release.

# Usage

## Ys I Chronicles +

### Font
1. Create a custom font for **FONT_PSP.SKI** with [ Twns font generator](https://github.com/TwnKey/YsIFontConverter " Twn font generator"), and place it the new **text.ini** into adoltranslator folder.
2. Create a custom font again for **FONT_DIA.SKI** and **FONT_DIA_PSP.SKI**, rename the new **text.ini** to **text2.ini** and place it into adoltranslator folder.
3. Now,  you need to edit **FONT_SX.BMP**, this font uses another encoding, a modified JIS, but we can use the ANSI 1552 encoding because they are similar.
For example, **á** in ANSI is 0xE1, so following this table you can replace the character in the 0xE1 box with **á**.
![font_layout.jpg](https://github.com/Darkmet98/AdolTranslator/blob/main/Images/font_layout.png?raw=true)

### Texts
* Scena: Just drag and drop the file or the generated po onto adoltranslator.

### Graphics
* WAKU0.bin: Just drag and drop the file or the png onto adoltranslator.

### Executables
* You can edit the **config.exe** with [Resource Hacker](http://www.angusj.com/resourcehacker/ "Resource Hacker"), but some texts aren't translatable, but with AdolTranslator you can translate them without any problems.
* Translate the downloaded pos and config.exe with Resource Hacker, place your translated **config.exe** and **ys1plus.exe** on adoltranslator folder and drag them one by one onto **Adoltranslator.exe** for generate a translated version.


# Supported games
* Ys I Chronicles + (PC)
* Ys II Chronicles + (PC) (Incomplete)

# Credits
* Thanks to Twn for ASM Hacks and explanations.
* Thanks to Kaplas for waku0 swizzling and C to C# code port.
* Thanks to Pleonex for Yarhl libraries.
