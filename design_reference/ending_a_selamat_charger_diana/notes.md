# Layar: Ending A (Selamat - Survived)

## Diambil dari referensi:
- Struktur layout: header status di top, konten narasi di tengah, footer metadata di bottom
- Komponen: header (REC status, lokasi, waktu, status), badge "[ ENDING 1 / 3 : SURVIVED ]", kalimat utama "Diana pulang dengan charger di tangan.", divider line merah, box status baterai (1% + charging bolt), narasi relief, tombol "[E] MENU UTAMA", footer metadata
- Warna: #D8D2C4 (paper) untuk teks utama, #9FB8C8 (phoneGlow) untuk highlight aktif dan waktu, #8B1E1E (blood) untuk REC indicator dan divider, #C9A227 (warn) untuk baterai kritis, #3E4A3A (mold) untuk border dan secondary text
- Font: VT323 untuk semua UI (size 48-64 untuk kalimat utama, 18-24 untuk metadata)
- Format teks: uppercase untuk semua label dan narasi, tracking wide
- Baterai: pixel-style icon dengan minimal fill (1%), charging bolt [⚡]

## Diabaikan dari referensi:
- Background image gerbang pagi (adegan 3D asli atau solid color #0B0A0A)
- CRT scanlines, vignette, dither noise, flicker (akan dibuat sebagai shader terpisah di milestone 9)
- Background hover transition pada tombol (Godot button states sudah menangani)
- KONSOL INDIE PSX // GODOT 4 CONTROL (opsional flavor text, bisa dihapus)

## Dikoreksi ke DESIGN.md:
- Corner radius 0 (tajam) untuk semua elemen termasuk tombol
- Warna hanya menggunakan 6 token yang diizinkan
- Font VT323 untuk semua UI
- Border 1-2px solid, tidak ada shadow atau glow kecuali outline teks
- Teks outline 2px gelap untuk semua label
- Tombol menggunakan StyleBoxFlat dengan border, bukan background transition

## Perlu keputusan pengguna:
- Tidak ada

## Cara tes:
- Buka scene EndingScreen.tscn di Godot editor (atau buat khusus Ending A jika belum ada)
- Cek layout: header top, narasi tengah, tombol bottom
- Test E atau ENTER untuk kembali ke menu utama
- Cek badge ending dan divider line merah
- Cek baterai icon dengan 1% fill dan charging bolt
