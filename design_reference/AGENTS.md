# AGENTS.md: design_reference/

Pedoman khusus untuk AI agent yang bekerja dengan **hasil desain Google Stitch** di folder ini. Pedoman umum proyek ada di `AGENTS.md` di direktori utama; file ini **melengkapi**-nya, bukan menggantikan.

> **Aturan emas:** isi folder ini adalah **referensi visual**, bukan kode yang boleh disalin. Tugasmu adalah **mengambil komponen spesifik**, lalu membangunnya ulang secara native di Godot.

---

## 1. Isi Folder & Statusnya

Folder ini berisi hasil ekspor dari Stitch (screenshot, HTML/CSS, gambar) untuk game **Charger Diana**.

```
design_reference/
  AGENTS.md              # file ini
  .gdignore              # supaya Godot tidak mengimpor isi folder ini
  screens/
    main_menu/
      screenshot.png     # sumber kebenaran visual
      code.html          # hasil ekspor Stitch (HANYA bacaan)
      notes.md           # apa yang diambil / diabaikan dari layar ini
    hud/
    phone_ui/
    note_reader/
    pause_menu/
    settings/
    game_over/
    ending_a/
    ending_b/
    intro/
  assets/                # gambar/ikon yang secara eksplisit boleh dipakai
```

**Status folder: READ-ONLY.**
- Jangan mengedit, memformat ulang, atau menghapus file di sini kecuali pengguna meminta.
- Jangan menambahkan file baru kecuali `notes.md` yang diminta, atau aset yang ditambahkan pengguna.
- Folder ini **tidak boleh ikut ke build/export** dan **tidak boleh diimpor Godot** (lihat bagian 8).

## 2. Hierarki Sumber Kebenaran

Kalau ada konflik, urutan prioritas dari tertinggi:

1. **Instruksi pengguna** di percakapan saat ini
2. **`DESIGN.md`** (token warna, font, prinsip UI, copywriting)
3. **`screenshot.png`** layar terkait (tata letak dan tampilan)
4. **`notes.md`** layar terkait (apa yang diambil/diabaikan)
5. **`code.html`** (hanya untuk membaca struktur dan nilai)
6. Asumsi agent (paling rendah, hindari)

Stitch sering "menyimpang" dari brief: sudut membulat, gradien, warna ekstra, font lain. Kalau begitu, **ikuti `DESIGN.md`**, bukan hasil Stitch.

## 3. Yang Boleh Diambil vs Dilarang

### Boleh diambil (diekstrak, bukan disalin mentah)

| Kategori | Contoh |
|---|---|
| **Struktur layout** | Hierarki elemen, posisi relatif (kiri-bawah, tengah-atas), jarak antar elemen |
| **Inventaris komponen** | Daftar komponen yang ada di layar (tombol menu, bar baterai, kotak subtitle) |
| **Nilai desain** | Warna, ukuran font, tebal border, padding, **yang cocok dengan token `DESIGN.md`** |
| **Teks UI** | Copywriting bahasa Indonesia (verifikasi dengan `DESIGN.md` bagian 6) |
| **Ikon/gambar** | Hanya yang ada di `assets/` atau yang disebut eksplisit di `notes.md` |

### Dilarang

- **Menyalin HTML, CSS, Tailwind, atau JavaScript** ke proyek Godot dalam bentuk apa pun.
- **Menambahkan dependensi web** (CDN Tailwind, Google Fonts via `<link>`, library JS).
- **Memakai screenshot utuh** sebagai latar atau sebagai UI (gambar flat yang ditaruh di `TextureRect` full-screen).
- **Memakai gambar yang teksnya sudah "dipanggang"** (baked text). Teks harus selalu `Label` supaya bisa diganti dan diterjemahkan.
- **Memakai grain/dithering/vignette yang ada di dalam screenshot.** Efek itu dibuat sebagai overlay shader terpisah (lihat `ARCHITECT.md` bagian 14).
- **Meniru bug atau kesalahan Stitch**: teks lorem ipsum, ikon generik modern, elemen yang tidak ada di brief.
- **Mengubah `DESIGN.md`** atau token warna/font berdasarkan apa yang tampak di hasil Stitch.
- **Menebak** bagian layar yang tidak terlihat atau ambigu. Tanyakan.

## 4. Alur Kerja Wajib

Setiap kali mengerjakan satu layar UI:

1. **Baca** `DESIGN.md` (token, prinsip, copywriting) dan bagian UI di `ARCHITECT.md` (bagian 13).
2. **Buka** `screens/<nama>/screenshot.png` dan `notes.md`.
3. **Buat daftar komponen** layar itu (inventaris), misalnya:
   - kontainer utama
   - judul
   - daftar tombol
   - indikator baterai
4. **Petakan** tiap komponen ke node Godot (tabel bagian 5).
5. **Cek token**: pastikan warna/font/border mengikuti `DESIGN.md`. Koreksi penyimpangan Stitch.
6. **Bangun** scene `.tscn` dan script C# sesuai pedoman `AGENTS.md` utama (namespace, penamaan, signal).
7. **Bandingkan** hasil di editor dengan screenshot (tata letak, proporsi, hierarki). Kemiripan **struktur**, bukan piksel-per-piksel.
8. **Laporkan** hasilnya dengan format di bagian 7.

