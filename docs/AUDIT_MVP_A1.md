# Audit MVP: Milestone A1 Report

**Tanggal:** 2026-10-08
**Milestone:** A1 - Audit MVP
**Tujuan:** Memverifikasi isi MVP vs PRD v0.3, mencatat bug, dan baseline FPS di Celeron

---

## Ringkasan Eksekutif

MVP memiliki **fondasi gameplay dasar yang sudah jadi**: gerak first-person, senter + baterai, stamina, tahan napas, noise system, hide spot, AI OSIS dan Kuntilanak dasar, dan puzzle sederhana. Namun, **infrastruktur V2 belum ada**: folder data, logika murni, autoload tambahan, sistem save/load, pengaturan lengkap, alur game utuh (boot → menu → prolog → babak → ending → kredit), dan UI belum sesuai desain.

---

## 1. Struktur Folder vs ARSCHITECT.md

### ✅ Yang sesuai:
- `autoload/` - EventBus, AudioManager, GameManager, PuzzleManager
- `scenes/` - actors (player, enemies), interactables, level, ui
- `scripts/` - ai, interactables, player, systems, ui, data
- `assets/` - fonts
- `resources/` - ui_theme.tres, shaders

### ❌ Yang belum ada:
- `data/` - **BELUM ADA** (seharusnya: osis/, puzzles/, notes/, chapters/, vo/, cutscenes/, presets/, strings/)
- `scripts/core/` - **BELUM ADA** (untuk logika murni tanpa dependensi Godot: BatteryModel, deteksi/hide, skema save, FlickerPolicy, dll)
- `scenes/boot/` - **BELUM ADA** (ContentWarning, Boot)
- `scenes/menu/` - MainMenu ada tapi bukan di folder ini
- `scenes/prolog/` - **BELUM ADA** (PrologRoom, PrologRoad)
- `scenes/levels/` - Hanya TestLevel, seharusnya Chapter1/2/3 + zones/
- `scenes/endings/` - **BELUM ADA** (EndingA, EndingB, GameOver - EndingScreen ada tapi generic)
- `scenes/ui/components/` - **BELUM ADA** (komponen bersama: MenuButton, BatteryIndicator, StaminaBar, dll)
- `tests/` - **BELUM ADA** (uji logika murni)

**Dampak:** Perlu reorganisasi folder dan pembuatan folder baru di milestone A2/B1.

---

## 2. Autoload vs ARCHITECT.md

### ✅ Yang sudah ada:
| Autoload | Status | Catatan |
|----------|--------|--------|
| EventBus | ✅ Ada | Signal sudah lengkap, sesuai ARCHITECT.md |
| AudioManager | ✅ Ada | Bus sudah ada (Master, SFX, Ambience, Ghost, VO, UI), placeholder procedural sudah jadi |
| GameManager | ✅ Ada | Status game, inventory, pause, terima signal ItemCollected/PlayerCaught |

### ❌ Yang belum ada:
| Autoload | Status | Milestone yang butuh |
|----------|--------|---------------------|
| GameLog | ❌ Belum | H2 (dasar di A2) |
| SettingsManager | ❌ Belum | C3 |
| SaveSystem | ❌ Belum | C4 |
| SceneManager | ❌ Belum | C1 |
| GameState | ❌ Belum | A2 (paripurna di GameManager sekarang, tapi perlu dipecah) |
| InputRemapper | ❌ Belum | C5 |

**Catatan:** PuzzleManager ada di autoload tapi tidak tercantum di ARCHITECT.md bagian 4. Ini bisa dipertahankan (tugas puzzle memang perlu persistensi), tapi ARCHITECT.md harus diupdate.

---

## 3. Gameplay Systems vs PRD V2

### ✅ Yang sudah jadi:
| Sistem | Status | Kualitas |
|--------|--------|----------|
| Gerak first-person (WASD, lari, jongkok) | ✅ | Player.cs lengkap, parameter `[Export]` |
| Senter + baterai | ✅ | Player.cs, drain per frame, low battery flicker |
| Stamina | ✅ | Player.cs, drain saat lari, regen, cooldown |
| Tahan napas | ✅ | Player.cs, hanya saat hidden, regen |
| Noise system | ✅ | NoiseSystem.cs statis, EventBus.NoiseEmitted |
| Hide + tahan napas | ✅ | HidingSpot.cs, Player.IsHidden/IsHoldingBreath |
| AI Kuntilanak dasar | ✅ | Kuntilanak.cs, state machine (Dormant → Roaming → Hunting → Retreat), tawa terbalik, bau melati |
| AI OSIS dasar | ✅ | OsisPatrol.cs, state machine (Patrol → Suspicious → Investigate → Chase → Search), waypoint, noise/sight, panic ke kuntilanak |
| Puzzle dasar | ✅ | PuzzleManager.cs, PuzzleData.cs, CodeLock, Switch, PowerPanel |
| Interactable base | ✅ | InteractableBase.cs, raycast Player |
| Ending trigger | ✅ | GameManager.OnPlayerCaught → EndingTriggered signal |

