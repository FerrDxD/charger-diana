# ART_BIBLE.md: Kitab Gaya & Budget 3D Charger Diana

**Status:** final untuk produksi (PRD_V3 milestone A3). Nilai di bawah mengunci titik awal PRD_V3 §3.
Perubahan setelah ini = keputusan baru (tanya dulu, AGENTS §10).
**Prioritas:** PRD_V3 > dokumen ini bila bertentangan.

**Catatan Pipeline (A1 - 2026-10-13):** Blender tidak layak di laptop target (Celeron N4020).
Pipeline diubah dari "Blender → glTF → Godot" menjadi "Download CC0 → Import Godot → Edit di Godot".
Sumber aset: Kenney + Poly Haven (CC0 tanpa atribusi).

## 1. Gaya (PS1 horror Indonesia)

- Low-poly bersudut, siluet jelas. Detail dari **tekstur**, bukan geometri.
- Tekstur kecil, filter **nearest**. Boleh kotor/berpiksel, jangan blur.
- **Tanpa** normal map, **tanpa** PBR. Satu shader PS1 standar (vertex snapping dkk.) dari V2.
- "Pencahayaan palsu": **vertex color AO** di sudut/kolong (murah di iGPU).
- Suasana: lembap, kusam, neon sekolah hampir mati, noda, kertas berserakan.
- Palet 3D di bawah **terpisah** dari 6 token UI (`DESIGN.md`). UI tetap 6 token.

## 2. Skala & satuan

- **1 unit Godot = 1 meter** (asumsi aset CC0 menggunakan satuan meter).
- Tinggi pintu **2,0 m**. Meja siswa ±0,75 m. Tinggi pemain ikut Godot (kapsul 1,8 m, kamera 1,6 m).
- **Grid modul 2 m** (dinding, lantai, plafon). Snap semua potongan modular ke grid.
- Tinggi lantai→plafon: **sekolah 3,2 m** (ruang kelas/koridor/lobi), **kamar Diana 2,8 m**.
- Pivot: **dasar objek** (props/furnitur), **sudut** (potongan modular).
- **Setelah import:** cek skala di Godot, atur ulang bila perlu (scale transform).

## 3. Budget poligon (tris, final)

| Kategori | Per aset | Catatan |
|---|---|---|
| Prop kecil (buku, baterai, kunci, charger) | 30–200 | Charger & HP boleh ±300 (dilihat dekat) |
| Prop sedang (tas, ember, tong sampah) | 100–500 | |
| Furnitur (meja, kursi, loker, papan tulis) | 150–800 | Meja/kursi siswa sehemat mungkin (diulang puluhan kali) |
| Modular (dinding, lantai, pintu) | 12–100 | |
| Luar ruang (pohon lama, lampu jalan) | 300–1.500 | Pohon lama = hero |
| Kuntilanak | 1.500–2.500 | Gaun/rambut dari bidang sederhana, tanpa simulasi |
| OSIS per karakter | 800–1.500 | Satu basis dipakai bersama |
| Tangan Diana + HP | 300–600 total | Tanpa badan penuh |

Budget per scene (divalidasi di C2): **≤ ±40.000 tris terlihat, ≤ ±20 material, ≤ ±150 draw call**.

## 4. Tekstur & atlas

- **Texel density: 64 px/m**, seragam semua aset (bila aset CC0 punya tekstur berbeda, downsampling mungkin diperlukan).
- Atlas per kelompok: **maks 256×256**; hero (pohon lama, charger) **maks 512×512**. Kelipatan 2, PNG.
- Kelompok atlas: `arch` (arsitektur), `furn` (furnitur), `props`, `out` (luar), `char` (karakter).
  Material: `mat_arch_atlas`, `mat_furn_atlas`, `mat_props_atlas`, `mat_out_atlas`, `mat_char_atlas`.
- Filter **nearest**. Mipmap: diputuskan lewat uji B3 (hipotesis awal: ikut uji, catat biaya GPU).
- **Pipeline baru:** Aset CC0 (Kenney, Poly Haven) diimport sebagai `.glb`, material embedded diganti ke material PS1 + atlas di Godot.
  Bila aset CC0 punya tekstur yang sudah cocok, boleh dipakai langsung (tulis di `ASSETS.md`).
- Semua sumber dicatat di `assets/ASSETS.md` (URL, lisensi, tanggal).
- Estimasi build: tekstur < ±100 MB, model < ±50 MB, total build < 500 MB.

## 5. Palet per zona (muram, final)

Hex adalah nilai dasar albedo; variasi via vertex color AO, bukan material baru.

