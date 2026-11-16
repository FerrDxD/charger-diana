# DESIGN.md: Briefing Desain UI Charger Diana (PRD V2)

Dokumen ini adalah **briefing untuk Google Stitch** sekaligus **spesifikasi visual** untuk implementasi di Godot. Cakupan layar mengikuti `PRD_V2.md` bagian 6.4. Cara memakai hasil Stitch di kode diatur di `design_reference/AGENTS.md`.

> **Catatan penting:** nilai hex di bagian 2 adalah **titik awal** yang disusun ulang untuk V2. Kalau screenshot di `design_reference/` atau `ui_theme.tres` yang sudah ada memakai nilai berbeda, **yang berlaku adalah hasil desain yang sudah disetujui**. Sinkronkan satu kali di milestone B1, lalu jadikan `ui_theme.tres` sebagai satu-satunya sumber.

## 1. Konsep Visual

**Rasa:** UI sederhana, kasar, dan dingin, seperti antarmuka HP murah dan kertas fotokopi di sekolah malam. Mirip estetika horror PS1 Indonesia: sedikit burik, tapi rapi dan terbaca.

Prinsip:
1. **Minimal.** Hanya elemen yang dibutuhkan. Banyak ruang gelap.
2. **Sudut tajam, border 1-2 px.** Tanpa sudut membulat, gradien, glow, blur, bayangan lembut.
3. **Satu aksen merah.** Merah dipakai hemat: item terpilih, peringatan, "TAMAT?", teks game over.
4. **Teks sungguhan.** Semua teks akan menjadi `Label` di Godot. Jangan menggambar teks sebagai bagian gambar.
5. **Terbaca di layar kecil.** Kontras tinggi, ukuran teks cukup di 1366x768.
6. **Tidak ada humor di UI.** Humor hanya di Ending B.

## 2. Token Warna (maksimum 6)

| Token | Hex (awal) | Dipakai untuk |
|---|---|---|
| `ink` | `#0B0B0D` | Latar utama |
| `ash` | `#3A3A40` | Border, panel, teks redup, slider kosong |
| `bone` | `#D8D3C4` | Teks utama, ikon, kertas catatan |
| `blood` | `#B3261E` | **Satu-satunya aksen:** pilihan terpilih, bahaya, game over, "TAMAT?" |
| `battery_ok` | `#3E7CB1` | Baterai normal, nama pembicara Diana |
| `battery_low` | `#D9A520` | Baterai rendah, nama pembicara OSIS (HT) |

Aturan:
- **Tidak boleh warna di luar tabel ini.** Transparansi (alpha) boleh dipakai untuk overlay hitam.
- Kontras teks `bone` di atas `ink` harus nyaman dibaca. Hindari teks `ash` untuk informasi penting.
- Warna nama pembicara di subtitle: **Diana = `battery_ok`**, **Cecillia / Adam / Ferdi = `battery_low`**. Nama selalu ditulis juga sebagai teks, jadi warna bukan satu-satunya pembeda.

## 3. Tipografi

| Peran | Font | Catatan |
|---|---|---|
| UI umum (menu, HUD, pengaturan, subtitle) | **VT323** | Gaya terminal piksel. Ukuran dasar 24 px di 1080p, skala ikut stretch |
| Catatan / kertas (`NoteSheet`) | **Special Elite** | Mesin ketik lusuh |
| Judul game dan judul besar | **Rock Salt** | Tulisan tangan kasar. Hanya untuk judul, bukan teks panjang |

- Subtitle: VT323, `bone`, outline gelap 2 px agar terbaca di latar apa pun.
- Huruf besar untuk item menu dan judul (gaya copywriting di bagian 6).
- Dilarang font lain. Jangan memakai lebih dari tiga ukuran dalam satu layar.

## 4. Bentuk, Spasi, Skala

