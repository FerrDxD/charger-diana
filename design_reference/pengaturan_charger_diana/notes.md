# Layar: Pengaturan (Settings)

## Diambil dari referensi:
- Struktur layout: header info sistem di top, panel pengaturan di tengah, tips navigasi di bawah panel, footer keybind di bottom
- Komponen: header dengan "[SYS.CONFIG]" dan versi, title "PENGATURAN SISTEM", 8 opsi pengaturan (slider, toggle, choice, action), tips navigasi, keybind hints
- Warna: #D8D2C4 (paper) untuk teks normal, #8B1E1E (blood) untuk selection/focus, #9FB8C8 (phoneGlow) untuk nilai aktif, #C9A227 (warn) untuk warning, #3E4A3A (mold) untuk divider dan nilai mati
- Font: VT323 untuk semua UI (size 24-32 untuk menu, 18 untuk tips/caption)
- Format teks: uppercase untuk judul dan label setting, prefix ">" untuk selected item
- Slider: stepped pixel blocks (■ dan □), 10 langkah total
- Toggle: bracketed binary state "[ AKTIF ]" atau "[ MATI ]"
- Choice: ditampilkan dengan < VALUE > di antara kurung siku

## Diabaikan dari referensi:
- Background image dithered sekolah (akan diganti dengan solid color atau adegan 3D nanti)
- CRT scanlines dan vignette overlay (akan dibuat sebagai shader terpisah di milestone 9)
- Gradient dan shadow CSS (tidak sesuai token DESIGN.md)
- Real-time clock JavaScript (clock statis atau diambil dari sistem nanti)
- Animasi hover/transition CSS (Godot button/selection states sudah menangani)
- Info memori "1024KB [OK]" (hanya flavor text, opsional)

## Dikoreksi ke DESIGN.md:
- Corner radius 0 (tajam) untuk semua elemen
- Warna hanya menggunakan 6 token yang diizinkan
- Font VT323 untuk semua UI (bukan font lain)
- Border 1-2px solid, tidak ada shadow atau glow
- Teks outline 2px gelap untuk semua label
- Slider menggunakan ■ dan □ (Unicode character), bukan grafik

## Perlu keputusan pengguna:
- Tidak ada

## Cara tes:
- Buka scene Settings.tscn di Godot editor (atau buat baru jika belum ada)
- Cek layout: header top, panel tengah, tips di bawah panel, footer bottom
- Test navigasi keyboard (W/S atau Arrow Up/Down) untuk fokus antar opsi
- Test A/D atau Arrow Left/Right untuk mengubah nilai slider/toggle
- Cek warna selection (#8B1E1E) dan outline di selected item
- Tekan ESC untuk kembali ke menu utama
