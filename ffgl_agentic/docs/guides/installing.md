# Installing FFGL Plugins

This guide covers installing built FFGL plugins into VJ software.

## Supported Software

FFGL plugins work in:
- **Resolume Arena/Avenue** (Windows/macOS)
- **VDMX** (macOS)
- **CoGe** (macOS)
- **Magic Music Visuals** (Windows/macOS)
- **Millumin** (macOS)
- **TouchDesigner** (Windows/macOS, via FFGL TOP)

## Installation Paths

### Resolume Arena/Avenue

**Windows:**
```
C:\Program Files\Resolume Arena\plugins\vfx\
```
or
```
C:\Users\YourName\Documents\Resolume Arena\plugins\vfx\
```

**macOS:**
```
/Applications/Resolume Arena.app/Contents/MacOS/plugins/vfx/
```
or
```
~/Library/Application Support/Resolume Arena/plugins/vfx/
```

### VDMX (macOS)

```
~/Library/Application Support/VDMX/Plugins/
```

### CoGe (macOS)

```
~/Library/Application Support/CoGe/PlugIns/FreeFrame/
```

### Magic Music Visuals

**Windows:**
```
C:\Program Files\Magic\Plugins\Effects\
```

**macOS:**
```
/Applications/Magic.app/Contents/PlugIns/
```

## Installation Steps

### 1. Build Plugins

Follow the [Building Guide](building.md) to compile plugins.

### 2. Locate Built Files

**Windows:**
```
ffgl_agentic/build/Release/SketchbookReveal.dll
```

**macOS:**
```
ffgl_agentic/build/SketchbookReveal.bundle
```

### 3. Copy to Plugin Folder

**Windows (Resolume):**
```bash
copy build\Release\SketchbookReveal.dll "C:\Program Files\Resolume Arena\plugins\vfx\"
```

**macOS (Resolume):**
```bash
cp build/SketchbookReveal.bundle "/Applications/Resolume Arena.app/Contents/MacOS/plugins/vfx/"
```

**Tip:** You may need administrator/sudo permissions.

### 4. Restart VJ Software

Close and reopen your VJ software to load the new plugin.

## Verification

### Resolume

1. Open Resolume Arena/Avenue
2. Add a video layer
3. Go to Effects panel
4. Search for "Sketchbook Reveal"
5. Plugin should appear in the list

### VDMX

1. Open VDMX
2. Add a layer
3. Add FX
4. Look for "Sketchbook Reveal" in effects list

## Troubleshooting

### Plugin doesn't appear

**Check file extension:**
- Windows: `.dll`
- macOS: `.bundle`

**Check plugin folder:**
- Make sure you copied to the correct path
- Some software has multiple possible paths

**Check software version:**
- Ensure VJ software supports FFGL 2.x
- Update to latest version if needed

**Check logs:**
- Resolume: Menu → View → Log Window
- Look for plugin loading errors

### Plugin crashes on load

**Windows:**
- Install Visual C++ Redistributable
- Download from Microsoft's website

**macOS:**
- Check that bundle is not quarantined:
  ```bash
  xattr -dr com.apple.quarantine SketchbookReveal.bundle
  ```

**Both:**
- Try Debug build for better error messages
- Check software's log files

### Missing dependencies

Plugin may need OpenGL drivers:

**Windows:**
- Update graphics drivers from manufacturer
- AMD/NVIDIA/Intel driver websites

**macOS:**
- OpenGL is built-in, update macOS if needed

## Custom Plugin Folder

Most VJ software allows setting a custom plugin folder:

**Resolume:**
1. Preferences → Video
2. "FFGL Effects Folders"
3. Add custom path

**VDMX:**
1. Preferences → Plugins
2. Add custom search path

This allows keeping plugins separate from software installation.

## Uninstalling

Simply delete the plugin file:

**Windows:**
```bash
del "C:\Program Files\Resolume Arena\plugins\vfx\SketchbookReveal.dll"
```

**macOS:**
```bash
rm "/Applications/Resolume Arena.app/Contents/MacOS/plugins/vfx/SketchbookReveal.bundle"
```

Restart the software after removal.

## Distribution

To share plugins with others:

1. Build in Release mode
2. Zip the `.dll` or `.bundle` file
3. Include installation instructions
4. Note minimum software versions

**Example:**
```
SketchbookReveal_v1.0_Windows.zip
  - SketchbookReveal.dll
  - README.txt (installation instructions)
```

## Automation

### Install Script (Windows)

Create `install.bat`:
```batch
@echo off
copy build\Release\*.dll "C:\Program Files\Resolume Arena\plugins\vfx\"
echo Plugins installed!
pause
```

### Install Script (macOS)

Create `install.sh`:
```bash
#!/bin/bash
cp build/*.bundle "/Applications/Resolume Arena.app/Contents/MacOS/plugins/vfx/"
echo "Plugins installed!"
```

Make executable: `chmod +x install.sh`

## Next Steps

- Test plugin with sample video
- Adjust parameters
- Create presets in VJ software
- Explore [Session Logs](../sessions/) for development notes

---

**Issues?** Check the [Building Guide](building.md) or open a GitHub issue.