- Sudut 0 px. Border 1 px (umum), 2 px (penekanan/subtitle outline).
- Spasi memakai kelipatan 8 px. Margin layar aman minimal 32 px dari tepi.
- Referensi desain 1920x1080, harus tetap terbaca dan rapi di **1366x768** dan **1600x900**. Layout memakai anchor/container, bukan posisi piksel mutlak.
- Gaya terpilih: awalan `> ` di depan teks dan warna `blood`. Hover mouse = gaya yang sama dengan fokus keyboard.
- Tidak ada animasi besar. Maksimum: kedip kursor, fade, sedikit grain. Efek kedip tunduk pada opsi **Kurangi kedip**.

## 5. Komponen Bersama

Dibangun **sekali** sebagai scene reusable (milestone B2), dipakai oleh semua layar.

| Komponen | Deskripsi | Dipakai di |
|---|---|---|
| `MenuButton` | Teks VT323 huruf besar. Terpilih: `> ` + `blood`. Biasa: `bone`. Tanpa kotak/latar | Menu, pause, pengaturan, layar akhir |
| `BatteryIndicator` | Ikon baterai piksel + persen. `battery_ok` normal, `battery_low` di bawah ambang | HUD, layar HP, intro, Ending A |
| `StaminaBar` | Garis tipis (sekitar 4 px), `bone`, tanpa bingkai tebal | HUD |
| `InteractPrompt` | `[E] Buka`, kecil, di tengah bawah reticle | HUD |
| `SubtitleBox` | Teks `bone` outline gelap 2 px, nama pembicara berwarna token, tanpa panel | HUD, prolog, ending |
| `NoteSheet` | Kertas `bone` agak miring, "tape" kecil, teks Special Elite | Pembaca catatan |
| `DialogueBox` | Kotak piksel: border 2 px `bone` di atas `ink`, nama pembicara di atas, teks VT323 | Prolog, Ending B |
| `SettingRow` | Label kiri, kontrol kanan. Slider **blok piksel** (kotak terisi/kosong), toggle teks `ON` / `OFF` | Pengaturan |
| `ScreenFader` | Overlay `ink` untuk fade in/out semua transisi | Semua transisi |

## 6. Copywriting (Bahasa Indonesia)

Huruf besar untuk item menu dan tombol. Nada: datar, singkat, tidak bercanda.

| Konteks | Teks |
|---|---|
| Judul game | Charger Diana |
| Menu utama | MULAI / LANJUTKAN / PENGATURAN / KREDIT / KELUAR |
| Konfirmasi MULAI saat ada save | "Mulai baru akan menimpa save?" dengan pilihan YA / BATAL |
| Pause | LANJUT / PENGATURAN / KE MENU UTAMA |
| Prompt interaksi | `[E] Buka`, `[E] Ambil`, `[E] Baca`, `[E] Sembunyi`, `[E] Tutup` |
| Hint tutorial (sekali) | `[Ctrl] Jongkok`, `[Shift] Lari`, `[Tahan] Tahan napas` (label mengikuti rebind) |
| Pengaturan | UTAMA, SFX, AMBIENCE, SUARA, SUBTITLE, UKURAN SUBTITLE, KECERAHAN, GRAFIS (RENDAH / SEDANG / TINGGI), EFEK RETRO, KURANGI KEDIP, SENSITIVITAS MOUSE, KONTROL |
| Game over (Ending C) | Satu teks merah, mis. "Dia menemukanmu." Lalu pilihan COBA LAGI / MENU |
| Ending A | Teks tenang singkat, ikon baterai 1% + petir kecil |
| Ending B | Dialog OSIS "cuma buat charger?", lalu layar **TAMAT?** (merah) |
| Loading | Teks atmosfer pendek, **bukan tip game**, mis. "Lorong terlalu sunyi..." |
| Tombol lewati | "Tahan [Spasi] untuk lewati" (indikator progres 1 detik) |
| Hint bawah pengaturan | "[Esc] Kembali" |

Peringatan konten (layar ContentWarning), isi minimum:
- Game ini mengandung **horror, suara keras mendadak, jumpscare, dan efek lampu berkedip** (berpotensi memicu sensitivitas cahaya).
- Tawaran: **KURANGI KEDIP** (ON/OFF) lalu **LANJUT**.

