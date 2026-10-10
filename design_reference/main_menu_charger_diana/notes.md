# Layar: Main Menu

## Diambil dari referensi:
- Struktur layout: title di top-left, menu items di tengah-kiri, keybind hints di bottom-left, battery status di bottom-right
- Komponen: title "CHARGER DIANA" dengan underline merah, 4 menu items (MULAI, LANJUTKAN, PENGATURAN, KELUAR), battery icon dengan 4 cells
- Warna: #D8D2C4 (paper) untuk title dan teks normal, #8B1E1E (blood) untuk selection dan underline, #C9A227 (warn) untuk battery low
- Font: Rock Salt untuk title (56px), VT323 untuk menu items (40px) dan hints (24px)
- Format teks: uppercase untuk semua menu items, prefix ">" untuk selected item
- Battery cells: stepped pixel-style cells, satu terisi warn untuk status low

## Diabaikan dari referensi:
- Background image (akan diganti dengan solid color atau adegan 3D nanti)
- Gradient dan shadow CSS (tidak sesuai token DESIGN.md)
- Animasi hover JS (Godot button style states sudah menangani ini)
- Flash selection effect (akan ditambah di polish milestone jika perlu)

## Dikoreksi ke DESIGN.md:
- Corner radius 0 (tajam) untuk semua elemen
- Warna hanya menggunakan 6 token yang diizinkan
- Font Rock Salt hanya untuk title, VT323 untuk UI lainnya
- Border 1-2px solid, tidak ada shadow atau glow
- Teks outline 2px gelap untuk semua label

## Perlu keputusan pengguna:
- Tidak ada

## Cara tes:
- Buka scene MainMenu.tscn di Godot editor
- Cek layout: title top-left, menu tengah-kiri, hints bottom-left, battery bottom-right
- Test navigasi keyboard (W/S atau Arrow Up/Down) untuk fokus antar tombol
- Cek warna selection (#8B1E1E) dan outline di selected item
- Tekan ENTER pada MULAI untuk masuk ke TestLevel
