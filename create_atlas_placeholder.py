import struct

# Buat PNG sederhana 256x256 dengan warna coklat (R=137, G=96, B=62)
# Format PNG dasar tanpa kompresi

width = 256
height = 256
r, g, b = 137, 96, 62

# Header PNG
png_signature = b'\x89PNG\r\n\x1a\n'

# IHDR chunk
ihdr_data = struct.pack('>IIBBBBB', width, height, 8, 2, 0, 0, 0)
ihdr_chunk = struct.pack('>I', 13) + b'IHDR' + ihdr_data + struct.pack('>I', 0x21_58_52_96)

# Create scanlines (RGB, no alpha)
scanlines = b''
for y in range(height):
    scanlines += b'\x00'  # Filter type (none)
    for x in range(width):
        scanlines += bytes([r, g, b])

# IDAT chunk (uncompressed)
import zlib
compressed = zlib.compress(scanlines)
idat_chunk = struct.pack('>I', len(compressed)) + b'IDAT' + compressed + struct.pack('>I', zlib.crc32(b'IDAT' + compressed) & 0xFFFFFFFF)

# IEND chunk
iend_chunk = struct.pack('>I', 0) + b'IEND' + struct.pack('>I', 0xAE_42_60_82)

# Combine all
png_data = png_signature + ihdr_chunk + idat_chunk + iend_chunk

# Save
with open('C:/Users/Hype GLK/charger-diana/assets/textures/atlas/atlas_furniture_placeholder.png', 'wb') as f:
    f.write(png_data)

print("Atlas placeholder created: atlas_furniture_placeholder.png")
