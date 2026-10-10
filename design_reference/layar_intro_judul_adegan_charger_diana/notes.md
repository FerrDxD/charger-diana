# Layar: Intro (Judul + Adegan)

## Diambil dari referensi:
- Struktur layout: minimalis, elemen tersebar di layar tanpa panel
- Komponen: top telemetry (REC stamp), judul lokasi di tengah ("23:47 — SMAN, Ruang Kelas XI"), status baterai HP di bawah judul, prompt "[ SPASI ] LANJUT" di bottom, footer system info
- Warna: #D8D2C4 (paper) untuk teks utama, #C9A227 (warn) untuk baterai dan highlight, #8B1E1E (blood) untuk prompt dan REC indicator, #3E4A3A (mold) untuk secondary text
- Font: VT323 untuk semua UI (size 32 untuk judul, 24 untuk prompt, 18 untuk footer)
- Format teks: uppercase untuk semua label, tracking wide (letter-spacing lebar)
- Baterai: pixel-style blocks dengan fill status
- Subliminal quote: "DIANA BELUM KELUAR DARI RUANGAN" muncul setelah 3.5 detik (opacity fade in)

## Diabaikan dari referensi:
- CRT scanlines dan vignette overlay (akan dibuat sebagai shader terpisah di milestone 9)
- Background image (adegan 3D asli atau solid color #0B0A0A)
- Animasi scanline dan pulse (tambah di polish milestone jika perlu)
- Audio cue JavaScript (akan ditrigger via AudioManager)
- Transisi fade out saat proceed (akan ditrigger via script)
- NTSC-J // 15.734 kHz (opsional flavor text, bisa dihapus untuk sederhana)

## Dikoreksi ke DESIGN.md:
- Corner radius 0 (tajam) untuk semua elemen
- Warna hanya menggunakan 6 token yang diizinkan
- Font VT323 untuk semua UI
- Border tidak diperlukan (minimalis layout)
- Teks outline 2px gelap untuk judul di tengah
- Baterai menggunakan pixel blocks, bukan grafik

## Perlu keputusan pengguna:
- Tidak ada

## Cara tes:
- Buka scene Intro.tscn di Godot editor (atau buat baru jika belum ada)
- Cek layout: top telemetry, judul tengah, baterai di bawah judul, prompt bottom
- Test SPASI atau ENTER untuk proceed ke next scene
- Cek subliminal quote muncul setelah 3.5 detik (fade in)
- Cek teks outline pada judul agar terbaca di background gelap
