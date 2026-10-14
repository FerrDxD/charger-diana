# Daftar Aset & Lisensi (assets/ASSETS.md)

Dokumen ini mencatat seluruh aset eksternal yang digunakan dalam proyek **Charger Diana** beserta atribusi dan lisensi resminya.

---

## 1. Tipografi / Font

| Nama Font | Pembuat / Desainer | Lisensi | Penggunaan |
|---|---|---|---|
| **VT323** | Peter Hull | SIL Open Font License (OFL) 1.1 | Antarmuka pengguna (UI), menu, teks terminal, subtitle dialog |
| **Special Elite** | Astigmatic (Brian J. Bonislawsky) | SIL Open Font License (OFL) 1.1 | Catatan di kertas lusuh (`NoteSheet`), lore |
| **Rock Salt** | Sideshow | Apache License, Version 2.0 | Judul game ("Charger Diana") |

---

## 2. Audio & Efek Suara (SFX)

| Kategori | Sumber / Tipe | Lisensi | Catatan |
|---|---|---|---|
| **Sintesis VO & Placeholder** | Mesin prosedural internal (`AudioManager.cs`) | Hak Cipta Pengembang (Maulana Ferdi) | Dihasilkan secara langsung oleh engine |
| **Ambience Sekolah Malam** | Rekaman suara lingkungan & Foley | Creative Commons Zero (CC0) / Public Domain | Suara jangkrik, angin malam, kesunyian lorong |
| **SFX Interaksi** | Fooley (pintu, laci, senter, ritsleting tas) | Creative Commons Zero (CC0) / Public Domain | Efek interaksi fisik |

---

## 3. Engine & Pustaka Pihak Ketiga

| Komponen | Pembuat | Lisensi |
|---|---|---|
| **Godot Engine 4.x** | Juan Linietsky, Ariel Manzur, & Godot Engine Contributors | MIT License |
| **Jolt Physics (godot-jolt)** | Jorrit Rouwe & godot-jolt contributors | MIT License |

---

## 3. Tekstur & Material (PS1 Pixel Art)

| Nama Berkas | Sumber / Tipe | Lisensi | Penggunaan |
|---|---|---|---|
| **tex_wood_parquet.png** | Prosedural internal | CC0 / Original Project Asset | Lantai parket kayu oak kamar Diana |
| **tex_wood_oak.png** | Prosedural internal | CC0 / Original Project Asset | Furnitur kayu oak (meja, nakas, ranjang, ambalan) |
| **tex_bed_linen.png** | Prosedural internal | CC0 / Original Project Asset | Kain linen kasur & selimut santai |
| **tex_wall_paint.png** | Prosedural internal | CC0 / Original Project Asset | Cat dinding plester kamar tidur |
| **tex_rug.png** | Prosedural internal | CC0 / Original Project Asset | Karpet rajut minimalis |
| **atlas_furniture_placeholder.png** | Prosedural internal (Python script) | CC0 / Original Project Asset | Atlas placeholder untuk furniture, warna coklat dasar kayu (256x256). Dibuat 2026-10-13 untuk B2. |
| **atlas_arch.png** | Prosedural internal (Python script, palet ART_BIBLE) | CC0 / Original Project Asset | Atlas arsitektur 256x256 (dinding, lantai, plafon, pintu, pagar). Dibuat 2026-10-13 untuk C1. |
| **atlas_diana_room.png** | Prosedural internal (Python script, palet ART_BIBLE kamar Diana) | CC0 / Original Project Asset | Atlas kamar Diana 256x256 (krem `#CFC6B4`, parket `#8A6B4A`, plafon `#D8D3C4`, oak `#9A7B52`). Dibuat 2026-10-13 untuk C2. |

---

## 4. Model 3D & Lingkungan