Kredit: nama pengisi suara ditampilkan sebagai berikut. Diana: *(nama teman, diisi saat G2)*. Cecillia, Adam, Ferdi: developer (Fer). Aset berlisensi diambil dari `assets/ASSETS.md`.

## 7. Spesifikasi per Layar

Prioritas dan kriteria parity mengikuti `PRD_V2.md` 6.4. Layar berstatus **BARU** belum punya referensi Stitch dan perlu digenerate. Layar lain sudah ada referensinya di `design_reference/screens/`.

### 7.1 Peringatan Konten (`ContentWarning.tscn`, Must, BARU)
- Latar `ink` polos. Blok teks tengah, kontras tinggi. Satu `SettingRow` untuk KURANGI KEDIP dan satu `MenuButton` LANJUT.
- Tanpa dekorasi. Boleh auto-lanjut hanya setelah pemain memilih.

### 7.2 Main Menu (`MainMenu.tscn`, Must)
- Latar: siluet sekolah malam, satu jendela menyala, grain halus, gerak minimal (lampu berkedip lambat).
- Judul "Charger Diana" (Rock Salt, `bone`), item menu kolom kiri atau tengah sesuai referensi, ikon baterai kecil sesuai referensi.
- LANJUTKAN tampil redup (`ash`) bila belum ada save.
- Referensi: `design_reference/screens/main_menu/`.

### 7.3 HUD (`Hud.tscn`, Must)
- Pojok: `BatteryIndicator`. Stamina tipis. `InteractPrompt` di tengah bawah reticle. `SubtitleBox` di bawah.
- **Tidak ada panel membulat**, tidak ada latar kotak tebal. HUD harus hampir tak terlihat.

### 7.4 Layar HP (`PhoneUi.tscn`, Must)
- Bingkai HP sederhana (sudut tajam). Jam, baterai, ikon senter dan catatan, grid catatan, pratinjau chat.
- Pratinjau chat memakai warna token. Chat teman di prolog (P-2) memakai layar ini juga.

### 7.5 Pembaca Catatan (`NoteReader.tscn`, Must)
- `NoteSheet` kertas miring + tape, teks Special Elite. Hint `[E] Tutup` di bawah.

### 7.6 Pause (`PauseMenu.tscn`, Must)
- Kolom sempit di sisi kiri, border 1 px `ash`, latar `ink` semi gelap, item LANJUT / PENGATURAN / KE MENU UTAMA.

### 7.7 Pengaturan (`Settings.tscn`, Must)
- Daftar `SettingRow` dalam beberapa grup (Audio, Subtitle & Tampilan, Grafis & Efek, Kontrol). Slider blok piksel, toggle ON/OFF, dropdown/pilihan teks untuk preset grafis.
- Hint bawah "[Esc] Kembali". Rebind kontrol: daftar aksi + tombol saat ini, klik untuk mengubah, konflik ditandai `blood`.
- **Variasi:** versi dari menu utama (latar gelap penuh) dan versi dari pause (kolom di atas game). Struktur sama.

### 7.8 Intro / Judul Adegan (`Intro.tscn`, Should)
- Hanya teks jam dan HP (mis. "22:30" + `BatteryIndicator` 35%). Tidak ada elemen lain. Dipakai di awal prolog.

### 7.9 Loading (BARU, Must untuk alur, Should untuk polesan)
- Latar `ink`, satu baris teks atmosfer kecil dan indikator minimal (titik berkedip atau garis tipis). Tanpa tip game.

### 7.10 Game Over / Ending C (`GameOver.tscn`, Must)
- Teks merah tunggal di tengah, siluet nyaris tak terlihat di latar. Setelahnya muncul COBA LAGI / MENU.

### 7.11 Ending A (`EndingA.tscn`, Must)
- Teks tenang. Ikon baterai **1%** dengan petir kecil. Lanjut ke kredit.

### 7.12 Ending B (`EndingB.tscn`, Must)
- `DialogueBox` piksel dengan dialog OSIS. Kemudian layar **TAMAT?** merah. Satu-satunya layar yang boleh terasa lucu.

