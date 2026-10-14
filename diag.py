import pathlib, re, os, json

print("=== IMPORT UIDs ===")
for p in sorted(pathlib.Path("assets").rglob("*.import")):
    t=open(p,encoding="utf-8",errors="ignore").read()
    m=re.search(r'uid="([^"]+)"',t)
    valid=("valid=false" in t)
    ctex=re.search(r'path="([^"]+\.ctex)"',t)
    print(f"{p} -> uid={m.group(1) if m else 'NO_UID'} valid_false={valid} ctex={ctex.group(1) if ctex else 'none'}")

print("\n=== TSCN ATLAS REFS ===")
for p in sorted(pathlib.Path("scenes").rglob("*.tscn")):
    t=open(p,encoding="utf-8").read()
    if "atlas" in t.lower():
        print(f"\n-- {p} --")
        for line in t.splitlines():
            if "atlas" in line.lower() or "uid://atlas" in line:
                print(line.strip()[:300])

print("\n=== UID CROSS CHECK ===")
all_import_uids={}
for p in pathlib.Path("assets").rglob("*.import"):
    t=open(p,encoding="utf-8",errors="ignore").read()
    for mm in re.finditer(r'uid://[^\"]+',t):
        all_import_uids[mm.group(0)]=str(p)
# check tscn uids that are atlas-like
for p in pathlib.Path("scenes").rglob("*.tscn"):
    t=open(p,encoding="utf-8").read()
    for m in re.finditer(r'uid://[^\"]+',t):
        u=m.group(0)
        if "atlas" in u:
            known="KNOWN" if u in all_import_uids else "UNKNOWN/FAKE"
            print(f"{known} {u} in {p}")

print("\n=== IMPORTED CTEX EXISTS ===")
imp_dir=pathlib.Path(".godot/imported")
if imp_dir.exists():
    for f in sorted(imp_dir.glob("atlas*")):
        print(f.name, f.stat().st_size)
else:
    print("no .godot/imported")

print("\n=== .godot/imported atlas check ===")
for name in ["atlas_diana_room","atlas_arch","atlas_furniture"]:
    hits=list(pathlib.Path(".godot/imported").glob(name+"*")) if pathlib.Path(".godot/imported").exists() else []
    print(name, hits)

print("\n=== PrologRoom load_steps sanity ===")
t=open("scenes/prolog/PrologRoom.tscn",encoding="utf-8").read()
print(t[:500])

print("\n=== Check Hud etc exists ===")
for p in ["scenes/ui/Hud.tscn","scenes/player/Player.tscn","scenes/prolog/PrologChatUi.tscn"]:
    print(p, pathlib.Path(p).exists())

print("\n=== SceneManager target path exists ===")
print(pathlib.Path("scenes/prolog/PrologRoom.tscn").exists(), pathlib.Path("scenes/prolog/PrologRoom.tscn").stat().st_size)
