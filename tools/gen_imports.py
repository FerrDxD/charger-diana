import pathlib, hashlib

chair_import = pathlib.Path(r"C:\Users\Hype GLK\charger-diana\assets\models\furniture\furn_chair_kenney_a.glb.import").read_text()

# atlas
for p in pathlib.Path(r"C:\Users\Hype GLK\charger-diana\assets\textures\atlas").glob("*.png"):
    imp_path = pathlib.Path(str(p) + ".import")
    if imp_path.exists():
        print(f"SKIP {p.name}")
        continue
    rel = "res://assets/textures/atlas/" + p.name
    template = pathlib.Path(r"C:\Users\Hype GLK\charger-diana\assets\textures\atlas\atlas_furniture_placeholder.png.import").read_text()
    new_content = template.replace(
        'source_file="res://assets/textures/atlas/atlas_furniture_placeholder.png"',
        f'source_file="{rel}"'
    )
    imp_path.write_text(new_content)
    print(f"Created {imp_path.name}")

# GLBs
for sub in ["arch", "prolog"]:
    dir_path = pathlib.Path(f"C:/Users/Hype GLK/charger-diana/assets/models/{sub}")
    for p in sorted(dir_path.glob("*.glb")):
        imp_path = pathlib.Path(str(p) + ".import")
        if imp_path.exists():
            print(f"SKIP {p.name}")
            continue
        rel = f"res://assets/models/{sub}/{p.name}"
        md5 = hashlib.md5(rel.encode()).hexdigest()[:32]
        scn_dest = f"res://.godot/imported/{p.name}-{md5}.scn"
        new_content = chair_import.replace(
            'source_file="res://assets/models/furniture/furn_chair_kenney_a.glb"',
            f'source_file="{rel}"'
        ).replace(
            'dest_files=["res://.godot/imported/furn_chair_kenney_a.glb-c60ca90e96d8647a7c25342437227c01.scn"]',
            f'dest_files=["{scn_dest}"]'
        ).replace(
            'path="res://.godot/imported/furn_chair_kenney_a.glb-c60ca90e96d8647a7c25342437227c01.scn"',
            f'path="{scn_dest}"'
        )
        imp_path.write_text(new_content)
        print(f"Created {imp_path.name}")

print("done")
