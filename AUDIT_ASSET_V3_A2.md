# Audit Aset V3 — Milestone A2

**Tanggal:** 2026-10-13
**Milestone:** A2 (PRD_V3 §10, Fase A) — Audit asset V2
**Metode:** inspeksi file (scene, model, tekstur, script). Tanpa Blender, tanpa editor Godot.

## 1. Verdict asumsi PRD_V3 §1.1

**TERKONFIRMASI.** Seluruh visual level saat ini bersifat sementara:

- Geometri level = CSG greybox (`CSGBox3D`/`CSGCombiner3D`) + primitif (`BoxMesh`, `PlaneMesh`).
- Karakter (kuntilanak, OSIS) = **tanpa mesh sama sekali**, hanya logika + collision.
- Dua file `.obj` (`school_building.obj` 372 KB, `diana_room.obj` 103 KB) adalah mesh tunggal non-pipeline (bukan `.glb`, bukan modular, material `.mtl` bukan atlas PS1).
- Tidak ada `.glb`, tidak ada atlas, tidak ada material PS1, tidak ada `tools/`, tidak ada `ART_BIBLE.md`.

Tidak ada aset yang dikeluarkan dari daftar kerja V3. Semua kategori K1–K10 tetap dikerjakan.

## 2. Inventaris per zona

| Zona / Scene | Isi visual saat ini | Status |
|---|---|---|
| Chapter1 (Babak 1–3, `scenes/levels/Chapter1.tscn`) | 8× `CSGBox3D` ruangan + 6× CSG pelubang pintu, 2 material datar (`Mat_Floor`, `Mat_Wall`), 2 `OmniLight` | Placeholder (greybox). Bukan kit modular, bukan snap grid 2 m |
| SchoolBuilding (`scenes/world/SchoolBuilding.tscn`) | 1× `school_building.obj` + 13 collision box manual + 5× `Label3D` papan nama + 5 `OmniLight` | Sementara. Mesh tunggal wajib diganti kit modular K1 via pipeline |
| DianaRoom / PrologRoom (`scenes/world/DianaRoom.tscn`, `scenes/prolog/PrologRoom.tscn`) | `diana_room.obj` (dipakai DianaRoom) + PrologRoom full-CSG ±45 box (kasur, meja, lemari, cermin) + material warna datar | Sementara. Proporsi CSG PrologRoom boleh jadi referensi ukuran saat modeling K8 di Blender |
| PrologRoad (`scenes/prolog/PrologRoad.tscn`) | CSG (aspal, trotoar, gerbang, pot) + material datar | Placeholder (K9) |
| Interactables (Pickup, ChargerPickup, KeyDoor, Switch, PowerPanel, CodeLock, NoteReadable, HidingSpot, ExitGate) | Semua `BoxMesh` polos | Placeholder. Charger = hero prop, prioritas D2 |
| Kuntilanak (`scenes/enemies/Kuntilanak.tscn`) | CharacterBody3D + kapsul + NavAgent. **Tanpa mesh** | Placeholder penuh (K6/E1). Script `scripts/ai/Kuntilanak.cs` tetap dipakai |
| OSIS (`scenes/enemies/OsisPatrol.tscn`) | CharacterBody3D + kapsul + NavAgent + SightRay. **Tanpa mesh** | Placeholder penuh (K6/E3). Script `scripts/ai/OsisPatrol.cs` tetap dipakai |
| Player (`scenes/player/Player.tscn`) | Kapsul + kamera + senter + ray. **Tanpa ViewModel tangan/HP** | Logika tetap. ViewModel E5 belum ada |
| TestLevel | `PlaneMesh` + WorldBoundary | Scene uji, bukan zona game. Tetap untuk uji jalan |
| Tekstur (`assets/textures/`, 5 PNG @ 228–361 byte) | Warna hampir datar, bukan atlas | Sementara. Diganti atlas B3 (maks 256×256) |

**Layak pakai (tidak diganti V3):** script logika (player, AI, puzzle, prolog), `Label3D` papan nama (teks, bukan geometri), struktur collision manual sebagai referensi ukuran.

## 3. Temuan tambahan

1. **Geometri ganda di Chapter1:** `SchoolBuilding` (mesh `.obj`) di-instance bersamaan dengan `SchoolGeo` (CSG ruangan di koordinat yang sama). Risiko z-fighting / tabrakan ganda. Saat G1, hanya satu sumber yang dipakai (kit modular), satunya dihapus.
2. **`.obj`/`.mtl` di repo game melanggar pipeline** (AGENTS §13.2: hanya `.glb` hasil ekspor; `art/` di repo terpisah). File tetap dibiarkan sampai penggantinya jadi (G1), lalu dihapus. Jangan tambah `.obj` baru.
3. **Lampu sudah melebihi budget rasa:** SchoolBuilding 5 omni + Chapter1 2 omni + senter, tanpa kontrol preset grafis. Dicatat untuk G3/G4 (batas lampu aktif per preset).
4. **Tidak ada animasi:** `assets/animations/` tidak ada, BoneMap belum ada, `CharacterAnimator` belum ada (E2/E4).

## 4. Kunci jumlah katalog (PRD_V3 §5)

Estimasi awal dipertahankan, tidak ada pengurangan (tidak ada aset layak pakai):

| Kode | Kategori | Jumlah | Pri. |
|---|---|---|---|
| K1 | Modular arsitektur | 25–30 | M |
| K2 | Furnitur kelas | 12–15 | M |
| K3 | Props umum | 25–30 (12 pertama M, sisa S) | M/S |
| K4 | Item & puzzle (termasuk charger) | 10–12 | M |
| K5 | Zona khusus | 30–35 | S |
| K6 | Karakter (kuntilanak, basis OSIS + 3 variasi, tangan+HP) | 5–6 | M (tangan Should/E5) |
| K7 | Luar ruangan | 10–12 | M |
| K8 | Kamar Diana | 10–12 | M |
| K9 | Jalan malam | 8–10 | M |
| K10 | Dekor cerita | 10–15 | C |

Inti Must ≈ 70–80 aset. Jumlah final per zona dikunci ulang setelah C2 bila vertical slice membuktikan kitbash cukup.

## 5. Status A1 (terkait, belum selesai)

Mesin ini = mesin target (Celeron N4020, RAM 8 GB). Blender dan Godot **belum terinstal / tidak di PATH**. A1 (pilih versi Blender 5.x / 4.5 LTS / 3.6 LTS + catatan viewport + setelan hemat) menunggu pengguna menginstal dan mencoba. Protokol uji ada di laporan sesi (chat), hasilnya dicatat di A1 sebelum A0 dimulai.

## 6. Definisi selesai A2

- [x] Inventaris placeholder vs layak pakai per zona (bagian 2)
- [x] Asumsi PRD_V3 §1.1 diverifikasi (bagian 1)
- [x] Jumlah katalog dikunci (bagian 4)
- [x] Tidak ada perubahan kode/game — repo tetap bisa dibuka seperti sebelumnya
- [ ] A1 (versi Blender) — menunggu pengguna, di luar A2

**Belum dites:** buka editor Godot (Godot tidak tersedia di PATH mesin ini).
**Asumsi:** script AI/player/puzzle yang ada dianggap jalan (audit MVP A1); naturally diverifikasi ulang saat G1/G4.
**Saran (bukan cakupan A2):** A3 `ART_BIBLE.md` berikutnya; B2 tooling; hapus `.obj` hanya di G1.
