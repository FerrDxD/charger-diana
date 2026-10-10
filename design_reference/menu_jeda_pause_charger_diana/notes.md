# Layar: Pause Menu

## Diambil dari referensi:
- Struktur layout: panel centered dengan border, menu items di dalam panel, background darkened 80%
- Komponen: title "JEDA", 3 menu items (LANJUTKAN, PENGATURAN, KEMBALI KE MENU), objective info di footer panel
- Warna: #D8D2C4 (paper) untuk teks normal, #8B1E1E (blood) untuk selection, #3E4A3A (mold) untuk border, #C9A227 (warn) untuk mission label
- Font: VT323, size 48 untuk title, 32 untuk menu items, 12-14 untuk info
- Format teks: uppercase untuk semua menu items, prefix ">" untuk selected item
- Info tambahan: mission status, notes count, keybind hints di bawah panel, tip di bottom-left

## Diabaikan dari referensi:
- Background image (adegan game akan visible di belakang background darken)
- CRT scanlines dan dither layer (akan dibuat sebagai shader terpisah di milestone 9)
- Gradient dan shadow CSS (tidak sesuai token DESIGN.md)
- Animasi flicker (akan ditambah di polish milestone)
- Clock dan battery status di top-right (redundant dengan HUD)

## Dikoreksi ke DESIGN.md:
- Corner radius 0 (tajam) untuk panel dan semua elemen
- Warna hanya menggunakan 6 token yang diizinkan
- Border 1-2px solid #3E4A3A, tidak ada shadow
- Teks outline 2px gelap untuk semua label
- Background darken menggunakan ColorRect dengan opacity 0.8

## Perlu keputusan pengguna:
- Tidak ada

## Cara tes:
- Buka scene PauseMenu.tscn di Godot editor
- Cek layout: panel centered, menu items di dalam, background darkened
- Test navigasi keyboard (W/S atau Arrow Up/Down) untuk fokus antar tombol
- Cek warna selection (#8B1E1E) dan outline di selected item
- Tekan ENTER pada LANJUTKAN untuk close menu (unpause)
- Tekan ESC untuk toggle pause menu
