# Adding a Package Icon to DotNet.WpfToolkit

## Why Add an Icon?

A package icon helps your package stand out on NuGet.org and makes it more recognizable to users.

## Icon Requirements

- **Format**: PNG
- **Size**: 128x128 pixels (recommended) or 64x64 pixels (minimum)
- **File size**: Under 1 MB
- **Background**: Transparent or solid color
- **Content**: Simple, clear logo or symbol

## Quick Icon Creation Options

### Option 1: Use an Online Tool

#### Canva (Recommended)
1. Go to https://www.canva.com/
2. Click "Custom size" ? 128 x 128 pixels
3. Design your icon using their templates
4. Download as PNG with transparent background

#### Photopea (Free Photoshop Alternative)
1. Go to https://www.photopea.com/
2. File ? New ? 128 x 128 pixels
3. Design your icon
4. File ? Export as ? PNG

#### Logo Maker Tools
- https://www.logomakr.com/ - Simple logo creation
- https://logo.com/ - AI-powered logo generation
- https://www.freelogodesign.org/ - Free logo templates

### Option 2: Use Microsoft Paint

1. Open Paint
2. Resize canvas to 128 x 128 pixels
3. Create your design
4. Save as PNG

### Option 3: Find Free Icons

#### Icon Libraries
- https://www.flaticon.com/ - Millions of free icons
- https://fontawesome.com/ - Popular icon library
- https://icons8.com/ - Flat icons
- https://www.iconfinder.com/ - Icon search engine

**Important**: Check the license! Many icons require attribution or are only free for personal use.

### Option 4: Use Text-Based Icon

Create a simple text-based icon with your package initials:

1. Open any image editor
2. Create 128x128 canvas
3. Add text "WPF" or "DW" (for DotNet.WpfToolkit)
4. Use bold font, centered
5. Add background color
6. Save as PNG

## Design Tips

### Good Icon Design
? Simple and clear
? Recognizable at small sizes
? Uses 2-3 colors max
? Has good contrast
? Represents the package purpose

### Avoid
? Too much detail (gets lost at small size)
? Thin lines (hard to see)
? Too many colors (looks messy)
? Text that's too small to read
? Copyrighted images/logos

## Suggested Design Ideas for DotNet.WpfToolkit

1. **WPF Letters**: Bold "WPF" text on colorful background
2. **Toolbox Icon**: Simple toolbox or wrench icon
3. **Puzzle Piece**: Representing toolkit components
4. **Gear/Cog**: Representing utilities and tools
5. **Building Blocks**: Representing MVVM components
6. **Window Icon**: Representing WPF windows

### Color Suggestions
- **Primary**: #0078D4 (Microsoft Blue)
- **Accent**: #68217A (Purple)
- **Background**: White or Transparent
- **Alternative**: Gradient from blue to purple

## Step-by-Step: Adding Your Icon

### 1. Create or Download Icon
Save your 128x128 PNG file as `icon.png`

### 2. Add to Project
Copy the file to your project directory:
```
DotNet.WpfToolkit/icon.png
```

### 3. Update .csproj File

Edit `DotNet.WpfToolkit/DotNet.WpfToolkit.csproj`:

Find the PropertyGroup section and add:
```xml
<PackageIcon>icon.png</PackageIcon>
```

Find the ItemGroup section and uncomment/add:
```xml
<None Include="icon.png" Pack="true" PackagePath="\" />
```

### 4. Verify Icon is Included

```bash
# Navigate to project
cd DotNet.WpfToolkit

# Build package
dotnet pack -c Release -o ./nupkg

# Check package contents
dotnet nuget verify nupkg/DotNet.WpfToolKit.1.0.0.nupkg
```

### 5. Publish Update

If you've already published version 1.0.0, you need to increment the version:

```xml
<Version>1.0.1</Version>
```

Then publish:
```bash
dotnet pack -c Release -o ./nupkg
dotnet nuget push nupkg/DotNet.WpfToolKit.1.0.1.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
```

## Example Icon Template (SVG)

Here's a simple SVG template you can modify and convert to PNG:

```svg
<svg width="128" height="128" xmlns="http://www.w3.org/2000/svg">
  <!-- Background -->
  <rect width="128" height="128" fill="#0078D4" rx="16"/>
  
  <!-- Text -->
  <text x="64" y="80" font-family="Arial" font-size="48" font-weight="bold" 
        fill="white" text-anchor="middle">WPF</text>
</svg>
```

Save this as `icon.svg`, then convert to PNG using:
- https://cloudconvert.com/svg-to-png
- Or any SVG editor like Inkscape

## Verifying Icon Display

After publishing with an icon:

1. Wait 5-10 minutes for NuGet to process
2. Visit: https://www.nuget.org/packages/DotNet.WpfToolKit
3. Your icon should appear next to the package name
4. Check in Visual Studio Package Manager - icon should appear there too

## Troubleshooting

### Error: Icon file not found
- Verify `icon.png` exists in `DotNet.WpfToolkit/` directory
- Check file name is exactly `icon.png` (case-sensitive on some systems)
- Ensure the file is committed to Git

### Icon doesn't appear on NuGet
- Wait 15-30 minutes for cache to refresh
- Clear your browser cache
- Try incognito/private browsing mode

### Icon looks blurry
- Use exactly 128x128 pixels
- Save as PNG (not JPEG)
- Don't use compression that reduces quality

### Wrong icon appears
- NuGet may cache old icon
- Increment package version
- Wait for cache to clear (can take hours)

## Sample Icons for Inspiration

Check out these popular packages for icon inspiration:
- **Newtonsoft.Json**: Simple "JSON" text
- **Serilog**: Sink icon
- **AutoMapper**: Arrow/mapping symbol
- **MediatR**: Communication icon
- **FluentValidation**: Checkmark symbol

## No Icon? No Problem!

Remember: **An icon is completely optional!** Your package will work perfectly fine without one. You can always add it in a future version.

Many successful packages on NuGet don't have custom icons and still get thousands of downloads.

---

**Need help?** Feel free to reach out or check the NuGet documentation:
https://docs.microsoft.com/nuget/reference/nuspec#icon
