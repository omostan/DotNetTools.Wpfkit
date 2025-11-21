# Package Icon Guidelines

## Recommended Icon Specifications

For optimal display across NuGet Gallery and Visual Studio:

### Size and Format
- **Recommended Size**: 128x128 pixels (minimum 64x64)
- **File Format**: PNG with transparency support
- **File Name**: `icon.png`
- **Location**: Place in `DotNetTools.Wpfkit` project root

### Design Guidelines
1. **Simple and Clear**: Icon should be recognizable at small sizes
2. **Professional**: Follow modern design principles
3. **Brand Colors**: Use colors that represent your brand
4. **Transparency**: Use transparent background for best integration

### Suggested Icon Concept
For DotNetTools.Wpfkit, consider:
- WPF window/desktop metaphor
- MVVM pattern symbols (arrows/binding)
- .NET branding colors (purple/blue)
- Toolbox/gear icons for "toolkit"

### Adding Icon to Project

Once you have your `icon.png` file ready:

1. Place it in the project root: `DotNetTools.Wpfkit/icon.png`

2. Update `DotNetTools.Wpfkit.csproj`:
   ```xml
   <PropertyGroup>
     <!-- ...other properties... -->
     <PackageIcon>icon.png</PackageIcon>
   </PropertyGroup>
   
   <ItemGroup>
     <None Include="icon.png" Pack="true" PackagePath="\" />
   </ItemGroup>
   ```

### Example Icon Creation Tools
- **Free**: GIMP, Paint.NET, Inkscape
- **Online**: Canva, Figma (free tier)
- **Professional**: Adobe Illustrator, Photoshop

### Icon Placeholder
Until a custom icon is created, you can use a text-based icon or the default NuGet icon.

---

**Note**: Creating a professional icon enhances your package's discoverability and credibility in the NuGet Gallery.
