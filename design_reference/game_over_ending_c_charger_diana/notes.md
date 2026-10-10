# Layar: Game Over (Ending C - Tertangkap Kuntilanak)

## Diambil dari referensi:
- Struktur layout: minimalis, elemen tersebar di layar tanpa panel
- Komponen: top metadata (berkas perkara, rekaman kamera), center epitaph "DIANA TIDAK PERNAH PULANG.", divider line, dua tombol action ("[E] COBA LAGI", "[ESC] MENU UTAMA"), bottom metadata (status siswa, penyebab), notice box opsional di bawah tombol
- Warna: #D8D2C4 (paper) untuk teks normal (opacity rendah), #8B1E1E (blood) untuk epitaph utama dan metadata penting, #3E4A3A (mold) untuk secondary metadata
- Font: VT323 untuk semua UI (size 56-68 untuk epitaph, 24 untuk tombol, 18 untuk metadata)
- Format teks: uppercase untuk semua label, tracking wide untuk epitaph
- Epitaph: "DIANA TIDAK PERNAH PULANG." (murni horror, tanpa humor)
- Metadata: "HILANG DI GEDUNG SEKOLAH TUA", "ENTITAS TIDAK DIKENAL (KUNTILANAK)"

## Diabaikan dari referensi:
- Background image lorong dengan siluet kuntilanak (adegan 3D asli atau solid color #0B0A0A)
- CRT scanlines dan vignette overlay (akan dibuat sebagai shader terpisah di milestone 9)
- Background gradient overlay (solid color void sudah cukup)
- Flicker/glyph transform animation pada epitaph (opsional, bisa ditambah di polish)
- Hover opacity transition pada tombol (Godot button states sudah menangani)
- Notice box "PINTU KELUAR TERKUNCI DARI LUAR" (opsional flavor text)
- SINYAL AUDIO: DERAU STATIK LEMAH (opsional flavor text)
- KONSOL INDIE PSX // GODOT CANVAS ENGINE (opsional flavor text, bisa dihapus)

## Dikoreksi ke DESIGN.md:
- Corner radius 0 (tajam) untuk semua elemen
- Warna hanya menggunakan 6 token yang diizinkan
- Font VT323 untuk semua UI
- Border tidak diperlukan (minimalis layout)
- Teks outline 2px gelap untuk epitaph agar terbaca di background gelap
- Tombol menggunakan StyleBoxFlat dengan border, bukan hover opacity transition
- Murni horror, tanpa humor (sesuai PRD)

## Perlu keputusan pengguna:
- Tidak ada

## Cara tes:
- Buka scene GameOver.tscn di Godot editor (atau buat baru jika belum ada)
- Cek layout: top metadata, epitaph tengah, tombol di bawah epitaph, bottom metadata
- Test E untuk coba lagi (load checkpoint)
- Test ESC untuk kembali ke menu utama
- Cek epitaph dengan outline agar terbaca
- Cek metadata di bottom left dan right
