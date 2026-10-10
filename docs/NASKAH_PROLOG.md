# Naskah & Storyboard Prolog V2

**Karakter:**
- **Diana** (Protagonis, MC): Remaja Gen Z, sangat bergantung pada *smartphone*. Cenderung *overthinking* jika tidak pegang HP, kepanikan nyata, bukan komedi.
- **Salsa** (Teman sekelas): Teman chat.

**Tujuan Prolog:** Menetapkan motivasi karakter yang spesifik ("selalu butuh HP, tak punya casan lain, tertinggal di kelas"), memberikan tutorial kontrol dasar (*movement*, UI HP), dan perlahan mengubah atmosfer dari aman (kamar) menjadi mencekam (gerbang sekolah).

---

## P-1: Kamar Diana (22:30)
**Visual / Kondisi Awal:**
Kamar tidur redup, hanya ada cahaya layar HP dan sedikit cahaya jalan dari jendela. Sudut pandang *First-Person*. Indikator baterai di UI sudut kanan atas menunjukkan **35%** dan berwarna kuning.

**Aksi:**
Pemain mendapatkan kendali (tutorial gerak: `W, A, S, D`).

**Dialog (VO):**
- **Diana [VO_P1_1]:** *(Menghela napas panik tertahan)* "Tigapuluh lima persen... Besok pagi pasti mati total ini."

**Interaksi & Tutorial:**
Pemain bisa menjelajahi kamar sempit. Ada dua *interactable object* bersinar samar (tutorial klik/E):
1. **Tas Sekolah (di kursi):**
   - *SFX: Suara sleting ditarik.*
   - **Diana [VO_P1_2]:** "Kosong. Gak ada di sini."
2. **Laci Meja Belajar:**
   - *SFX: Suara laci kayu ditarik.*
   - **Diana [VO_P1_3]:** "Cuma ada kabel bekas... ujungnya beda. Kenapa sih aku nggak pernah kepikiran beli adaptor cadangan?"

---

## P-2: Chat HP
**Pemicu:** Setelah mengecek laci, bunyi notifikasi *Chat* terdengar. Tutorial muncul di layar: `[TAB / M] Buka HP`.

**Visual / UI:**
Pemain menekan tombol, layar HP *pop-up* (menggunakan UI Phone dari `PhoneUi.tscn`). Masuk ke aplikasi pesan.

**Teks Chat (di layar HP):**
- **Salsa:** "Casannya ketemu? Perasaan tadi sore lo pake pas jam terakhir. Ketinggalan di kelas kali."
- **Diana:** "Serius?! Kunci kelas gimana?"
- **Salsa:** "Kalo berani sih, ambil aja. Kunci cadangan biasanya diselipin pak satpam di bawah pot dekat pos depan."
- **Diana:** "Oke, thanks."

**Dialog (VO) saat membaca:**
- **Diana [VO_P2_1]:** *(Bergumam saat membaca)* "Ketinggalan di kelas..."

---

## P-3: Keputusan
**Pemicu:** Pemain menutup layar HP (`[ESC]` atau `[TAB]`).

**Visual / Aksi:**
Kamera sedikit turun seperti menatap lantai sebentar, lalu naik lagi.

**Dialog (VO):**
- **Diana [VO_P3_1]:** *(Nada putus asa tapi tekadnya bulat)* "Gak bisa. Aku gak bisa hidup tanpa HP besok. Seharian bengong di sekolah? Bisa gila."

**Interaksi:**
Pemain diarahkan (secara visual oleh komposisi cahaya) menuju pintu kamar. Ada jaket menggantung di balik pintu. Tombol interaksi muncul: `[E] Ambil Jaket & Pergi`.
- *SFX: Suara kain tebal dipakai, langkah kaki bergegas, pintu ditutup.*
- **Diana [VO_P3_2]:** "Sekolah malam-malam... Gak apa-apa, cepet kok. Masuk, ambil, pulang."

**Efek:** Layar memudar menjadi hitam (*Fade to Black*).

---

## P-4: Perjalanan Malam
**Visual / Kondisi:**
*Fade in*. Diana kini berdiri di ujung jalan aspal menuju sekolah. Suasana malam: sepi, suara jangkrik, angin malam, dan gonggongan anjing di kejauhan. Lampu jalan temaram dengan sedikit kabut (Fog linear pendek).

**Aksi:**
Tutorial `[SHIFT] Lari` muncul. Pemain berjalan menyusuri jalan lurus. Tidak ada bahaya/musuh di sini.

**Dialog (VO) - Dipicu saat pemain mulai berjalan:**
- **Diana [VO_P4_1]:** *(Napas agak terengah, jalan cepat, sedikit kedinginan)* "Dingin banget... Harusnya tadi pakai jaket yang lebih tebal."

**Event Skrip:**
Saat mendekati gerbang sekolah, satu lampu jalan di atas kepala Diana tiba-tiba berkedip cepat (*flicker*) lalu padam total. Suara jangkrik mendadak berhenti sejenak.
- **Diana [VO_P4_2]:** *(Kaget kecil)* "Eh... Untung aja bawa HP buat senter." *(Petunjuk implisit bahwa senter ada di HP).*

---

## P-5: Gerbang Sekolah Tertutup
**Visual / Kondisi:**
Pemain tiba di depan gerbang besi besar sekolah. Di balik gerbang, sekolah tampak gelap gulita dan mengancam. Suasana audio berubah lebih berat.

**Interaksi:**
Pemain mencoba membuka gerbang kecil di samping.
- *SFX: Suara gembok besi besar ditarik dan digerakkan (keras).*

**Dialog (VO):**
- **Diana [VO_P5_1]:** *(Berbisik cemas)* "Digembok... Berarti harus cari jalan masuk lain. Pos satpam dulu deh, cari kuncinya."

**Efek Transisi Akhir:**
Layar perlahan *Fade to Black* yang lambat.
*Loading menuju `Chapter1.tscn` (Babak 1).*

---

## Daftar Baris Suara (Voice Over Checklist)

| ID | Karakter | Nada / Emosi | Baris Teks |
|---|---|---|---|
| `VO_P1_1` | Diana | Panik tertahan, menghela napas | "Tigapuluh lima persen... Besok pagi pasti mati total ini." |
| `VO_P1_2` | Diana | Kecewa | "Kosong. Gak ada di sini." |
| `VO_P1_3` | Diana | Menyesal, bingung | "Cuma ada kabel bekas... ujungnya beda. Kenapa sih aku nggak pernah kepikiran beli adaptor cadangan?" |
| `VO_P2_1` | Diana | Bergumam pelan, sadar sesuatu | "Ketinggalan di kelas..." |
| `VO_P3_1` | Diana | Putus asa, tapi tekad bulat | "Gak bisa. Aku gak bisa hidup tanpa HP besok. Seharian bengong di sekolah? Bisa gila." |
| `VO_P3_2` | Diana | Mencoba menenangkan diri sendiri | "Sekolah malam-malam... Gak apa-apa, cepet kok. Masuk, ambil, pulang." |
| `VO_P4_1` | Diana | Kedinginan, napas agak cepat | "Dingin banget... Harusnya tadi pakai jaket yang lebih tebal." |
| `VO_P4_2` | Diana | Kaget kecil, meyakinkan diri | "Eh... Untung aja bawa HP buat senter." |
| `VO_P5_1` | Diana | Berbisik cemas | "Digembok... Berarti harus cari jalan masuk lain. Pos satpam dulu deh, cari kuncinya." |
