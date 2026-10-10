# AGENTS.md: Pedoman untuk AI Agent (Charger Diana)

Dokumen ini adalah aturan kerja untuk AI agent (Claude Code, Codex, Cursor, dll.) yang mengerjakan repo ini. Baca sampai habis sebelum menyentuh kode.

> Aturan khusus untuk hasil desain Stitch ada di `design_reference/AGENTS.md`. Wajib dibaca sebelum mengerjakan apa pun yang menyangkut UI.

## 1. Urutan Baca

1. `PRD_V2.md`: apa yang dibuat, milestone, prioritas. **Sumber kebenaran untuk cakupan.** (`PRD.md` v0.3 sudah usang, jangan dipakai.)
2. `ARCHITECT.md`: bagaimana dibuat, struktur folder, autoload, kontrak antar sistem.
3. `DESIGN.md`: token visual, komponen, daftar layar, copywriting.
4. `design_reference/AGENTS.md`: cara memakai screenshot/HTML hasil Stitch (hanya untuk tugas UI).

Kalau dokumen bertentangan, urutan prioritasnya: PRD_V2 > ARCHITECT > DESIGN > AGENTS. Jangan diam-diam memilih sendiri. Catat konflik di laporan dan tanyakan.

## 2. Fakta Proyek

| Item | Nilai |
|---|---|
| Game | **Charger Diana**: horror first-person 3D, sekolah malam, durasi 25-35 menit |
| Engine | Godot 4.x **.NET (C#)**, renderer **Compatibility** |
| Platform | PC Windows, input **hanya mouse + keyboard** (tanpa gamepad) |
| Hardware target | Intel Celeron, RAM 8 GB, iGPU. Performa adalah fitur |
| Bahasa | Teks dan voice acting **Bahasa Indonesia** |
| Distribusi | itch.io, "bayar sesukamu" |
| Audio | Ambience + SFX saja. **Tanpa musik berlagu** |
| Status | MVP (milestone 1-10 PRD v0.3) diklaim selesai. Sekarang mengerjakan PRD V2 |

### Karakter (nama final)
- **Diana**: MC. Gen Z yang selalu butuh HP dan tidak punya casan lain. Panik beneran, bukan bercanda. Suara: teman developer.
- **OSIS patroli**: **Cecillia, Adam, Ferdi**. Semua diisi suara developer, dibedakan lewat nada, tempo, dan filter radio HT. Sifat tiap karakter ditetapkan di milestone E5, jadi simpan sebagai data (`OsisProfile`), jangan di-hardcode.
- **Kuntilanak**: satu-satunya hantu di 1.0. Sumber suaranya belum diputuskan. Pakai placeholder dan jangan mengarang keputusan.

## 3. Cara Kerja per Milestone

1. **Kerjakan satu milestone sekaligus**, sesuai ID di `PRD_V2.md` bagian 7 (A1, B1, C2, dst.). Jangan lompat atau menggabungkan milestone tanpa diminta.
2. Sebelum mengubah kode: tulis rencana singkat (file yang disentuh, risiko). Untuk perubahan kecil cukup 2-3 baris.
3. Kerjakan hanya yang tercantum di "Deliverable & kriteria selesai". Ide tambahan masuk ke bagian "Saran" di laporan, bukan ke kode.
4. Setiap milestone harus menghasilkan **build yang bisa dimainkan** (prinsip kerja PRD V2 no. 3). Jangan tinggalkan repo dalam keadaan tidak bisa dibuka.
5. Akhiri dengan laporan singkat: apa yang diubah, cara mengujinya, apa yang belum, dan asumsi yang diambil.
6. Prioritas **Must > Should > Could**. Kalau diminta memotong, buang dari bawah. Jangan mengerjakan Could sebelum semua Must/Should selesai.

### Jangan berasumsi soal MVP
PRD V2 hanya *menduga* isi MVP. Milestone **A1** yang memverifikasinya. Sebelum membangun di atas sebuah sistem, **cek dulu kodenya**, dan kalau ternyata tidak ada atau berbeda, laporkan. Jangan menulis ulang sistem yang sudah jalan tanpa alasan tertulis.

## 4. Aturan Kode (C# / Godot)

- Bahasa: identifier, komentar, dan nama commit dalam **bahasa Inggris**. String yang tampil ke pemain dalam **bahasa Indonesia** dan dikumpulkan di satu tempat (lihat `ARCHITECT.md` bagian Data & String), bukan tersebar di scene.
- Satu class per file, nama file = nama class (PascalCase). Node script memakai `partial class`.
- Nilai tuning (kecepatan, durasi baterai, radius suara, dsb.) lewat `[Export]` atau `Resource`. **Tidak ada angka ajaib** di logika.
- Komunikasi antar sistem lewat **signal / `EventBus`**, bukan `GetNode("../../..")` panjang.
- **Autoload hanya yang tercantum di `ARCHITECT.md`.** Mau menambah autoload baru: ubah `ARCHITECT.md` dulu, sebutkan alasannya.
- Logika murni (hitung baterai, skor deteksi, skema save, resolusi preset) taruh di `scripts/core/` **tanpa dependensi Godot** supaya bisa diuji terpisah. Build C# lambat di Celeron.
- Hindari alokasi per-frame di `_Process` / `_PhysicsProcess` (string baru, LINQ, array baru). Cache referensi node di `_Ready`.
- Tidak ada `Thread` atau `async` yang menyentuh scene tree tanpa `CallDeferred`.
- Jangan membiarkan warning baru. Build harus bersih (`dotnet build`).
- Jangan commit file di luar kebutuhan: `.godot/`, `.mono/`, `bin/`, `obj/`, hasil export, rekaman VO mentah.

## 5. Aturan Konten & Nada

- **Humor hanya di Ending B** (tertangkap OSIS). Selain itu murni horror. Jangan menyelipkan lelucon di UI, catatan, atau dialog lain.
- Motivasi Diana sengaja receh dan dimainkan serius. **Jangan menambah alasan** seperti "besok ada hal penting".
- Telegraph kuntilanak harus konsisten dan adil: tawa terbalik, bau melati, lampu berkedip. Jangan membuat kematian yang tidak bisa dipelajari.
- Prolog **tidak boleh membunuh pemain** dan tidak punya musuh.
- Peringatan konten (horror, suara keras, jumpscare, lampu berkedip) wajib ada, dan opsi **Kurangi kedip** harus benar-benar mematikan efek berkedip, termasuk event terskrip.

## 6. Aturan Alur Game & Penyimpanan

- Perpindahan scene besar (boot, menu, prolog, babak, ending, kredit) **hanya lewat `SceneManager`**. Jangan memanggil `GetTree().ChangeSceneToFile` langsung dari sistem lain.
- Pengaturan disimpan di `user://settings.cfg` lewat `SettingsManager`. **Setiap opsi pengaturan harus langsung berefek dan tersimpan.** Jangan ada opsi dekoratif.
- Save game: **satu slot**, auto-save di checkpoint, lewat `SaveSystem`. Format dan versioning ada di `ARCHITECT.md`. Save lama yang tidak cocok harus ditolak dengan aman (tidak crash).
- Pause (`Esc`) menghentikan game sungguhan (`GetTree().Paused`) dan meredam ambience.
- Log ditulis ke `user://logs/` lewat `GameLog`. Jangan memakai `GD.Print` mentah untuk hal yang perlu dilacak playtester.
- Prolog bisa dilewati (tahan 1 detik) setelah pertama kali. Setiap cutscene lewat `CutsceneController`.

## 7. Aturan UI (ringkas)

Detail lengkap di `DESIGN.md` dan `design_reference/AGENTS.md`. Yang paling penting:

- **UI mengikuti desain, bukan sebaliknya.** Satu `ui_theme.tres`, tanpa override gaya per node.
- Hanya **6 warna token**, satu aksen merah. Sudut tajam, border 1-2 px, tanpa gradien/glow/blur/panel membulat.
- Semua teks adalah `Label`/`RichTextLabel`. Dilarang teks "dipanggang" ke gambar.
- Komponen bersama (`MenuButton`, `BatteryIndicator`, `SubtitleBox`, dst.) dibangun sekali di `scenes/ui/components/` lalu dipakai ulang. Jangan menduplikasi.
- Referensi Stitch **bukan kode untuk disalin**. Ambil hanya struktur dan komponen yang dibutuhkan, lalu tulis ulang dengan Control node Godot dan Theme. Setiap tugas UI diakhiri **laporan diambil/diabaikan/dikoreksi** (format di `design_reference/AGENTS.md`).
- Dukung 1366x768, 1600x900, 1920x1080. Uji ketiganya.

## 8. Aturan Performa

Target: **30 FPS stabil minimum** di Celeron/8 GB pada preset Rendah (45+ ideal), memori < 1.5 GB, menu ke gameplay < 15 detik, build < 500 MB.

- Cek FPS di akhir tiap fase. Kalau sebuah perubahan menurunkan FPS, laporkan sebelum lanjut.
- Bayangan real-time hanya senter, dan hanya di preset Sedang ke atas. Batasi jumlah lampu aktif lewat preset grafis.
- Tekstur kecil (gaya PS1), filter nearest. Jangan menambah aset besar.
- Audio: OGG mono untuk VO. Tidak ada WAV mentah di build.
- Shader efek (grain, dither, vignette, PS1) harus bisa dimatikan dan dicatat biaya GPU-nya.

## 9. Aset & Lisensi

- Setiap aset eksternal (tekstur, model, SFX, ambience, font) **dicatat sumber + lisensinya** di `assets/ASSETS.md` pada saat dimasukkan. Data ini dipakai layar Kredit.
- Jangan memasukkan aset tanpa lisensi jelas. Kalau ragu, jangan masukkan, tanyakan.
- Font tetap: **VT323** (UI), **Special Elite** (catatan), **Rock Salt** (judul).
- Rekaman VO: simpan master di luar repo, yang masuk repo hanya OGG mono hasil kompres. Nama berkas mengikuti `ARCHITECT.md` (Audio).

## 10. Kapan Harus Bertanya

**Bertanya dulu** bila:
- Dokumen saling bertentangan, atau milestone butuh keputusan desain yang belum ada (mis. sumber suara kuntilanak, sifat karakter OSIS).
- Perubahan menyentuh lebih dari satu sistem inti, mengubah format save, atau menambah autoload.
- Perlu menghapus fitur MVP yang sudah jalan.

**Putuskan sendiri dan catat di laporan** bila:
- Detail implementasi kecil yang tidak mengubah perilaku yang terlihat pemain.
- Nama variabel, struktur internal class, urutan kode.

## 11. Definisi Selesai (setiap milestone)

- [ ] Kriteria "Deliverable & kriteria selesai" milestone terpenuhi.
- [ ] `dotnet build` bersih, tanpa warning baru.
- [ ] Game bisa dijalankan dari menu (atau dari scene uji yang disebutkan) tanpa error di log.
- [ ] Nilai tuning ada di `[Export]`/Resource.
- [ ] Untuk UI: checklist UI parity (`PRD_V2.md` 6.5) lulus untuk layar yang disentuh, laporan ditulis.
- [ ] Uji singkat dilakukan dan hasilnya ditulis (apa yang dites, apa yang belum).
- [ ] Aset baru tercatat di `assets/ASSETS.md`.
- [ ] Tidak ada pekerjaan di luar cakupan milestone.

## 12. Yang Dilarang

- Musik berlagu, dukungan gamepad, hantu tambahan (pocong, genderuwo), localization selain Indonesia, port non-Windows. Semuanya di luar cakupan 1.0.
- Mengerjakan Ending D, trailer, statistik, atau mode "Santai" (Could) sebelum Must/Should selesai.
- Menyalin kode HTML/CSS dari Stitch ke dalam game, atau men-"bake" teks ke gambar.
- Menambah humor di luar Ending B.
- Menulis ulang sistem yang sudah jalan "supaya lebih bersih" tanpa milestone yang meminta itu.