### ⚠️ Yang perlu perluasan:
| Sistem | Status | Gap |
|--------|--------|-----|
| Kuntilanak | ⚠️ Dasar | Belum ada encounter terskrip per babak, variasi tawa/bisikan 5-8 klip, telegraph konsisten sepenuhnya (PRD 4.5) |
| OSIS | ⚠️ Dasar | Belum ada profil karakter (Cecillia, Adam, Ferdi), rute per babak, HT chatter penuh, VO dengan filter |
| Puzzle | ⚠️ Dasar | P1-P5 mungkin ada tapi belum diverifikasi, belum ada 2-3 puzzle kecil tambahan (Should) |
| Level | ⚠️ TestLevel | Hanya test level, belum ada 3 babak lengkap dengan zona |

### ❌ Yang belum ada:
| Sistem | Status | Milestone |
|--------|--------|-----------|
| Save/load | ❌ Belum | C4 |
| Pengaturan (volume, grafis, subtitle, kontrol) | ❌ Belum | C3, C5 |
| Prolog (kamar, chat HP, perjalanan) | ❌ Belum | D1-D3 |
| Cutscene controller | ❌ Belum | D1-D3, F1 |
| Tutorial kontekstual | ❌ Belum | E2 |
| Event terskrip (lampu padam, pintu tertutup sendiri) | ❌ Belum | E7 |
| Catatan lore | ❌ Belum | E6 |
| ContextHint system | ❌ Belum | E2 |
| Pengendali babak (ChapterController) | ❌ Belum | E1 |

---

## 4. UI vs DESIGN.md & PRD V2

### ✅ Scene yang sudah ada:
- MainMenu.tscn
- Hud.tscn
- PhoneUi.tscn
- NoteReader.tscn
- PauseMenu.tscn
- Settings.tscn
- Intro.tscn
- EndingScreen.tscn (generic)

### ❌ Scene yang belum ada:
- ContentWarning.tscn (boot, peringatan konten) - Must
- Boot.tscn (logo) - Must
- Loading.tscn - Must
- EndingA.tscn (khusus) - Must
- EndingB.tscn (khusus, humor) - Must
- GameOver.tscn (Ending C, khusus) - Must
- Credits.tscn - Should
- Komponen bersama di `scenes/ui/components/` - Must (B2)

### ⚠️ UI Theme:
- `ui_theme.tres` sudah ada di `resources/`
- Belum dicek apakah sesuai DESIGN.md (6 warna token, font VT323/Special Elite/Rock Salt, sudut 0, border 1-2 px)
- Perlu audit di milestone A3 (Audit UI vs Stitch)

### ⚠️ UI Parity:
- Belum ada audit vs referensi Stitch di `design_reference/`
- Milestone A3 akan melakukan ini

---

## 5. Data & Resources

### ❌ Resource yang belum ada:
- `OsisProfile.tres` (profil Cecillia, Adam, Ferdi) - E5
- `PuzzleDef.tres` (extended dari PuzzleData) - E3
- `NoteDef.tres` (catatan lore) - E6
- `ChapterDef.tres` (babak 1-3, event, encounter) - E1
- `CutsceneDef.tres` (prolog, ending A/B/C) - D1, F1
- `VoLine.tres` (baris VO) - G2
- `GraphicsPreset.tres` (Rendah/Sedang/Tinggi) - H1
- `data/strings/` (teks UI dan copywriting Indonesia) - B1

### ⚠️ Data yang ada:
- `PuzzleData.cs` sudah ada sebagai Resource C#, tapi belum banyak instance .tres

---

## 6. Audio

### ✅ Yang sudah ada:
- AudioManager dengan bus lengkap
- Placeholder procedural audio (game punya suara bahkan tanpa aset)
- SFX call (PlaySfx, PlayGhostSound, PlayUi, PlayVo, PlayAmbience)

### ❌ Yang belum ada:
- Ambience per zona (AudioZone) - G1
- VO rekaman asli (sekarang semua placeholder) - G2
- Stinger momen kunci - G3
- Ducking otomatis saat VO (di AudioManager belum ada)
- Subtitle sinkron dengan VO (ada SubtitleRequested signal tapi belum sinkron durasi asli)

---

## 7. Bug yang Diketahui

Belum ada bug yang dilaporkan. Game diasumsikan bisa dijalankan (TestLevel sebagai main scene).

**Perlu dicek:**
- Apakah TestLevel bisa dijalankan tanpa error?
- Apakah semua node reference tercache dengan benar?
- Apakah ada warning di log saat startup?

---

## 8. Baseline FPS di Celeron