### 7.13 Kredit (`Credits.tscn`, Should, BARU)
- Gulir pelan ke atas. Font UI (VT323), palet token. Bagian: pembuat, pengisi suara, aset berlisensi, terima kasih. Tombol lewati `[Esc]`.

### 7.14 Elemen Prolog (memakai komponen yang ada)
- Prolog memakai `SubtitleBox`, `DialogueBox`, `PhoneUi`, `ScreenFader`, indikator lewati. Tidak ada layar baru selain Intro.

## 8. Prompt Stitch (untuk layar BARU)

Tempel sebagai konteks di setiap generasi layar baru, lalu tambahkan prompt layar:

**Konteks bersama:**
> Design a UI screen for a Godot horror game "Charger Diana", set at a school at night. Style: minimal, PS1-era Indonesian horror, raw and cold. Strict rules: sharp 0px corners, 1-2px borders, no gradients, no glow, no blur, no rounded panels, no drop shadows. Use ONLY these 6 colors: #0B0B0D (background), #3A3A40 (borders, dim text), #D8D3C4 (main text), #B3261E (single red accent), #3E7CB1 (battery ok), #D9A520 (battery low). Fonts: VT323 for UI, Special Elite for paper notes, Rock Salt for the title only. All text in Bahasa Indonesia, real live text (not baked into images). Target 1920x1080, must still work at 1366x768. Selected menu item style: "> " prefix and red.

**Peringatan Konten:**
> Screen "Peringatan Konten". Dark flat background. Centered text block warning about horror, sudden loud sounds, jumpscares, and flickering lights (photosensitivity). One ON/OFF row "KURANGI KEDIP" and one menu button "LANJUT". High contrast, no decoration.

**Loading:**
> Screen "Loading". Dark background. One small atmospheric line of text ("Lorong terlalu sunyi...") bottom-left and a minimal thin progress line or blinking dots. No game tips.

**Kredit:**
> Screen "Kredit". Slow-scrolling credits on a dark background with subtle grain. Sections: Pembuat, Pengisi Suara (Diana, Cecillia, Adam, Ferdi), Aset, Terima Kasih. Centered text, VT323, token colors only, small "[Esc] Lewati" hint bottom-right.

**Variasi Pengaturan (rebind kontrol):**
> Screen "Pengaturan > Kontrol". Narrow list of actions (Maju, Mundur, Kiri, Kanan, Lari, Jongkok, Interaksi, Senter, HP) with current key on the right, selected row with "> " in red, conflict marked in red. Hint "[Esc] Kembali" at the bottom. Pixel-block sliders elsewhere, ON/OFF text toggles.

## 9. Aset untuk Folder `design_reference/`

Untuk setiap layar: `screen.png` (screenshot), `code.html` (jika ada, sebagai referensi struktur saja), dan `notes.md` yang berisi: apa yang diambil, apa yang diabaikan, apa yang dikoreksi. Layar baru (Peringatan Konten, Loading, Kredit, variasi Pengaturan) ditambahkan di folder baru dengan struktur yang sama. Aturan memakai referensi ada di `design_reference/AGENTS.md`.

## 10. Checklist UI Parity (ringkas)

Sesuai `PRD_V2.md` 6.5. Sebuah layar lulus bila:
- [ ] Struktur dan hierarki sama dengan screenshot referensi.
- [ ] Hanya 6 warna token, satu aksen merah.
- [ ] Font benar: VT323 UI, Special Elite catatan, Rock Salt judul.
- [ ] Sudut tajam, border 1-2 px, tanpa gradien/glow/blur.
- [ ] Semua teks `Label`, bukan teks dipanggang.
- [ ] Teks Indonesia sesuai copywriting (bagian 6).
- [ ] Skala benar di 1366x768, 1600x900, 1920x1080.
- [ ] Gaya dari Theme, bukan override per node.
- [ ] Laporan diambil/diabaikan/dikoreksi ditulis.
