import pathlib, hashlib, re, random

# Fix duplicate UIDs in .import files
def gen_uid():
    # Godot uid format: uid:// + base62-like string (we use hex + random)
    chars = "abcdefghijklmnopqrstuvwxyz0123456789"
    # Generate 11-char uid similar to Godot's
    import random, string
    # Use hash of random
    h = hashlib.md5(str(random.random()).encode()).hexdigest()
    # Take 11 chars and map to allowed
    uid = "".join(random.choice("abcdefghijklmnopqrstuvwxyz0123456789") for _ in range(12))
    return f"uid://{uid}"

# Fix atlas textures
atlas_dir = pathlib.Path(r"C:\Users\Hype GLK\charger-diana\assets\textures\atlas")
for p in atlas_dir.glob("*.png"):
    imp = pathlib.Path(str(p) + ".import")
    if not imp.exists():
        continue
    content = imp.read_text(encoding="utf-8")
    # Generate unique uid per file (deterministic via file name hash)
    uid = "uid://" + hashlib.md5(f"atlas_{p.name}".encode()).hexdigest()[:12]
    # Replace uid line
    content_new = re.sub(r'uid="uid://[^"]+"', f'uid="{uid}"', content, count=1)
    if content_new != content:
        imp.write_text(content_new, encoding="utf-8")
        print(f"Fixed atlas {p.name} uid -> {uid}")

# Fix GLB imports
for sub in ["arch", "prolog"]:
    d = pathlib.Path(f"C:/Users/Hype GLK/charger-diana/assets/models/{sub}")
    for p in sorted(d.glob("*.glb")):
        imp = pathlib.Path(str(p) + ".import")
        if not imp.exists():
            continue
        content = imp.read_text(encoding="utf-8")
        uid = "uid://" + hashlib.md5(f"glb_{sub}_{p.name}".encode()).hexdigest()[:12]
        # Replace uid
        content_new = re.sub(r'uid="uid://[^"]+"', f'uid="{uid}"', content, count=1)
        # Also fix dest path hash to be unique per file (already done but ensure)
        if content_new != content:
            imp.write_text(content_new, encoding="utf-8")
            print(f"Fixed {sub}/{p.name} uid -> {uid}")

print("UID fix done")