**Belum diukur.** Developer perlu menjalankan game di laptop target (Intel Celeron, RAM 8 GB, iGPU) dan mengukur FPS di scene terberat saat ini (TestLevel dengan semua aktor).

**Target setelah A1:** Tidak ada target FPS spesifik di A1, tapi perlu baseline untuk H1 (Performa pass).

---

## 9. Selisih dengan PRD V2

### Asumsi PRD V2 yang benar:
- ✅ Gerak first-person, senter, baterai, stamina, noise, hide, AI dasar sudah ada
- ✅ Puzzle P1-P5 dasar sudah ada (perlu verifikasi di scene)
- ✅ Ending trigger sudah ada (B dan C)
- ✅ Checkpoint mungkin ada (perlu verifikasi)

### Asumsi PRD V2 yang salah/masih belum:
- ❌ UI mengikuti desain Stitch - **BELUM**, perlu audit A3
- ❌ Alur game utuh (boot → menu → prolog → babak → ending → kredit) - **BELUM**
- ❌ Konten sebatas bukti konsep - **BENAR**, hanya TestLevel

---

## 10. Rekomendasi Langkah Selanjutnya

### Immediate (milestone A2):
1. Buat folder `data/` dan `scripts/core/`
2. Buat autoload yang hilang: GameLog, GameState (pecah dari GameManager), persiapan SettingsManager/SaveSystem/SceneManager
3. Pindahkan nilai tuning yang masih hardcode ke `[Export]` atau Resource
4. Build bersih (`dotnet build`) tanpa warning baru

### Sebelum B1 (Fondasi UI):
1. Audit UI vs Stitch (milestone A3) - laporan celah per layar
2. Buat `ui_theme.tres` yang sesuai DESIGN.md (6 warna token, font, stylebox)
3. Buat komponen bersama di `scenes/ui/components/`

### Sebelum C1 (Alur & Menu):
1. Buat SceneManager dengan state machine alur game
2. Buat ContentWarning dan Boot scene
3. Buat Loading scene

### Sebelum D1 (Prolog):
1. Buat CutsceneController
2. Buat CutsceneDef untuk prolog
3. Buat scene PrologRoom dan PrologRoad

---

## 11. Daftar Pekerjaan Tambahan (Di luar MVP)

Berikut adalah fitur V2 yang **sama sekali belum ada** dan perlu dibangun dari nol:

| Fitur | Milestone | Ukuran |
|-------|-----------|--------|
| Boot & peringatan konten | C1 | S |
| Main menu proper (navigasi keyboard/mouse, LANJUTKAN nonaktif tanpa save) | C2 | M |
| Pengaturan lengkap (volume, grafis, subtitle, kontrol, rebind) | C3, C5 | L |
| Pause menyeluruh, save/load checkpoint | C4 | M |
| Prolog (kamar, chat HP, perjalanan) | D1-D3 | M |
| Level 3 babak lengkap (Chapter1/2/3) | E1 | L |
| Tutorial kontekstual | E2 | S |
| Puzzle P1-P5 polished + 2-3 puzzle kecil | E3 | M |
| Kuntilanak V2 (encounter terskrip, variasi tawa) | E4 | L |
| OSIS V2 (profil Cecillia/Adam/Ferdi, rute per babak, HT chatter) | E5 | M |
| Catatan lore | E6 | M |
| Event terskrip & pacing | E7 | M |
| Ending A/B/C proper scenes | F1 | M |
| Kredit | F2 | S |
| Ambience per zona, SFX lengkap | G1 | M |
| Voice acting (rekaman, kompres, integrasi) | G2 | L |
| Stinger momen kunci | G3 | S |
| Preset grafis & performa pass | H1 | M |
| Build & export | H2 | S |
| UI parity pass akhir | H3 | M |
| Playtest | H4-H6 | L |

**Total estimasi:** ~20 milestone lagi dari A1 ke H6.

---

## 12. Kesimpulan

MVP memiliki **fondasi gameplay yang solid** dan kode yang rapi. Namun, untuk mencapai "game sungguhan" sesuai PRD V2, perlu:

1. **Infrastruktur data** (folder data/, Resource .tres untuk semua konfigurasi)
2. **Logika murni terpisah** (scripts/core/ untuk testing tanpa editor)
3. **Autoload tambahan** (SettingsManager, SaveSystem, SceneManager, GameState, GameLog, InputRemapper)
4. **Alur game utuh** (boot → menu → prolog → 3 babak → ending → kredit)
5. **UI parity** dengan desain Stitch
6. **Konten level penuh** (3 babak, event terskrip, catatan lore)
7. **Audio lengkap** (ambience per zona, VO, stinger)
8. **Sistem save/load & pengaturan**

**Prioritas:** Lanjut ke A2 (Rapikan & tuning) untuk mempersiapkan fondasi, lalu A3 (Audit UI vs Stitch) sebelum masuk fase B (Fondasi UI).
