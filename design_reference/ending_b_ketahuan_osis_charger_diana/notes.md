# Layar: Ending B (Ketahuan OSIS - Tertangkap Basah)

## Diambil dari referensi:
- Struktur layout: header status di top, konten narasi di tengah, dialog box di bottom, footer metadata di bawah dialog
- Komponen: header (insiden status, lokasi, waktu, status), badge "[ ENDING 2 / 3 : TERTANGKAP BASAH ]", judul besar "TAMAT?" dengan underline, subtext "Tidak ada kutukan kuno. Tidak ada arwah penasaran. Hanya surat panggilan BK.", dialog box interogasi OSIS, tombol "[E] MENU UTAMA", footer metadata
- Warna: #D8D2C4 (paper) untuk teks normal, #9FB8C8 (phoneGlow) untuk nama Diana dan highlight, #8B1E1E (blood) untuk judul, badge, dan OSIS labels, #C9A227 (warn) untuk status interogasi dan OSIS labels, #3E4A3A (mold) untuk border dan divider
- Font: VT323 untuk UI (judul besar, prompt, metadata), Special Elite untuk body text narasi (size 18)
- Format teks: uppercase untuk semua label dan nama karakter, tracking wide untuk judul
- Dialog: script format dengan nama di kiri, dialog di kanan, indentasi untuk Diana

## Diabaikan dari referensi:
- Background image lorong kelas (adegan 3D asli atau solid color #0B0A0A)
- CRT scanlines dan vignette overlay (akan dibuat sebagai shader terpisah di milestone 9)
- Background hover transition pada tombol (Godot button states sudah menangani)
- Blinking pagination prompt ▼ (opsional, tidak perlu untuk ending screen)
- KONSOL INDIE PSX // GODOT 4 CONTROL (opsional flavor text, bisa dihapus)

## Dikoreksi ke DESIGN.md:
- Corner radius 0 (tajam) untuk semua elemen termasuk dialog box dan tombol
- Warna hanya menggunakan 6 token yang diizinkan
- Font VT323 untuk UI, Special Elite hanya untuk narasi body
- Border 1-2px solid, tidak ada shadow atau glow kecuali outline teks
- Teks outline 2px gelap untuk semua label
- Tombol menggunakan StyleBoxFlat dengan border, bukan background transition
- Humor hanya di dialog interogasi (sesuai PRD)

## Perlu keputusan pengguna:
- Tidak ada

## Cara tes:
- Buka scene EndingScreen.tscn (atau buat khusus Ending B jika belum ada)
- Cek layout: header top, judul tengah, dialog box bottom
- Test E atau ENTER untuk kembali ke menu utama
- Cek dialog interogasi dengan format nama dan teks
- Cek humor ceramah OSIS (punchline "Charger Diana")
