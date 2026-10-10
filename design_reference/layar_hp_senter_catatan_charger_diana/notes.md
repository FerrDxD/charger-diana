# Layar: HP UI (Senter + Catatan)

## Diambil dari referensi:
- Struktur layout: frame HP di tengah layar (diegetic), background adegan 3D di belakang, HUD info di top-left dan top-right, keybind hints di bottom
- Komponen frame HP: status bar (jam, sinyal, baterai), dua tile besar (SENTER, CATATAN), grid thumbnail catatan (3 item), preview SMS di bawah, keybind hints dalam HP
- Warna HP: #0B0A0A (void) untuk background HP, #9FB8C8 (phoneGlow) untuk teks dan border aktif, #D8D2C4 (paper) untuk teks normal, #3E4A3A (mold) untuk border non-aktif, #8B1E1E (blood) untuk highlight penting
- Warna HUD luar: #D8D2C4 untuk teks info, #C9A227 (warn) untuk peringatan
- Font: VT323 untuk semua UI HP (size 12-18), Special Elite untuk preview teks catatan (size 8-10)
- Format teks: uppercase untuk judul dan label, jam format 24 jam (23:47)
- Baterai: pixel-style icon dengan fill percentage
- Thumbnail catatan: mini preview dengan teks Special Elite, angka index di pojok

## Diabaikan dari referensi:
- Background image kelas gelap (adegan 3D asli yang tampil di belakang HP)
- CRT scanlines dan screen noise overlay (akan dibuat sebagai shader terpisah di milestone 9)
- Glass crack effect di pojok HP (opsional, flavor visual)
- Animasi pulse/blink (tambah di polish milestone jika perlu)
- Real-time clock JavaScript (clock statis atau dari sistem)
- Pesan SMS Bayu (opsional, bisa ditambah sebagai lore di milestone polish)
- Hover effects CSS (Godot selection states sudah menangani)

## Dikoreksi ke DESIGN.md:
- Corner radius 0 (tajam) untuk semua elemen frame HP dan tile
- Warna hanya menggunakan 6 token yang diizinkan
- Font VT323 untuk UI HP, Special Elite hanya untuk preview catatan
- Border 1-2px solid, tidak ada shadow atau glow
- Teks outline 2px gelap untuk semua label di luar HP
- Thumbnail catatan menggunakan preview teks, bukan gambar baked

## Perlu keputusan pengguna:
- Tidak ada

## Cara tes:
- Buka scene PhoneUi.tscn di Godot editor (atau buat baru jika belum ada)
- Cek layout: frame HP di tengah, status bar top, dua tile di tengah, thumbnail di bawah, SMS preview di footer
- Test navigasi keyboard (A/D atau Arrow Left/Right) untuk pilih tile
- Test ENTER untuk buka catatan/gallery
- Test F untuk toggle senter
- Cek warna baterai berubah ke warn (#C9A227) saat < 10%
- Test E untuk tutup HP