| Zona | Dinding | Lantai | Langit-langit | Aksen | Lampu |
|---|---|---|---|---|---|
| Kamar Diana (prolog) | krem kusam `#CFC6B4` | parket `#8A6B4A` | putih kotor `#D8D3C4` | seprai `#B9C4B0`, kayu oak `#9A7B52` | hangat `#F2E3C2` |
| Jalan malam | abu gelap `#2A2A2E` | aspal `#1E1E22` | — (langit `#0B0B10`) | pot tanah `#6B4A34`, daun `#2E4034` | jalan kuning redup `#C9A227` |
| Gerbang & halaman | beton `#5A5A58` | tanah `#4A4238` | — | gerbang besi `#232326`, papan `SMA NEGERI 3` hijau pudar `#1F4A2E` | demi-kuning `#D9C48A` |
| Lobi & koridor L1 | hijau sekolah pudar `#5E6E5E` bawah / `#B8B4A6` atas | tegel kusam `#7A766C` | `#A8A49A` | pintu kayu `#6E5238`, papan nama | neon hampir mati `#CFE0D8` |
| Ruang listrik | abu `#4E4E52` | semen `#5E5E60` | `#6A6A6E` | panel kuning peringatan `#8A7420`, kabel merah `#7A2018` | redup dingin |
| Tangga | sama dengan koridor | semen tepi kuning aus `#8A7A3A` | — | railing besi `#2E2E32` | hangat redup |
| Lantai 2 (koridor, OSIS, kelas, lab) | sama dengan L1, lebih gelap 10% | sama | sama | papan tulis `#2E4038`, meja kayu `#6E5238`, loker `#4E5E5E` | neon berkedip (ikut `FlickerPolicy`) |
| Toilet/UKS/kantin/guru/lab/perpus (Should) | keramik kotor `#9AA0A0` / krem | tegel basah `#5E6464` | `#8A8A86` | noda karat `#6E4A2E` | putih dingin redup |
| Pohon lama (lore) | kulit `#3E3228` | tanah/akar `#33291F` | — | kain/tali sesaji merah pudar `#6E1E1A` | tanpa lampu (siluet) |

Aturan: tiap zona maks 4–5 warna dasar + vertex AO. Merah murni (`#B3261E`) hanya untuk hal yang harus dibaca pemain
(kunci, charger, darah, `TAMAT?`), konsisten dengan aksen UI.

## 6. Penamaan & struktur

- Aset: `{kategori}_{nama}_{variasi}` huruf kecil (`arch_wall_straight_2m`, `furn_desk_student_a`,
  `item_charger`, `char_kuntilanak`, `char_osis_base`, `out_tree_old`).
- Animasi berulang bersufiks `-loop` (`osis_walk-loop`). Detail sufiks collision (`-colonly`,
  `-convcolonly`) dikunci di B2 terhadap Godot 4.7.
- UV 0–1 pada atlas kelompoknya (bila aset CC0 punya UV sendiri, dicek di import).
- Scene turunan per aset di `scenes/props/<kategori>/` atau `scenes/actors/`: root Node3D = id aset,
  anak `Visual` (instance `.glb` dari CC0) + `Collision` (box/convex sederhana, **bukan** mesh visual).
- Level hanya merakit scene turunan. Zona = `ZoneRoot`, modular di-snap grid 2 m.
- File `.glb` CC0 disimpan di `assets/models/<kategori>/` (bukan di repo terpisah).

## 7. Karakter & animasi

- Satu **skeleton humanoid standar** (nama tulang kompatibel `SkeletonProfileHumanoid` + BoneMap).
  Tiga OSIS = satu basis/rig/animasi, beda via `OsisProfile` (rambut, warna, aksesori).
- Kuntilanak: gaun/rambut bidang sederhana ikut tulang pinggul/kepala. Wajah tidak detail (tertutup rambut).
- Daftar animasi ikut PRD_V3 §6. Sumber: **Universal Animation Library** (Quaternius, CC0,
  versi dikunci di E2/E4 saat dicek isinya); animasi khas dibuat sendiri setelah L6.
- AI tidak memanggil `AnimationPlayer` — hanya `CharacterAnimator.SetAnimState()`.
- Diana: **tangan + HP saja** (ViewModel anak Camera3D). `PhoneUi` tetap overlay UI 2D, bukan tekstur 3D.

## 8. Versi alat (dikunci)

| Alat | Versi | Catatan |
|---|---|---|
| Godot | **4.7** (.NET/C#, renderer Compatibility) | Sesuai `project.godot`. Jangan upgrade tanpa uji pipeline |
| Blender | **TIDAK DIPAKAI** | Tidak layak di Celeron N4020 (A1 - 2026-10-13). Pipeline diubah ke CC0 import di Godot |
| Krita | terbaru stabil saat B3 | Untuk atlas piksel (bila perlu editing tekstur) |
| Fisika | Jolt | Sudah di `project.godot` |

Jangan upgrade Godot di tengah fase tanpa persetujuan + uji ulang pipeline.

## 9. Referensi belajar & sumber

- **Pipeline baru (tanpa Blender):** Download aset CC0 → Import `.glb` ke Godot → Edit di Godot (skala, material, collision).
- **Sumber aset 3D CC0 (utama):**
  - Kenney: Furniture Kit (140 item), Graveyard Kit (91 item), dsb. URL: https://kenney.nl/assets/
  - Poly Haven: Furnitur low-poly, tekstur CC0. URL: https://polyhaven.com/
- **Sumber animasi CC0:** Universal Animation Library (Quaternius) untuk karakter.
- Gerbang belajar L0–L6 dari PRD_V3 §1.5 tidak relevan lagi (tidak ada Blender). Fokus: editing di Godot.

## 10. Gerbang kualitas (ringkasan, detail PRD_V3 §9)

Tercatat di katalog → download CC0 → import `.glb` ke Godot → cek skala/pivot → tris di bawah budget →
material diganti ke PS1/atlas (atau pakai tekstur CC0 bila cocok) → collision dibuat manual →
tampil benar di Godot (nearest) → FPS zona tidak turun → lisensi tercatat di `ASSETS.md`.