Kerjakan **satu layar per tugas**. Jangan membangun semua layar sekaligus.

## 5. Pemetaan Web ke Godot

Dipakai untuk **menerjemahkan niat desain**, bukan menyalin kodenya.

| Di HTML/CSS Stitch | Di Godot |
|---|---|
| `div` flex kolom | `VBoxContainer` |
| `div` flex baris | `HBoxContainer` |
| Grid | `GridContainer` |
| `position: absolute` / posisi tetap | `Control` dengan **anchor** + offset (hindari posisi piksel absolut) |
| Padding/margin | `MarginContainer` atau `theme_override_constants` |
| Teks | `Label` (satu baris) / `RichTextLabel` (jika perlu format) |
| Tombol | `Button` dengan `StyleBoxFlat` dari Theme |
| `border: 1px solid` | `StyleBoxFlat` (`border_width_*`, `corner_radius = 0`) |
| `background-color` | `StyleBoxFlat.bg_color` atau `ColorRect` |
| `font-family` / `font-size` | Font di `Theme` (VT323, Special Elite, Rock Salt) + `font_size` |
| `text-shadow` / outline | `outline_size` + `font_outline_color` pada Label |
| Progress bar | `ProgressBar` atau `TextureProgressBar` dengan `StyleBoxFlat` |
| Hover/active | State `hover` / `pressed` / `focus` pada StyleBox, bukan logika manual |
| Animasi CSS | `AnimationPlayer` / `Tween` (hanya jika diminta; default tanpa animasi) |
| Gambar latar adegan | **Dilewati** (adegan 3D asli yang tampil di belakang UI, bukan gambar) |

## 6. Aturan Gaya yang Tidak Boleh Dilanggar

Ringkasan token dari `DESIGN.md`. Kalau Stitch memberi hasil berbeda, **koreksi ke sini**.

- Warna hanya: `#0B0A0A`, `#D8D2C4`, `#8B1E1E`, `#9FB8C8`, `#C9A227`, `#3E4A3A`.
- Satu aksen merah (`#8B1E1E`) per layar.
- Font: **VT323** (UI), **Special Elite** (catatan), **Rock Salt** (judul saja).
- Sudut **tajam** (`corner_radius = 0`), border 1-2 px.
- Tanpa gradien halus, glow, blur, atau glassmorphism.
- Teks di atas adegan memakai outline gelap 2 px.
- Semua teks tampil dalam **bahasa Indonesia**.
- Filter tekstur **nearest** untuk ikon bitmap.
- **Humor hanya di `ending_b`.**

Semua gaya dipusatkan di **satu `Theme` resource** (`resources/ui_theme.tres`). **Jangan** menaruh warna/font langsung di tiap node kalau bisa lewat Theme.

## 7. Format Laporan Setelah Selesai

Setelah satu layar selesai, tulis ringkasan seperti ini:

```
Layar: <nama>
Scene:  scenes/ui/<Nama>.tscn
Script: scripts/ui/<Nama>.cs

Diambil dari referensi:
- <struktur/komponen/nilai yang dipakai>

Diabaikan dari referensi (dan alasan):
- <mis. gradien latar: melanggar token; teks lorem: bukan copy final>

Dikoreksi ke DESIGN.md:
- <mis. corner radius 8 -> 0; font Inter -> VT323>

Perlu keputusan pengguna:
- <hal ambigu, kalau ada>

Cara tes: <langkah singkat di editor>
```

Kalau tidak ada yang perlu dikoreksi atau ditanyakan, tulis "tidak ada".

## 8. Catatan Teknis Folder

- Buat file kosong **`design_reference/.gdignore`**. Godot akan mengabaikan folder ini sepenuhnya (tidak diimpor, tidak muncul di FileSystem dock). Ini juga **menghemat waktu import** di laptop Celeron.
- Pastikan folder ini **tidak masuk preset export** (periksa filter export).
- Boleh dikomit ke Git, tetapi **jangan** Git LFS-kan kecuali ukurannya besar.
- Kalau ada `screenshot.png` yang ukurannya besar, jangan dibaca berulang tanpa perlu.

## 9. Kapan Harus Bertanya

Hentikan dan tanyakan ke pengguna kalau:

- Screenshot dan `DESIGN.md` bertentangan pada hal yang bukan sekadar token (mis. susunan elemen berbeda dari brief).
- Ada komponen di layar yang tidak jelas fungsinya atau tidak ada di PRD.
- Layar yang diminta belum ada foldernya di `screens/`.
- Referensi menuntut efek yang berat untuk iGPU (blur, banyak lapisan transparan).
- Teks di referensi terbaca tidak jelas.

Lebih baik bertanya satu kali daripada membangun versi yang salah dan harus dibuang.

## 10. Ringkasan Cepat

1. Referensi, **bukan** kode untuk disalin.
2. `DESIGN.md` mengalahkan Stitch.
3. Ambil **struktur, komponen, nilai**; bangun ulang dengan `Control` dan `Theme`.
4. Satu layar per tugas, laporkan yang diambil/diabaikan/dikoreksi.
5. Ragu? Tanya.
