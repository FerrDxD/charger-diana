# Layar: In-Game HUD

## Diambil dari referensi:
- Struktur layout: crosshair di tengah, prompt interaksi di bottom-center, baterai/stamina di bottom-left
- Komponen: crosshair (2px dot), prompt label dengan outline, battery icon pixel-style, stamina bar tipis
- Warna: #D8D2C4 (paper) untuk teks, #9FB8C8 (phoneGlow) untuk baterai, #3E4A3A (mold) untuk stamina bar background
- Font: VT323, size 24 untuk prompt, 20 untuk baterai, 32 untuk subtitle
- Format teks: "[E] BUKA PINTU" uppercase untuk prompt, "HP 35%" untuk baterai
- Keybind hints: secondary hints di bawah prompt dengan opacity 60%

## Diabaikan dari referensi:
- CRT scanlines dan vignette overlay (akan dibuat sebagai shader terpisah di milestone 9)
- Background image (adegan 3D asli di belakang UI)
- Gradient dan shadow CSS (tidak sesuai token DESIGN.md)
- Animasi flicker (akan ditambah di polish milestone)

## Dikoreksi ke DESIGN.md:
- Corner radius 0 (tajam) untuk semua elemen
- Warna hanya menggunakan 6 token yang diizinkan
- Teks outline 2px gelap untuk semua label di atas adegan
- Font VT323 untuk semua UI (bukan font lain)

## Perlu keputusan pengguna:
- Tidak ada

## Cara tes:
- Buka scene TestLevel.tscn di Godot editor
- Play scene
- Cek posisi elemen: crosshair tengah, prompt bottom-center, baterai/stamina bottom-left
- Interaksi dengan objek untuk melihat prompt muncul
- Cek warna baterai berubah ke warn (#C9A227) saat < 10%