| Aset | Sumber / Format | Lisensi | Catatan |
|---|---|---|---|
| **Gedung SMA Negeri 3** (`assets/models/school_building.obj`, `.mtl`) | Prosedural internal / Proyek Charger Diana | Creative Commons Zero (CC0) / Original Project Asset | Model 3D arsitektur sekolah negeri 2 lantai (lobi, selasar, tangga, ruang OSIS, kelas, lab, atap pelana) |
| **Kamar Gadis Remaja Minimalis Diana** (`assets/models/diana_room.obj`, `.mtl`) | Prosedural internal / Proyek Charger Diana | Creative Commons Zero (CC0) / Original Project Asset | Model 3D interior kamar tidur Diana (platform bed, meja belajar, kursi Nordic, lemari flush, ambalan melayang, cermin lengkung berdiri) |
| **Furniture Kit - Chair** (`assets/models/furniture/furn_chair_kenney_a.glb`) | Kenney (https://kenney.nl/assets/furniture-kit) | Creative Commons Zero (CC0) | Kursi low-poly dari Kenney Furniture Kit, digunakan sebagai aset uji pipeline CC0. Diunduh 2026-10-13. |
| **Modular Kit Sekolah C1** (`assets/models/arch/*.glb`, 23 file) | Prosedural internal (Python `tools/generate_modular_kit.py`) | CC0 / Original Project Asset | Kit modular grid 2 m: dinding lurus/sudut/T/cross, kusen pintu/jendela, lantai/plafon, pilar, daun pintu/jendela, tangga, railing, pagar, gerbang, cornice, baseboard. Budget 12-84 tris, 64.6 KB total. Dibuat 2026-10-13 untuk C1. |
| **Material Arsitektur** (`assets/materials/mat_arch_atlas.tres` + `assets/textures/atlas/atlas_arch.png`) | Prosedural internal | CC0 / Original Project Asset | Material PS1 arsitektur, filter nearest, palet ART_BIBLE. Dibuat 2026-10-13 untuk C1. |
| **Material Kamar Diana** (`assets/materials/mat_diana_room_atlas.tres` + `assets/textures/atlas/atlas_diana_room.png`) | Prosedural internal | CC0 / Original Project Asset | Material PS1 kamar Diana, filter nearest, palet kamar `#CFC6B4`/`#8A6B4A`. Dibuat 2026-10-13 untuk C2. |
| **Kit Kamar Diana C2** (`assets/models/prolog/*.glb`, 22 file) | Prosedural internal (`tools/gen_diana_room.py`) | CC0 / Original Project Asset | Kit kamar 4×4×2.8 m: dinding/pintu/jendela, lantai/plafon, ranjang/mattress/duvet/pillow, meja/kursi, lemari, rak, karpet, charger/phone/buku/tas. 12-48 tris per aset, ±51 KB total. Dibuat 2026-10-13 untuk C2. |
| **Scene Turunan Kamar Diana** (`scenes/props/prolog/*.tscn`, 22 scene) | Prosedural internal (`tools/gen_prolog_props.py`) | CC0 / Original Project Asset | Scene turunan kamar: Visual + Collision. Dibuat 2026-10-13 untuk C2. |
| **Vertical Slice Kamar Diana** (`scenes/level/DianaRoomSlice.tscn`) | Prosedural internal | CC0 / Original Project Asset | Slice gameplay kamar Diana (arsitektur + furnitur + charger hero) tanpa CSG, grid 2 m. Dibuat 2026-10-13 untuk C2. |
| **PrologRoom Modular** (`scenes/prolog/PrologRoom.tscn` rewrite) | Prosedural internal | CC0 / Original Project Asset | PrologRoom CSG dihapus, diganti kit modular prolog (logic Bag/Drawer/Jacket preserved). Dibuat 2026-10-13 untuk C2. |
| **Scene Turunan Arsitektur** (`scenes/props/arch/*.tscn`, 23 scene) | Prosedural internal (`tools/gen_props.py`) | CC0 / Original Project Asset | Scene turunan per aset: Visual (instance glb + material_override) + Collision (box). Dibuat 2026-10-13 untuk C1. |

---

## 5. Tim & Pengembang

- **Sutradara & Pengembang:** Maulana Ferdi (`FerrDxD`) — `maulanaferdi0678@gmail.com`
- **Voice Acting:**
  - Diana: Teman Pengembang
  - OSIS Patroli (Cecillia, Adam, Ferdi): Maulana Ferdi
